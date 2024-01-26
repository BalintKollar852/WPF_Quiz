using System;
using System.Collections.Generic;
using System.IO;
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

namespace WPF_Quiz
{
    /// <summary>
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : Page
    {
        public Results()
        {
            InitializeComponent();
            //A mostani kör eredménye
            Current_resultslabel.Content = $"A játék eredménye ({MainWindow.Topic_text}):";
            Current_resultstext.Text = Quiz.CurrentResults_ToAnotherPage;

            //Eddigi eredmények kiírása
            Previous_resultslabel.Content = $"{MainWindow.Name} eddigi eredményei: ";
            string word = $"Informatika témakörben helyes válaszainak száma: 0/0\n" + "Edzőtererm témakörben helyes válaszainak száma: 0/0\n" + "Gaming témakörben helyes válaszainak száma: 0/0";
            foreach (string line in File.ReadAllLines(@"players.txt"))
            {
                string[] l = line.Split(';');
                if (l[0] == MainWindow.Name)
                {
                    word = String.Empty;
                    word += $"Informatika témakörben helyes válaszainak száma: {l[1]}/{l[2]}\n";
                    word += $"Edzőtererm témakörben helyes válaszainak száma: {l[3]}/{l[4]}\n";
                    word += $"Gaming témakörben helyes válaszainak száma: {l[5]}/{l[6]}\n";
                    word += $"Átlagos válasz idő: {Convert.ToInt32(l[7]) / (Convert.ToInt32(l[2]) + Convert.ToInt32(l[4]) + Convert.ToInt32(l[6]))} másodperc";
                }
            }
            Previous_resultstext.Text = word;

        }
        public void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            /*System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();*/
        }

        private void Current_resultstext_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
