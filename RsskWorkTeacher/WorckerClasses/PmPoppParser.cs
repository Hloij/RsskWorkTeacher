using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using RsskWorkTeacher.Models.RP;

namespace RsskWorkTeacher.Services
{
    public static class PmPoppParser
    {
        public static Dictionary<string, PmTemplate> Cache { get; private set; } = new();

        public static Dictionary<string, PmTemplate> ParseDocx(string docxPath)
        {
            var result = new Dictionary<string, PmTemplate>();

            using (WordprocessingDocument doc = WordprocessingDocument.Open(docxPath, false))
            {
                var body = doc.MainDocumentPart.Document.Body;
                PmTemplate currentTemplate = null;

                // Строгий указатель текущего раздела
                string currentSection = "";

                // "Выпрямляем" документ, доставая все параграфы и таблицы из любых контейнеров
                foreach (var element in GetAllBlocks(body))
                {
                    if (element is Paragraph p)
                    {
                        // Для текста сохраняем переносы строк (важно для списков в 3.1)
                        string text = CleanText(p.InnerText, keepLineBreaks: true);
                        if (string.IsNullOrWhiteSpace(text)) continue;

                        // 1. Поиск заголовка нового ПМ
                        var match = Regex.Match(text, @"ПМ\s*\.?\s*(\d+)", RegexOptions.IgnoreCase);
                        if (match.Success && (text.IndexOf("Примерная рабочая", StringComparison.OrdinalIgnoreCase) >= 0 || text.Contains("«ПМ")))
                        {
                            string num = match.Groups[1].Value.PadLeft(2, '0');
                            string code = $"ПМ.{num}";

                            if (currentTemplate != null) result[currentTemplate.Code] = currentTemplate;

                            string name = text.Substring(match.Index + match.Length).Trim().Trim('«', '»', '"', '.', ' ');
                            currentTemplate = new PmTemplate { Code = code, Name = name };
                            currentSection = "";
                            continue;
                        }

                        if (currentTemplate == null) continue;

                        // 2. Определение разделов
                        string lowerText = text.ToLowerInvariant();

                        // Ищем строго начало раздела (например: "1.1 ", "3.1.", "4 " и т.д.)
                        if (Regex.IsMatch(lowerText, @"^1\.1[\.\s]")) currentSection = "1.1";
                        else if (Regex.IsMatch(lowerText, @"^1\.2[\.\s]")) currentSection = "1.2";
                        else if (Regex.IsMatch(lowerText, @"^2\.3[\.\s]")) currentSection = "2.3";
                        else if (Regex.IsMatch(lowerText, @"^3\.1[\.\s]")) currentSection = "3.1";
                        else if (Regex.IsMatch(lowerText, @"^3\.2[\.\s]")) currentSection = "3.2";
                        else if (Regex.IsMatch(lowerText, @"^4[\.\s]")) currentSection = "4";

                        // 3. Сбор текста (если он не в таблице)
                        if (currentSection == "1.1")
                        {
                            // Убираем сам заголовок, чтобы он не попал в текст цели
                            string clean = Regex.Replace(text, @"^1\.1\.?\s*Цель.*?:\s*", "", RegexOptions.IgnoreCase);
                            if (!string.IsNullOrWhiteSpace(clean))
                                currentTemplate.Purpose += (string.IsNullOrEmpty(currentTemplate.Purpose) ? "" : "\n") + clean;
                        }
                        else if (currentSection == "3.1")
                        {
                            string clean = Regex.Replace(text, @"^3\.1\.?\s*Требования.*?:\s*", "", RegexOptions.IgnoreCase);
                            if (!string.IsNullOrWhiteSpace(clean))
                                currentTemplate.MaterialTech += (string.IsNullOrEmpty(currentTemplate.MaterialTech) ? "" : "\n") + clean;
                        }
                        else if (currentSection == "3.2")
                        {
                            // Литература, если она списком (начинается с цифры и точки)
                            if (Regex.IsMatch(text, @"^\d+\."))
                                currentTemplate.LiteratureMain.Add(text);
                        }
                    }

                    // === ОБРАБОТКА ТАБЛИЦ ===
                    else if (element is Table t)
                    {
                        if (currentTemplate == null) continue;

                        // Если текст разделов 1.1 или 3.1 запрятан в таблицу (рамку)
                        if (currentSection == "1.1" || currentSection == "3.1")
                        {
                            string tableText = CleanText(string.Join("\n", t.Elements<TableRow>().SelectMany(r => r.Elements<TableCell>()).Select(c => c.InnerText)), keepLineBreaks: true);
                            if (!string.IsNullOrWhiteSpace(tableText))
                            {
                                if (currentSection == "1.1")
                                    currentTemplate.Purpose += (string.IsNullOrEmpty(currentTemplate.Purpose) ? "" : "\n") + tableText;
                                else
                                    currentTemplate.MaterialTech += (string.IsNullOrEmpty(currentTemplate.MaterialTech) ? "" : "\n") + tableText;
                            }
                            continue; // Нам не нужно парсить эту таблицу как что-то другое
                        }

                        // Для остальных разделов определяем тип таблицы по её шапке
                        string header = GetTableHeader(t);

                        if (header.Contains("Код") && header.Contains("Уметь") && header.Contains("Знать"))
                        {
                            ParseCompetenciesTable(t, currentTemplate); // п. 1.2
                        }
                        else if (header.Contains("Наименование разделов") || header.Contains("Наименование разделов и тем"))
                        {
                            ParseThemesTable(t, currentTemplate); // п. 2.3
                        }
                        else if (header.Contains("Библиотечная запись") || header.Contains("Основные печатные"))
                        {
                            ParseLiteratureTable(t, currentTemplate); // п. 3.2
                        }
                        else if (header.Contains("Результаты обучения") || header.Contains("Критерии оценки") || header.Contains("Код ПК, ОК"))
                        {
                            ParseEvaluationsTable(t, currentTemplate); // п. 4
                        }
                    }
                }

                if (currentTemplate != null)
                    result[currentTemplate.Code] = currentTemplate;
            }

            Cache = result;
            return result;
        }

