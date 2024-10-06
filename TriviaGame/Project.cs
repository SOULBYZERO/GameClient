using Common;
using System.Windows.Media.Imaging;

namespace TriviaGame
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Trivia";
        public BitmapImage Image => new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/Assets/TriviaGame.webp"));
        public string AppInfo { get; set; } = "Welcome to the Trivia Game!" +
            "\n\nInstructions:" +
            "\nAnswer 10 trivia questions correctly to win! Read the question and choose the correct answer. " +
            "\nClick 'Select Answer' to confirm your choice. Green indicates correct answers, and red indicates wrong ones. " +
            "\nThe game ends after 10 questions with your score displayed." +
            "\n\nFeatures:" +
            "\nDynamic loading and display of trivia questions. Randomized question order for each session. " +
            "\nInteractive UI with radio button selections. Visual feedback on correct and incorrect answers. " +
            "\nEnd screen showing total correct answers out of 10." +
            "\n\nEnjoy the Trivia Game!";
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
