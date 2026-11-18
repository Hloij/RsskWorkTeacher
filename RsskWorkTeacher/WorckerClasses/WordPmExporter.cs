using System;
using System.Linq;
using System.Collections.Generic;
using Xceed.Document.NET;
using Xceed.Words.NET;
using RsskWorkTeacher.Models.RP;

namespace RsskWorkTeacher.Services
{
    public class WordPmExporter
    {
        public static void ExportPmToWord(WorkingModuleViewModel vm, string filePath)
        {
            using (var document = DocX.Create(filePath))
            {
                // 1. Настройка полей по ГОСТ
                document.MarginLeft = 85f;    // 3 см
                document.MarginRight = 42.5f; // 1.5 см
                document.MarginTop = 56.7f;   // 2 см
                document.MarginBottom = 56.7f;// 2 см

                // 2. Базовый шрифт документа
                document.SetDefaultFont(new Xceed.Document.NET.Font("Times New Roman"), 14d, Xceed.Drawing.Color.Black);

                // ==========================================
                // СТРАНИЦА 1: ТИТУЛЬНЫЙ ЛИСТ
                // ==========================================
                document.InsertParagraph("Министерство науки и высшего образования Российской Федерации").FontSize(12).Alignment = Alignment.center;
                document.InsertParagraph("Федеральное государственное бюджетное образовательное \nучреждение высшего образования").FontSize(12).Alignment = Alignment.center;
                document.InsertParagraph("«РЯЗАНСКИЙ ГОСУДАРСТВЕННЫЙ РАДИОТЕХНИЧЕСКИЙ УНИВЕРСИТЕТ \nИМЕНИ В.Ф. УТКИНА»").FontSize(12).Alignment = Alignment.center;
                document.InsertParagraph("Колледж Рязанского государственного радиотехнического университета имени В.Ф. Уткина").FontSize(12).Alignment = Alignment.center;

                for (int i = 0; i < 12; i++)
                {
                    document.InsertParagraph();
                }


                document.InsertParagraph("РАБОЧАЯ ПРОГРАММА").FontSize(12).Alignment = Alignment.center;
                document.InsertParagraph("профессионального модуля").FontSize(12).Alignment = Alignment.center;
                document.InsertParagraph().SpacingAfter(20);

                string fullDisciplineTitle = string.IsNullOrWhiteSpace(vm.DisciplineKod)
                    ? vm.DisciplineName.ToUpper()
                    : $"{vm.DisciplineKod} {vm.DisciplineName}".ToUpper();

                document.InsertParagraph(fullDisciplineTitle).FontSize(12).Alignment = Alignment.center;

                document.InsertParagraph();

                var tableTitle = document.AddTable(6, 2);
                tableTitle.Design = TableDesign.None;
                tableTitle.Alignment = Alignment.left;
                tableTitle.SetWidths(new float[] { 150f, 318f }, true);

                tableTitle.Rows[0].Cells[0].Paragraphs.First().Append("Специальность").FontSize(12);
                tableTitle.Rows[0].Cells[1].Paragraphs.First().Append(vm.Specialty).FontSize(12);

                tableTitle.Rows[1].Cells[0].Paragraphs.First().Append("Квалификация выпускника").FontSize(12);
                tableTitle.Rows[1].Cells[1].Paragraphs.First().Append(vm.Qualification).FontSize(12);

                tableTitle.Rows[4].Cells[0].Paragraphs.First().Append("Форма обучения").FontSize(12);
                tableTitle.Rows[4].Cells[1].Paragraphs.First().Append(vm.EducationForm).FontSize(12);

                tableTitle.Rows[5].Cells[0].Paragraphs.First().Append("Год набора").FontSize(12);
                tableTitle.Rows[5].Cells[1].Paragraphs.First().Append($"{vm.EnrollmentYear} г.").FontSize(12);

                document.InsertTable(tableTitle);
                for (int i = 0; i < 13; i++)
                {
                    document.InsertParagraph();
                }
                document.InsertParagraph(vm.CityAndYear).FontSize(12).Alignment = Alignment.center;

                //document.InsertParagraph().InsertPageBreakAfterSelf();

                // ==========================================
                // СТРАНИЦА 2: УТВЕРЖДЕНИЕ И РАЗРАБОТЧИКИ
                // ==========================================
                document.InsertParagraph("Рассмотрено и рекомендовано к утверждению на заседании цикловой комиссии информационных систем и программирования").FontSize(12).Alignment = Alignment.both;
                document.InsertParagraph();
                document.InsertParagraph($"Протокол от {vm.ProtocolDate:dd.MM.yyyy} г. №{vm.ProtocolNumber}").FontSize(12);
                document.InsertParagraph($"Председатель комиссии {vm.ProtocolPredsedatel}").FontSize(12);

                document.InsertParagraph();
                document.InsertParagraph("Утверждено на методическом совете").FontSize(12);
                document.InsertParagraph($"Протокол от {vm.ProtocolSovetDate:dd.MM.yyyy} г. №{vm.ProtocolSovetNumber}").FontSize(12);

                document.InsertParagraph();

                if (vm.Developers != null && vm.Developers.Count > 0)
                {
                    var razrab = document.InsertParagraph($"Разработчик: {vm.Developers[0].FullName}, {vm.Developers[0].Position}").FontSize(12);
                    if (vm.Developers.Count > 1)
                        razrab.InsertText(",");

                    for (int i = 1; i < vm.Developers.Count; i++)
                    {
                        if (i + 1 == vm.Developers.Count)
                            document.InsertParagraph($"             {vm.Developers[i].FullName}, {vm.Developers[i].Position}").FontSize(12);
                        else
                            document.InsertParagraph($"             {vm.Developers[i].FullName}, {vm.Developers[i].Position},").FontSize(12);

                    }

                }
                else
                {
                    document.InsertParagraph("Разработчик: Юдаев А.Н., преподаватель Колледжа РГРТУ").FontSize(12);
                }
                
                document.InsertParagraph().InsertPageBreakAfterSelf();

                // ==========================================
                // СТРАНИЦА 3: СОДЕРЖАНИЕ
                // ==========================================
                var tocTitle = document.InsertParagraph("СОДЕРЖАНИЕ");
                tocTitle.Alignment = Alignment.center;
                tocTitle.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black);

                var tocSwitches = new Dictionary<TableOfContentsSwitches, string>()
                {
                    { TableOfContentsSwitches.O, "1-3" },
                    { TableOfContentsSwitches.U, "" },
                    { TableOfContentsSwitches.Z, "" },
                    { TableOfContentsSwitches.H, "" },
                };
                
                document.InsertTableOfContents("", tocSwitches);
                
                document.InsertParagraph().InsertPageBreakAfterSelf();

                // ==========================================
                // СТРАНИЦА 4: РАЗДЕЛ 1
                // ==========================================
                var h1_1 = document.InsertParagraph("1 ОБЩАЯ ХАРАКТЕРИСТИКА РАБОЧЕЙ ПРОГРАММЫ ПРОФЕССИОНАЛЬНОГО МОДУЛЯ");
                h1_1.StyleName = "Heading1";
                h1_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f; 

                var h1_1_1 = document.InsertParagraph("1.1 Область применения рабочей программы");
                h1_1_1.StyleName = "Heading2";
                h1_1_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f; 

                var pOblast = document.InsertParagraph($"Рабочая программа профессионального модуля является частью основной образовательной программы в соответствии с ФГОС СПО {vm.Specialty}.");//въебать сюда план
                pOblast.FontSize(12).Alignment = Alignment.both;
                pOblast.IndentationFirstLine = 35.4f;
                var cel = document.InsertParagraph($"Цель модуля: {vm.Purpose}");
                cel.FontSize(12);
                cel.Alignment = Alignment.both;
                cel.IndentationFirstLine = 35.4f;
                var h1_2 = document.InsertParagraph("1.2 Планируемые результаты освоения профессионального модуля");
                h1_2.StyleName = "Heading2";
                h1_2.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f; 

                var pPurpose = document.InsertParagraph($"В результате изучения профессионального модуля студент должен освоить основной вид деятельности {vm.Purpose.Remove(0, vm.Purpose.IndexOf("«"))}");
                pPurpose.FontSize(12);
                pPurpose.Alignment = Alignment.both;
                pPurpose.IndentationFirstLine = 35.4f;

                var pPurpose1 = document.InsertParagraph("Результаты освоения профессионального модуля соотносятся с планируемыми результатами освоения образовательной программы, представленными в матрице компетенций выпускника");
                pPurpose1.FontSize(12);
                pPurpose1.Alignment = Alignment.both;
                pPurpose1.IndentationFirstLine = 35.4f;

                document.InsertParagraph("В результате освоения профессионального модуля обучающийся должен: ").FontSize(12).SpacingBefore(10).IndentationFirstLine = 35.4f;

                var tableComp = document.AddTable(vm.Competencies.Count + 1, 4);
                tableComp.Design = TableDesign.TableGrid;
                tableComp.Alignment = Alignment.left;
                tableComp.SetWidths(new float[] { 80f, 150f, 130f, 120f }, true);

                tableComp.Rows[0].Cells[0].Paragraphs.First().Append("Код ПК").FontSize(12).Bold().Alignment = Alignment.center;
                tableComp.Rows[0].Cells[1].Paragraphs.First().Append("Уметь").FontSize(12).Bold().Alignment = Alignment.center;
                tableComp.Rows[0].Cells[2].Paragraphs.First().Append("Знать").FontSize(12).Bold().Alignment = Alignment.center;
                tableComp.Rows[0].Cells[3].Paragraphs.First().Append("Владеть навыками").FontSize(12).Bold().Alignment = Alignment.center;

                for (int i = 0; i < vm.Competencies.Count; i++)
                {
                    tableComp.Rows[i + 1].BreakAcrossPages = false;
                    tableComp.Rows[i + 1].Cells[0].Paragraphs.First().Append(vm.Competencies[i].Code).FontSize(12);
                    tableComp.Rows[i + 1].Cells[1].Paragraphs.First().Append(vm.Competencies[i].KnowHow).FontSize(12);
                    tableComp.Rows[i + 1].Cells[2].Paragraphs.First().Append(vm.Competencies[i].Knowledge).FontSize(12);
                    tableComp.Rows[i + 1].Cells[3].Paragraphs.First().Append(vm.Competencies[i].Skil).FontSize(12);
                }
                document.InsertTable(tableComp);
                document.InsertParagraph().InsertPageBreakAfterSelf();

                // ==========================================
                // СТРАНИЦА 5: РАЗДЕЛ 2.1 (Объем)
                // ==========================================
                var h2_1 = document.InsertParagraph("2 СТРУКТУРА И СОДЕРЖАНИЕ ПРОФЕССИОНАЛЬНОГО МОДУЛЯ");
                h2_1.StyleName = "Heading1";
                h2_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).IndentationFirstLine = 35.4f;

