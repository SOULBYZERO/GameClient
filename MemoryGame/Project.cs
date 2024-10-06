using Common;
using System.Windows.Media.Imaging;

namespace MemoryGame
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "MemoryGame";
        public BitmapImage Image => new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/Assets/MemoryGame.png"));
        public string AppInfo { get; set; } = "User Manual:" +
            "\r\nMemorize the sequence of buttons that light up, then press them in order. " +
            "\r\nEvery time you finish the pattern, it gets longer. " +
            "\r\nMake 5 mistake, and the test is over. ";

        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}