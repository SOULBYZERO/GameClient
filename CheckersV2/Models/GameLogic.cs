namespace CheckersV2.Models
{
    public class GameLogic
    {
        public event Action<int, int> MoveMade;
        public event Action<Results> GameEnded;
        public event Action Restart;
        public GridValues[,] GridBuild { get; set; }
        public GridValues CurrentPlayer { get; set; }
        public bool GameOver { get; set; }
        public int PieceCounter { get; set; }

        public GameLogic()
        {
            PieceCounter = 24;
            GridBuild = new GridValues[8, 8];
            GameOver = false;
            CurrentPlayer = GridValues.Black;
        }

        public void CurrentPlayerSwitch()
        {
            CurrentPlayer = CurrentPlayer == GridValues.Black ? GridValues.Red : GridValues.Black;
        }

        private bool CanMakeMove()
        {
            return !GameOver;
        }

        private bool DidMoveWin(out Results result)
        {
            if (PieceCounter < 13)
            {
                int redCounter = 0;
                int blackCounter = 0;
                foreach (var item in GridBuild)
                {
                    if (item == GridValues.Black || item == GridValues.BlackKing) blackCounter++;
                    if (item == GridValues.Red || item == GridValues.RedKing) redCounter++;
                }
                if (redCounter == 0)
                {
                    result = new Results { Winner = CurrentPlayer == GridValues.Black ? GridValues.Red : GridValues.Black, GOType = GameOverTypes.BlackWin };
                    return true;
                }
                else if (blackCounter == 0)
                {
                    result = new Results { Winner = CurrentPlayer == GridValues.Red ? GridValues.Black : GridValues.Red, GOType = GameOverTypes.RedWin };
                    return true;
                }
            }
            result = null;
            return false;
        }

        public void MakeMove(int row, int col)
        {
            if (CanMakeMove())
            {
                CurrentPlayerSwitch();
                MoveMade?.Invoke(row, col);
                if (DidMoveWin(out Results result))
                {
                    GameOver = true;
                    GameEnded.Invoke(result);
                }
            }
        }

        public void Reset()
        {
            GridBuild = new GridValues[8, 8];
            CurrentPlayer = GridValues.Black;
            PieceCounter = 24;
            GameOver = false;
            Restart.Invoke();
        }
    }
}
