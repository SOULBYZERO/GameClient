using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersV2.Models
{
    public class Results
    {
        public GridValues Winner { get; set; }
        public GameOverTypes GOType { get; set; }
    }
}
