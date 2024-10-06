namespace TicTacToe.Models
{
    public class GameLogic
    {
        public event Action<int, int> MoveMade;
        public event Action<Results> GameEnded;
        public event Action Restart;
        public GridValue[,] GridBuild { get; set; }
        public GridValue CurrentPlayer { get; set; }
        public bool GameOver { get; set; }
        public int TurnsCounter { get; set; }

        public GameLogic()
        {
            GridBuild = new GridValue[3, 3];
            CurrentPlayer = GridValue.X;
            GameOver = false;

        }
        private void CurrentPlayerSwitch()
        {
            CurrentPlayer = CurrentPlayer == GridValue.X ? GridValue.O : GridValue.X;
        }
        private bool IsFull()
        {
            return TurnsCounter == 9;
        }
        private bool CanMakeMove(int row, int col)
        {
            return !GameOver && GridBuild[row, col] == GridValue.None;
        }

        private bool UsedSlotCheck((int, int)[] slots, GridValue CurrentPlayer)
        {
            foreach ((int row, int col) in slots)
            {
                if (GridBuild[row, col] != CurrentPlayer)
                {
                    return false;
                }
            }
            return true;
        }

        private bool DidMoveWin(int r, int c, out GameOver gameOver)
        {
            var row = new (int, int)[] { (r, 0), (r, 1), (r, 2) };
            var col = new (int, int)[] { (0, c), (1, c), (2, c) };
            var tlbr = new (int, int)[] { (0, 0), (1, 1), (2, 2) };
            var bltr = new (int, int)[] { (0, 2), (1, 1), (2, 0) };

            if (UsedSlotCheck(row, CurrentPlayer))
            {
                gameOver = new GameOver { GOType = GameOverType.Row, Num = r };
                return true;
            }

            if (UsedSlotCheck(col, CurrentPlayer))
            {
                gameOver = new GameOver { GOType = GameOverType.Column, Num = c };
                return true;
            }

            if (UsedSlotCheck(tlbr, CurrentPlayer))
            {
                gameOver = new GameOver { GOType = GameOverType.TopLeftBottomRight };
                return true;
            }

            if (UsedSlotCheck(bltr, CurrentPlayer))
            {
                gameOver = new GameOver { GOType = GameOverType.BottomLeftTopRight };
                return true;
            }

            gameOver = null;
            return false;
        }

        private bool DidMoveEndGame(int row, int col, out Results result)
        {
            if (DidMoveWin(row, col, out GameOver gameOver))
            {
                result = new Results { Winner = CurrentPlayer, GameOverResult = gameOver };
                return true;
            }

            if (IsFull())
            {
                result = new Results { Winner = GridValue.None };
                return true;
            }

            result = null;
            return false;
        }

        public void MakeMove(int row, int col)
        {
            if (!CanMakeMove(row, col)) return;

            GridBuild[row, col] = CurrentPlayer;
            TurnsCounter++;

            if (DidMoveEndGame(row,col, out Results result))
            {
                GameOver = true;
                MoveMade?.Invoke(row, col);
                GameEnded?.Invoke(result);
            } else
            {
                CurrentPlayerSwitch();
                MoveMade?.Invoke(row,col);
            }
        }

        public void Reset()
        {
            GridBuild = new GridValue[3, 3];
            CurrentPlayer = GridValue.X;
            TurnsCounter = 0;
            GameOver = false;
            Restart?.Invoke();
        }
    }
}