                var h2_1_1 = document.InsertParagraph("2.1 Трудоемкость освоения модуля");
                h2_1_1.StyleName = "Heading2";
                h2_1_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f;

                var tableVol = document.AddTable(9, 3);
                tableVol.Design = TableDesign.TableGrid;
                tableVol.Alignment = Alignment.left;
                tableVol.SetWidths(new float[] { 234f, 93f, 141f }, true);

                tableVol.Rows[0].Cells[0].Paragraphs.First().Append("Наименование составных частей модуля").FontSize(12).Bold().Alignment = Alignment.center;
                tableVol.Rows[0].Cells[1].Paragraphs.First().Append("Объем в часах").FontSize(12).Bold().Alignment = Alignment.center;
                tableVol.Rows[0].Cells[2].Paragraphs.First().Append("В т.ч. в форме практической подготовки").FontSize(12).Bold().Alignment = Alignment.center;

                tableVol.Rows[1].Cells[0].Paragraphs.First().Append("Учебные занятия").FontSize(12);
                tableVol.Rows[1].Cells[1].Paragraphs.First().Append(vm.Module21.LearnTotal).FontSize(12).Alignment = Alignment.center;
                tableVol.Rows[1].Cells[2].Paragraphs.First().Append(vm.Module21.LearnPractical).FontSize(12).Alignment = Alignment.center;

