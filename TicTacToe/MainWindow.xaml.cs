using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TicTacToe.Models;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> sources = new()
        {
            { GridValue.X, new BitmapImage(new Uri("pack://application:,,,/TicTacToe;component/Assets/X.png")) },
            { GridValue.O, new BitmapImage(new Uri("pack://application:,,,/TicTacToe;component/Assets/O.png")) }
        };

        private readonly Image[,] gridImages = new Image[3, 3];
        private readonly GameLogic gameState = new GameLogic();
        private readonly Random random = new();

        private int xWinCounter = 0;
        private int oWinCounter = 0;

        private bool isPvE = false;

        public MainWindow()
        {
            InitializeComponent();
            CreateEmptyBoard();

            gameState.MoveMade += OnMoveMade;
            gameState.GameEnded += OnGameOver;
            gameState.Restart += OnRestart;
        }

        private void CreateEmptyBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Image gridImage = new Image();
                    Grid.SetRow(gridImage, row);
                    Grid.SetColumn(gridImage, col);
                    GameBoardGrid.Children.Add(gridImage);
                    gridImages[row, col] = gridImage;
                }
            }
        }

        private void DisplayGameOver(string text, ImageSource winner)
        {
            TurnDisplay.Visibility = Visibility.Hidden;
            GameBoardGrid.Visibility = Visibility.Hidden;
            EndScreen.Visibility = Visibility.Visible;
            ResultImage.Source = winner;
            ResultText.Text = text;
        }

        private void DisplayGameBoard()
        {
            EndScreen.Visibility = Visibility.Hidden;
            TurnDisplay.Visibility = Visibility.Visible;
            GameBoardGrid.Visibility = Visibility.Visible;
        }

        private void OnRestart()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    gridImages[row, col].Source = null;
                }
            }
            WinLine.Visibility = Visibility.Hidden;
            DisplayGameBoard();
        }

        private void OnMoveMade(int row, int col)
        {
            GridValue player = gameState.GridBuild[row, col];
            gridImages[row, col].Source = sources[player];
            TurnImage.Source = sources[gameState.CurrentPlayer];
            if (isPvE && gameState.CurrentPlayer == GridValue.O)
            {
                ComputerMove();
            }
        }

        private void IncreaseWinCount(GridValue winner)
        {
            if (winner == GridValue.X)
            {
                xWinCounter++;
                XCounter.Text = xWinCounter.ToString();
            }
            else
            {
                oWinCounter++;
                OCounter.Text = oWinCounter.ToString();
            }
        }

        private async void OnGameOver(Results gameOverType)
        {
            if (gameOverType.Winner == GridValue.None)
            {
                DisplayGameOver("Its a tie", null);
            }
            else
            {
                ShowWinningLine(gameOverType.GameOverResult);
                await Task.Delay(1000);
                DisplayGameOver("Winner: ", sources[gameOverType.Winner]);
                IncreaseWinCount(gameOverType.Winner);
            }
        }

        private void GameBoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isPvE && gameState.CurrentPlayer == GridValue.O) return; 

            double slotSize = GameBoard.Width / 3;
            Point click = e.GetPosition(GameBoard);
            int row = (int)(click.Y / slotSize);
            int col = (int)(click.X / slotSize);
            gameState.MakeMove(row, col);
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState.Reset();
        }

        private (Point, Point) WinningLine(GameOver gameOver)
        {
            double slotSize = GameBoard.Width / 3;
            double margin = slotSize / 2;
            if (gameOver.GOType == GameOverType.Row)
            {
                double y = gameOver.Num * slotSize + margin;
                return (new Point(0, y), new Point(GameBoardGrid.Width, y));
            }
            if (gameOver.GOType == GameOverType.Column)
            {
                double x = gameOver.Num * slotSize + margin;
                return (new Point(x, 0), new Point(x, GameBoardGrid.Height));
            }
            if (gameOver.GOType == GameOverType.TopLeftBottomRight)
            {
                return (new Point(0, 0), new Point(GameBoardGrid.Width, GameBoardGrid.Height));
            }
            return (new Point(GameBoardGrid.Width, 0), new Point(0, GameBoardGrid.Height));
        }

        private void ShowWinningLine(GameOver gameOver)
        {
            (Point one, Point two) = WinningLine(gameOver);

            WinLine.X1 = one.X;
            WinLine.X2 = two.X;
            WinLine.Y1 = one.Y;
            WinLine.Y2 = two.Y;
            WinLine.Visibility = Visibility.Visible;

        }

        private void PvP_Click(object sender, RoutedEventArgs e)
        {
            HomeScreen.Visibility = Visibility.Hidden;
            GameInfo.Visibility = Visibility.Visible;
            GameBoard.Visibility = Visibility.Visible;
        }
        private void PvE_Click(object sender, RoutedEventArgs e)
        {
            HomeScreen.Visibility = Visibility.Hidden;
            GameInfo.Visibility = Visibility.Visible;
            GameBoard.Visibility = Visibility.Visible;
            isPvE = true;
        }

        private async void ComputerMove()
        {
            {
                List<(int row, int col)> possibleMoves = new List<(int, int)>();

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (gameState.GridBuild[row, col] == GridValue.None)
                        {
                            possibleMoves.Add((row, col));
                        }
                    }
                }

                if (possibleMoves.Count > 0)
                {
                    await Task.Delay(1000);
                    var (row, col) = possibleMoves[random.Next(possibleMoves.Count)];
                    gameState.MakeMove(row, col);
                }
            }
        }
    }
}