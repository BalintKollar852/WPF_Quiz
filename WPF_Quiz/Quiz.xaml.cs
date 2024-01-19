using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        public int AverageTimeSum { get; private set; }
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
            AverageTimeSum = Convert.ToInt32(s[7]);
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
        public string ImageSource { get; private set; }
        public Questions(string line)
        {
            string[] s = line.Split(';');
            Question = s[2];
            CorrectNumber = Convert.ToInt32(s[1]);
            Answer1 = s[3];
            Answer2 = s[4];
            Answer3 = s[5];
            Answer4 = s[6];
            ImageSource = s.ElementAtOrDefault(7);
        }
    }
    public partial class Quiz : Page
    {
        public static string CurrentResults_ToAnotherPage;
        private Random rng = new Random();
        private float timerNumber = 30;
        private int CurrentQuestionNumber = 0;
        private List<Questions> shuffledquestions = new List<Questions>();
        private List<RadioButton> Answer_buttonslist;
        private int correctanswer_num = 0;
        private string CurrentResults;
        private DispatcherTimer timer;
        private List<int> timesateachr = new List<int>();
        private static int alreadyExecuted = 0;

        public Quiz()
        {
            InitializeComponent();

            //Ez arra kell hogy futtassuk minden masodpercben a metodot
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            //A válaszok RadioButton-jét egy listába rakom hogy ne kelljen egyesével írni rájuk az utasításokat
            Answer_buttonslist = new List<RadioButton>
            {
                Answer1_button,
                Answer2_button,
                Answer3_button,
                Answer4_button
             };

            //Eddigi eredmények kiírása
            Previous_resultslabel.Content = $"{MainWindow.Name} eddigi eredményei: ";
            string word =  $"Informatika témakörben helyes válaszainak száma: 0/0\n" + "Edzőtererm témakörben helyes válaszainak száma: 0/0\n" + "Gaming témakörben helyes válaszainak száma: 0/0";
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

            //A témakör kiírása
            Topic_text.Text = MainWindow.Topic_text;

            //Kérdések random kiválasztása
            List<Questions> questionslist = new List<Questions>();
            foreach (string line in File.ReadAllLines(@"questions.txt"))
            {
                string[] l = line.Split(';');
                if (MainWindow.MultipleAnswer)
                {
                    if (l[0] == "i" && MainWindow.Type_number == 0) questionslist.Add(new Questions(line));
                    if (l[0] == "e" && MainWindow.Type_number == 1) questionslist.Add(new Questions(line));
                    if (l[0] == "g" && MainWindow.Type_number == 2) questionslist.Add(new Questions(line));
                }
                else if (l[1].Count() == 1)
                {
                    if (l[0] == "i" && MainWindow.Type_number == 0) questionslist.Add(new Questions(line));
                    if (l[0] == "e" && MainWindow.Type_number == 1) questionslist.Add(new Questions(line));
                    if (l[0] == "g" && MainWindow.Type_number == 2) questionslist.Add(new Questions(line));
                }
            }

            //Ezek már a randomizált kérdések/válaszok a beolvasott adatokból a menüben kiválasztott darab számba
            shuffledquestions = (List<Questions>)questionslist.OrderBy(a => rng.Next()).Take(MainWindow.Question_number).ToList();
            QuestionsAndAnswersLoad();

        }
        //ez fut le folyamatosan (masodperc megfigyelesem szerint)
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timerNumber > 0 && Answer_buttonslist.Count(b => b.IsChecked == true) != shuffledquestions[CurrentQuestionNumber - 1].CorrectNumber.ToString().Length) timerLabel.Content = timerNumber--;
            else if (Answer_buttonslist.All(a => a.IsEnabled == true))
            {
                QuizGame(new List<int> {0}, true); timerLabel.Content = "Az idő lejárt!";
                timesateachr.Add(0);
            }
            else
            {
                if (alreadyExecuted == 0) { timesateachr.Add(Convert.ToInt32(29 - timerNumber)); alreadyExecuted++; }
            }
        }

        //Kérdéseket és válaszokat betölti
        private void QuestionsAndAnswersLoad()
        {
            timerNumber = 29;
            alreadyExecuted = 0;
            timerLabel.Content = "30";
            CurrentQuestionNumber++;
            Questions_text.Text = shuffledquestions[CurrentQuestionNumber - 1].Question;
            Answer1_button.Content = shuffledquestions[CurrentQuestionNumber - 1].Answer1;
            Answer2_button.Content = shuffledquestions[CurrentQuestionNumber - 1].Answer2;
            Answer3_button.Content = shuffledquestions[CurrentQuestionNumber - 1].Answer3;
            Answer4_button.Content = shuffledquestions[CurrentQuestionNumber - 1].Answer4;
            //Kép beillesztése ha van
            QuestionImage.Source = new BitmapImage(new Uri($"{shuffledquestions[CurrentQuestionNumber - 1].ImageSource}", UriKind.RelativeOrAbsolute));
            Image_Grid.SetValue(Grid.ColumnProperty, shuffledquestions[CurrentQuestionNumber - 1].ImageSource != null ? 0 : 1);
            Answers_Grid.SetValue(Grid.ColumnProperty, shuffledquestions[CurrentQuestionNumber - 1].ImageSource != null ? 1 : 0);
            //Leellenőrzi hogy több válasz is lehet-e
            if (shuffledquestions[CurrentQuestionNumber - 1].CorrectNumber.ToString().Length > 1)
            {
                QuestionLabel.Content = $"Kérdés ({CurrentQuestionNumber}/{MainWindow.Question_number}) - Több válaszos kérdés";
                //Mivel a többválaszos kérdés opció be van kapcsolva akkor, a RadioButtons-t beállítja külön GroupName-ekre hogy rálehessen nyomni többre is
                Answer_buttonslist.ForEach(b => b.GroupName = b.Name);
            }
            else
            {
                QuestionLabel.Content = $"Kérdés ({CurrentQuestionNumber}/{MainWindow.Question_number})";
                //Vissza/beállítja a RadionButtons GroupName-ét azonosra hogy csak az egyik-re lehessen nyomni
                Answer_buttonslist.ForEach(b => b.GroupName = "AnswerButtonsGroup");
            }
            //Ellenőrzi hogy a jelenlegi krédés nem érte-e el a menüben kiválasztott kérdés db számot, ha eléri akkor kikapcsolja a 'Következő kérdés' gombot
            if (CurrentQuestionNumber < MainWindow.Question_number)
            {
                NextQuestion_button.Content = $"Következő kérdés ({CurrentQuestionNumber + 1}/{MainWindow.Question_number})";
            }
            else
            {
                NextQuestion_button.IsEnabled = false;
            }
        }

        private void QuizGame(List<int> answer_number, bool timer_passed)
        {
            string correctanswertext = "";
            //Kikapcsolja a RadioButtons-t
            for (int i = 0; i < Answer_buttonslist.Count(); i++)
            {
                Answer_buttonslist[i].IsEnabled = false ;
            }
            //Megnézi hogy a kiválasztott válasz egyezik-e a helyes válasszal és zölddel jelzi az helyességét, pirossal pedig ha téves volt
            if (shuffledquestions[CurrentQuestionNumber - 1].CorrectNumber == int.Parse(string.Concat(answer_number)))
            {

                for (int i = 0; i < answer_number.Count(); i++)
                {
                    Answer_buttonslist[answer_number[i] - 1].Foreground = Brushes.LawnGreen;
                    correctanswertext += Convert.ToString(Answer_buttonslist[i].Content);
                }
                correctanswer_num++;
            }
            else
            {
                if (!timer_passed)
                {
                    for (int i = 0; i < answer_number.Count(); i++)
                    {
                        Answer_buttonslist[answer_number[i] - 1].Foreground = Brushes.Red;
                    }
                }
                //Ha lejár az idő az inkorrekt válaszokat pirosra rakja
                else
                {
                    for (int i = 0; i < Answer_buttonslist.Count; i++)
                    {
                        if (!shuffledquestions[CurrentQuestionNumber - 1].CorrectNumber.ToString().Contains($"{i + 1}"))
                        {
                            Answer_buttonslist[i].Foreground = Brushes.Red;
                        }
                    }
                }
                for (int i = 0; i < Answer_buttonslist.Count; i++)
                {
                    if (shuffledquestions[CurrentQuestionNumber - 1].CorrectNumber.ToString().Contains($"{i + 1}"))
                    {
                        Answer_buttonslist[i].Foreground = Brushes.LawnGreen;
                        correctanswertext += Convert.ToString(Answer_buttonslist[i].Content);
                    }
                }
            }

            //A jelenlegi eredményt elmenti egy string változóban amit majd a Result oldadlon felhasználunk
            CurrentResults += $"{CurrentQuestionNumber}.kérdés: {Questions_text.Text} - Helyes válasz: {correctanswertext} - Adott válasz: {(shuffledquestions[CurrentQuestionNumber - 1].CorrectNumber == int.Parse(string.Concat(answer_number)) ? "jó" : "rossz")}\n";

            //Hogyha az utolsó kérdésre is lett válasz adva akkor az 'Eredmények' gombot bekapcsolja
            if (CurrentQuestionNumber >= MainWindow.Question_number && Answer_buttonslist.All(b=>b.IsEnabled == false))
            {
                WriteToFile(timesateachr.Sum());
                CurrentResults_ToAnotherPage = CurrentResults + $"Összesen {correctanswer_num} jó választ adott.\nÁtlagos válasz idő: {timesateachr.Average()} másodperc.";
                Results_button.IsEnabled = true ;
            }
            else if (Answer_buttonslist.All(b => b.IsEnabled == false))
            {
                NextQuestion_button.IsEnabled = true ;
            }
        }

        //Következő kérdésre lép a 'QuestionsAndAnswersLoad' metódus segítségével
        public void NextQuestionClick(object sender, RoutedEventArgs e)
        {
            //Kikapcsolja a következő válasz gombot (hogy, ameddig nem válaszoltál addig ne lehessen a következő kérdésre lépni)
            NextQuestion_button.IsEnabled = false;
            //Kitörli a bejelölt RadioButtont, visszakapcsolja a RadioButtons-t és fehérre állítja a szövegét
            for (int i = 0; i < Answer_buttonslist.Count(); i++)
            {
                Answer_buttonslist[i].IsChecked = false;
                Answer_buttonslist[i].IsEnabled = true;
                Answer_buttonslist[i].Foreground = Brushes.White;
            }
            QuestionsAndAnswersLoad();
        }

        private void CheckMultipleAnswerButton(int answer_number)
        {
            if (shuffledquestions[CurrentQuestionNumber - 1].CorrectNumber.ToString().Length == 1)
            {
                QuizGame(new List<int> { answer_number }, false);
            }
            else if (Answer_buttonslist.Count(b => b.IsChecked == true) == shuffledquestions[CurrentQuestionNumber - 1].CorrectNumber.ToString().Length)
            {
                List<int> answerindexlist = new List<int>();
                for (int i = 0; i < Answer_buttonslist.Count(); i++)
                {
                    if (Answer_buttonslist[i].IsChecked == true)
                    {
                        answerindexlist.Add(i + 1);
                    }
                }
                QuizGame(answerindexlist, false);
            }
        }

        //Fájlba írás a játékos adatait (csak akkor menti el ha az összes kérdés véget ért)
        public void WriteToFile(int averagetime)
        {
            List<Players> playerslist = new List<Players>();
            string fullPath = $"players.txt";
            foreach (string line in File.ReadAllLines(@"players.txt"))
            {
                playerslist.Add(new Players(line));
            }
            if (Convert.ToInt32(playerslist.Where(j => j.Name == MainWindow.Name).Count()) >= 1)
            {
                List<string> lines = new List<string>();
                switch (MainWindow.Type_number)
                {
                    case 0:
                        playerslist.Where(j => j.Name == MainWindow.Name).ToList().ForEach(j => { lines.Add($"{j.Name};{j.Category1GuessedNumber + correctanswer_num};{j.Category1QuestionsNumber + MainWindow.Question_number};{j.Category2GuessedNumber};{j.Category2QuestionsNumber};{j.Category3GuessedNumber};{j.Category3QuestionsNumber};{j.AverageTimeSum + averagetime}"); });
                        break;
                    case 1:
                        playerslist.Where(j => j.Name == MainWindow.Name).ToList().ForEach(j => { lines.Add($"{j.Name};{j.Category1GuessedNumber};{j.Category1QuestionsNumber};{j.Category2GuessedNumber + correctanswer_num};{j.Category2QuestionsNumber + MainWindow.Question_number};{j.Category3GuessedNumber};{j.Category3QuestionsNumber};{j.AverageTimeSum + averagetime}"); });
                        break;
                    case 2:
                        playerslist.Where(j => j.Name == MainWindow.Name).ToList().ForEach(j => { lines.Add($"{j.Name};{j.Category1GuessedNumber};{j.Category1QuestionsNumber};{j.Category2GuessedNumber};{j.Category2QuestionsNumber};{j.Category3GuessedNumber + correctanswer_num};{j.Category3QuestionsNumber + MainWindow.Question_number};{j.AverageTimeSum + averagetime}"); });
                        break;
                }
                playerslist.Where(j => j.Name != MainWindow.Name).ToList().ForEach(j => { lines.Add($"{j.Name};{j.Category1GuessedNumber};{j.Category1QuestionsNumber};{j.Category2GuessedNumber};{j.Category2QuestionsNumber};{j.Category3GuessedNumber};{j.Category3QuestionsNumber};{j.AverageTimeSum}"); });
                File.WriteAllLines(fullPath, lines);
            }
            else
            {
                List<string> lines = new List<string>();
                switch (MainWindow.Type_number)
                {
                    case 0:
                        lines.Add($"{MainWindow.Name};{correctanswer_num};{MainWindow.Question_number};{0};{0};{0};{0};{averagetime}");
                        break;
                    case 1:
                        lines.Add($"{MainWindow.Name};{0};{0};{correctanswer_num};{MainWindow.Question_number};{0};{0};{averagetime}");
                        break;
                    case 2:
                        lines.Add($"{MainWindow.Name};{0};{0};{0};{0};{correctanswer_num};{MainWindow.Question_number};{averagetime}");
                        break;
                }
                playerslist.Where(j => j.Name != MainWindow.Name).ToList().ForEach(j => { lines.Add($"{j.Name};{j.Category1GuessedNumber};{j.Category1QuestionsNumber};{j.Category2GuessedNumber};{j.Category2QuestionsNumber};{j.Category3GuessedNumber};{j.Category3QuestionsNumber};{j.AverageTimeSum}"); });
                File.WriteAllLines(fullPath, lines);
            }
        }
        public void Answer1_Click(object sender, RoutedEventArgs e)
        {
            CheckMultipleAnswerButton(1);
        }
        public void Answer2_Click(object sender, RoutedEventArgs e)
        {

            CheckMultipleAnswerButton(2);
        }
        public void Answer3_Click(object sender, RoutedEventArgs e)
        {
            CheckMultipleAnswerButton(3);
        }
        public void Answer4_Click(object sender, RoutedEventArgs e)
        {
            CheckMultipleAnswerButton(4);
        }
        public void Results_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Results();
        }
    }
}
