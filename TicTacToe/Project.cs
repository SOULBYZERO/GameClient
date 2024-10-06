using Common;
using System.Windows.Media.Imaging;

namespace TicTacToe
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "Tic Tac Toe";
        public BitmapImage Image => new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/Assets/TicTacToe.png"));
        public string AppInfo { get; set; } = "Tic Tac Toe Game Instructions:" +
            "\r\r\n\r\r\nWelcome to Tic Tac Toe!" +
            "\r\r\n- Select 'PvP' or 'PvE' mode to start." +
            "\r\r\n- Click on the board to make a move." +
            "\r\r\n- Match three symbols vertically, horizontally, or diagonally to win!" +
            "\r\r\n- Play again to restart the game." +
            "\r\r\n\r\r\nGame Features:" +
            "\r\r\n- Home screen with mode selection (PvP or PvE)." +
            "\r\r\n- Game board with interactive grid for player moves." +
            "\r\r\n- Scoreboard displaying wins for Player X and Player O." +
            "\r\r\n- End screen showing the winner or a tie." +
            "\r\r\n- Computer AI for PvE mode." +
            "\r\r\n\r\r\nImplementation Details:" +
            "\r\r\nThe game logic is handled by the GameLogic class." +
            "\r\r\n- Tracks player turns and board state." +
            "\r\r\n- Determines win conditions and ties." +
            "\r\r\n- Raises events for move made, game ended, and restart." +
            "\r\r\n\r\r\nGraphics and UI:" +
            "\r\r\n- Uses X.png and O.png for player symbols." +
            "\r\r\n- Displays winning line animations." +
            "\r\r\n- Utilizes grids and stack panels for layout management." +
            "\r\r\n\r\nEvent Handling:" +
            "\r\r\n- Handles mouse click events for player moves." +
            "\r\r\n- Updates UI based on game events (move made, game over)." +
            "\r\r\n- Allows resetting the game for a new round." +
            "\r\r\n\r\r\nEnjoy playing Tic Tac Toe!\" ";
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
