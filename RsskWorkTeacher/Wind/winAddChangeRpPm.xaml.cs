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
    public partial class winAddChangeRpPm : Window
    {
        WorkingModuleViewModel working;
        WorkingModuleViewModel shablon;
        bool context = false;
        RsskContext _db = new();
        List<PlanRow> planRow;
        Document doc;

        ModelsDB.Rp planRp = new();
        public winAddChangeRpPm(string ck, string plan, string name, bool context)
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
            var serializer = new XmlSerializer(typeof(Document));
            using (var fs = File.OpenRead(_db.Plans.First(p => p.YchebPlan == plan).Path.Replace("FILE-SERVER", "192.168.24.3")))
                doc = (Document)serializer.Deserialize(fs);

            planRow = doc.Diffgram.Ds.PlanRows;
            foreach (var item in planRow)
                if(item.ДисциплинаКод.Contains("ПМ"))
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
                working = JsonConvert.DeserializeObject<WorkingModuleViewModel>(planRp.Rp1);
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
            var viewModel = this.DataContext as WorkingModuleViewModel;
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
                    WordPmExporter.ExportPmToWord(viewModel, saveFileDialog.FileName);
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

            FillModuleVolume(working, doc.Diffgram.Ds, row);
                FillModuleStructure(working, doc.Diffgram.Ds, row);
            
            
            if (DataContext == working)
                if(MessageBoxResult.OK == MessageBox.Show("Этот выбор может стереть всё что вы уже сделали!","Выбрать всё из пооп?", MessageBoxButton.OKCancel))
                    ApplyPmTemplateToCurrentModule(row.ДисциплинаКод.Trim().ToUpperInvariant().Replace(" ", ""));

              
        }
        private void FillModuleStructure(WorkingModuleViewModel vm, DsMMISDB ds, PlanRow row)
        {
            if (vm == null || ds == null || row == null) return;

            vm.ModuleStructures.Clear();

            // Переменные для накопления итогов
            decimal sumTotal = 0;
            decimal sumPract = 0;
            decimal sumMdk = 0;
            decimal sumLessons = 0;
            decimal sumKurs = 0;
            decimal sumSr = 0;
            decimal sumUp = 0;
            decimal sumPp = 0;
            decimal sumAtt = 0;

            // 1. Получаем дочерние элементы
            var children = ds.PlanRows.Where(r => r.КодРодителя == row.Код).ToList();

            var mdks = children.Where(r => r.ТипОбъекта == 2).ToList(); // МДК
            var up = children.FirstOrDefault(r => r.ТипОбъекта == 3 && r.ДисциплинаКод.StartsWith("УП")); // УП
            var pp = children.FirstOrDefault(r => r.ТипОбъекта == 3 && r.ДисциплинаКод.StartsWith("ПП")); // ПП
            var exam = children.FirstOrDefault(r => r.ТипОбъекта == 4); // Экзамен

            // 2. Обрабатываем каждый МДК
            foreach (var mdk in mdks)
            {
                var comps = PlxQuery.GetCompetenciesForRow(ds, mdk.Код);
                string compStr = string.Join(", ", comps.Select(c => c.ШифрКомпетенции).Distinct());

                decimal SumMdk(int workCode, int typeCode = 1) =>
                    ds.NewHours.Where(h => h.КодОбъекта == mdk.Код && h.КодВидаРаботы == workCode && h.КодТипаЧасов == typeCode)
                               .Sum(h => h.Количество);

                decimal sr = SumMdk(107);
                decimal kurs = SumMdk(140) + SumMdk(4) + SumMdk(5);
                decimal att = SumMdk(108);
                decimal pract = ds.NewHours.Where(h => h.КодОбъекта == mdk.Код && h.КодТипаЧасов == 5).Sum(h => h.Количество);

                decimal total = mdk.ЧасовПоПлану;
                decimal lessons = total - sr - kurs - att;
                if (lessons < 0) lessons = 0;

                // Накапливаем суммы
                sumTotal += total;
                sumPract += pract;
                sumMdk += total;
                sumLessons += lessons;
                sumKurs += kurs;
                sumSr += sr;
                sumAtt += att;

                vm.ModuleStructures.Add(new ModuleStructureRow
                {
                    CompetencyCodes = compStr,
                    ModuleName = $"{mdk.ДисциплинаКод} {mdk.Дисциплина}",

                    TotalHours = total.ToString("0"),
                    PracticalPrepHours = pract > 0 ? pract.ToString("0") : "-",

                    MdkHours = total.ToString("0"),
                    TrainingSessionsHours = lessons > 0 ? lessons.ToString("0") : "-",
                    CourseworkHours = kurs > 0 ? kurs.ToString("0") : "-",
                    IndependentWorkHours = sr > 0 ? sr.ToString("0") : "-",

                    EducationalPracticeHours = "-",
                    IndustrialPracticeHours = "-",
                    AssessmentHours = att > 0 ? att.ToString("0") : "-"
                });
            }

            // 3. Обрабатываем Учебную практику
            if (up != null)
            {
                var comps = PlxQuery.GetCompetenciesForRow(ds, up.Код);
                string compStr = string.Join(", ", comps.Select(c => c.ШифрКомпетенции).Distinct());

                decimal pract = ds.NewHours.Where(h => h.КодОбъекта == up.Код && h.КодТипаЧасов == 5).Sum(h => h.Количество);
                if (pract == 0) pract = up.ЧасовПоПлану;

                sumTotal += up.ЧасовПоПлану;
                sumPract += pract;
                sumUp += up.ЧасовПоПлану;

                vm.ModuleStructures.Add(new ModuleStructureRow
                {
                    CompetencyCodes = compStr,
                    ModuleName = $"{up.ДисциплинаКод} {up.Дисциплина}",
                    TotalHours = up.ЧасовПоПлану.ToString("0"),
                    PracticalPrepHours = pract.ToString("0"),
                    EducationalPracticeHours = up.ЧасовПоПлану.ToString("0"),

                    MdkHours = "x",
                    TrainingSessionsHours = "-",
                    CourseworkHours = "-",
                    IndependentWorkHours = "-",
                    IndustrialPracticeHours = "-",
                    AssessmentHours = "-"
                });
            }

            // 4. Обрабатываем Производственную практику
            if (pp != null)
            {
                var comps = PlxQuery.GetCompetenciesForRow(ds, pp.Код);
                string compStr = string.Join(", ", comps.Select(c => c.ШифрКомпетенции).Distinct());

                decimal pract = ds.NewHours.Where(h => h.КодОбъекта == pp.Код && h.КодТипаЧасов == 5).Sum(h => h.Количество);
                if (pract == 0) pract = pp.ЧасовПоПлану;

                sumTotal += pp.ЧасовПоПлану;
                sumPract += pract;
                sumPp += pp.ЧасовПоПлану;

                vm.ModuleStructures.Add(new ModuleStructureRow
                {
                    CompetencyCodes = compStr,
                    ModuleName = $"{pp.ДисциплинаКод} {pp.Дисциплина}",
                    TotalHours = pp.ЧасовПоПлану.ToString("0"),
                    PracticalPrepHours = pract.ToString("0"),
                    IndustrialPracticeHours = pp.ЧасовПоПлану.ToString("0"),

                    MdkHours = "x",
                    TrainingSessionsHours = "-",
                    CourseworkHours = "-",
                    IndependentWorkHours = "-",
                    EducationalPracticeHours = "-",
                    AssessmentHours = "-"
                });
            }

            // 5. Обрабатываем Модульный экзамен
            if (exam != null)
            {
                sumTotal += exam.ЧасовПоПлану;
                sumAtt += exam.ЧасовПоПлану;

                vm.ModuleStructures.Add(new ModuleStructureRow
                {
                    CompetencyCodes = "-",
                    ModuleName = $"{exam.ДисциплинаКод} {exam.Дисциплина}",
                    TotalHours = exam.ЧасовПоПлану.ToString("0"),
                    PracticalPrepHours = "-",
                    AssessmentHours = exam.ЧасовПоПлану.ToString("0"),

                    MdkHours = "x",
                    TrainingSessionsHours = "-",
                    CourseworkHours = "-",
                    IndependentWorkHours = "-",
                    EducationalPracticeHours = "-",
                    IndustrialPracticeHours = "-"
                });
            }

            // 6. ДОБАВЛЯЕМ ИТОГОВУЮ СТРОКУ "ВСЕГО"
            vm.ModuleStructures.Add(new ModuleStructureRow
            {
                CompetencyCodes = "",
                ModuleName = "Всего:",
                TotalHours = sumTotal > 0 ? sumTotal.ToString("0") : "-",
                PracticalPrepHours = sumPract > 0 ? sumPract.ToString("0") : "-",
                MdkHours = sumMdk > 0 ? sumMdk.ToString("0") : "-",
                TrainingSessionsHours = sumLessons > 0 ? sumLessons.ToString("0") : "-",
                CourseworkHours = sumKurs > 0 ? sumKurs.ToString("0") : "-",
                IndependentWorkHours = sumSr > 0 ? sumSr.ToString("0") : "-",
                EducationalPracticeHours = sumUp > 0 ? sumUp.ToString("0") : "-",
                IndustrialPracticeHours = sumPp > 0 ? sumPp.ToString("0") : "-",
                AssessmentHours = sumAtt > 0 ? sumAtt.ToString("0") : "-"
            });
        }
        private void FillModuleVolume(WorkingModuleViewModel vm, DsMMISDB ds, PlanRow row)
        {
            if (vm == null || ds == null || row == null) return;
            if (vm.Module21 == null) vm.Module21 = new ModuleLaborIntensityModel();

            // 1. Получаем только ДОЧЕРНИЕ элементы
            var children = ds.PlanRows.Where(r => r.КодРодителя == row.Код).ToList();

            var mdks = children.Where(r => r.ТипОбъекта == 2).ToList(); // МДК
            var up = children.FirstOrDefault(r => r.ТипОбъекта == 3 && r.ДисциплинаКод.StartsWith("УП")); // УП
            var pp = children.FirstOrDefault(r => r.ТипОбъекта == 3 && r.ДисциплинаКод.StartsWith("ПП")); // ПП
            var exam = children.FirstOrDefault(r => r.ТипОбъекта == 4); // Экзамен

            var mdkIds = mdks.Select(m => m.Код).ToList();

            // --- ХЕЛПЕРЫ ДЛЯ ПОДСЧЕТА ---
            // Сумма часов МДК по коду работы и типу часов
            decimal SumMdk(int workCode, int typeCode = 1) =>
                ds.NewHours.Where(h => mdkIds.Contains(h.КодОбъекта) && h.КодВидаРаботы == workCode && h.КодТипаЧасов == typeCode)
                           .Sum(h => h.Количество);

            // Сумма всех часов МДК (чтобы не зависеть от строки "Итого")
            decimal totalMdkPlanHours = mdks.Sum(m => m.ЧасовПоПлану);

            // --- 2. РАСЧЕТ ПО МДК ---
            decimal srTotal = SumMdk(107); // Самостоятельная работа

            // Курсовая (140) + Курсовой проект (4) + Курсовая работа (5)
            decimal kursTotal = SumMdk(140) + SumMdk(4) + SumMdk(5);
            decimal kursPract = kursTotal; // Вся курсовая - практика

            // Учебные занятия = Все часы МДК минус СР, Курсовая и Аттестация внутри МДК
            decimal attMdk = SumMdk(108); // Часы контроля внутри МДК
            decimal learnTotal = totalMdkPlanHours - srTotal - kursTotal - attMdk;

            // Практическая подготовка в Учебных занятиях (лабы 102 + практ 103, тип 5)
            decimal learnPract = ds.NewHours
                .Where(h => mdkIds.Contains(h.КодОбъекта) && h.КодТипаЧасов == 5 && (h.КодВидаРаботы == 102 || h.КодВидаРаботы == 103))
                .Sum(h => h.Количество);

            // --- 3. РАСЧЕТ ПРАКТИК (УП и ПП) ---
            decimal upTotal = up?.ЧасовПоПлану ?? 0;
            decimal ppTotal = pp?.ЧасовПоПлану ?? 0;
            decimal practiceTotal = upTotal + ppTotal;

            decimal upPract = ds.NewHours.Where(h => up != null && h.КодОбъекта == up.Код && h.КодТипаЧасов == 5).Sum(h => h.Количество);
            decimal ppPract = ds.NewHours.Where(h => pp != null && h.КодОбъекта == pp.Код && h.КодТипаЧасов == 5).Sum(h => h.Количество);

            // Если по какой-то причине тип 5 не проставлен, берем общий объем
            if (upPract == 0) upPract = upTotal;
            if (ppPract == 0) ppPract = ppTotal;

            decimal practiceTotalPract = upPract + ppPract;

            // --- 4. РАСЧЕТ АТТЕСТАЦИИ ---
            // Аттестация = Часы контроля МДК (108) + Часы экзамена
            decimal attExam = exam?.ЧасовПоПлану ?? 0;
            decimal attTotal = attMdk + attExam;

            // --- 5. ИТОГИ ---
            decimal totalHours = totalMdkPlanHours + practiceTotal + attExam;
            decimal totalPract = learnPract + kursPract + practiceTotalPract;

            // --- 6. ЗАПИСЬ ---
            vm.Module21.LearnTotal = learnTotal > 0 ? learnTotal.ToString("0") : "0";
            vm.Module21.LearnPractical = learnPract > 0 ? learnPract.ToString("0") : "0";

            vm.Module21.CourseworkTotal = kursTotal == 0 ? "-" : kursTotal.ToString("0");
            vm.Module21.CourseworkPractical = kursPract == 0 ? "-" : kursPract.ToString("0");

            vm.Module21.SelfWorkTotal = srTotal.ToString("0");

            vm.Module21.PracticeTotal = practiceTotal.ToString("0");
            vm.Module21.PracticePractical = practiceTotalPract.ToString("0");

            vm.Module21.PracticeEduTotal = upTotal.ToString("0");
            vm.Module21.PracticeEduPractical = upPract.ToString("0");

            vm.Module21.PracticeIndTotal = ppTotal.ToString("0");
            vm.Module21.PracticeIndPractical = ppPract.ToString("0");

            vm.Module21.Attestation = attTotal == 0 ? "-" : attTotal.ToString("0");

            vm.Module21.TotalHours = totalHours.ToString("0");
            vm.Module21.TotalPractical = totalPract.ToString("0");
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

                   
                        // Импорт для ПМ
                        var templates = PmPoppParser.ParseDocx(openFileDialog.FileName);
                        MessageBox.Show($"Распознано ПМ: {string.Join(", ", templates.Keys)}", "Успех");

                        // Применяем данные к текущему ПМ
                        ApplyPmTemplateToCurrentModule(kod);
                   
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
        private void ApplyPmTemplateToCurrentModule(string pmCode)
        {
            // Нормализуем код (из "ПМ.01" делаем "ПМ01" или "ПМ.1")
            string cleanKod = pmCode.Replace(".", "").Trim();

            var template = PmPoppParser.Cache.Values.FirstOrDefault(t =>
                t.Code.Replace(".", "").Trim() == cleanKod);

            if (template != null)
            {
                // 1. Компетенции (обновляем существующие или добавляем новые)
                foreach (var comp in template.Competencies)
                {
                    var existing = working.Competencies.FirstOrDefault(c => c.Code.Replace(".", "").Replace(" ", "") == comp.Code.Replace(".", "").Replace(" ", ""));
                    if (existing != null)
                    {
                        existing.KnowHow = comp.KnowHow;
                        existing.Knowledge = comp.Knowledge;
                        existing.Skil = comp.Skil;
                    }

                }

                // 2. Темы (очищаем старые и записываем новые)
                if (template.Themes.Any())
                {
                    working.Themes.Clear();
                    foreach (var theme in template.Themes)
                    {
                        working.Themes.Add(theme);
                    }
                }

                // 3. Литература
                if (template.LiteratureMain.Any())
                {
                    working.OsnovLibrary.Clear();
                    foreach (var lib in template.LiteratureMain)
                    {
                        working.OsnovLibrary.Add(new LibraryRecord { Record = lib });
                    }
                }

                // 4. Критерии оценки
                if (template.Evaluations.Any())
                {
                    working.Evaluations.Clear();
                    foreach (var eval in template.Evaluations)
                    {
                        working.Evaluations.Add(eval);
                    }
                }

                MessageBox.Show("Данные ПМ успешно импортированы!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"В файле ПООП не найден модуль с кодом: {pmCode}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            string errors = working.CheckModuleHoursConsistency();
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show($"Внимание! Есть расхождения часов между п. 2.1 и п. 2.2:\n\n{errors}",
                    "Проверка часов", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            planRp.Rp1 = JsonConvert.SerializeObject(DataContext);
            if (planRp.Id == null)
            {
                do
                    planRp.Id = KeyGenerator.GenerateKey(16);
                while (_db.Rps.Where(p => p.Id == planRp.Id).FirstOrDefault() != null);
                _db.Rps.Add(planRp);
            }
            try
            {
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
                PmPoppParser.Cache.Clear();
            }


        }
    }
}