using RsskWorkTeacher.Models;
using RsskWorkTeacher.ModelsDB;
using RsskWorkTeacher.Wind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RsskWorkTeacher.Pages
{
    /// <summary>
    /// Логика взаимодействия для pgKtpWork.xaml
    /// </summary>
    public partial class pgKtpWork : Page
    {
        RsskContext _db = new();
        public pgKtpWork()
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

        private void OpenWindowAddChange(object sender, RoutedEventArgs e)
        {
            winAddChangeKtp winAddChangeKtp = new winAddChangeKtp(cbCik.Text, cbPlan.Text, lbSpisokKTP.SelectedItem.ToString());
            winAddChangeKtp.ShowDialog();
        }

        private void cbCik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbSpisokKTP.Items.Clear();
            if (cbPlan.Text == "")
                foreach (var item in _db.Ktps)
                {
                    if (item.Ck == ((ComboBoxItem)e.AddedItems[0]).Content.ToString())
                    {
                        lbSpisokKTP.Items.Add(item.Name);
                    }
                }
            else
            {
                foreach (var item in _db.Ktps)
                {
                    if (item.Plan == cbPlan.Text.ToString() && item.Ck == ((ComboBoxItem)e.AddedItems[0]).Content.ToString())
                    {
                        lbSpisokKTP.Items.Add(item.Name);
                    }
                }
            }
        }

        private void cbPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbSpisokKTP.Items.Clear();
            if (cbCik.Text == "")
                foreach (var item in _db.Ktps)
                {
                    if (item.Plan == ((ComboBoxItem)e.AddedItems[0]).Content.ToString())
                    {
                        lbSpisokKTP.Items.Add(item.Name);
                    }
                }
            else
            {
                foreach (var item in _db.Ktps)
                {
                    if (item.Ck == cbCik.Text.ToString() && item.Plan == ((ComboBoxItem)e.AddedItems[0]).Content.ToString())
                    {
                        lbSpisokKTP.Items.Add(item.Name);
                    }
                }
            }

        }

        private void lbSpisokRp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