        // --- ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ---

        // Рекурсивно достает все параграфы и таблицы из любых скрытых контейнеров Word
        private static IEnumerable<OpenXmlElement> GetAllBlocks(OpenXmlElement parent)
        {
            foreach (var child in parent.ChildElements)
            {
                if (child is Paragraph || child is Table) yield return child;
                else if (child.HasChildren)
                    foreach (var sub in GetAllBlocks(child)) yield return sub;
            }
        }

        // Очистка текста от невидимых символов
        private static string CleanText(string input, bool keepLineBreaks = false)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string cleaned = input.Replace("\u00A0", " ").Replace("\u200B", "");
            // Если нужно сохранить переносы (для списков), чистим только лишние пробелы
            if (keepLineBreaks)
                return string.Join("\n", cleaned.Split('\n').Select(l => Regex.Replace(l, @"\s+", " ").Trim()).Where(l => !string.IsNullOrEmpty(l)));
            // Иначе схлопываем всё в одну строку
            return Regex.Replace(cleaned, @"\s+", " ").Trim();
        }

        private static string GetTableHeader(Table table)
        {
            var firstRow = table.Elements<TableRow>().FirstOrDefault();
            if (firstRow == null) return "";
            return CleanText(string.Join(" ", firstRow.Elements<TableCell>().Select(c => c.InnerText)));
        }

        // ==========================================
        // ПАРСЕРЫ КОНКРЕТНЫХ ТАБЛИЦ
        // ==========================================

        // п. 1.2 (Компетенции)
        private static void ParseCompetenciesTable(Table table, PmTemplate template)
        {
            foreach (var row in table.Elements<TableRow>())
            {
                var cells = row.Elements<TableCell>().ToList();
                if (cells.Count >= 4)
                {
                    string code = CleanText(cells[0].InnerText);
                    string knowHow = CleanText(cells[1].InnerText);
                    string knowledge = CleanText(cells[2].InnerText);
                    string skills = CleanText(cells[3].InnerText);

                    if (code.Contains("Код") || code.Contains("ПК, ОК")) continue;
                    if (string.IsNullOrWhiteSpace(knowHow) && string.IsNullOrWhiteSpace(knowledge) && string.IsNullOrWhiteSpace(skills)) continue;

                    if (Regex.IsMatch(code, @"^(ОК|ПК)"))
                    {
                        var codeMatch = Regex.Match(code, @"^(ОК|ПК)\s*\d+(\.\d+)?");
                        if (codeMatch.Success) code = codeMatch.Value;

                        template.Competencies.Add(new Competency
                        {
                            Code = code,
                            KnowHow = knowHow,
                            Knowledge = knowledge,
                            Skil = skills
                        });
                    }
                }
            }
        }

