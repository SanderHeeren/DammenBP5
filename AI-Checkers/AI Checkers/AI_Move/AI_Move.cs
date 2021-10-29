using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AICheckers
{
    class AI_Move : InterfaceAI
    {
        CheckerColor color;

        public CheckerColor Color
        {
            get { return color; }
            set { color = value; }
        }

        public Move ProcessI(Square[,] Board)
        {
            List<Move> moves = new List<Move>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j].Color == Color)
                    {
                        moves.AddRange(Utility.GetAvailableSquares(Board, new Point(j, i)));
                    }
                }
            }

            return moves[(new Random()).Next(moves.Count)];
        }
    }
}