                tableVol.Rows[2].Cells[0].Paragraphs.First().Append("Курсовая работа (проект)").FontSize(12);
                tableVol.Rows[2].Cells[1].Paragraphs.First().Append(vm.Module21.CourseworkTotal).FontSize(12).Alignment = Alignment.center;
                tableVol.Rows[2].Cells[2].Paragraphs.First().Append(vm.Module21.CourseworkPractical).FontSize(12).Alignment = Alignment.center;

                tableVol.Rows[3].Cells[0].Paragraphs.First().Append("Самостоятельная работа").FontSize(12);
                tableVol.Rows[3].Cells[1].Paragraphs.First().Append(vm.Module21.SelfWorkTotal).FontSize(12).Alignment = Alignment.center;

                tableVol.Rows[4].Cells[0].Paragraphs.First().Append("Практика, в т.ч.:").FontSize(12);
                tableVol.Rows[4].Cells[1].Paragraphs.First().Append(vm.Module21.PracticeTotal).FontSize(12).Alignment = Alignment.center;
                tableVol.Rows[4].Cells[2].Paragraphs.First().Append(vm.Module21.PracticePractical).FontSize(12).Alignment = Alignment.center;

                var pUp = tableVol.Rows[5].Cells[0].Paragraphs.First().Append("учебная").FontSize(12);
                pUp.IndentationBefore = 10f;
                tableVol.Rows[5].Cells[1].Paragraphs.First().Append(vm.Module21.PracticeEduTotal).FontSize(12).Alignment = Alignment.center;
                tableVol.Rows[5].Cells[2].Paragraphs.First().Append(vm.Module21.PracticeEduPractical).FontSize(12).Alignment = Alignment.center;

