using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using Newtonsoft.Json;
using PlxModels;
using RsskWorkTeacher.Models;
using RsskWorkTeacher.Models.RP;
using RsskWorkTeacher.ModelsDB;
using RsskWorkTeacher.Services;
using RsskWorkTeacher.Wind;
using RsskWorkTeacher.WorckerClasses;
using System.IO;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;


namespace RsskWorkTeacher.Pages
{
    /// <summary>
    /// Логика взаимодействия для pgRpWork.xaml
    /// </summary>

    public partial class pgRpWork : Page
    {
        WorkingProgramViewModel working;
        WorkingModuleViewModel workingmodule;
        ModelsDB.Rp planRp = new();
        public pgRpWork()
        {
            InitializeComponent();
            var a = _db.Plans.ToList();

            foreach (var item in a)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.YchebPlan;
                cbPlan.Items.Add(comboBoxItem);
            }
            

            foreach (var item in BufApi.Комиссии.value)
            {
                if (!item.Работает) continue;
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.Description;
                cbCik.Items.Add(comboBoxItem);
            }


        }

        RsskContext _db = new();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            winAddChangeRp winAddChangeRp;
            winAddChangeRpPm winAddChangeRpPm;
            if (((TextBlock)(sender as Button).Content).Text.Contains("модуля"))
            {
                switch (((TextBlock)(sender as Button).Content).Text)
                {
                    case "Добавить РП модуля":
                        winAddChangeRpPm = new(cbCik.Text, cbPlan.Text, "", true);
                        winAddChangeRpPm.ShowDialog();
                        break;
                    case "Редактировать выбранную РП модуля":
                        winAddChangeRpPm = new(cbCik.Text, cbPlan.Text, lbSpisokRp.SelectedItem.ToString(), false);
                        winAddChangeRpPm.ShowDialog(); break;
                }

            }
            else
            {


                switch (((TextBlock)(sender as Button).Content).Text)
                {
                    case "Добавить РП дисциплины":
                        winAddChangeRp = new(cbCik.Text, cbPlan.Text, "", true);
                        winAddChangeRp.ShowDialog();
                        break;
                    case "Редактировать выбранную РП дисциплины":
                        winAddChangeRp = new(cbCik.Text, cbPlan.Text, lbSpisokRp.SelectedItem.ToString(), false);
                        winAddChangeRp.ShowDialog(); break;
                    case "Импорт":



                        break;
                    case "Экспорт": break;
                }
            }


        }

        private void cbCik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbSpisokRp.Items.Clear();
            if (cbPlan.Text == "")
                foreach (var item in _db.Rps)
                {
                    if (item.Ck == ((ComboBoxItem)e.AddedItems[0]).Content.ToString())
                    {
                        lbSpisokRp.Items.Add(item.Name);
                    }
                }
            else
            {
                foreach (var item in _db.Rps)
                {
                    if (item.Plan == cbPlan.Text.ToString() && item.Ck == ((ComboBoxItem)e.AddedItems[0]).Content.ToString())
                    {
                        lbSpisokRp.Items.Add(item.Name);
                    }
                }
            }
        }

        private void cbPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            lbSpisokRp.Items.Clear();
            if (cbCik.Text == "")
                foreach (var item in _db.Rps)
                {
                    if (item.Plan == ((ComboBoxItem)e.AddedItems[0]).Content.ToString())
                    {
                        lbSpisokRp.Items.Add(item.Name);
                    }
                }
            else
            {
                foreach (var item in _db.Rps)
                {
                    if (item.Ck == cbCik.Text.ToString() && item.Plan == ((ComboBoxItem)e.AddedItems[0]).Content.ToString())
                    {
                        lbSpisokRp.Items.Add(item.Name);
                    }
                }
            }
        }

        private void lbSpisokRp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbSpisokRp.SelectedItem.ToString().Contains("ПМ"))
                {
                    planRp = _db.Rps.Where(p => p.Ck == cbCik.Text && p.Plan == cbPlan.Text && p.Name.Contains(lbSpisokRp.SelectedItem.ToString())).FirstOrDefault();
                    workingmodule = JsonConvert.DeserializeObject<WorkingModuleViewModel>(planRp.Rp1);
                    DataContext = working;
                    exModule21.Visibility = Visibility.Visible;
                    exModule22.Visibility = Visibility.Visible;
                    exDiscip21.Visibility = Visibility.Hidden;
                    exModule21.IsExpanded = true;
                    exModule22.IsExpanded = true;
                    exDiscip21.IsExpanded = false;
                    dgcSkil.Visibility = Visibility.Visible;
                    DataContext = workingmodule;
                }
                else
                {
                    exModule21.Visibility = Visibility.Hidden;
                    exModule22.Visibility = Visibility.Hidden;
                    exDiscip21.Visibility = Visibility.Visible;
                    exModule21.IsExpanded = false;
                    exModule22.IsExpanded = false;
                    exDiscip21.IsExpanded = true;
                    dgcSkil.Visibility = Visibility.Hidden;
                    planRp = _db.Rps.Where(p => p.Ck == cbCik.Text && p.Plan == cbPlan.Text && p.Name.Contains(lbSpisokRp.SelectedItem.ToString())).FirstOrDefault();
                    // planRp = _db.Rps.Where(p => p.Ck == cbCik.Text && p.Plan == cbPlan.Text && p.Name.Contains(((ListBoxItem)e.AddedItems[0]).Content.ToString())).FirstOrDefault();
                    working = JsonConvert.DeserializeObject<WorkingProgramViewModel>(planRp.Rp1);
                    DataContext = working;
                }
            }
            catch { }
        }

        private void BtnExportWord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnImportRp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Ещё в разработке", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //var openFileDialog = new OpenFileDialog
            //{
            //    Filter = "Документ Word (*.docx)|*.docx",
            //    Title = "Выберите файл рабочей программы для импорта"
            //};

            //if (openFileDialog.ShowDialog() == true)
            //{
            //    try
            //    {
            //        // Проверяем, является ли это файлом ПМ (например, по имени файла)
            //        bool isPm = openFileDialog.FileName.ToUpperInvariant().Contains("ПМ.") ||
            //                    openFileDialog.FileName.ToUpperInvariant().Contains("МОДУЛ");

            //        WorkingProgramViewModel importedVm = new();

            //        if (isPm)
            //        {
            //            importedVm = WordPmParser.ParsePm(openFileDialog.FileName);
            //        }
            //        else
            //        {
            //           // importedVm = WordRpParser.ParseRp(openFileDialog.FileName);
            //        }

            //        // 2. Получаем контекст (специальность и план)
            //        // ВАЖНО: Замените эти строки на реальные значения из ваших ComboBox на странице pgRpWork!
            //        string ck = cbCik.Text; string plan = cbPlan.Text;

            //        string rpName = $"{importedVm.DisciplineKod} {importedVm.DisciplineName}";

            //        // 3. Проверяем, существует ли такая РП
            //        var existingRp = _db.Rps.FirstOrDefault(p => p.Ck == ck && p.Plan == plan && p.Name == rpName);

            //        // 4. Сериализуем в JSON для базы
            //        string jsonContent = JsonConvert.SerializeObject(importedVm);

            //        if (existingRp != null)
            //        {
            //            var result = MessageBox.Show("Рабочая программа с таким названием уже существует. Перезаписать её?",
            //                                         "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //            if (result == MessageBoxResult.No) return;

            //            existingRp.Rp1 = jsonContent;
            //            _db.SaveChanges();
            //            MessageBox.Show("Рабочая программа успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            //        }
            //        else
            //        {
            //            var newRp = new ModelsDB.Rp
            //            {
            //                Ck = ck,
            //                Plan = plan,
            //                Name = rpName,
            //                Rp1 = jsonContent
            //            };

            //            do
            //            {
            //                newRp.Id = KeyGenerator.GenerateKey(16);
            //            }
            //            while (_db.Rps.Any(p => p.Id == newRp.Id));

            //            _db.Rps.Add(newRp);
            //            _db.SaveChanges();

            //            MessageBox.Show($"Рабочая программа '{rpName}' успешно импортирована!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            //        }

            //        // 5. Обновляем таблицу на странице
            //        // LoadData(); 
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Ошибка при импорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
        }
    }
}
