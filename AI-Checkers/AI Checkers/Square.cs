using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AICheckers
{
    public enum CheckerColor
    {
        Empty,
        White,
        Black
    }

    class Square
    {
        public CheckerColor Color = CheckerColor.Empty;
        public bool king = false;
    }
}
