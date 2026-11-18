using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using RsskWorkTeacher.Models.RP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace RsskWorkTeacher.Services
{
    public static class PoppParser
    {
        public static Dictionary<string, DisciplineTemplate> Cache { get; private set; } = new();

        public static Dictionary<string, DisciplineTemplate> ParseDocx(string docxPath)
        {
            var result = new Dictionary<string, DisciplineTemplate>();

            using (WordprocessingDocument doc = WordprocessingDocument.Open(docxPath, false))
            {
                var body = doc.MainDocumentPart.Document.Body;
                DisciplineTemplate currentTemplate = null;
                string currentSection = "";
                bool expectingTable = false;
                bool expectingDisciplineCode = false;

                foreach (var element in body.Elements())
                {
                    if (element is Paragraph p)
                    {
                        string rawText = p.InnerText.Trim();
                        // Убираем лишние пробелы и невидимые символы
                        string text = Regex.Replace(rawText, @"\s+", " ");
                        if (string.IsNullOrWhiteSpace(text)) continue;

                        // --- 1. ПОИСК КОДА ДИСЦИПЛИНЫ ---
                        // Ищем код вида ОП.01, СГ.02, ПМ.03
                        var codeMatch = Regex.Match(text, @"\b([А-Я]{2,3}\.\d{2})\b");

                        if (expectingDisciplineCode && codeMatch.Success)
                        {
                            if (currentTemplate != null) result[currentTemplate.DisciplineKod] = currentTemplate;
                            currentTemplate = new DisciplineTemplate { DisciplineKod = codeMatch.Groups[1].Value };
                            currentSection = "";
                            expectingDisciplineCode = false;
                            continue;
                        }

                        // Если встретили заголовок "Примерная рабочая программа дисциплины"
                        if (text.Contains("Примерная рабочая программа дисциплины", StringComparison.OrdinalIgnoreCase) ||
                            text.Contains("ПРИМЕРНАЯ РАБОЧАЯ ПРОГРАММА ДИСЦИПЛИНЫ", StringComparison.OrdinalIgnoreCase))
                        {
                            if (codeMatch.Success) // Если код на этой же строке
                            {
                                if (currentTemplate != null) result[currentTemplate.DisciplineKod] = currentTemplate;
                                currentTemplate = new DisciplineTemplate { DisciplineKod = codeMatch.Groups[1].Value };
                                currentSection = "";
                            }
                            else // Если код будет на следующей строке
                            {
                                expectingDisciplineCode = true;
                            }
                            continue;
                        }

                        if (currentTemplate == null) continue;

                        // --- 2. ОПРЕДЕЛЕНИЕ СЕКЦИЙ ---
                        if (text.StartsWith("1.1.") || text.Contains("Цель и место дисциплины"))
                            currentSection = "Goal";
                        else if (text.StartsWith("1.2.") || text.Contains("Планируемые результаты"))
                            currentSection = "Competencies";
                        else if (text.StartsWith("2.2.") || text.Contains("Примерное содержание дисциплины"))
                            currentSection = "Themes";
                        else if (text.StartsWith("3.1.") || text.Contains("Материально-техническое обеспечение"))
                            currentSection = "MaterialTech";
                        else if (text.StartsWith("3.2.1.") || text.Contains("Основные печатные"))
                            currentSection = "LitMain";
                        else if (text.StartsWith("3.2.2.") || text.Contains("Дополнительные"))
                            currentSection = "LitAdd";
                        else if (text.StartsWith("3.2.") || text.Contains("Учебно-методическое обеспечение"))
                            currentSection = "LitIntro"; // Перехватываем 3.2, чтобы он не попал в MaterialTech
                        else if (text.StartsWith("4.") || text.Contains("КОНТРОЛЬ И ОЦЕНКА РЕЗУЛЬТАТОВ"))
                        {
                            currentSection = "Evaluations";
                            expectingTable = true;
                        }


                        // --- 3. СБОР ТЕКСТА ---
                        if (currentSection == "Goal")
                        {
                            if (!text.StartsWith("1.1") && !text.Contains("структуре образовательной программы") &&
                                !text.Contains("ПРИМЕРНОЙ РАБОЧЕЙ ПРОГРАММЫ"))
                            {
                                currentTemplate.Purpose = string.IsNullOrEmpty(currentTemplate.Purpose)
                                    ? text : currentTemplate.Purpose + " " + text;
                            }
                        }
                        else if (currentSection == "MaterialTech")
                        {
                            // Игнорируем заголовок, собираем только текст про кабинеты/лаборатории
                            if (!text.StartsWith("3.1") && !text.Contains("Материально-техническое"))
                            {
                                currentTemplate.MaterialTech = string.IsNullOrEmpty(currentTemplate.MaterialTech)
                                    ? text : currentTemplate.MaterialTech + " " + text;
                            }
                        }
                        else if (currentSection == "LitMain")
                        {
                            if (!text.StartsWith("3.2.1") && !text.Contains("Основные печатные") && !text.Contains("библиотечный фонд"))
                                currentTemplate.LiteratureMain.Add(text);
                        }
                        else if (currentSection == "LitAdd")
                        {
                            if (!text.StartsWith("3.2.2") && !text.Contains("Дополнительные"))
                                currentTemplate.LiteratureAdditional.Add(text);
                        }
                    }
                    else if (element is Table t)
                    {
                        if (currentTemplate == null) continue;

                        // --- 4. ПАРСИНГ ТАБЛИЦ ---
                        if (currentSection == "Competencies" && expectingTable)
                        {
                            ParseCompetenciesTable(t, currentTemplate);
                            expectingTable = false;
                            currentSection = "";
                        }
                        else if (currentSection == "Themes")
                        {
                            ParseThemesTable(t, currentTemplate);
                            currentSection = ""; // Сбрасываем, чтобы следующие таблицы не прочитались как темы
                        }
                        else if (currentSection == "Evaluations")
                        {
                            ParseEvaluationsTable(t, currentTemplate);
                            expectingTable = false;
                            currentSection = "";
                        }
                    }
                }

                if (currentTemplate != null)
                    result[currentTemplate.DisciplineKod] = currentTemplate;
            }

            Cache = result;
            return result;
        }

        private static void ParseCompetenciesTable(Table table, DisciplineTemplate template)
        {
            foreach (var row in table.Elements<TableRow>().Skip(1)) // Пропускаем шапку
            {
                var cells = row.Elements<TableCell>().ToList();
                if (cells.Count >= 3)
                {
                    string code = Regex.Replace(cells[0].InnerText.Trim(), @"\s+", " ");
                    if (code.StartsWith("ОК") || code.StartsWith("ПК"))
                    {
                        template.Competencies.Add(new Competency
                        {
                            Code = code,
                            KnowHow = cells[1].InnerText.Trim(),
                            Knowledge = cells[2].InnerText.Trim()
                        });
                    }
                }
            }
        }

        private static void ParseThemesTable(Table table, DisciplineTemplate template)
        {
            string currentThemeName = "";
            var currentBlocks = new List<ContentBlock>();

            foreach (var row in table.Elements<TableRow>())
            {
                var cells = row.Elements<TableCell>().ToList();
                if (cells.Count == 0) continue;

                string text0 = cells.Count > 0 ? cells[0].InnerText.Trim() : "";
                string text1 = cells.Count > 1 ? cells[1].InnerText.Trim() : "";

                if (string.IsNullOrEmpty(text0) && !string.IsNullOrEmpty(text1))
                {
                    AddBlock(currentBlocks, text1);
                }
                else if (!string.IsNullOrEmpty(text0))
                {
                    if (text0.StartsWith("Раздел") || text0.StartsWith("Тема") ||
                        text0.StartsWith("Самостоятельная работа") || text0.StartsWith("Промежуточная аттестация"))
                    {
                        if (!string.IsNullOrEmpty(currentThemeName) && currentBlocks.Count > 0)
                        {
                            template.Themes.Add(new ThemePlan { ThemeName = currentThemeName, ContentBlocks = new ObservableCollection<ContentBlock>(currentBlocks) });
                        }
                        currentThemeName = text0;
                        currentBlocks = new List<ContentBlock>();
                        if (!string.IsNullOrEmpty(text1)) AddBlock(currentBlocks, text1);
                    }
                    else if (!string.IsNullOrEmpty(text1))
                    {
                        AddBlock(currentBlocks, text1);
                    }
                }
            }

            if (!string.IsNullOrEmpty(currentThemeName) && currentBlocks.Count > 0)
            {
                template.Themes.Add(new ThemePlan { ThemeName = currentThemeName, ContentBlocks = new ObservableCollection<ContentBlock>(currentBlocks) });
            }
        }

        private static void AddBlock(List<ContentBlock> blocks, string text)
        {
            string formatted = text.Replace("Содержание", "\nСодержание:\n")
                                   .Replace("В том числе практических и лабораторных занятий", "\nВ том числе практических и лабораторных занятий:\n")
                                   .Replace("В том числе самостоятельная работа обучающихся", "\nВ том числе самостоятельная работа обучающихся:\n")
                                   .Replace("Необходимость и тематика определяются образовательной организацией", "\nНеобходимость и тематика определяются образовательной организацией")
                                   .Trim();
            if (!string.IsNullOrEmpty(formatted))
                blocks.Add(new ContentBlock { Description = formatted, Hours = "" });
        }

        private static void ParseEvaluationsTable(Table table, DisciplineTemplate template)
        {
            foreach (var row in table.Elements<TableRow>().Skip(1))
            {
                var cells = row.Elements<TableCell>().ToList();
                if (cells.Count >= 3)
                {
                    string col1 = cells[0].InnerText.Trim();
                    if (col1 == "Результаты обучения" || col1 == "Показатели освоенности компетенций" || string.IsNullOrEmpty(col1))
                        continue;

                    template.Evaluations.Add(new EvaluationCriteria
                    {
                        LearningOutcome = col1,
                        Indicators = cells[1].InnerText.Trim(),
                        AssessmentMethod = cells[2].InnerText.Trim()
                    });
                }
            }
        }
    }
}