using System;
using System.Linq;
using System.Collections.Generic;
using Xceed.Document.NET;
using Xceed.Words.NET;
using RsskWorkTeacher.Models.RP;

namespace RsskWorkTeacher.Services
{
    public class WordExporter
    {
        public static void ExportToWord(WorkingProgramViewModel vm, string filePath)
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
                document.InsertParagraph("учебной дисциплины").FontSize(12).Alignment = Alignment.center;
                document.InsertParagraph().SpacingAfter(20);

                // --- ВЫВОД КОДА И НАЗВАНИЯ ДИСЦИПЛИНЫ (УБРАН .Bold()) ---
                string fullDisciplineTitle = string.IsNullOrWhiteSpace(vm.DisciplineKod)
                    ? vm.DisciplineName.ToUpper()
                    : $"{vm.DisciplineKod} {vm.DisciplineName}".ToUpper();

                document.InsertParagraph(fullDisciplineTitle).FontSize(12).Alignment = Alignment.center;

                document.InsertParagraph();
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
                var h1_1 = document.InsertParagraph("1 ОБЩАЯ ХАРАКТЕРИСТИКА РАБОЧЕЙ ПРОГРАММЫ УЧЕБНОЙ ДИСЦИПЛИНЫ");
                h1_1.StyleName = "Heading1";
                h1_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).IndentationFirstLine = 35.4f;