                var pPp = tableVol.Rows[6].Cells[0].Paragraphs.First().Append("производственная").FontSize(12);
                pPp.IndentationBefore = 10f;
                tableVol.Rows[6].Cells[1].Paragraphs.First().Append(vm.Module21.PracticeIndTotal).FontSize(12).Alignment = Alignment.center;
                tableVol.Rows[6].Cells[2].Paragraphs.First().Append(vm.Module21.PracticeIndPractical).FontSize(12).Alignment = Alignment.center;

                tableVol.Rows[7].Cells[0].Paragraphs.First().Append("Промежуточная аттестация").FontSize(12);
                tableVol.Rows[7].Cells[1].Paragraphs.First().Append(vm.Module21.Attestation).FontSize(12).Alignment = Alignment.center;

                tableVol.Rows[8].Cells[0].Paragraphs.First().Append("Всего").FontSize(12).Bold();
                tableVol.Rows[8].Cells[1].Paragraphs.First().Append(vm.Module21.TotalHours).FontSize(12).Bold().Alignment = Alignment.center;
                tableVol.Rows[8].Cells[2].Paragraphs.First().Append(vm.Module21.TotalPractical).FontSize(12).Bold().Alignment = Alignment.center;

                document.InsertTable(tableVol);
                document.InsertParagraph().SpacingAfter(10).InsertPageBreakAfterSelf();
                // ==========================================
                // СТРАНИЦА 5.1: РАЗДЕЛ 2.2 (Структура) - Финальный вид
                // ==========================================
                var h2_2 = document.InsertParagraph("2.2 Структура профессионального модуля");
                h2_2.StyleName = "Heading2";
                h2_2.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f;

                var tableStruct = document.AddTable(vm.ModuleStructures.Count + 2, 11);
                tableStruct.Design = TableDesign.TableGrid;
                tableStruct.Alignment = Alignment.center;

                // Высота первой строки для вертикального текста
                tableStruct.Rows[0].Height = 200f;

                // Ширины колонок (сумма ~470, чтобы точно влезло в А4 с полями)
                tableStruct.SetWidths(new float[] { 80f, 105f, 40f, 90f, 30f, 30f, 30f, 35f, 30f, 30f, 35f }, true);

                // Вспомогательная функция для вертикального текста
                void SetVerticalHeader(Cell cell, string text, bool isBold = false)
                {
                    cell.Paragraphs[0].Remove(false);
                    var p = cell.Paragraphs.First().Append((text.Trim())).FontSize(12);

                    p.Alignment = Alignment.center;
                    if (isBold) p.Bold();
                    cell.TextDirection = TextDirection.btLr;
                    cell.VerticalAlignment = VerticalAlignment.Center;

                }

                // --- СТРОКА 1: ЗАГОЛОВКИ ---

                // 0: Код ПК
                tableStruct.Rows[0].Cells[0].Paragraphs[0].Remove(false);
                var p0 = tableStruct.Rows[0].Cells[0].InsertParagraph();
                p0.Append("Код ПК").FontSize(12);
                p0.Alignment = Alignment.center;
                tableStruct.Rows[0].Cells[0].VerticalAlignment = VerticalAlignment.Center;

                // 1: Наименования
                tableStruct.Rows[0].Cells[1].Paragraphs[0].Remove(false);
                var p1 = tableStruct.Rows[0].Cells[1].InsertParagraph();
                p1.Append("Наименования разделов профессионального модуля").FontSize(12);
                p1.Alignment = Alignment.center;
                tableStruct.Rows[0].Cells[1].VerticalAlignment = VerticalAlignment.Center;

                // 2: Всего
                tableStruct.Rows[0].Cells[2].Paragraphs[0].Remove(false);
                var p2 = tableStruct.Rows[0].Cells[2].InsertParagraph();
                p2.Append("Всего,\nчас.").FontSize(12);
                p2.Alignment = Alignment.center;
                tableStruct.Rows[0].Cells[2].VerticalAlignment = VerticalAlignment.Center;

