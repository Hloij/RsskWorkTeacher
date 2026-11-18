using RsskWorkTeacher.Models;
using RsskWorkTeacher.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RsskWorkTeacher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!await BufApi.Conect())
            {
                MessageBox.Show($"Не удалось загрузить информацию из 1С: \nПерезапустите 1С");
                Close();
            }
            else
            {
                btnKtp.IsEnabled = true;
                btnProf.IsEnabled = true;
                btnRp.IsEnabled = true;
                _pgProfil = new();
                _pgKtpWork = new();
                _pgRpWork = new();
            }
        }

        pgProfil  _pgProfil;
        pgKtpWork _pgKtpWork;
        pgRpWork  _pgRpWork;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch((sender as Button).Content)
            {
                case "Профиль":frMain.Navigate(_pgProfil);break;
                case "Работа с РП": frMain.Navigate(_pgRpWork);break;
                case "Работа с КТП": frMain.Navigate(_pgKtpWork);break;
                  
            }
        }
    }
}