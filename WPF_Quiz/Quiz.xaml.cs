using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    class Questions
    {
        public string Question { get; private set; }
        public int CorrectNumber { get; private set; }
        public string Answer1 { get; private set; }
        public string Answer2 { get; private set; }
        public string Answer3 { get; private set; }
        public string Answer4 { get; private set; }
        public Questions(string line)
        {
            string[] s = line.Split(';');
            Question = s[2];
            CorrectNumber = Convert.ToInt32(s[1]);
            Answer1 = s[3];
            Answer2 = s[4];
            Answer3 = s[5];
            Answer4 = s[6];
        }
    }
    public partial class Quiz : Page
    {
        private Random rng = new Random();
        private float timerNumber = 30;
        public Quiz()
        {
            InitializeComponent();

            //Ez arra kell hogy futtassuk minden masodpercben a metodot
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += dispatcherTimer_Tick;
            timer.Start();
            /*
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
            /* foreach (string sor in File.ReadAllLines(@"questions.txt"))
             {
                 string[] s = sor.Split(';');
                 if (s[0] == "i" && MainWindow.Type_number == 0) cat1questions.Add(s);
                 if (s[0] == "e" && MainWindow.Type_number == 1) cat2questions.Add(s);
                 if (s[0] == "g" && MainWindow.Type_number == 2) cat3questions.Add(s);
             }*/
            List<Questions> questionslist = new List<Questions>();
            foreach (string sor in File.ReadAllLines(@"questions.txt"))
            {
                string[] s = sor.Split(';');
                if (s[0] == "i" && MainWindow.Type_number == 0) questionslist.Add(new Questions(sor));
                if (s[0] == "e" && MainWindow.Type_number == 1) questionslist.Add(new Questions(sor));
                if (s[0] == "g" && MainWindow.Type_number == 2) questionslist.Add(new Questions(sor));
            }
            List<Questions> shuffledquestions = new List<Questions>();
            shuffledquestions = (List<Questions>)questionslist.OrderBy(a => rng.Next()).Take(10).ToList();
            //Ez már randomizált kérdés a beolvasott adatokból és 10db van elvéve belőle(ezt késöbb akár változtathatóra is lehet majd csinálni)
            Questions_text.Text = shuffledquestions[0].Question;


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
            }*/
        }
        //ez fut le folyamatosan (masodperc megfigyelesem szerint)
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (timerNumber >= 0) { timerLabel.Content = timerNumber--; Passed(); }
        }
        // ez akkor fut le ha idozitonek vege
        private void Passed()
        {

        }
    }
}
