using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public class Results
    {
        public GridValue Winner { get; set; }
        public GameOver GameOverResult { get; set; }
    }
}
