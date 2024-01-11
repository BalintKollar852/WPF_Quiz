using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Quiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int Type_number;
        public static string Name;
        public MainWindow()
        {
            InitializeComponent();
            foreach (string sor in File.ReadAllLines(@"players.txt"))
            {
                string[] s = sor.Split(';');
                Name_input.Items.Add(s[0]);
            }
        }
        private void ComboBoxName_TextChanged(object sender, EventArgs e)
        {
            if (Name_input.Text.Length > 0 && Regex.IsMatch(Name_input.Text, @"^[a-záéúőóüö A-ZZÁÉÚŐÓÜÖÍ]+$"))
            {
                GameStart_Button.IsEnabled = true;
            }
            else
            {
                GameStart_Button.IsEnabled = false;
            }
        }
        public void GameClick(object sender, RoutedEventArgs e)
        {
            Name = Name_input.Text;
            if (category1_button.IsChecked == true) Type_number = 0;
            else if (category2_button.IsChecked == true) Type_number = 1;
            else if (category3_button.IsChecked == true) Type_number = 2;
            MainFrame.Content = new Quiz();
        }
    }
}
