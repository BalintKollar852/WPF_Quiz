using System;
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
        public int timerNumber = 0;
        public Quiz()
        {
            InitializeComponent();
        }
    }
}