                // 3: Практ. подготовка (ГОРИЗОНТАЛЬНО с переносами)
                tableStruct.Rows[0].Cells[3].Paragraphs[0].Remove(false);
                var p3 = tableStruct.Rows[0].Cells[3].InsertParagraph();
                p3.Append("в т.ч. в\nформе\nпракти-\nческой\nподго-\nтовки").FontSize(12);
                p3.Alignment = Alignment.center;
                tableStruct.Rows[0].Cells[3].VerticalAlignment = VerticalAlignment.Center;

                // 4: Обучение по МДК (ВЕРТИКАЛЬНО, с переносом)
                SetVerticalHeader(tableStruct.Rows[0].Cells[4], "Обучение по МДК,\nв т.ч.:", true);

                // 5: Учебные занятия
                SetVerticalHeader(tableStruct.Rows[0].Cells[5], "учебные занятия");

                // 6: Курсовая
                SetVerticalHeader(tableStruct.Rows[0].Cells[6], "курсовая работа (проект)");

                // 7: Самостоятельная
                SetVerticalHeader(tableStruct.Rows[0].Cells[7], "самостоятельная работа");

                // 8: Учебная практика
                SetVerticalHeader(tableStruct.Rows[0].Cells[8], "Учебная практика", true);

                // 9: Производственная
                SetVerticalHeader(tableStruct.Rows[0].Cells[9], "Производственная практика", true);

                // 10: Аттестация
                SetVerticalHeader(tableStruct.Rows[0].Cells[10], "Промежуточная аттестация", true);


                // --- СТРОКА 2: НУМЕРАЦИЯ ---
                for (int i = 0; i < 11; i++)
                {
                    var cell = tableStruct.Rows[1].Cells[i];
                    cell.Paragraphs.First().Append((i + 1).ToString()).FontSize(12).Alignment = Alignment.center;
                    cell.VerticalAlignment = VerticalAlignment.Center;
                }

                // --- СТРОКИ 3+: ДАННЫЕ ---
                for (int i = 0; i < vm.ModuleStructures.Count; i++)
                {
                    var row = tableStruct.Rows[i + 2];
                    row.BreakAcrossPages = false;
                    var data = vm.ModuleStructures[i];

                    bool isTotalRow = data.ModuleName == "Всего:";

                    row.Cells[0].Paragraphs.First().Append(data.CompetencyCodes).FontSize(12).Alignment = Alignment.left;

                    var pName = row.Cells[1].Paragraphs.First().Append(data.ModuleName).FontSize(12);
                    pName.Alignment = isTotalRow ? Alignment.left : Alignment.left;
                    if (isTotalRow) pName.Bold();

                    string[] rowValues = {
                        data.TotalHours, data.PracticalPrepHours, data.MdkHours, data.TrainingSessionsHours,
                        data.CourseworkHours, data.IndependentWorkHours, data.EducationalPracticeHours,
                        data.IndustrialPracticeHours, data.AssessmentHours
                    };

                    for (int c = 0; c < rowValues.Length; c++)
                    {
                        var val = string.IsNullOrEmpty(rowValues[c]) ? "-" : rowValues[c];
                        var cellPara = row.Cells[c + 2].Paragraphs.First().Append(val).Bold().FontSize(12).Alignment = Alignment.center;

                    }
                }
                document.InsertTable(tableStruct);

                // ==========================================
                // СТРАНИЦА 6: РАЗДЕЛ 2.3 (Альбомная ориентация)
                // ==========================================
                document.InsertSectionPageBreak();
                var landscapeSection = document.Sections[document.Sections.Count - 1];
                landscapeSection.PageLayout.Orientation = Orientation.Landscape;

                var h2_3 = document.InsertParagraph("2.3 Тематический план и содержание профессионального модуля");
                h2_3.StyleName = "Heading2";
                h2_3.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f;
                h2_3.SpacingAfter(10);

                int totalRows = 2;
                foreach (var t in vm.Themes)
                {
                    if (t.ThemeName.StartsWith("Всего", StringComparison.OrdinalIgnoreCase)) continue;
                    if (t.IsSection) totalRows++;
                    else if (t.ThemeName.StartsWith("Самостоятельная", StringComparison.OrdinalIgnoreCase) || t.ThemeName.StartsWith("Промежуточная", StringComparison.OrdinalIgnoreCase))
                        totalRows += Math.Max(1, t.ContentBlocks.Count);
                    else totalRows += 1 + Math.Max(1, t.ContentBlocks.Count);
                }
                totalRows++;

