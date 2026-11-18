using RsskWorkTeacher.Models.RP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Thema.xaml
    /// </summary>
    public partial class Thema : Window
    {
        public ThemePlan Theme { get; private set; }
        public ObservableCollection<CompetenceSelection> AvailableCompetences { get; set; } = new();

        public Thema(ThemePlan theme = null)
        {
            InitializeComponent();

            if (theme == null)
            {
                Theme = new ThemePlan();
                this.Title = "Добавление темы";
            }
            else
            {
                Theme = theme.Clone();
                this.Title = "Редактирование темы";
            }

            // Формируем список доступных компетенций (основываясь на вашем документе)
            if(AvailableCompetences == null) 
            AvailableCompetences = new ObservableCollection<CompetenceSelection>
            {
                new CompetenceSelection { Name = "ОК 01" },
                new CompetenceSelection { Name = "ОК 02" },
                new CompetenceSelection { Name = "ОК 03" },
                new CompetenceSelection { Name = "ОК 04" },
                new CompetenceSelection { Name = "ОК 05" },
                new CompetenceSelection { Name = "ОК 06" },
                new CompetenceSelection { Name = "ОК 07" },
                new CompetenceSelection { Name = "ОК 08" },
                new CompetenceSelection { Name = "ОК 09" },
                new CompetenceSelection { Name = "ПК 1.1" },
                new CompetenceSelection { Name = "ПК 1.2" }
            };

            // Если компетенции уже заданы, отмечаем нужные чекбоксы
            if (!string.IsNullOrWhiteSpace(Theme.Competences))
            {
                var selectedComps = Theme.Competences.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < selectedComps.Length; i++)
                {
                    selectedComps[i] = selectedComps[i].Trim();
                }
                foreach (var comp in AvailableCompetences)
                {
                    if (selectedComps.Contains(comp.Name.Trim()))
                    {
                        comp.IsSelected = true;
                    }
                }
            }

            // Устанавливаем источник данных для ComboBox
            cbCompetences.ItemsSource = AvailableCompetences;

            this.DataContext = Theme;
        }

        // Обработчик, который переключает чекбокс и НЕ дает ComboBox закрыться
        private void ComboBoxItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is ComboBoxItem item && item.DataContext is CompetenceSelection comp)
            {
                comp.IsSelected = !comp.IsSelected;
                e.Handled = true;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Theme.ThemeName))
            {
                MessageBox.Show("Введите наименование темы/раздела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            

            if (!Theme.IsSection) // Если это не раздел, то собираем компетенции
            {
                var selectedComps = AvailableCompetences.Where(x => x.IsSelected).Select(x => x.Name);
                Theme.Competences = string.Join(", ", selectedComps);
            }

            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

         private void dgContentBlocks_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Используем Dispatcher, чтобы дождаться, пока WPF применит введенное значение к модели
            Dispatcher.BeginInvoke(new Action(() => 
            {
                CalculateTotalHours();
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        // Метод подсчета суммы часов
        private void CalculateTotalHours()
        {
            if (Theme == null || Theme.IsSection) return;

            int total = 0;
            foreach (var block in Theme.ContentBlocks)
            {
                // Пытаемся перевести строку в число и суммируем
                if (int.TryParse(block.Hours, out int h))
                {
                    total += h;
                }
            }

            // Записываем результат в модель
            Theme.TotalHours = total.ToString();

            // Принудительно обновляем TextBox на экране
            if (tbTotalHours != null)
            {
                tbTotalHours.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            }
        }
        private void HoursTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, что вводимый символ является цифрой
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            if (!e.Handled)
            {

            }
        }
        private void dgContentBlocks_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, что нажат именно Ctrl+Enter
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                // Находим TextBox, в котором сейчас редактируется текст
                if (Keyboard.FocusedElement is TextBox textBox)
                {
                    int caretIndex = textBox.CaretIndex;

                    // Вставляем перенос строки в текущую позицию курсора
                    textBox.Text = textBox.Text.Insert(caretIndex, Environment.NewLine);

                    // Сдвигаем курсор после переноса
                    textBox.CaretIndex = caretIndex + Environment.NewLine.Length;

                    // Говорим WPF, что событие обработано (чтобы DataGrid не перескочил на новую строку)
                    e.Handled = true;
                }
            }
        }
    }
}
    