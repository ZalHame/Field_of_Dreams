using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Field_of_Dreams
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RandomLetterQuestion();
            LettersAnswer();
        }

        static MongoClient client = new MongoClient();
        static IMongoDatabase database = client.GetDatabase("QuestionsDB");
        static IMongoCollection<Question> collection = database.GetCollection<Question>("Questions");

        static Random rnd = new Random();
        static List<Question> Quest = collection.AsQueryable().ToList<Question>();
        static string ABC = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        static int NumberQuestion = rnd.Next(Quest.Count);

        char[] Letters = new char[ABC.Length]; 
        Button[] ButtonsRandom = new Button[40];
        Button[] ButtonsAnswer = new Button[Quest[NumberQuestion].Answer.Count()];

        private void RandomLetterQuestion()
        {           
            lb_TextQuestion.Content = Quest[NumberQuestion].TextQuestion;

            for (int i = 0; i < ABC.Count(); i++)
            {
                ABC.Split(' ');
                Letters[i] = ABC[i];
            }

            for (int i = 0; i < 40; i++)
            {
                Button button = new Button();
                ButtonsRandom[i] = button;
                button.Click += ButtonClickRandomLetters;
                button.Height = 25;
                button.Width = 25;
                wp_Question_Letters.Children.Add(button);
            }

            
            for (int i = 0; i < Quest[NumberQuestion].Answer.Count(); i++)
            {
                Quest[NumberQuestion].Answer.Split(' ');
                int x = rnd.Next(40);
                if (ButtonsRandom[x].Content == null)
                {
                    ButtonsRandom[x].Content = Quest[NumberQuestion].Answer[i];
                }
                else { i--; }
            }

            for (int i = 0; i < 40; i++)
            {
                if (ButtonsRandom[i].Content == null)
                {
                    ButtonsRandom[i].Content = Letters[rnd.Next(33)];
                }
            }
        }

        private void LettersAnswer()
        {
            for (int i = 0; i < Quest[NumberQuestion].Answer.Count(); i++)
            {
                Button button = new Button();
                ButtonsAnswer[i] = button;
                button.Height = 25;
                button.Width = 25;
                sp_Answer_Letters.Children.Add(button);
            }
        }

        private void ButtonClickRandomLetters(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Quest[NumberQuestion].Answer.Count(); i++)
            {
                if (ButtonsAnswer[i].Content == null)
                {
                    ButtonsAnswer[i].Content = (sender as Button).Content.ToString();
                    (sender as Button).Visibility = Visibility.Hidden;
                    break;
                }
            }
        }

        private void ButtonClickClearAnswerLetters(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Quest[NumberQuestion].Answer.Count(); i++)
            {
                ButtonsAnswer[i].Content = null;
            }

            for (int i = 0; i < 40; i++)
            {
                ButtonsRandom[i].Visibility = Visibility.Visible;
            }
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            int NumberCorrectLetters = Quest[NumberQuestion].Answer.Count();
            char[] check = new char[Quest[NumberQuestion].Answer.Count()];

            for (int i = 0; i < Quest[NumberQuestion].Answer.Count(); i++)
            {
                Quest[NumberQuestion].Answer.Split();
                check[i] = Quest[NumberQuestion].Answer[i];
            }

            for (int i = 0; i < Quest[NumberQuestion].Answer.Count(); i++)
            {
                
                if (check[i] == char.Parse(ButtonsAnswer[i].Content.ToString()))
                {
                    NumberCorrectLetters--;
                }
            }

            if (NumberCorrectLetters == 0)
            {
                MessageBox.Show("Поздравляю! Вы выйграли");
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверно! Попробуйте еще раз");
            }
        }
    }
}
