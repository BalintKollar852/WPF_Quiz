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
        public static int Question_number;
        public static string Topic_text;
        public static bool MultipleAnswer;
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
            CheckIfEverythingIsFilled();
        }
        public void MultipleAnswer_YesClick(object sender, RoutedEventArgs e)
        { 
            CheckIfEverythingIsFilled() ;
        }
        public void MultipleAnswer_NoClick(object sender, RoutedEventArgs e)
        {
            CheckIfEverythingIsFilled();
        }
        public void GameClick(object sender, RoutedEventArgs e)
        {
            MultipleAnswer = MultipleAnswer_YesButon.IsChecked == true ? true : false;
            Question_number = Convert.ToInt32(QuestionNumberSlider.Value);
            Name = Convert.ToString(Name_input.Text);
            if (category1_button.IsChecked == true) { Type_number = 0; Topic_text = "Informatika"; }
            else if (category2_button.IsChecked == true) { Type_number = 1; Topic_text = "Edzőterem"; }
            else if (category3_button.IsChecked == true) { Type_number = 2; Topic_text = "Gaming"; }
            MainFrame.Content = new Quiz();
        }
        private void CheckIfEverythingIsFilled()
        {
            if (Name_input.Text.Length > 0 && Regex.IsMatch(Name_input.Text, @"^[a-záéúőóüö A-ZZÁÉÚŐÓÜÖÍ]+$") && (MultipleAnswer_YesButon.IsChecked == true || MultipleAnswer_NoButon.IsChecked == true))
            {
                GameStart_Button.IsEnabled = true;
            }
            else
            {
                GameStart_Button.IsEnabled = false;
            }
        }
    }
}
