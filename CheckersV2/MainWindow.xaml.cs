using CheckersV2.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheckersV2
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValues, ImageSource> imageSources = new()
        {
            {GridValues.Red, new BitmapImage(new Uri("pack://application:,,,/CheckersV2;component/Assets/Red.png")) },
            {GridValues.RedKing, new BitmapImage(new Uri("pack://application:,,,/CheckersV2;component/Assets/RedKing.png")) },
            {GridValues.Black, new BitmapImage(new Uri("pack://application:,,,/CheckersV2;component/Assets/Black.png")) },
            {GridValues.BlackKing, new BitmapImage(new Uri("pack://application:,,,/CheckersV2;component/Assets/BlackKing.png")) }
        };

        private readonly Dictionary<GridValues, int> dir = new()
        {
            {GridValues.Red, 1 },
            {GridValues.Black, -1 },
        };

        public PieceState selectedPiece = new PieceState();
        public PieceState possibleCapture = new PieceState();

        private readonly Image[,] gridImages = new Image[8, 8];
        private readonly Rectangle[,] gridRectangles = new Rectangle[8, 8];
        private readonly GameLogic gameState = new GameLogic();

        private bool IsSelected { get; set; }
        private Rectangle SelectedGrid { get; set; }
        private List<(int, int)> possibleMoves = new List<(int, int)>();
        private (int, int) OriginalCords { get; set; }
        private List<(int, int)> CaptureCords { get; set; } = new List<(int, int)>();
        private List<(int, int)> CaptureTriggerCords { get; set; } = new List<(int, int)>();

        private bool canCaptureMore = false;
        private bool hasCaptured = false;

        private int blackScore = 0;
        private int redScore = 0;

        public MainWindow()
        {
            InitializeComponent();
            GenerateGameBoard();
            GeneratePieces();

            gameState.MoveMade += OnMoveMade;
            gameState.Restart += OnReset;
            gameState.GameEnded += OnGameEndedAsync;
        }

        private void GenerateGameBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if ((row + col) % 2 != 0)
                    {
                        Rectangle gridRect = new Rectangle
                        {
                            Fill = Brushes.Transparent,
                        };
                        Grid.SetRow(gridRect, row);
                        Grid.SetColumn(gridRect, col);
                        GameBoardGrid.Children.Add(gridRect);
                        gridRectangles[row, col] = gridRect;
                    }
                }
            }
        }
        private void GeneratePieces()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if ((row + col) % 2 != 0)
                    {
                        if (row < 3)
                        {
                            Image gridImage = new Image
                            {
                                Source = imageSources[GridValues.Red],
                            };
                            Grid.SetRow(gridImage, row);
                            Grid.SetColumn(gridImage, col);
                            GameBoardGrid.Children.Add(gridImage);
                            gameState.GridBuild[row, col] = GridValues.Red;
                            gridImages[row, col] = gridImage;
                        }
                        else if (row > 4)
                        {
                            Image gridImage = new Image
                            {
                                Source = imageSources[GridValues.Black],
                            };
                            Grid.SetRow(gridImage, row);
                            Grid.SetColumn(gridImage, col);
                            GameBoardGrid.Children.Add(gridImage);
                            gameState.GridBuild[row, col] = GridValues.Black;
                            gridImages[row, col] = gridImage;
                        }
                        else
                        {
                            Image gridImage = new Image
                            {
                                Source = null,
                            };
                            Grid.SetRow(gridImage, row);
                            Grid.SetColumn(gridImage, col);
                            GameBoardGrid.Children.Add(gridImage);
                            gridImages[row, col] = gridImage;
                            gameState.GridBuild[row, col] = GridValues.Empty;
                        }
                    }
                }
            }
        }

        private void GameBoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double squareSize = GameBoard.Width / 8;
            Point click = e.GetPosition(GameBoard);
            int row = (int)(click.Y / squareSize);
            int col = (int)(click.X / squareSize);
            if (!IsSelected)
            {
                if (IsCurrentPlayerPiece(gameState.GridBuild[row, col]))
                {
                    SelectPiece(row, col);
                    return;
                }
            }
            if (IsSelected)
            {
                if (SelectedGrid == gridRectangles[row, col] && !hasCaptured)
                {
                    DeselectPiece();
                    return;
                }
            }
            foreach (var (r, c) in possibleMoves)
            {
                if (row == r && col == c)
                {
                    gameState.MakeMove(row, col);
                    return;
                }
            }
        }
        private void SelectPiece(int row, int col)
        {
            selectedPiece.Piece = gridImages[row, col];
            selectedPiece.PieceGrid = gameState.GridBuild[row, col];
            Rectangle selectedRect = gridRectangles[row, col];
            selectedRect.Fill = Brushes.LightGreen;
            SelectedGrid = selectedRect;
            IsSelected = true;
            OriginalCords = (row, col);
            GeneratePossibleMoves(row, col);
        }

        private void DeselectPiece()
        {
            SelectedGrid.Fill = Brushes.Transparent;
            IsSelected = false;
            possibleCapture.PieceGrid = GridValues.Empty;
            ClearPossibleMoves();
        }

        private bool IsMoveOutsideGrid(int row, int col)
        {
            return row < 0 || row >= 8 || col < 0 || col >= 8;
        }

        private bool IsSlotEmpty(int row, int col)
        {
            if (gameState.GridBuild[row, col] == GridValues.Empty)
            {
                return true;
            }
            return false;
        }

        private bool CanCapture(int row, int col, int rowOffSet, int colOffSet, out int nextRow, out int nextCol)
        {
            GridValues opponent = gameState.CurrentPlayer == GridValues.Red ? GridValues.Black : GridValues.Red;
            GridValues opponentKing = gameState.CurrentPlayer == GridValues.Red ? GridValues.BlackKing : GridValues.RedKing;

            if (gameState.GridBuild[row, col] == opponent || gameState.GridBuild[row, col] == opponentKing)
            {
                nextRow = row + rowOffSet;
                nextCol = col + colOffSet;
                if (!IsMoveOutsideGrid(nextRow, nextCol) && IsSlotEmpty(nextRow, nextCol))
                {
                    CaptureTriggerCords.Add((nextRow, nextCol));
                    PieceToCapture(row, col);
                    return true;
                }
            }

            nextRow = 0;
            nextCol = 0;
            return false;
        }

        private void PieceToCapture(int row, int col)
        {
            possibleCapture.Piece = gridImages[row, col];
            possibleCapture.PieceGrid = gameState.GridBuild[row, col];
            CaptureCords.Add((row, col));
        }

        private void GeneratePossibleMoves(int row, int col)
        {
            GridValues selectedGrid = gameState.GridBuild[row, col];
            int[] rowOffsets, colOffsets;

            CaptureCords.Clear();
            CaptureTriggerCords.Clear();

            if (selectedGrid == GridValues.Red || selectedGrid == GridValues.Black)
            {
                rowOffsets = [dir[selectedGrid]];
                colOffsets = [-1, 1];
            }
            else
            {
                rowOffsets = [-1, 1];
                colOffsets = [-1, 1];
            }

            foreach (int rowOffSet in rowOffsets)
            {
                foreach (int colOffSet in colOffsets)
                {
                    int nextRow = row + rowOffSet;
                    int nextCol = col + colOffSet;

                    while (!IsMoveOutsideGrid(nextRow, nextCol))
                    {
                        if (IsSlotEmpty(nextRow, nextCol) && !hasCaptured)
                        {
                            gridRectangles[nextRow, nextCol].Fill = Brushes.LightGreen;
                            possibleMoves.Add((nextRow, nextCol));
                        }
                        else
                        {
                            if (CanCapture(nextRow, nextCol, rowOffSet, colOffSet, out int jumpRow, out int jumpCol))
                            {
                                gridRectangles[jumpRow, jumpCol].Fill = Brushes.LightGreen;
                                possibleMoves.Add((jumpRow, jumpCol));
                            }
                            break;
                        }

                        if (selectedGrid == GridValues.Red || selectedGrid == GridValues.Black)
                        {
                            break;
                        }

                        nextRow += rowOffSet;
                        nextCol += colOffSet;
                    }
                }
            }
        }

        private void ClearPossibleMoves()
        {
            foreach (var (row, col) in possibleMoves)
            {
                gridRectangles[row, col].Fill = Brushes.Transparent;
            }
            possibleMoves.Clear();
        }

        private bool IsCurrentPlayerPiece(GridValues piece)
        {
            return (piece == gameState.CurrentPlayer ||
                    (gameState.CurrentPlayer == GridValues.Red && piece == GridValues.RedKing) ||
                    (gameState.CurrentPlayer == GridValues.Black && piece == GridValues.BlackKing));
        }

        private void OnMoveMade(int row, int col)
        {
            for (int i = 0; i < CaptureTriggerCords.Count; i++)
            {
                if (CaptureTriggerCords[i] == (row, col))
                {
                    gameState.GridBuild[CaptureCords[i].Item1, CaptureCords[i].Item2] = GridValues.Empty;
                    gridImages[CaptureCords[i].Item1, CaptureCords[i].Item2].Source = null;
                    hasCaptured = true;
                    gameState.PieceCounter--;
                    break;
                }
            }
            if (hasCaptured)
            {
                CaptureTriggerCords.Clear();
                CaptureCords.Clear();
            }
            PlayerTurnDisplay.Text = gameState.CurrentPlayer == GridValues.Black ? "Black" : "Red";
            gameState.CurrentPlayerSwitch();
            gridImages[row, col].Source = selectedPiece.Piece.Source;
            selectedPiece.Piece.Source = null;
            gameState.GridBuild[row, col] = selectedPiece.PieceGrid;
            gameState.GridBuild[OriginalCords.Item1, OriginalCords.Item2] = GridValues.Empty;

            DeselectPiece();
            if (CheckForAdditionalCaptures(row, col) && hasCaptured)
            {
                PlayerTurnDisplay.Text = gameState.CurrentPlayer == GridValues.Black ? "Black" : "Red";
                SelectPiece(row, col);
            }
            else
            {
                gameState.CurrentPlayerSwitch();
                hasCaptured = false;
            }
            if ((row == 0 || row == 7) && selectedPiece.PieceGrid != GridValues.RedKing && selectedPiece.PieceGrid != GridValues.BlackKing)
            {
                gridImages[row, col].Source = gridImages[row, col].Source == imageSources[GridValues.Red] ? imageSources[GridValues.RedKing] : imageSources[GridValues.BlackKing];
                gameState.GridBuild[row, col] = gameState.GridBuild[row, col] == GridValues.Red ? GridValues.RedKing : GridValues.BlackKing;
            }
        }

        private bool CheckForAdditionalCaptures(int row, int col)
        {
            canCaptureMore = false;
            GridValues selectedGrid = gameState.GridBuild[row, col];

            if (selectedGrid == GridValues.Red || selectedGrid == GridValues.RedKing)
            {
                canCaptureMore = CheckCaptureDirections(row, col, dir[GridValues.Red]);
            }
            else if (selectedGrid == GridValues.Black || selectedGrid == GridValues.BlackKing)
            {
                canCaptureMore = CheckCaptureDirections(row, col, dir[GridValues.Black]);
            }

            return canCaptureMore;
        }

        private bool CheckCaptureDirections(int row, int col, int direction)
        {
            int[] rowOffsets, colOffsets;
            GridValues selectedGrid = gameState.GridBuild[row, col];

            if (selectedGrid == GridValues.Red || selectedGrid == GridValues.Black)
            {
                rowOffsets = [direction];
                colOffsets = [-1, 1];
            }
            else
            {
                rowOffsets = [-1, 1];
                colOffsets = [-1, 1];
            }

            foreach (int rowOffSet in rowOffsets)
            {
                foreach (int colOffSet in colOffsets)
                {
                    int nextRow = row + rowOffSet;
                    int nextCol = col + colOffSet;
                    if (!IsMoveOutsideGrid(nextRow, nextCol) && CanCapture(nextRow, nextCol, rowOffSet, colOffSet, out int jumpRow, out int jumpCol))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnReset()
        {
            var elementsToRemove = new List<UIElement>();
            foreach (UIElement element in GameBoardGrid.Children)
            {
                if (element is Image || element is Rectangle)
                {
                    elementsToRemove.Add(element);
                }
            }
            foreach (var element in elementsToRemove)
            {
                GameBoardGrid.Children.Remove(element);
            }

            Array.Clear(gridImages, 0, gridImages.Length);
            Array.Clear(gridRectangles, 0, gridRectangles.Length);
            GenerateGameBoard();
            GeneratePieces();
            DisplayGameBoard();
        }

        private async void OnGameEndedAsync(Results result)
        {
            await Task.Delay(1000);
            IncreaseScore(result);
            DisplayGameOver(result);
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState.Reset();
        }

        private void DisplayGameBoard()
        {
            GameOverScreen.Visibility = Visibility.Hidden;
            GameBoard.Visibility = Visibility.Visible;
            GameInfo.Visibility = Visibility.Visible;
        }

        private void DisplayGameOver(Results result)
        {
            GameBoard.Visibility = Visibility.Hidden;
            GameInfo.Visibility = Visibility.Hidden;
            GameOverScreen.Visibility = Visibility.Visible;
            WinnerDisplay.Text = result.Winner.ToString();
        }

        private void IncreaseScore(Results result)
        {
            if (result.GOType == GameOverTypes.BlackWin)
            {
                blackScore++;
                BlackScore.Text = blackScore.ToString();
            }
            else
            {
                redScore++;
                RedScore.Text = redScore.ToString();
            }
        }
    }
}