                var tablePlan = document.AddTable(totalRows, 4);
                tablePlan.Design = TableDesign.TableGrid;
                tablePlan.Alignment = Alignment.left;
                tablePlan.SetWidths(new float[] { 178f, 393f, 58f, 85f }, true);

                // Заголовки строго по шаблону
                tablePlan.Rows[0].Cells[0].Paragraphs.First().Append("Наименование разделов и тем профессионального модуля (ПМ), междисциплинарных курсов (МДК)").FontSize(12).Bold().Alignment = Alignment.center;
                tablePlan.Rows[0].Cells[1].Paragraphs.First().Append("Содержание учебного материала, лабораторные работы и практические занятия, самостоятельная учебная работа обучающихся, курсовая работа (проект)").FontSize(12).Bold().Alignment = Alignment.center;
                tablePlan.Rows[0].Cells[2].Paragraphs.First().Append("Объем часов").FontSize(12).Bold().Alignment = Alignment.center;
                tablePlan.Rows[0].Cells[3].Paragraphs.First().Append("ПК").FontSize(12).Bold().Alignment = Alignment.center;

                tablePlan.Rows[1].Cells[0].Paragraphs.First().Append("1").FontSize(12).Bold().Alignment = Alignment.center;
                tablePlan.Rows[1].Cells[1].Paragraphs.First().Append("2").FontSize(12).Bold().Alignment = Alignment.center;
                tablePlan.Rows[1].Cells[2].Paragraphs.First().Append("3").FontSize(12).Bold().Alignment = Alignment.center;
                tablePlan.Rows[1].Cells[3].Paragraphs.First().Append("4").FontSize(12).Bold().Alignment = Alignment.center;

                int rowIndex = 2;
                foreach (var theme in vm.Themes)
                {
                    if (theme.ThemeName.StartsWith("Всего", StringComparison.OrdinalIgnoreCase)) continue;

                    if (theme.IsSection)
                    {
                        var row = tablePlan.Rows[rowIndex];
                        row.MergeCells(0, 3);
                        row.Cells[0].Paragraphs.First().Append(theme.ThemeName).FontSize(12).Bold().Alignment = Alignment.left;
                        rowIndex++;
                        continue;
                    }

                    if (theme.ThemeName.StartsWith("Самостоятельная", StringComparison.OrdinalIgnoreCase) || theme.ThemeName.StartsWith("Промежуточная", StringComparison.OrdinalIgnoreCase))
                    {
                        int blockCount = Math.Max(1, theme.ContentBlocks.Count);
                        int themeStartRow = rowIndex;

                        for (int i = 0; i < blockCount; i++)
                        {
                            var row = tablePlan.Rows[themeStartRow + i];
                            row.BreakAcrossPages = false;
                            row.MergeCells(0, 1);
                        }

                        var row0 = tablePlan.Rows[themeStartRow];
                        var contentPara = row0.Cells[0].Paragraphs.First();
                        contentPara.Append(theme.ThemeName).FontSize(12).Bold();
                        contentPara.AppendLine();

                        var hoursPara = row0.Cells[1].Paragraphs.First();
                        hoursPara.Alignment = Alignment.center;
                        if (!string.IsNullOrEmpty(theme.TotalHours) && theme.TotalHours != "0") hoursPara.Append(theme.TotalHours).FontSize(12).Bold();

                        row0.Cells[2].Paragraphs.First().Append(theme.Competences).FontSize(12).Alignment = Alignment.center;

                        foreach (var block in theme.ContentBlocks)
                        {
                            var lines = block.Description.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var line in lines) { contentPara.Append(line.Trim()).FontSize(12); contentPara.AppendLine(); }
                            if (!string.IsNullOrEmpty(block.Hours) && block.Hours != "0") { hoursPara.AppendLine(); hoursPara.Append(block.Hours).FontSize(12); }
                        }

                        if (blockCount > 1)
                        {
                            tablePlan.MergeCellsInColumn(0, themeStartRow, themeStartRow + blockCount - 1);
                            tablePlan.MergeCellsInColumn(1, themeStartRow, themeStartRow + blockCount - 1);
                            tablePlan.MergeCellsInColumn(2, themeStartRow, themeStartRow + blockCount - 1);
                        }
                        rowIndex += blockCount;
                        continue;
                    }

                    // --- ОБЫЧНАЯ ТЕМА ---
                    int themeTotalHours = theme.ContentBlocks.Sum(b => int.TryParse(b.Hours, out int h) ? h : 0);
                    if (themeTotalHours == 0 && int.TryParse(theme.TotalHours, out int th2)) themeTotalHours = th2;

