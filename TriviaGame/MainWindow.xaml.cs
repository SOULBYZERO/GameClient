using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TriviaGame.Models;

namespace TriviaGame
{
    public partial class MainWindow : Window
    {
        private List<Question> questions;
        private int currentQuestionIndex;
        private List<Question> shuffledQuestions;
        private int correctAnswerCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            LoadQuestions();
            ShuffleQuestions();
            DisplayQuestion();
        }

        private void LoadQuestions()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions.txt");
            string json = File.ReadAllText(filePath);
            questions = JsonSerializer.Deserialize<List<Question>>(json);
        }

        private void ShuffleQuestions()
        {
            shuffledQuestions = questions.OrderBy(q => Guid.NewGuid()).Take(10).ToList();
            currentQuestionIndex = 0; 
        }

        private void DisplayQuestion()
        {
            if (currentQuestionIndex < shuffledQuestions.Count)
            {
                Question currentQuestion = shuffledQuestions[currentQuestionIndex];
                txtQuestion.Text = currentQuestion.QuestionText;
                A1.Content = currentQuestion.Answers[0];
                A2.Content = currentQuestion.Answers[1];
                A3.Content = currentQuestion.Answers[2];
                A4.Content = currentQuestion.Answers[3];

                A1.IsChecked = false;
                A2.IsChecked = false;
                A3.IsChecked = false;
                A4.IsChecked = false;

                A1.IsEnabled = true;
                A2.IsEnabled = true;
                A3.IsEnabled = true;
                A4.IsEnabled = true;
                btnNext.IsEnabled = true;

                currentQuestionIndex++;
            } else
            {
                QuestionsArea.Visibility = Visibility.Hidden;
                txtQuestion.Visibility = Visibility.Hidden;
                btnNext.Visibility = Visibility.Hidden;

                string displayCount = correctAnswerCount.ToString();
                EndScreen.Visibility = Visibility.Visible;
                CorrectCount.Text = displayCount + "/10";
            }
        }
        private async void btnConfirmAnswer_Click(object sender, RoutedEventArgs e)
        {
            RadioButton selectedRadioButton = null;
            if (A1.IsChecked == true) selectedRadioButton = A1;
            else if (A2.IsChecked == true) selectedRadioButton = A2;
            else if (A3.IsChecked == true) selectedRadioButton = A3;
            else if (A4.IsChecked == true) selectedRadioButton = A4;

            if (selectedRadioButton != null)
            {
                A1.IsEnabled = false;
                A2.IsEnabled = false;
                A3.IsEnabled = false;
                A4.IsEnabled = false;
                btnNext.IsEnabled = false;

                string selectedAnswer = selectedRadioButton.Content.ToString();
                string correctAnswer = shuffledQuestions[currentQuestionIndex - 1].CorrectAnswer;

                if (selectedAnswer == correctAnswer)
                {
                    selectedRadioButton.Background = Brushes.LightGreen;
                    correctAnswerCount++;
                }
                else
                {
                    selectedRadioButton.Background = Brushes.Red;
                }

                await Task.Delay(2000);

                selectedRadioButton.Background = Brushes.Blue;
                DisplayQuestion();
            }
        }
    }
}