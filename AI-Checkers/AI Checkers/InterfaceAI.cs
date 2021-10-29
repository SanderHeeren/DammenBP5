using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AICheckers
{
    interface InterfaceAI
    {
        CheckerColor Color { get; set; }
        Move ProcessI(Square[,] Board);
    }
}