                    int themeStartRowNormal = rowIndex;
                    int themeRowCountNormal = 1;

                    tablePlan.Rows[rowIndex].BreakAcrossPages = false;
                    tablePlan.Rows[rowIndex].Cells[0].Paragraphs.First().Append(theme.ThemeName).FontSize(12).Bold();
                    tablePlan.Rows[rowIndex].Cells[1].Paragraphs.First().Append("Содержание учебного материала").FontSize(12).Bold();
                    tablePlan.Rows[rowIndex].Cells[2].Paragraphs.First().Append(themeTotalHours > 0 ? themeTotalHours.ToString() : "").FontSize(12).Bold().Alignment = Alignment.center;
                    tablePlan.Rows[rowIndex].Cells[3].Paragraphs.First().Append(theme.Competences).FontSize(12).Alignment = Alignment.center;
                    rowIndex++;

                    if (theme.ContentBlocks.Count > 0)
                    {
                        foreach (var block in theme.ContentBlocks)
                        {
                            tablePlan.Rows[rowIndex].BreakAcrossPages = false;
                            var cellPara = tablePlan.Rows[rowIndex].Cells[1].Paragraphs.First();
                            var lines = block.Description.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < lines.Length; j++)
                            {
                                string lineText = lines[j].Trim();
                                var run = cellPara.Append(lineText).FontSize(12);
                                if (j < lines.Length - 1) cellPara.AppendLine();
                            }
                            tablePlan.Rows[rowIndex].Cells[2].Paragraphs.First().Append(block.Hours).FontSize(12).Alignment = Alignment.center;
                            rowIndex++;
                            themeRowCountNormal++;
                        }
                    }

