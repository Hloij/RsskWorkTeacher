using RsskWorkTeacher.Models;
using RsskWorkTeacher.Models.RP;
using System;
using System.Collections.Generic;
using System.Linq;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace RsskWorkTeacher.Services
{
    public class WordKtpExporter
    {
        public static void ExportKtpToWord(WorkingProgramViewModel vm,KtpData ktp, string filePath)
        {
            
            if (ktp == null) ktp = new KtpData();

            using (var document = DocX.Create(filePath))
            {
                // 1. Настройка полей
                document.MarginLeft = 85f;    // 3 см
                document.MarginRight = 42.5f; // 1.5 см
                document.MarginTop = 56.7f;   // 2 см
                document.MarginBottom = 56.7f;// 2 см
                document.SetDefaultFont(new Xceed.Document.NET.Font("Times New Roman"), 12d, Xceed.Drawing.Color.Black);

                // ==========================================
                // СТРАНИЦА 1: ТИТУЛ
                // ==========================================
                document.InsertParagraph("РЯЗАНСКИЙ КОЛЛЕДЖ РГРТУ").FontSize(14).Bold().Alignment = Alignment.center;
      

                // ==========================================
                // СТРАНИЦА 2: УТВЕРЖДЕНИЕ
                // ==========================================
                var tableApproval = document.AddTable(1, 2);
                tableApproval.Design = TableDesign.None;
                tableApproval.SetWidths(new float[] { 250f, 250f }, true);

                // Левая колонка
                var cellLeft = tableApproval.Rows[0].Cells[0];
                cellLeft.Paragraphs[0].Append("РЕКОМЕНДОВАНО").FontSize(12);
                cellLeft.InsertParagraph("к применению цикловой комиссией").FontSize(12);
                cellLeft.InsertParagraph("___________________________").FontSize(12);
                cellLeft.InsertParagraph("Протокол от ___________ № ___").FontSize(12);
                cellLeft.InsertParagraph("\nПредседатель ЦК _________________").FontSize(12);

                // Правая колонка
                var cellRight = tableApproval.Rows[0].Cells[1];
                cellRight.Paragraphs[0].Append("УТВЕРЖДАЮ").FontSize(12);
                cellRight.InsertParagraph("Зам. директора по учебной работе").FontSize(12);
                cellRight.InsertParagraph("").FontSize(12);
                cellRight.InsertParagraph("«___» ____________ 20__ г.").FontSize(12);
                cellRight.InsertParagraph("\n\n").FontSize(12);

                document.InsertTable(tableApproval);
                

                // ==========================================
                // СТРАНИЦА 3: ЗАГОЛОВОК
                // ==========================================
                document.InsertParagraph("КАЛЕНДАРНО-ТЕМАТИЧЕСКИЙ ПЛАН").FontSize(16).Bold().Alignment = Alignment.center;
                document.InsertParagraph().SpacingAfter(15);

                // ==========================================
                // СТРАНИЦА 3.1: ИНФОРМАЦИЯ О ДИСЦИПЛИНЕ
                // ==========================================
                var tableInfo = document.AddTable(5, 2);
                tableInfo.Design = TableDesign.None;
                tableInfo.SetWidths(new float[] { 160f, 340f }, true);

                tableInfo.Rows[0].Cells[0].Paragraphs.First().Append("1. По дисциплине:").FontSize(12).Bold();
                tableInfo.Rows[0].Cells[1].Paragraphs.First().Append($"«{vm.DisciplineName}»").FontSize(12);

                tableInfo.Rows[1].Cells[0].Paragraphs.First().Append("2. Учебный период:").FontSize(12).Bold();
                tableInfo.Rows[1].Cells[1].Paragraphs.First().Append(ktp.Period).FontSize(12);

                tableInfo.Rows[2].Cells[0].Paragraphs.First().Append("3. Группы:").FontSize(12).Bold();
                tableInfo.Rows[2].Cells[1].Paragraphs.First().Append(ktp.Groups).FontSize(12);

                tableInfo.Rows[3].Cells[0].Paragraphs.First().Append("4. Преподаватели:").FontSize(12).Bold();
                tableInfo.Rows[3].Cells[1].Paragraphs.First().Append(ktp.Teachers).FontSize(12);

                tableInfo.Rows[4].Cells[0].Paragraphs.First().Append("5. Составлен в соответствии с РП, утвержденной").FontSize(12);
                string dateStr = vm.ProtocolDate.HasValue ? vm.ProtocolDate.Value.ToString("dd.MM.yyyy") : "31.07.2024";
                tableInfo.Rows[4].Cells[1].Paragraphs.First().Append($"{dateStr} РГРТУ").FontSize(12);

                document.InsertTable(tableInfo);
                document.InsertParagraph().SpacingAfter(5);

                // ==========================================
                // СТРАНИЦА 3.2: ТАБЛИЦА С ЧАСАМИ
                // ==========================================
                var tableHours = document.AddTable(12, 4);
                tableHours.Design = TableDesign.TableGrid;
                tableHours.SetWidths(new float[] { 230f, 70f, 70f, 70f }, true);

                // Заголовки
                tableHours.Rows[0].Cells[0].Paragraphs.First().Append("Количество часов за период").Bold().Alignment = Alignment.center;
                tableHours.Rows[0].Cells[1].Paragraphs.First().Append("Уч. год").Bold().Alignment = Alignment.center;
                tableHours.Rows[0].Cells[2].Paragraphs.First().Append("5 семестр").Bold().Alignment = Alignment.center;
                tableHours.Rows[0].Cells[3].Paragraphs.First().Append("6 семестр").Bold().Alignment = Alignment.center;

                // Данные
                tableHours.Rows[1].Cells[0].Paragraphs.First().Append("Объем учебной дисциплины:").Bold();
                tableHours.Rows[1].Cells[1].Paragraphs.First().Append(vm.ChasRabProg1).Bold().Alignment = Alignment.center;

                tableHours.Rows[2].Cells[0].Paragraphs.First().Append("Объем работы обучающихся во взаимодействии с преподавателем").Bold();
                tableHours.Rows[2].Cells[1].Paragraphs.First().Append(vm.ChasRabProg1).Bold().Alignment = Alignment.center;

                tableHours.Rows[3].Cells[0].Paragraphs.First().Append("в том числе:").Bold();
                tableHours.Rows[3].Cells[1].Paragraphs.First().Append("-").Alignment = Alignment.center;

                tableHours.Rows[4].Cells[0].Paragraphs.First().Append("- учебные занятия (без консультаций), из них:");
                tableHours.Rows[4].Cells[1].Paragraphs.First().Append(vm.ChasOsnov).Alignment = Alignment.center;

                tableHours.Rows[5].Cells[0].Paragraphs.First().Append("- лекции");
                tableHours.Rows[5].Cells[1].Paragraphs.First().Append(vm.TeaorChasOsnov).Alignment = Alignment.center;

                tableHours.Rows[6].Cells[0].Paragraphs.First().Append("- практические занятия");
                tableHours.Rows[6].Cells[1].Paragraphs.First().Append(vm.PractChas1).Alignment = Alignment.center;

                tableHours.Rows[7].Cells[0].Paragraphs.First().Append("- самостоятельная работа");
                tableHours.Rows[7].Cells[1].Paragraphs.First().Append(vm.SamRabChas).Alignment = Alignment.center;

                tableHours.Rows[8].Cells[0].Paragraphs.First().Append("Консультации:").Bold();
                tableHours.Rows[8].Cells[1].Paragraphs.First().Append("-").Alignment = Alignment.center;

                tableHours.Rows[9].Cells[0].Paragraphs.First().Append("Промежуточная аттестация").Bold();
                tableHours.Rows[9].Cells[1].Paragraphs.First().Append(vm.AtesChas).Alignment = Alignment.center;

                tableHours.Rows[10].Cells[0].Paragraphs.First().Append("Формы промежуточной аттестации");
                tableHours.Rows[10].Cells[1].Paragraphs.First().Append(ktp.AssessmentForm ?? "-").Alignment = Alignment.center;

                tableHours.Rows[11].Cells[0].Paragraphs.First().Append("Формы итоговой аттестации");
                tableHours.Rows[11].Cells[1].Paragraphs.First().Append("-").Alignment = Alignment.center;

                document.InsertTable(tableHours);
                document.InsertParagraph().InsertPageBreakAfterSelf();

                // ==========================================
                // СТРАНИЦА 4+: ОСНОВНАЯ ТАБЛИЦА КТП
                // ==========================================
                // Проверяем, есть ли данные. Если нет, создаем пустую строку для примера
                int rowCount = ktp.Rows.Count > 0 ? ktp.Rows.Count : 1;

                var tableKtp = document.AddTable(rowCount + 1, 7);
                tableKtp.Design = TableDesign.TableGrid;
                // Ширины колонок (сумма ~500)
                tableKtp.SetWidths(new float[] { 50f, 200f, 50f, 50f, 50f, 50f, 50f }, true);

                // Заголовки
                tableKtp.Rows[0].Cells[0].Paragraphs.First().Append("№ занятия").Bold().Alignment = Alignment.center;
                tableKtp.Rows[0].Cells[1].Paragraphs.First().Append("Наименование разделов и тем").Bold().Alignment = Alignment.center;
                tableKtp.Rows[0].Cells[2].Paragraphs.First().Append("Кол-во часов").Bold().Alignment = Alignment.center;
                tableKtp.Rows[0].Cells[3].Paragraphs.First().Append("Календарные сроки").Bold().Alignment = Alignment.center;
                tableKtp.Rows[0].Cells[4].Paragraphs.First().Append("Вид занятия").Bold().Alignment = Alignment.center;
                tableKtp.Rows[0].Cells[5].Paragraphs.First().Append("Наглядные пособия и тех. средства").Bold().Alignment = Alignment.center;
                tableKtp.Rows[0].Cells[6].Paragraphs.First().Append("Задания для студентов").Bold().Alignment = Alignment.center;

                if (ktp.Rows.Count > 0)
                {
                    for (int i = 0; i < ktp.Rows.Count; i++)
                    {
                        var r = ktp.Rows[i];
                        var row = tableKtp.Rows[i + 1];
                        row.BreakAcrossPages = false;

                        row.Cells[0].Paragraphs.First().Append(r.LessonNums ?? "").Alignment = Alignment.center;
                        row.Cells[1].Paragraphs.First().Append(r.ThemeName ?? "");
                        row.Cells[2].Paragraphs.First().Append(r.Hours ?? "").Alignment = Alignment.center;
                        row.Cells[3].Paragraphs.First().Append(r.Dates ?? "").Alignment = Alignment.center;
                        row.Cells[4].Paragraphs.First().Append(r.Type ?? "").Alignment = Alignment.center;
                        row.Cells[5].Paragraphs.First().Append(r.Aids ?? "").Alignment = Alignment.center;
                        row.Cells[6].Paragraphs.First().Append(r.Tasks ?? "");
                    }
                }
                else
                {
                    tableKtp.Rows[1].Cells[1].Paragraphs.First().Append("Нет добавленных занятий").Italic();
                }

                document.InsertTable(tableKtp);
                document.InsertParagraph().InsertPageBreakAfterSelf();

                // ==========================================
                // СПИСОК ИСТОЧНИКОВ
                // ==========================================
                document.InsertParagraph("Список используемых источников").FontSize(14).Bold().Alignment = Alignment.center;

                document.InsertParagraph("Основные источники:").Bold().SpacingBefore(12);
                int libIndex = 1;
                foreach (var lib in vm.OsnovLibrary)
                {
                    var p = document.InsertParagraph($"{libIndex}. {lib.Record}");
                    p.Alignment = Alignment.both;
                    libIndex++;
                }

                if (vm.DopLibrary.Count > 0)
                {
                    document.InsertParagraph("Дополнительные источники:").Bold().SpacingBefore(12);
                    libIndex = 1;
                    foreach (var lib in vm.DopLibrary)
                    {
                        var p = document.InsertParagraph($"{libIndex}. {lib.Record}");
                        p.Alignment = Alignment.both;
                        libIndex++;
                    }
                }

                // Сохраняем файл
                document.Save();
            }
        }
    }
}