                var h1_2 = document.InsertParagraph("1.1 Цель и место учебной дисциплины в структуре образовательной программы");
                h1_2.StyleName = "Heading2";
                h1_2.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).IndentationFirstLine = 35.4f; ;

                var pPurpose = document.InsertParagraph(vm.Purpose);
                pPurpose.FontSize(12);
                pPurpose.Alignment = Alignment.both;
                pPurpose.IndentationFirstLine = 35.4f;

                document.InsertParagraph("Компетенции, раскрываемые в дисциплине:").FontSize(12).IndentationFirstLine = 35.4f; ;
                string[] comps = {
                    "ОК 01 Выбирать способы решения задач профессиональной деятельности применительно к различным контекстам",
                    "ОК 02 Использовать современные средства поиска, анализа и интерпретации информации, и информационные технологии для выполнения задач профессиональной деятельности",
                    "ОК 04 Эффективно взаимодействовать и работать в коллективе и команде",
                    "ОК 05 Осуществлять устную и письменную коммуникацию на государственном языке Российской Федерации с учетом особенностей социального и культурного контекста",
                    "ОК 09 Пользоваться профессиональной документацией на государственном и иностранном языках"
                };
                foreach (var c in comps)
                {
                    var pComp = document.InsertParagraph(c);
                    pComp.FontSize(12);
                    pComp.IndentationFirstLine = 35.4f;
                }
                document.InsertParagraph();
                var h1_3 = document.InsertParagraph("1.2 Планируемые результаты освоения дисциплины");
                h1_3.StyleName = "Heading2";
                h1_3.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).IndentationFirstLine = 35.4f; ;

                var pMatrix = document.InsertParagraph("Результаты освоения дисциплины соотносятся с планируемыми результатами освоения образовательной программы, представленными в матрице компетенций выпускника (п. 4.3 ПОП).");
                pMatrix.FontSize(12);
                pMatrix.IndentationFirstLine = 35.4f;

                var pResult = document.InsertParagraph("В результате освоения дисциплины обучающийся должен:");
                pResult.FontSize(12);
                pResult.IndentationFirstLine = 35.4f;

                var tableComp = document.AddTable(vm.Competencies.Count + 1, 3);
                tableComp.Design = TableDesign.TableGrid;
                tableComp.Alignment = Alignment.left;
                tableComp.SetWidths(new float[] { 80f, 210f, 188f }, true);

                tableComp.Rows[0].Cells[0].Paragraphs.First().Append("Код ОК, ПК").FontSize(12).Bold().Alignment = Alignment.center;
                tableComp.Rows[0].Cells[1].Paragraphs.First().Append("Уметь").FontSize(12).Bold().Alignment = Alignment.center;
                tableComp.Rows[0].Cells[2].Paragraphs.First().Append("Знать").FontSize(12).Bold().Alignment = Alignment.center;

                for (int i = 0; i < vm.Competencies.Count; i++)
                {
                    tableComp.Rows[i + 1].BreakAcrossPages = false;
                    tableComp.Rows[i + 1].Cells[0].Paragraphs.First().Append(vm.Competencies[i].Code).FontSize(12);
                    tableComp.Rows[i + 1].Cells[1].Paragraphs.First().Append(vm.Competencies[i].KnowHow).FontSize(12);
                    tableComp.Rows[i + 1].Cells[2].Paragraphs.First().Append(vm.Competencies[i].Knowledge).FontSize(12);
                }
                document.InsertTable(tableComp);
                document.InsertParagraph().InsertPageBreakAfterSelf();

                // ==========================================
                // СТРАНИЦА 5: РАЗДЕЛ 2.1 (Объем)
                // ==========================================
                var h2_1 = document.InsertParagraph("2 СТРУКТУРА И СОДЕРЖАНИЕ УЧЕБНОЙ ДИСЦИПЛИНЫ");
                h2_1.StyleName = "Heading1";
                h2_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).IndentationFirstLine = 35.4f; ;

                var h2_2 = document.InsertParagraph("2.1 Объем учебной дисциплины и виды учебной работы");
                h2_2.StyleName = "Heading2";
                h2_2.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f; ;

                var tableVol = document.AddTable(9, 3);
                tableVol.Design = TableDesign.TableGrid;
                tableVol.Alignment = Alignment.left;
                tableVol.SetWidths(new float[] { 234f, 93f, 141f }, true);

                tableVol.Rows[0].Cells[0].Paragraphs.First().Append("Вид учебной работы").FontSize(12).Bold().Alignment = Alignment.center;
                tableVol.Rows[0].Cells[1].Paragraphs.First().Append("Объем в часах").FontSize(12).Bold().Alignment = Alignment.center;
                tableVol.Rows[0].Cells[2].Paragraphs.First().Append("В т.ч. в форме практической подготовки").FontSize(12).Bold().Alignment = Alignment.center;

                tableVol.Rows[1].Cells[0].Paragraphs.First().Append("Объем рабочей программы дисциплины").FontSize(12);
                tableVol.Rows[1].Cells[1].Paragraphs.First().Append(vm.ChasRabProg1).FontSize(12).Alignment = Alignment.center;
                tableVol.Rows[1].Cells[2].Paragraphs.First().Append(vm.ChasRabProg2).FontSize(12).Alignment = Alignment.center;

                tableVol.Rows[2].Cells[0].Paragraphs.First().Append("Основное содержание").FontSize(12);
                tableVol.Rows[2].Cells[1].Paragraphs.First().Append(vm.ChasOsnov).FontSize(12).Alignment = Alignment.center;

                tableVol.Rows[3].Cells[0].Paragraphs.First().Append("в том числе:").FontSize(12).Bold();

                var pTeor = tableVol.Rows[4].Cells[0].Paragraphs.First().Append("теоретическое обучение").FontSize(12);
                pTeor.IndentationBefore = 10f;
                tableVol.Rows[4].Cells[1].Paragraphs.First().Append(vm.TeaorChasOsnov).FontSize(12).Alignment = Alignment.center;

                var pPract = tableVol.Rows[5].Cells[0].Paragraphs.First().Append("практические занятия").FontSize(12);
                pPract.IndentationBefore = 10f;
                tableVol.Rows[5].Cells[1].Paragraphs.First().Append(vm.PractChas1).FontSize(12).Alignment = Alignment.center;
                tableVol.Rows[5].Cells[2].Paragraphs.First().Append(vm.PractChas2).FontSize(12).Alignment = Alignment.center;

                var pKurs = tableVol.Rows[6].Cells[0].Paragraphs.First().Append("курсовая работа/проект").FontSize(12);
                pKurs.IndentationBefore = 10f;
                tableVol.Rows[6].Cells[1].Paragraphs.First().Append(vm.KursachChas).FontSize(12).Alignment = Alignment.center;

                tableVol.Rows[7].Cells[0].Paragraphs.First().Append("Самостоятельная работа").FontSize(12).Bold();
                tableVol.Rows[7].Cells[1].Paragraphs.First().Append(vm.SamRabChas).FontSize(12).Alignment = Alignment.center;

                tableVol.Rows[8].Cells[0].Paragraphs.First().Append("Промежуточная аттестация").FontSize(12).Bold();
                tableVol.Rows[8].Cells[1].Paragraphs.First().Append(vm.AtesChas).FontSize(12).Alignment = Alignment.center;

                document.InsertTable(tableVol);

                // ==========================================
                // СТРАНИЦА 6: РАЗДЕЛ 2.2 (Альбомная ориентация)
                // ==========================================
                document.InsertSectionPageBreak();
                var landscapeSection = document.Sections[document.Sections.Count - 1];
                landscapeSection.PageLayout.Orientation = Orientation.Landscape;

                var h2_3 = document.InsertParagraph("2.2 Тематический план и содержание учебной дисциплины");
                h2_3.StyleName = "Heading2";
                h2_3.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).IndentationFirstLine = 35.4f; ;
                h2_3.SpacingAfter(6);

                // Считаем строки: 1 (заголовок) + 1 (номера) + темы + 1 (для итоговой строки "Всего")
                int totalRows = 2;
                foreach (var t in vm.Themes)
                {
                    if (t.ThemeName.StartsWith("Всего", StringComparison.OrdinalIgnoreCase)) continue;

                    if (t.IsSection)
                    {
                        totalRows++;
                    }
                    else if (t.ThemeName.StartsWith("Самостоятельная работа", StringComparison.OrdinalIgnoreCase) ||
                             t.ThemeName.StartsWith("Промежуточная аттестация", StringComparison.OrdinalIgnoreCase))
                    {
                        totalRows += Math.Max(1, t.ContentBlocks.Count);
                    }
                    else
                    {
                        totalRows++;
                        totalRows += Math.Max(1, t.ContentBlocks.Count);
                    }
                }
                totalRows++; // Для строки "Всего"

                var tablePlan = document.AddTable(totalRows, 4);
                tablePlan.Design = TableDesign.TableGrid;
                tablePlan.Alignment = Alignment.left;
                tablePlan.SetWidths(new float[] { 178f, 393f, 58f, 85f }, true);

                // Строка 0: Заголовки
                tablePlan.Rows[0].Cells[0].Paragraphs.First().Append("Наименование разделов и тем").FontSize(11).Bold().Alignment = Alignment.center;
                tablePlan.Rows[0].Cells[1].Paragraphs.First().Append("Содержание учебного материала и формы организации деятельности обучающихся").FontSize(11).Bold().Alignment = Alignment.center;
                tablePlan.Rows[0].Cells[2].Paragraphs.First().Append("Объём часов").FontSize(11).Bold().Alignment = Alignment.center;
                tablePlan.Rows[0].Cells[3].Paragraphs.First().Append("Формируемые компетенции").FontSize(11).Bold().Alignment = Alignment.center;

                // Строка 1: Номера столбцов
                tablePlan.Rows[1].Cells[0].Paragraphs.First().Append("1").FontSize(11).Bold().Alignment = Alignment.center;
                tablePlan.Rows[1].Cells[1].Paragraphs.First().Append("2").FontSize(11).Bold().Alignment = Alignment.center;
                tablePlan.Rows[1].Cells[2].Paragraphs.First().Append("3").FontSize(11).Bold().Alignment = Alignment.center;
                tablePlan.Rows[1].Cells[3].Paragraphs.First().Append("4").FontSize(11).Bold().Alignment = Alignment.center;

                int rowIndex = 2;
                foreach (var theme in vm.Themes)
                {
                    if (theme.ThemeName.StartsWith("Всего", StringComparison.OrdinalIgnoreCase)) continue;

                    if (theme.IsSection)
                    {
                        var row = tablePlan.Rows[rowIndex];
                        row.MergeCells(0, 3);
                        row.Cells[0].Paragraphs.First().Append(theme.ThemeName).FontSize(11).Bold().Alignment = Alignment.left;
                        rowIndex++;
                        continue;
                    }

                    // --- СПЕЦИАЛЬНЫЕ БЛОКИ ---
                    if (theme.ThemeName.StartsWith("Самостоятельная работа", StringComparison.OrdinalIgnoreCase) ||
                        theme.ThemeName.StartsWith("Промежуточная аттестация", StringComparison.OrdinalIgnoreCase))
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
                        contentPara.Append(theme.ThemeName).FontSize(11).Bold();
                        contentPara.AppendLine();

                        var hoursPara = row0.Cells[1].Paragraphs.First();
                        hoursPara.Alignment = Alignment.center;

                        if (theme.ThemeName.StartsWith("Промежуточная", StringComparison.OrdinalIgnoreCase))
                            hoursPara.AppendLine();

                        if (!string.IsNullOrEmpty(theme.TotalHours) && theme.TotalHours != "0")
                            hoursPara.Append(theme.TotalHours).FontSize(11).Bold();

                        row0.Cells[2].Paragraphs.First().Append(theme.Competences).FontSize(11).Alignment = Alignment.center;

                        foreach (var block in theme.ContentBlocks)
                        {
                            var lines = block.Description.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            if (lines.Length == 0) contentPara.AppendLine();
                            else
                            {
                                foreach (var line in lines)
                                {
                                    contentPara.Append(line.Trim()).FontSize(11);
                                    contentPara.AppendLine();
                                }
                            }

                            if (!string.IsNullOrEmpty(block.Hours) && block.Hours != "0")
                            {
                                hoursPara.AppendLine();
                                hoursPara.Append(block.Hours).FontSize(11);
                            }
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
                    tablePlan.Rows[rowIndex].Cells[0].Paragraphs.First().Append(theme.ThemeName).FontSize(11).Bold();
                    tablePlan.Rows[rowIndex].Cells[1].Paragraphs.First().Append("Содержание учебного материала").FontSize(11).Bold();
                    tablePlan.Rows[rowIndex].Cells[2].Paragraphs.First().Append(themeTotalHours > 0 ? themeTotalHours.ToString() : "").FontSize(11).Bold().Alignment = Alignment.center;
                    tablePlan.Rows[rowIndex].Cells[3].Paragraphs.First().Append(theme.Competences).FontSize(11).Alignment = Alignment.center;
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
                                var run = cellPara.Append(lineText).FontSize(11);
                                if (j == 0 && (lineText.Contains("Самостоятельная работа"))) run.Bold();
                                if (j < lines.Length - 1) cellPara.AppendLine();
                            }

                            tablePlan.Rows[rowIndex].Cells[2].Paragraphs.First().Append(block.Hours).FontSize(11).Alignment = Alignment.center;

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

                // ==========================================
                // ДОБАВЛЯЕМ ИТОГОВУЮ СТРОКУ "ВСЕГО" (ПО ЛЕВОМУ КРАЮ)
                // ==========================================
                var totalRow = tablePlan.Rows[rowIndex];
                totalRow.MergeCells(0, 1);
                totalRow.Cells[0].Paragraphs.First().Append("Всего:").FontSize(11).Bold().Alignment = Alignment.left;

                int grandTotal = 0;
                foreach (var t in vm.Themes)
                {
                    if (t.IsSection || t.ThemeName.StartsWith("Всего", StringComparison.OrdinalIgnoreCase)) continue;
                    int themeTotal = t.ContentBlocks.Sum(b => int.TryParse(b.Hours, out int h) ? h : 0);
                    if (themeTotal == 0 && int.TryParse(t.TotalHours, out int th)) themeTotal = th;
                    grandTotal += themeTotal;
                }

                string totalToShow = string.IsNullOrEmpty(vm.ChasRabProg1) ? grandTotal.ToString() : vm.ChasRabProg1;
                totalRow.Cells[1].Paragraphs.First().Append(totalToShow).FontSize(11).Bold().Alignment = Alignment.center;

                document.InsertTable(tablePlan);

                // ==========================================
                // СТРАНИЦА 7: РАЗДЕЛ 3 (Возврат в книжную)
                // ==========================================
                document.InsertSectionPageBreak();
                var portraitSection = document.Sections[document.Sections.Count - 1];
                portraitSection.PageLayout.Orientation = Orientation.Portrait;

                var h3_1 = document.InsertParagraph("3 УСЛОВИЯ РЕАЛИЗАЦИИ УЧЕБНОЙ ДИСЦИПЛИНЫ");
                h3_1.StyleName = "Heading1";
                h3_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).IndentationFirstLine = 35.4f; 
                var h3_2 = document.InsertParagraph("3.1 Требования к минимальному материально-техническому обеспечению");
                h3_2.StyleName = "Heading2";
                h3_2.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).SpacingAfter(6).IndentationFirstLine = 35.4f; ;

                var pTreb = document.InsertParagraph(vm.TrebKab);
                pTreb.FontSize(12);
                pTreb.Alignment = Alignment.both;
                pTreb.IndentationFirstLine = 35.4f;

                var h3_3 = document.InsertParagraph("3.2 Учебно-методическое и информационное обеспечение");
                h3_3.StyleName = "Heading2";
                h3_3.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).IndentationFirstLine = 35.4f;
                document.InsertParagraph("Для реализации программы в образовательной организации имеют печатные и/или электронные образовательные и информационные ресурсы:");
                document.InsertParagraph();
                document.InsertParagraph("Основная литература:").FontSize(12).Bold().IndentationFirstLine = 35.4f;

                int libIndex = 1;
                foreach (var lib in vm.OsnovLibrary)
                {
                    var pLib = document.InsertParagraph($"{libIndex}. {lib.Record}");
                    pLib.FontSize(12);
                    pLib.Alignment = Alignment.both;
                    pLib.IndentationFirstLine = 35.4f;
                    libIndex++;
                }

                if (vm.DopLibrary.Count > 0)
                {
                    document.InsertParagraph("Дополнительная литература:").FontSize(12).Bold().SpacingBefore(10).IndentationFirstLine = 35.4f;
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
                    document.InsertParagraph("Перечень используемого программного обеспечения").FontSize(12).Bold().IndentationFirstLine = 35.4f;
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
                var h4_1 = document.InsertParagraph("4 КОНТРОЛЬ И ОЦЕНКА РЕЗУЛЬТАТОВ ОСВОЕНИЯ УЧЕБНОЙ ДИСЦИПЛИНЫ");
                h4_1.StyleName = "Heading1";
                h4_1.Font("Times New Roman").FontSize(12).Bold().Color(Xceed.Drawing.Color.Black).IndentationFirstLine = 35.4f;
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