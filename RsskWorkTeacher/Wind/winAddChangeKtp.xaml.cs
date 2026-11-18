using ConsoleApp2;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using Newtonsoft.Json;
using RsskWorkTeacher.Models;
using RsskWorkTeacher.Models.RP;
using RsskWorkTeacher.ModelsDB;
using RsskWorkTeacher.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RsskWorkTeacher.Wind
{
    /// <summary>
    /// Логика взаимодействия для winAddChangeKtp.xaml
    /// </summary>
    public partial class winAddChangeKtp : Window
    {
        private WorkingProgramViewModel _vm;
        Ktp ktp = new Ktp();
        KtpData working = new();
        public winAddChangeKtp(string ck, string plan, string name)
        {
            InitializeComponent();
            ktp = _db.Ktps.Where(p => p.Ck == ck && p.Plan == plan && p.Name.Contains(name)).FirstOrDefault();
            working = JsonConvert.DeserializeObject<KtpData>(ktp.Ktp1);
          
            this.Loaded += WinAddChangeKtp_Loaded;
        }

        private async void WinAddChangeKtp_Loaded(object sender, RoutedEventArgs e)
        {
           
            for (int i = 0; i <BufApi.УчебныеГруппы.Count; i++)
            {
                string str = BufApi.УчебныеГруппы[i].Description;
                try
                {
                    str = str.Remove(str.IndexOf('/'));
                    if (!cbGroups.Items.Contains(str))
                    {
                        cbGroups.Items.Add(str);
                    }
                }
                catch
                {
                    cbGroups.Items.Add(str);
                }
            }
           
            foreach (var item in BufApi.Сотрудники)
            {
                if (item.Преподаватель == true || item.Мастер == true)
                {
                    cbTeachers.Items.Add( item.Description );
                }
            }
            DataContext = working;
        }


        RsskContext _db = new();
       
        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сгенерировать КТП заново? Текущие изменения будут потеряны.", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _vm.GenerateKtpFromThemes();
            }
        }

        private void BtnAddRow_Click(object sender, RoutedEventArgs e)
        {
            var newRow = new KtpRow();
            var editor = new winAddChangeKtpRow(newRow);
            editor.Owner = this;

            if (editor.ShowDialog() == true)
            {
                var ktpData = this.DataContext as KtpData;
                ktpData?.Rows.Add(newRow);
            }
        }

        // Обработчик кнопки "Редактировать строку"
        private void BtnEditRow_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedRow();
        }

        // Обработчик двойного клика по DataGrid
        private void dgKtp_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditSelectedRow();
        }

        // Общий метод для редактирования
        private void EditSelectedRow()
        {
            if (dgKtp.SelectedItem is KtpRow selectedRow)
            {
                // Передаем копию, чтобы при нажатии "Отмена" данные не изменились
                var rowCopy = selectedRow.Clone();
                var editor = new winAddChangeKtpRow(rowCopy);
                editor.Owner = this;

                if (editor.ShowDialog() == true)
                {
                    // Переносим изменения обратно в оригинальный объект
                    selectedRow.LessonNums = rowCopy.LessonNums;
                    selectedRow.ThemeName = rowCopy.ThemeName;
                    selectedRow.Hours = rowCopy.Hours;
                    selectedRow.Dates = rowCopy.Dates;
                    selectedRow.Type = rowCopy.Type;
                    selectedRow.Aids = rowCopy.Aids;
                    selectedRow.Tasks = rowCopy.Tasks;

                    // Обновляем DataGrid (чтобы UI подхватил изменения)
                    dgKtp.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для редактирования.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (dgKtp.SelectedItem is KtpRow row)
            {
                working.Rows.Remove(row);
            }
        }
        private void BtnExportKtpToWord_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Документ Word (*.docx)|*.docx",
                FileName = $"КТП_{ktp.Name}_{working.Teachers}.docx",
                Title = "Сохранить КТП"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    WorkingProgramViewModel rp = JsonConvert.DeserializeObject<WorkingProgramViewModel>( _db.Rps.Where(p => p.Id == ktp.Id).FirstOrDefault().Rp1);
                    WordKtpExporter.ExportKtpToWord(rp,working, saveFileDialog.FileName);
                    MessageBox.Show("КТП успешно сформирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при генерации КТП: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            ktp.Ktp1= JsonConvert.SerializeObject(DataContext);
            _db.SaveChanges();
            this.Close();
        }
    }
}