                    if (themeRowCountNormal > 1)
                    {
                        tablePlan.MergeCellsInColumn(0, themeStartRowNormal, themeStartRowNormal + themeRowCountNormal - 1);
                        tablePlan.MergeCellsInColumn(3, themeStartRowNormal, themeStartRowNormal + themeRowCountNormal - 1);
                    }
                }

                var totalRow = tablePlan.Rows[rowIndex];
                totalRow.MergeCells(0, 1);
                totalRow.Cells[0].Paragraphs.First().Append("Всего:").FontSize(12).Bold().Alignment = Alignment.left;

                int grandTotal = 0;
                foreach (var t in vm.Themes)
                {
                    if (t.IsSection || t.ThemeName.StartsWith("Всего", StringComparison.OrdinalIgnoreCase)) continue;
                    int themeTotal = t.ContentBlocks.Sum(b => int.TryParse(b.Hours, out int h) ? h : 0);
                    if (themeTotal == 0 && int.TryParse(t.TotalHours, out int th)) themeTotal = th;
                    grandTotal += themeTotal;
                }

                string totalToShow = string.IsNullOrEmpty(vm.Module21.TotalHours) || vm.Module21.TotalHours == "-" ? grandTotal.ToString() : vm.Module21.TotalHours;
                totalRow.Cells[1].Paragraphs.First().Append(totalToShow).FontSize(12).Bold().Alignment = Alignment.center;

                document.InsertTable(tablePlan);

                // ==========================================
                // СТРАНИЦА 7: РАЗДЕЛ 3 (Возврат в книжную)
                // ==========================================
                document.InsertSectionPageBreak();
                var portraitSection = document.Sections[document.Sections.Count - 1];
                portraitSection.PageLayout.Orientation = Orientation.Portrait;

                var h3_1 = document.InsertParagraph("3 УСЛОВИЯ РЕАЛИЗАЦИИ ПРОФЕССИОНАЛЬНОГО МОДУЛЯ");
                h3_1.StyleName = "Heading1";
                h3_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f;

                var h3_1_1 = document.InsertParagraph("3.1 Требования к минимальному материально-техническому обеспечению");
                h3_1_1.StyleName = "Heading2";
                h3_1_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f;

                var pTreb = document.InsertParagraph(vm.TrebKab);

                pTreb.FontSize(12).IndentationFirstLine = 35.4f;
                pTreb.IndentationHanging= 35.4f;
             

                var h3_3 = document.InsertParagraph("3.2 Учебно-методическое и информационное обеспечение");
                h3_3.StyleName = "Heading2";
                h3_3.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black);
                h3_3.IndentationFirstLine = 35.4f;

                document.InsertParagraph("Для реализации программы в образовательной организации имеют печатные и/или электронные образовательные и информационные ресурсы:").FontSize(12).IndentationFirstLine = 35.4f;
                document.InsertParagraph();
                document.InsertParagraph("Основные источники:").FontSize(12).Bold().IndentationFirstLine = 35.4f;

                int libIndex = 1;
                foreach (var lib in vm.OsnovLibrary)
                {
                    var pLib = document.InsertParagraph($"{libIndex}. {lib.Record}");
                    pLib.FontSize(12);
                    pLib.Alignment = Alignment.both;
                    pLib.IndentationFirstLine = 35.4f;
                    libIndex++;
                }

                document.InsertSectionPageBreak();
                if (vm.DopLibrary.Count > 0)
                {
                    document.InsertParagraph("Дополнительные источники:").FontSize(12).Bold().SpacingBefore(10).IndentationFirstLine = 35.4f; ;
                    int dopIndex = 1;
                    foreach (var lib in vm.DopLibrary)
                    {
                        var pLib = document.InsertParagraph($"{dopIndex}. {lib.Record}");
                        pLib.FontSize(12);
                        pLib.Alignment = Alignment.both;
                        pLib.IndentationFirstLine = 35.4f;
                        dopIndex++;
                    }
                }

                if (vm.Softwares != null && vm.Softwares.Count > 0)
                {
                    document.InsertParagraph("Перечень используемого программного обеспечения:").FontSize(12).Bold().SpacingBefore(15).IndentationFirstLine = 35.4f; ;
                    document.InsertParagraph();
                    var tableSoft = document.AddTable(vm.Softwares.Count + 1, 2);
                    tableSoft.Design = TableDesign.TableGrid;
                    tableSoft.Alignment = Alignment.left;
                    tableSoft.SetWidths(new float[] { 187f, 281f }, true);

                    tableSoft.Rows[0].Cells[0].Paragraphs.First().Append("Наименование").FontSize(12).Bold().Alignment = Alignment.center;
                    tableSoft.Rows[0].Cells[1].Paragraphs.First().Append("Описание").FontSize(12).Bold().Alignment = Alignment.center;
                    for (int i = 0; i < vm.Softwares.Count; i++)
                    {
                        tableSoft.Rows[i + 1].Cells[0].Paragraphs.First().Append(vm.Softwares[i].Name).FontSize(12);
                        tableSoft.Rows[i + 1].Cells[1].Paragraphs.First().Append(vm.Softwares[i].LicenseType).FontSize(12);
                    }
                    document.InsertTable(tableSoft);
                }

                document.InsertParagraph().InsertPageBreakAfterSelf();

                // ==========================================
                // СТРАНИЦА 8: РАЗДЕЛ 4
                // ==========================================
                var h4_1 = document.InsertParagraph("4 КОНТРОЛЬ И ОЦЕНКА РЕЗУЛЬТАТОВ ОСВОЕНИЯ ПРОФЕССИОНАЛЬНОГО МОДУЛЯ");
                h4_1.StyleName = "Heading1";
                h4_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f;
                h4_1.Alignment = Alignment.left;

                var tableEval = document.AddTable(vm.Evaluations.Count + 1, 3);
                tableEval.Design = TableDesign.TableGrid;
                tableEval.Alignment = Alignment.left;
                tableEval.SetWidths(new float[] { 140f, 234f, 94f }, true);

                tableEval.Rows[0].Cells[0].Paragraphs.First().Append("Результаты обучения").FontSize(12).Bold().Alignment = Alignment.center;
                tableEval.Rows[0].Cells[1].Paragraphs.First().Append("Показатели освоенности компетенций").FontSize(12).Bold().Alignment = Alignment.center;
                tableEval.Rows[0].Cells[2].Paragraphs.First().Append("Методы оценки").FontSize(12).Bold().Alignment = Alignment.center;

                for (int i = 0; i < vm.Evaluations.Count; i++)
                {
                    tableEval.Rows[i + 1].BreakAcrossPages = false;
                    tableEval.Rows[i + 1].Cells[0].Paragraphs.First().Append(vm.Evaluations[i].LearningOutcome).FontSize(12);
                    tableEval.Rows[i + 1].Cells[1].Paragraphs.First().Append(vm.Evaluations[i].Indicators).FontSize(12);
                    tableEval.Rows[i + 1].Cells[2].Paragraphs.First().Append(vm.Evaluations[i].AssessmentMethod).FontSize(12).Alignment = Alignment.center;
                }
                document.InsertTable(tableEval);

                // Сохраняем файл
                document.Save();
            }
        }
    }
}