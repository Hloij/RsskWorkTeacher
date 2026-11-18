using DocumentFormat.OpenXml.Drawing;
using Microsoft.Win32;
using Newtonsoft.Json;
using PlxModels;
using QuestPDF.Drawing;
using RsskWorkTeacher.Models.RP;
using RsskWorkTeacher.ModelsDB;
using RsskWorkTeacher.Services;
using RsskWorkTeacher.Wind;
using RsskWorkTeacher.WorckerClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace RsskWorkTeacher.Wind
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// Scaffold-DbContext "Server=192.168.24.4,49171; Database=rssk; User=dima1; Password=; Encrypt=false" Microsoft.EntityFrameworkCore.SqlServer -OutputDir ModelsDB -force
    /// </summary>
    public partial class winAddChangeRp : Window
    {
        WorkingProgramViewModel working;
        WorkingProgramViewModel shablon;
        bool context = false;
        RsskContext _db = new();
        List<PlanRow> planRow;
        Document doc;

        ModelsDB.Rp planRp = new();
        public winAddChangeRp(string ck, string plan, string name, bool context)
        {
            shablon = new("s");

            this.context = context;
            InitializeComponent();
            try
            {
                FontManager.RegisterFontFromFile("Visual/LiberationSerif-Regular.ttf");
                FontManager.RegisterFontFromFile("Visual/LiberationSerif-Bold.ttf");
                FontManager.RegisterFontFromFile("Visual/LiberationSerif-Italic.ttf");
                FontManager.RegisterFontFromFile("Visual/LiberationSerif-BoldItalic.ttf");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить шрифты: {ex.Message}\nПроверьте папку Fonts.");
            }
            // ВСЕГДА ставим DataContext — иначе при context=false
            // всё окно «висит» без биндингов
            try
            {
                var serializer = new XmlSerializer(typeof(Document));
                using (var fs = File.OpenRead(_db.Plans.First(p => p.YchebPlan == plan).Path/*.Replace("FILE-SERVER", "192.168.24.3")*/))
                    doc = (Document)serializer.Deserialize(fs);
            }
            catch
            {
                MessageBox.Show($"Не найден фалй учебного плана.");

            }
            planRow = doc.Diffgram.Ds.PlanRows;
            foreach (var item in planRow)
                if (!item.ДисциплинаКод.Contains("ПМ") && !item.ДисциплинаКод.Contains("УП") && !item.ДисциплинаКод.Contains("ПП") && !item.ДисциплинаКод.Contains("МДК"))
                    tbdicip.Items.Add(new ComboBoxItem { Content = item.Дисциплина });
            if (context)
            {
                planRp.Ck = ck;
                planRp.Plan = plan;
                working = new();

                working.Specialty = doc.Diffgram.Ds.Plans[0].Титул;
                working.Qualification = doc.Diffgram.Ds.Plans[0].Квалификация;

                // Заполняем ComboBox элементами


                // Автовыбор первого элемента — вызовет SelectionChanged и заполнит VM
                if (tbdicip.Items.Count > 0)
                    tbdicip.SelectedIndex = 0;
            }
            else
            {
                planRp = _db.Rps.Where(p => p.Ck == ck && p.Plan == plan && p.Name.Contains(name)).FirstOrDefault();
                working = JsonConvert.DeserializeObject<WorkingProgramViewModel>(planRp.Rp1);
            }




        }

        private void btnAddThem(object sender, RoutedEventArgs e)
        {
            Thema theme = new();
            theme.Owner = this;
            for (int i = 0; i < working.Competencies.Count; i++)
            {
                theme.AvailableCompetences.Add(new Models.RP.CompetenceSelection { Name = working.Competencies[i].Code, IsSelected = false });
            }
            if (theme.ShowDialog() == true)
                working.Themes.Add(theme.Theme);
        }

        private void btnChangeThem(object sender, RoutedEventArgs e)
        {
            if (dgPlan.SelectedItem is ThemePlan selectedTheme)
            {
                var editWindow = new Thema(selectedTheme);
                editWindow.Owner = this;
                for (int i = 0; i < working.Competencies.Count; i++)
                {
                    editWindow.AvailableCompetences.Add(new Models.RP.CompetenceSelection { Name = working.Competencies[i].Code, IsSelected = false });
                }

                if (editWindow.ShowDialog() == true)
                {
                    int index = working.Themes.IndexOf(selectedTheme);
                    if (index >= 0)
                    {
                        working.Themes[index] = editWindow.Theme;
                        dgPlan.SelectedIndex = index;
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите тему в таблице для редактирования.",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnDelThem(object sender, RoutedEventArgs e)
        {
            if (dgPlan.SelectedItem is ThemePlan selectedTheme)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить тему \"{selectedTheme.ThemeName}\"?",
                                             "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                    working.Themes.Remove(selectedTheme);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите тему в таблице для удаления.",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnExportPdf_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as WorkingProgramViewModel;
            if (viewModel == null) return;

            // ПРОВЕРКА ЧАСОВ ПЕРЕД ЭКСПОРТОМ
            string errors = viewModel.CheckHoursConsistency();
            if (!string.IsNullOrEmpty(errors))
            {
                var result = MessageBox.Show($"Внимание! Есть расхождения часов между п. 2.1 и п. 2.2:\n\n{errors}\n\nВы уверены, что хотите экспортировать документ?",
                    "Проверка часов", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No) return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word документ (*.docx)|*.docx",
                FileName = "Рабочая_программа.docx",
                Title = "Сохранить документ как Word"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    WordExporter.ExportToWord(viewModel, saveFileDialog.FileName);
                    MessageBox.Show("Документ успешно экспортирован в Word!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ВАЖНО: пишем ТОЛЬКО в VM, не трогаем .Text у контролов,
        // иначе WPF снимает BindingExpression и биндинг «умирает» навсегда.
        private void tbdicip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (doc == null || planRow == null) return;
            if (e.AddedItems.Count == 0) return;

            var content = (e.AddedItems[0] as ComboBoxItem)?.Content?.ToString();
            if (string.IsNullOrEmpty(content)) return;
            var row = planRow.FirstOrDefault(r => r.Дисциплина == content);
            if (row == null) return;

            planRp.Name = $"{row.ДисциплинаКод} {row.Дисциплина}";
            working.DisciplineName = row.Дисциплина;
            working.DisciplineKod = row.ДисциплинаКод;

            var map = PlxQuery.BuildDisciplineCompetencyMap(doc.Diffgram.Ds);
            var comps = map.GetValueOrDefault(row.Код) ?? new List<PlanCompetency>();

            working.CompetenciesLabel = string.Join(
                Environment.NewLine,
                comps.Select(c => $"{c.ШифрКомпетенции} {c.Наименование}"));

            if (working.Competencies.Count == 0 || working.Competencies.Count != working.CompetenciesLabel.Split("\n", StringSplitOptions.RemoveEmptyEntries).Length)
            {

                working.Competencies.Clear();
                foreach (var item in comps)
                {
                    if (working.CompetenciesLabel.Contains(item.ШифрКомпетенции))
                    {
                        working.Competencies.Add(new Models.RP.Competency { Code = item.ШифрКомпетенции });
                    }
                }
            }


            FillVolume(working, doc.Diffgram.Ds, row);

            if (DataContext == working)
                if (MessageBoxResult.OK == MessageBox.Show("Этот выбор может стереть всё что вы уже сделали!", "Выбрать всё из пооп?", MessageBoxButton.OKCancel))
                    ApplyTemplateToCurrentDiscipline();
        }

        private void FillVolume(WorkingProgramViewModel vm, DsMMISDB ds, PlanRow row)
        {
            if (vm == null || ds == null || row == null) return;

            // ---- 1. Справочник форм контроля (КодТипРабот == 7) ----
            var attestationNames = ds.WorkTypeItems
                .Where(w => w.КодТипРабот == 7)
                .GroupBy(w => w.Код)
                .ToDictionary(g => g.Key, g => g.First().Название);

            // ---- 2. Строки часов: свои + дочерние (для ПМ) ----
            var ownerIds = new List<int> { row.Код };
            if (row.ТипОбъекта == 1) // модуль (ПМ)
            {
                ownerIds.AddRange(ds.PlanRows
                    .Where(r => r.КодРодителя == row.Код)
                    .Select(r => r.Код));
            }

            var hours = ds.NewHours
                .Where(h => ownerIds.Contains(h.КодОбъекта))
                .ToList();

            if (hours.Count == 0)
            {
                vm.ChasRabProg1 = row.ЧасовПоПлану.ToString();
                vm.ChasRabProg2 = "0";
                vm.ChasOsnov = row.ЧасовПоПлану.ToString();
                vm.TeaorChasOsnov = "0";
                vm.PractChas1 = "0";
                vm.PractChas2 = "0";
                vm.KursachChas = "-";
                vm.SamRabChas = "0";
                vm.AtesChas = "-";
                vm.Semestr = "-";
                vm.Attestation = "-";
                return;
            }

            // ---- 3. Линейный номер семестра ----
            int semPerCourse = ds.Plans.FirstOrDefault()?.СеместровНаКурсе ?? 2;
            if (semPerCourse <= 0) semPerCourse = 2;

            int LinearSem(int course, int sem)
            {
                if (course <= 0) course = 1;
                if (sem <= 0) sem = 1;
                return (course - 1) * semPerCourse + sem;
            }

            // ---- 4. Часы по видам работ ----
            // ВАЖНО: Берем только часы с КодТипаЧасов == 1 (объемные показатели), 
            // чтобы не удваивать практику, которая продублирована с КодТипаЧасов == 5.
            decimal Sum(int code) => hours
                .Where(h => h.КодВидаРаботы == code && h.КодТипаЧасов == 1)
                .Sum(h => h.Количество);

            decimal lek = Sum(101);
            decimal lab = Sum(102);
            decimal prak = Sum(103);
            decimal indiv = Sum(140);
            decimal sr = Sum(107);
            decimal ctrl = Sum(108);
            decimal kurs = Sum(4) + Sum(5) + Sum(140); // Курсовой проект + Курсовая работа

            // ---- 5. Практическая подготовка (КодТипаЧасов == 5) ----
            decimal praktPrep = hours
                .Where(h => h.КодТипаЧасов == 5 && (h.КодВидаРаботы == 102 || h.КодВидаРаботы == 103 || h.КодВидаРаботы == 140))
                .Sum(h => h.Количество);

            // ---- 6. Считаем итоги ----
            decimal total = row.ЧасовПоПлану;
            decimal totalPract = lab + prak + indiv; // Вся практика: лабы + практ + инд.проект
            decimal osnov = total - sr - ctrl;       // Основное содержание

            // ---- 7. Запись в VM ----
            vm.ChasRabProg1 = total.ToString("0");                  // Общий объем (150)
            vm.ChasRabProg2 = praktPrep.ToString("0");              // В т.ч. практ. подготовка (88)
            vm.ChasOsnov = osnov.ToString("0");                     // Основное (136)
            vm.TeaorChasOsnov = lek.ToString("0");                  // Теория (48)
            vm.PractChas1 = totalPract.ToString("0");               // Практические занятия (88)
            vm.PractChas2 = praktPrep.ToString("0");                // В т.ч. практ. (88)
            vm.KursachChas = kurs == 0 ? "-" : kurs.ToString("0");
            vm.SamRabChas = sr.ToString("0");                       // Самостоятельная (2)
            vm.AtesChas = ctrl == 0 ? "-" : ctrl.ToString("0");     // Аттестация (12)

            // ---- 8. Семестры ----
            var semesters = hours
                .Where(h => h.КодВидаРаботы == 1000 && h.КодТипаЧасов == 1)
                .Select(h => LinearSem(h.Курс, h.Семестр))
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            vm.Semestr = semesters.Count == 0
                ? "-"
                : string.Join(", ", semesters.Select(s => $"{s} семестр"));

            // ---- 9. Формы аттестации ----
            var attBySem = hours
                .Where(h => attestationNames.ContainsKey(h.КодВидаРаботы) && h.КодТипаЧасов == 1)
                .GroupBy(h => LinearSem(h.Курс, h.Семестр))
                .OrderBy(g => g.Key)
                .Select(g =>
                {
                    var names = g
                        .Select(x => attestationNames[x.КодВидаРаботы])
                        .Distinct()
                        .ToList();

                    var ctrlInSem = hours
                        .Where(h => h.КодВидаРаботы == 108 && h.КодТипаЧасов == 1 && LinearSem(h.Курс, h.Семестр) == g.Key)
                        .Sum(h => h.Количество);

                    var suffix = ctrlInSem > 0 ? $" ({ctrlInSem} ч.)" : "";
                    return $"{g.Key} семестр — {string.Join(", ", names)}{suffix}";
                })
                .ToList();

            vm.Attestation = attBySem.Count == 0
                ? "-"
                : string.Join(";\n", attBySem);
        }
        private void BtnImportPopp_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Документ Word (*.docx)|*.docx",
                Title = "Выберите файл примерной программы (ПООП)"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;

                    string kod = working.DisciplineKod.Trim().ToUpperInvariant().Replace(" ", "");


                    // Старый импорт для дисциплин
                    var templates = PoppParser.ParseDocx(openFileDialog.FileName);
                    MessageBox.Show($"Распознано дисциплин: {templates.Count}", "Успех");
                    ApplyTemplateToCurrentDiscipline();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении ПООП: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void ApplyTemplateToCurrentDiscipline()
        {
            if (string.IsNullOrWhiteSpace(working.DisciplineKod)) return;
            if (PoppParser.Cache.Count == 0) return;

            var kod = working.DisciplineKod.Trim().ToUpperInvariant().Replace(" ", "");

            if (PoppParser.Cache.TryGetValue(kod, out var template))
            {
                // 1. Цель
                if (!string.IsNullOrEmpty(template.Purpose))
                    working.Purpose = template.Purpose;

                // 2. Материально-техническое обеспечение (п. 3.1)
                if (!string.IsNullOrEmpty(template.MaterialTech))
                    working.TrebKab = template.MaterialTech;

                // 3. Основная и дополнительная литература (п. 3.2)
                if (template.LiteratureMain.Any())
                {
                    working.OsnovLibrary.Clear();
                    foreach (var lib in template.LiteratureMain)
                        working.OsnovLibrary.Add(new LibraryRecord { Record = lib });
                }

                if (template.LiteratureAdditional.Any())
                {
                    working.DopLibrary.Clear();
                    foreach (var lib in template.LiteratureAdditional)
                        working.DopLibrary.Add(new LibraryRecord { Record = lib });
                }

                // 4. Критерии оценки (п. 4)
                if (template.Evaluations.Any())
                {
                    working.Evaluations.Clear();
                    foreach (var eval in template.Evaluations)
                        working.Evaluations.Add(eval);
                }
               
                // 5. ДОПОЛНЕНИЕ КОМПЕТЕНЦИЙ (п. 1.2)
                if (template.Competencies.Any())
                {
                    foreach (var poopedComp in template.Competencies)
                    {
                        var existingComp = working.Competencies.FirstOrDefault(c =>
                            c.Code.Replace(".", "").Replace(" ", "").Equals(poopedComp.Code.Replace(".", "").Replace(" ", ""), StringComparison.OrdinalIgnoreCase));

                        if (existingComp != null)
                        {
                            existingComp.KnowHow = poopedComp.KnowHow;
                            existingComp.Knowledge = poopedComp.Knowledge;
                        }
                    }
                }

                // 6. Тематический план (п. 2.2) - АДАПТАЦИЯ ПОД НАШУ ТАБЛИЦУ
                if (template.Themes.Any())
                {
                    working.Themes.Clear();

                    foreach (var importedTheme in template.Themes)
                    {
                        // А) Проверяем, является ли это разделом
                        // Если парсер сохранил текст "Раздел..." в названии, ставим галочку
                        if (importedTheme.ThemeName.Trim().StartsWith("Раздел", StringComparison.OrdinalIgnoreCase) ||
                            importedTheme.ThemeName.Trim().StartsWith("РАЗДЕЛ", StringComparison.OrdinalIgnoreCase))
                        {
                            importedTheme.IsSection = true;
                            working.Themes.Add(importedTheme);
                            continue;
                        }

                        // Б) Нормализуем обычную тему
                        // Считаем сумму часов из блоков (если парсер не заполнил TotalHours)
                        int sumHours = importedTheme.ContentBlocks
                            .Sum(b => int.TryParse(b.Hours, out int h) ? h : 0);

                        // Если сумма больше 0, записываем ее в TotalHours
                        if (sumHours > 0)
                        {
                            importedTheme.TotalHours = sumHours.ToString();
                        }
                        else if (!string.IsNullOrEmpty(importedTheme.TotalHours))
                        {
                            // Если часы есть только в TotalHours, но нет в блоках, 
                            // создаем один блок, чтобы в Word они красиво отобразились
                            if (!importedTheme.ContentBlocks.Any())
                            {
                                importedTheme.ContentBlocks.Add(new ContentBlock
                                {
                                    Description = "Содержание учебного материала",
                                    Hours = importedTheme.TotalHours
                                });
                            }
                        }

                        // В) Убеждаемся, что первый блок не пустой (чтобы таблица в Word не ломалась)
                        if (!importedTheme.ContentBlocks.Any())
                        {
                            importedTheme.ContentBlocks.Add(new ContentBlock { Description = "Содержание учебного материала", Hours = "0" });
                        }

                        // Добавляем тему в общий список
                        working.Themes.Add(importedTheme);
                    }
                }
            }
            else
            {
                MessageBox.Show($"В загруженном ПООП не найдена дисциплина с кодом: {kod}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Защита от срабатывания при инициализации окна
            if (e.AddedItems.Count == 0 || !(e.AddedItems[0] is ComboBoxItem)) return;

            // 1. ПРИНУДИТЕЛЬНО ЗАВЕРШАЕМ РЕДАКТИРОВАНИЕ И СБРАСЫВАЕМ ФОКУС
            if (dgCompetencies != null)
            {
                // Завершаем редактирование ячейки и строки
                dgCompetencies.CommitEdit(DataGridEditingUnit.Cell, true);
                dgCompetencies.CommitEdit(DataGridEditingUnit.Row, true);

                // Сбрасываем фокус с таблицы на само окно
                Keyboard.ClearFocus();
                FocusManager.SetFocusedElement(this, this);
            }

            // 2. Меняем контекст
            string selectedItem = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();

            if (selectedItem == "Шаблон рабочей программы")
            {
                shablon = new("S");
                DataContext = shablon;
            }
            else
            {


                shablon = working;
                DataContext = shablon;
            }
        }


        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string errors = working.CheckHoursConsistency();
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show($"Внимание! Есть расхождения часов между п. 2.1 и п. 2.2:\n\n{errors}",
                    "Проверка часов", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            planRp.Rp1 = JsonConvert.SerializeObject(DataContext);
            Ktp ktp = new();
            ktp.Ck = planRp.Ck;
            ktp.Plan = planRp.Plan;
            ktp.Id = planRp.Id;
            ktp.Name = planRp.Name;
            working.GenerateKtpFromThemes();
            ktp.Ktp1 = JsonConvert.SerializeObject(working.Ktp);
            if (planRp.Id == null)
            {
                do
                    planRp.Id = KeyGenerator.GenerateKey(16);
                while (_db.Rps.Where(p => p.Id == planRp.Id).FirstOrDefault() != null);
                ktp.Id = planRp.Id;
                _db.Rps.Add(planRp);
                _db.Ktps.Add(ktp);
            }
            try
            {
                if (_db.Ktps.Where(p => p.Id == planRp.Id).FirstOrDefault() != null)
                {
                    _db.Ktps.Add(ktp);
                }
                _db.SaveChanges();
                MessageBox.Show("Запись успешно добавлена/отредактирована", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.",
                               "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            finally
            {
                PoppParser.Cache.Clear();
            }
        }
    }
}