        // п. 2.3 (Темы и разделы)
        private static void ParseThemesTable(Table table, PmTemplate template)
        {
            string currentThemeName = "";
            bool isSection = false;
            var currentBlocks = new List<ContentBlock>();

            foreach (var row in table.Elements<TableRow>())
            {
                var cells = row.Elements<TableCell>().ToList();
                if (cells.Count == 0) continue;

                string text0 = CleanText(cells[0].InnerText);
                string text1 = cells.Count > 1 ? CleanText(cells[1].InnerText, keepLineBreaks: true) : "";

                // Если это МДК или Раздел — закрываем предыдущую тему
                if (text0.StartsWith("МДК") || text0.StartsWith("Раздел") ||
                    text0.StartsWith("Тема") || text0.StartsWith("Самостоятельная работа") || text0.StartsWith("Промежуточная аттестация"))
                {
                    if (!string.IsNullOrEmpty(currentThemeName) && (currentBlocks.Count > 0 || isSection))
                    {
                        template.Themes.Add(new ThemePlan
                        {
                            ThemeName = currentThemeName,
                            ContentBlocks = new ObservableCollection<ContentBlock>(currentBlocks),
                            IsSection = isSection
                        });
                    }

                    currentThemeName = text0;
                    isSection = text0.StartsWith("МДК") || text0.StartsWith("Раздел");
                    currentBlocks = new List<ContentBlock>();

                    if (!string.IsNullOrEmpty(text1)) AddBlock(currentBlocks, text1);
                }
                else if (!string.IsNullOrEmpty(text1))
                {
                    // Это продолжение контента предыдущей темы
                    AddBlock(currentBlocks, text1);
                }
            }

            // Сохраняем последнюю тему
            if (!string.IsNullOrEmpty(currentThemeName) && (currentBlocks.Count > 0 || isSection))
            {
                template.Themes.Add(new ThemePlan
                {
                    ThemeName = currentThemeName,
                    ContentBlocks = new ObservableCollection<ContentBlock>(currentBlocks),
                    IsSection = isSection
                });
            }
        }

        private static void AddBlock(List<ContentBlock> blocks, string text)
        {
            string formatted = text.Replace("Содержание", "\nСодержание:\n")
                                   .Replace("В том числе практических и лабораторных занятий", "\nВ том числе практических и лабораторных занятий:\n")
                                   .Trim();
            if (!string.IsNullOrEmpty(formatted))
                blocks.Add(new ContentBlock { Description = formatted, Hours = "" });
        }

        // п. 3.2 (Литература в таблице)
        private static void ParseLiteratureTable(Table table, PmTemplate template)
        {
            foreach (var row in table.Elements<TableRow>())
            {
                string text = CleanText(string.Join(" ", row.Elements<TableCell>().Select(c => c.InnerText)));
                if (string.IsNullOrWhiteSpace(text)) continue;

                // Если началась таблица оценок, прекращаем сбор литературы
                if (text.Contains("Код ПК, ОК") || text.Contains("Критерии оценки") || text.Contains("Результаты обучения")) break;
                if (text.Contains("Библиотечная запись") || text.Contains("Основные печатные") || text.Contains("3.2.1.")) continue;

                template.LiteratureMain.Add(text);
            }
        }

        // п. 4 (Оценки)
        private static void ParseEvaluationsTable(Table table, PmTemplate template)
        {
            foreach (var row in table.Elements<TableRow>())
            {
                var cells = row.Elements<TableCell>().ToList();
                if (cells.Count >= 3)
                {
                    string col1 = CleanText(cells[0].InnerText);
                    if (col1.Contains("Код ПК") || col1.Contains("Критерии") || col1.Contains("Результаты обучения") || string.IsNullOrEmpty(col1)) continue;

                    template.Evaluations.Add(new EvaluationCriteria
                    {
                        LearningOutcome = col1,
                        Indicators = CleanText(cells[1].InnerText),
                        AssessmentMethod = CleanText(cells[2].InnerText)
                    });
                }
            }
        }
    }
}