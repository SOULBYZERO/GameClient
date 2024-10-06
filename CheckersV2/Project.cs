using Common;
using System.Windows.Media.Imaging;

namespace CheckersV2
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Checkers";
        public BitmapImage Image => new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/Assets/Checkers.png"));
        public string AppInfo { get; set; } = "User Manual:\r\nLaunch the game to start playing. Black always starts first. " +
            "\r\nTo move a piece, click on it and then click on the target square. " +
            "\r\nYou can capture opponent's pieces by jumping over them diagonally. " +
            "\r\nReach the opposite end of the board to crown your piece as a 'King', " +
            "\r\nallowing it to move diagonally both forward and backward. " +
            "\r\nThe game ends when a player captures all opponent's pieces or no valid moves are left. " +
            "\r\nThe winner is displayed on the game over screen, and you can restart the game by clicking 'Play Again'." +
            "\r\n\r\nFunctionality Explanation:\r\nThe game board is displayed using a Grid with an ImageBrush background." +
            " \r\nPlayer turns are managed through a display that updates to show the current player." +
            " \r\nThe scoreboard keeps track of captured pieces for each player." +
            " \r\nMouse click events handle piece selection and movement on the board." +
            " \r\nGame logic, encapsulated in the GameLogic class, validates moves, manages piece capturing, and handles game ending conditions." +
            " \r\nThe game over screen displays the winner and offers a restart option." +
            " \r\nGame state management includes methods for making moves, switching players, and resetting the game.";

        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
