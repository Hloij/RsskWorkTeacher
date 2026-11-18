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
    public static class WordPmParser
    {
        public static WorkingModuleViewModel ParsePm(string filePath)
        {
            var vm = new WorkingModuleViewModel();

            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
            {
                // Получаем документ в виде плоского текста (ячейки через \t, строки через \n)
                string flatText = GetFlatText(doc);
                var blocks = GetAllBlocks(doc.MainDocumentPart.Document.Body).ToList();

                // 1. Титульный лист и утверждение
                ParseTitleAndApprovals(flatText, vm);

                // 2. Раздел 1.1 (Цель) и 1.2 (Компетенции)
                ParseGoalAndCompetencies(flatText, blocks, vm);

                // 3. Раздел 2.1 (Трудоемкость модуля)
                ParseModuleLaborIntensity(flatText, vm);

                // 4. Раздел 2.2 (Структура модуля)
                ParseModuleStructure(flatText, vm);

                // 5. Раздел 2.3 (Тематический план)
                ParseThemes(flatText, vm);

                // 6. Разделы 3.1, 3.2, 4
                ParseConditionsAndEvaluations(flatText, vm);
            }

            return vm;
        }

        // --- 1. ТИТУЛЬНЫЙ ЛИСТ ---
        private static void ParseTitleAndApprovals(string text, WorkingModuleViewModel vm)
        {
            var pmMatch = Regex.Match(text, @"(ПМ\.\d+)\s+([^\n]+)");
            if (pmMatch.Success)
            {
                vm.DisciplineKod = pmMatch.Groups[1].Value.Trim();
                vm.DisciplineName = pmMatch.Groups[2].Value.Trim();
            }

            var specMatch = Regex.Match(text, @"Специальность\s+([^\n]+)");
            if (specMatch.Success) vm.Specialty = specMatch.Groups[1].Value.Trim();

            var qualMatch = Regex.Match(text, @"Квалификация выпускника\s+([^\n]+)");
            if (qualMatch.Success) vm.Qualification = qualMatch.Groups[1].Value.Trim();

            var devMatches = Regex.Matches(text, @"Разработчик:\s*([^,\n]+),?\s*([^\n]*)");
            foreach (Match m in devMatches)
                vm.Developers.Add(new Developer { FullName = m.Groups[1].Value.Trim(), Position = m.Groups[2].Value.Trim() });

            var protocols = Regex.Matches(text, @"Протокол от\s*([\d\.]+)\s*г?\.?\s*№\s*(\d+)");
            if (protocols.Count > 0)
            {
                vm.ProtocolDate = DateTime.TryParse(protocols[0].Groups[1].Value, out var d1) ? d1 : null;
                vm.ProtocolNumber = protocols[0].Groups[2].Value;
            }
            if (protocols.Count > 1)
            {
                vm.ProtocolSovetDate = DateTime.TryParse(protocols[1].Groups[1].Value, out var d2) ? d2 : null;
                vm.ProtocolSovetNumber = protocols[1].Groups[2].Value;
            }

            var predMatch = Regex.Match(text, @"Председатель комиссии\s+([^\n]+)");
            if (predMatch.Success) vm.ProtocolPredsedatel = predMatch.Groups[1].Value.Trim();
        }

        // --- 2. РАЗДЕЛ 1 (Цель и Компетенции) ---
        private static void ParseGoalAndCompetencies(string text, List<OpenXmlElement> blocks, WorkingModuleViewModel vm)
        {
            // Цель
            var goalMatch = Regex.Match(text, @"Цель модуля:\s*([^\n]+(?:\n[^\n]+)*?)(?=\nВ результате|\n1\.2|\n2 )", RegexOptions.Singleline);
            if (goalMatch.Success)
                vm.Purpose = CleanText(goalMatch.Groups[1].Value);

            // Компетенции (ищем таблицу)
            foreach (var table in blocks.OfType<Table>())
            {
                string header = GetTableHeader(table);
                if (header.Contains("Код ПК") && header.Contains("Уметь"))
                {
                    foreach (var row in table.Elements<TableRow>().Skip(1))
                    {
                        var cells = row.Elements<TableCell>().Select(c => CleanText(c.InnerText)).ToList();
                        if (cells.Count >= 4 && Regex.IsMatch(cells[0], @"^ПК"))
                        {
                            vm.Competencies.Add(new Competency
                            {
                                Code = cells[0].Split(' ')[0] + " " + cells[0].Split(' ')[1], // Берем только код (ПК 3.1)
                                KnowHow = cells[1],
                                Knowledge = cells[2],
                                Skil = cells[3]
                            });
                        }
                    }
                }
            }
        }

        // --- 3. РАЗДЕЛ 2.1 (Трудоемкость) ---
        private static void ParseModuleLaborIntensity(string text, WorkingModuleViewModel vm)
        {
            // Регулярки для поиска строк в плоском тексте. Ищем название, затем первое число (объем), затем второе (практика)
            vm.Module21.LearnTotal = GetValue(text, @"Учебные занятия\t(\d+)");
            vm.Module21.LearnPractical = GetValue(text, @"Учебные занятия\t\d+\t(\d+)");

            vm.Module21.CourseworkTotal = GetValue(text, @"Курсовая работа.*?\t(\d+|-)");
            vm.Module21.CourseworkPractical = GetValue(text, @"Курсовая работа.*?\t\d+\t(\d+|-)");

            vm.Module21.SelfWorkTotal = GetValue(text, @"Самостоятельная работа\t(\d+|-)");

            vm.Module21.PracticeTotal = GetValue(text, @"Практика, в т.ч.:\t(\d+)");
            vm.Module21.PracticePractical = GetValue(text, @"Практика, в т.ч.:\t\d+\t(\d+)");

            vm.Module21.PracticeEduTotal = GetValue(text, @"учебная\t(\d+)");
            vm.Module21.PracticeEduPractical = GetValue(text, @"учебная\t\d+\t(\d+)");

            vm.Module21.PracticeIndTotal = GetValue(text, @"производственная\t(\d+)");
            vm.Module21.PracticeIndPractical = GetValue(text, @"производственная\t\d+\t(\d+)");

            vm.Module21.Attestation = GetValue(text, @"Промежуточная аттестация\t(\d+|-)");

            vm.Module21.TotalHours = GetValue(text, @"Всего\t(\d+)");
            vm.Module21.TotalPractical = GetValue(text, @"Всего\t\d+\t(\d+)");
        }

        // --- 4. РАЗДЕЛ 2.2 (Структура) ---
        private static void ParseModuleStructure(string text, WorkingModuleViewModel vm)
        {
            // Отрезаем блок от 2.2 до 2.3
            var match = Regex.Match(text, @"2\.2 Структура.*?\n(.*?)2\.3 ", RegexOptions.Singleline);
            if (!match.Success) return;

            var lines = match.Groups[1].Value.Split('\n');
            foreach (var line in lines)
            {
                var cells = line.Split('\t');
                // В таблице 2.2 минимум 10 колонок (от Код ПК до Аттестации)
                if (cells.Length >= 9 && !cells[0].Contains("Код ПК"))
                {
                    // Если это строка с УП или ПП или Экзаменом
                    if (cells[0].Contains("ПК") || cells[1].Contains("Учебная практика") || cells[1].Contains("Производственная") || cells[1].Contains("Экзамен"))
                    {
                        vm.ModuleStructures.Add(new ModuleStructureRow
                        {
                            CompetencyCodes = cells[0],
                            ModuleName = cells[1],
                            TotalHours = cells[2],
                            PracticalPrepHours = cells[3],
                            MdkHours = cells[4],
                            TrainingSessionsHours = cells[5],
                            CourseworkHours = cells[6],
                            IndependentWorkHours = cells[7],
                            EducationalPracticeHours = cells.Length > 8 ? cells[8] : "-",
                            IndustrialPracticeHours = cells.Length > 9 ? cells[9] : "-",
                            AssessmentHours = cells.Length > 10 ? cells[10] : "-"
                        });
                    }
                }
            }
        }

        // --- 5. РАЗДЕЛ 2.3 (Темы) ---
        private static void ParseThemes(string text, WorkingModuleViewModel vm)
        {
            var match = Regex.Match(text, @"2\.3 Тематический план.*?\n(.*?)3 УСЛОВИЯ", RegexOptions.Singleline);
            if (!match.Success) return;

            var lines = match.Groups[1].Value.Split('\n');
            string currentTheme = "";
            bool isSection = false;
            var currentBlocks = new List<ContentBlock>();
            string currentHours = "";
            string currentComp = "";

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cells = line.Split('\t');

                // Раздел (объединенная ячейка, 1 столбец)
                if (cells.Length == 1 && cells[0].StartsWith("Раздел"))
                {
                    if (!string.IsNullOrEmpty(currentTheme))
                        vm.Themes.Add(new ThemePlan { ThemeName = currentTheme, IsSection = isSection, TotalHours = currentHours, Competences = currentComp, ContentBlocks = new ObservableCollection<ContentBlock>(currentBlocks) });

                    currentTheme = cells[0];
                    isSection = true;
                    currentBlocks = new List<ContentBlock>();
                    currentHours = ""; currentComp = "";
                }
                // Тема (начинается с "Тема", "Самостоятельная", "Промежуточная")
                else if (cells.Length >= 3 && (cells[0].StartsWith("Тема") || cells[0].StartsWith("Самостоятельная") || cells[0].StartsWith("Промежуточная")))
                {
                    if (!string.IsNullOrEmpty(currentTheme))
                        vm.Themes.Add(new ThemePlan { ThemeName = currentTheme, IsSection = isSection, TotalHours = currentHours, Competences = currentComp, ContentBlocks = new ObservableCollection<ContentBlock>(currentBlocks) });

                    currentTheme = cells[0];
                    isSection = false;
                    // Ищем часы (обычно в 3-й колонке) и компетенции (в 4-й)
                    currentHours = cells[2];
                    currentComp = cells.Length > 3 ? cells[3] : "";
                    currentBlocks = new List<ContentBlock>();

                    if (!string.IsNullOrWhiteSpace(cells[1]) && !cells[1].Contains("Содержание учебного материала"))
                        currentBlocks.Add(new ContentBlock { Description = cells[1], Hours = cells[2] });
                }
                // Блок содержания (сдвинут вправо, начинается с табуляции)
                else if (line.StartsWith("\t") && cells.Length >= 2)
                {
                    if (!string.IsNullOrWhiteSpace(cells[1]))
                        currentBlocks.Add(new ContentBlock { Description = cells[1], Hours = cells.Length > 2 ? cells[2] : "" });
                }
            }

            if (!string.IsNullOrEmpty(currentTheme))
                vm.Themes.Add(new ThemePlan { ThemeName = currentTheme, IsSection = isSection, TotalHours = currentHours, Competences = currentComp, ContentBlocks = new ObservableCollection<ContentBlock>(currentBlocks) });
        }

        // --- 6. РАЗДЕЛЫ 3.1, 3.2, 4 ---
        private static void ParseConditionsAndEvaluations(string text, WorkingModuleViewModel vm)
        {
            // 3.1 Кабинеты
            var kabMatch = Regex.Match(text, @"3\.1 Для реализации.*?\n(.*?)3\.2", RegexOptions.Singleline);
            if (kabMatch.Success) vm.TrebKab = CleanText(kabMatch.Groups[1].Value, true);

            // 3.2 Литература
            var mainLit = Regex.Match(text, @"Основные источники:\n(.*?)(?:Дополнительные источники:|Перечень)", RegexOptions.Singleline);
            if (mainLit.Success)
            {
                foreach (var l in mainLit.Groups[1].Value.Split('\n').Where(x => Regex.IsMatch(x, @"^\d+\.")))
                    vm.OsnovLibrary.Add(new LibraryRecord { Record = l.Trim() });
            }

            var dopLit = Regex.Match(text, @"Дополнительные источники:\n(.*?)(?:Перечень|4 КОНТРОЛЬ)", RegexOptions.Singleline);
            if (dopLit.Success)
            {
                foreach (var l in dopLit.Groups[1].Value.Split('\n').Where(x => Regex.IsMatch(x, @"^\d+\.")))
                    vm.DopLibrary.Add(new LibraryRecord { Record = l.Trim() });
            }

            // ПО (ищем таблицу, где 2 колонки)
            var poLines = text.Split('\n').Where(l => l.Contains("\t") && (l.Contains("Windows") || l.Contains("OpenOffice") || l.Contains("лицензия") || l.Contains("ПО")));
            foreach (var l in poLines)
            {
                var parts = l.Split('\t');
                if (parts.Length >= 2 && parts[0] != "Наименование")
                    vm.Softwares.Add(new Software { Name = parts[0].Trim(), LicenseType = parts[1].Trim() });
            }

            // 4 Оценки (таблица)
            var evalMatch = Regex.Match(text, @"ПК \d\.[\d\s,ПК]+\t(.*?)(?=ПК \d|\Z)", RegexOptions.Singleline);
            // Вместо сложных регулярок лучше пройтись по строкам с ПК, где есть 3 колонки
            foreach (var line in text.Split('\n'))
            {
                var cells = line.Split('\t');
                if (cells.Length >= 3 && Regex.IsMatch(cells[0], @"^ПК \d"))
                {
                    vm.Evaluations.Add(new EvaluationCriteria
                    {
                        LearningOutcome = cells[0].Trim(),
                        Indicators = cells[1].Trim(),
                        AssessmentMethod = cells[2].Trim()
                    });
                }
            }
        }

        // --- ХЕЛПЕРЫ ---
        private static string GetValue(string text, string pattern)
        {
            var m = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
            return m.Success ? m.Groups[1].Value.Trim() : "-";
        }

        private static string GetFlatText(WordprocessingDocument doc)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var element in GetAllBlocks(doc.MainDocumentPart.Document.Body))
            {
                if (element is Paragraph p)
                {
                    string text = CleanText(p.InnerText);
                    if (!string.IsNullOrWhiteSpace(text)) sb.AppendLine(text);
                }
                else if (element is Table t)
                {
                    foreach (var row in t.Elements<TableRow>())
                    {
                        var cells = row.Elements<TableCell>().Select(c => CleanText(c.InnerText).Replace("\n", " ")).ToList();
                        sb.AppendLine(string.Join("\t", cells));
                    }
                }
            }
            return sb.ToString();
        }

        private static IEnumerable<OpenXmlElement> GetAllBlocks(OpenXmlElement parent)
        {
            foreach (var child in parent.ChildElements)
            {
                if (child is Paragraph || child is Table) yield return child;
                else if (child.HasChildren)
                    foreach (var sub in GetAllBlocks(child)) yield return sub;
            }
        }

        private static string CleanText(string input, bool keepBreaks = false)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string c = input.Replace("\u00A0", " ").Replace("\u200B", "");
            if (keepBreaks) return string.Join("\n", c.Split('\n').Select(l => Regex.Replace(l, @"[ \t]+", " ").Trim()).Where(l => !string.IsNullOrEmpty(l)));
            return Regex.Replace(c, @"\s+", " ").Trim();
        }

        private static string GetTableHeader(Table table)
        {
            var firstRow = table.Elements<TableRow>().FirstOrDefault();
            if (firstRow == null) return "";
            return CleanText(string.Join(" ", firstRow.Elements<TableCell>().Select(c => c.InnerText)));
        }
    }
}