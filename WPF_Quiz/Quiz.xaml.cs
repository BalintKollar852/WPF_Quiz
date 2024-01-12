using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_Quiz
{
    /// <summary>
    /// Interaction logic for Quiz.xaml
    /// </summary>
    class Players
    {
        public string Name { get; private set; }
        public int Category1GuessedNumber { get; private set; }
        public int Category1QuestionsNumber { get; private set; }
        public int Category2GuessedNumber { get; private set; }
        public int Category2QuestionsNumber { get; private set; }
        public int Category3GuessedNumber { get; private set; }
        public int Category3QuestionsNumber { get; private set; }
        public Players(string line)
        {
            string[] s = line.Split(';');
            Name = s[0];
            Category1GuessedNumber = Convert.ToInt32(s[1]);
            Category1QuestionsNumber = Convert.ToInt32(s[2]);
            Category2GuessedNumber = Convert.ToInt32(s[3]);
            Category2QuestionsNumber = Convert.ToInt32(s[4]);
            Category3GuessedNumber = Convert.ToInt32(s[5]);
            Category3QuestionsNumber = Convert.ToInt32(s[6]);
        }
    }
    public partial class Quiz : Page
    {
        private List<string> cat1questions = new List<string>();
        private List<string> cat2questions = new List<string>();
        private List<string> cat3questions = new List<string>();
        public Quiz()
        {
            InitializeComponent();
            Previous_resultslabel.Content = $"{MainWindow.Name} eddigi eredményei: ";
            foreach (string sor in File.ReadAllLines(@"players.txt"))
            {
                string[] s = sor.Split(';');
                string szo = "";
                if (s[0] == MainWindow.Name)
                {
                    szo += $"Informatika témakörben nyert {s[1]}, vesztett {s[2]} játékot. \n";
                    szo += $"Edzőterem témakörben nyert {s[3]}, vesztett {s[4]} játékot. \n";
                    szo += $"Gaming témakörben nyert {s[5]}, vesztett {s[6]} játékot.";
                }
                else
                {
                    szo += $"Informatika témakörben nyert 0, vesztett 0 játékot. \n";
                    szo += $"Edzőterem témakörben nyert 0, vesztett 0 játékot. \n";
                    szo += $"Gaming témakörben nyert 0, vesztett 0 játékot.";
                }
                Previous_resultstext.Text = szo;
            }
            foreach (string sor in File.ReadAllLines(@"szavak.txt"))
            {
                string[] s = sor.Split(';');
                if (s[1] == "i") cat1questions.Add(s[0]);
                if (s[1] == "e") cat2questions.Add(s[0]);
                if (s[1] == "g") cat3questions.Add(s[0]);
            }

            switch (MainWindow.Type_number)
            {
                case 0:
                    Topic_text.Text = "Informatika";
                    break;
                case 1:
                    Topic_text.Text = "Edzőterem";
                    break;
                case 2:
                    Topic_text.Text = "Gaming";
                    break;
            }
        }
    }
}
