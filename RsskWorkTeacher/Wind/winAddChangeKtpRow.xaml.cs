using System.Windows;
using RsskWorkTeacher.Models;

namespace RsskWorkTeacher.Wind
{
    public partial class winAddChangeKtpRow : Window
    {
        private KtpRow _row;

        public winAddChangeKtpRow(KtpRow row)
        {
            InitializeComponent();
            _row = row;
            this.DataContext = _row;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Простейшая валидация
            if (string.IsNullOrWhiteSpace(_row.ThemeName))
            {
                MessageBox.Show("Наименование темы не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}