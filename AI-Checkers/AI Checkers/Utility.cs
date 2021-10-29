using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AICheckers
{
    static class Utility
    {
        public static Move[] GetAvailableSquares(Square[,] Board, Point square)
        {
            return GetAvailableSquares(Board, square, new Move(-1, -1, -1, -1), null);
        }

        private static Move[] GetAvailableSquares(Square[,] Board, Point square, Move previousMove, List<Point> previousPositions)
        {
            if (previousPositions == null)
            {
                previousPositions = new List<Point>();
                previousPositions.Add(square);
            }

            List<Move> EmptySquares = new List<Move>();

            // Naar links boven
            if (Board[previousPositions[0].Y, previousPositions[0].X].Color != CheckerColor.White || Board[previousPositions[0].Y, previousPositions[0].X].king)        
            {
                if (IsValidPoint(square.X - 1, square.Y - 1))
                {
                    if (Board[square.Y - 1, square.X - 1].Color == CheckerColor.Empty && previousMove.NextPosition.X == -1)  
                    {
                        EmptySquares.Add(new Move(previousPositions[0], square.X - 1, square.Y - 1));
                    }
                    // Controleer of slaan mogelijk is
                    else if (IsValidPoint(square.X - 2, square.Y - 2)
                        && ((square.X - 2) != previousMove.NextPosition.X || (square.Y - 2) != previousMove.NextPosition.Y)
                        && ((square.X - 2) != previousPositions[0].X || (square.Y - 2) != previousPositions[0].Y)
                        && Board[square.Y - 1, square.X - 1].Color != Board[square.Y, square.X].Color
                        && Board[square.Y - 2, square.X - 2].Color == CheckerColor.Empty)
                    {
                        Point newDestination = new Point(square.X - 2, square.Y - 2);
                        if (!previousPositions.Contains(newDestination))
                        {
                            Move move = new Move(previousPositions[0], newDestination);
                            move.ListCaptures.Add(new Point(square.X - 1, square.Y - 1));
                            move.ListCaptures.AddRange(previousMove.ListCaptures);
                            EmptySquares.Add(move);

                            previousPositions.Add(newDestination);

                            // Kijk of er meerdere keer geslagen kan worden met behulp van recursie                     
                            EmptySquares.AddRange(GetAvailableSquares(Board, new Point(square.X - 2, square.Y - 2), move, previousPositions));
                        }
                    }
                }
            }

            // Naar rechts boven
            if (Board[previousPositions[0].Y, previousPositions[0].X].Color != CheckerColor.White || Board[previousPositions[0].Y, previousPositions[0].X].king)
            {
                if (IsValidPoint(square.X + 1, square.Y - 1))
                {
                    if (Board[square.Y - 1, square.X + 1].Color == CheckerColor.Empty && previousMove.NextPosition.X == -1)
                    {
                        EmptySquares.Add(new Move(previousPositions[0], square.X + 1, square.Y - 1));
                    }
                    // Controleer of slaan mogelijk is
                    else if (IsValidPoint(square.X + 2, square.Y - 2)
                        && ((square.X + 2) != previousMove.NextPosition.X || (square.Y - 2) != previousMove.NextPosition.Y)
                        && ((square.X + 2) != previousPositions[0].X || (square.Y - 2) != previousPositions[0].Y)
                        && Board[square.Y - 1, square.X + 1].Color != Board[square.Y, square.X].Color
                        && Board[square.Y - 2, square.X + 2].Color == CheckerColor.Empty)
                    {
                        Point newDestination = new Point(square.X + 2, square.Y - 2);
                        if (!previousPositions.Contains(new Point(square.X + 2, square.Y - 2)))
                        {
                            Move move = new Move(previousPositions[0], newDestination);
                            move.ListCaptures.Add(new Point(square.X + 1, square.Y - 1));
                            move.ListCaptures.AddRange(previousMove.ListCaptures);
                            EmptySquares.Add(move);

                            previousPositions.Add(newDestination);

                            // Kijk of er meerdere keer geslagen kan worden met behulp van recursie                     
                            EmptySquares.AddRange(GetAvailableSquares(Board, new Point(square.X + 2, square.Y - 2), move, previousPositions));
                        }
                    }
                }
            }

            // Naar links beneden
            if (Board[previousPositions[0].Y, previousPositions[0].X].Color != CheckerColor.Black || Board[previousPositions[0].Y, previousPositions[0].X].king)
            {
                if (IsValidPoint(square.X - 1, square.Y + 1))
                {
                    if (Board[square.Y + 1, square.X - 1].Color == CheckerColor.Empty && previousMove.NextPosition.X == -1)
                    {
                        EmptySquares.Add(new Move(previousPositions[0], square.X - 1, square.Y + 1));
                    }
                    // Controleer of slaan mogelijk is
                    else if (IsValidPoint(square.X - 2, square.Y + 2)
                        && ((square.X - 2) != previousMove.NextPosition.X || (square.Y + 2) != previousMove.NextPosition.Y)
                        && ((square.X - 2) != previousPositions[0].X || (square.Y + 2) != previousPositions[0].Y)
                        && Board[square.Y + 1, square.X - 1].Color != Board[square.Y, square.X].Color
                        && Board[square.Y + 2, square.X - 2].Color == CheckerColor.Empty)
                    {
                        Point newDestination = new Point(square.X - 2, square.Y + 2);
                        if (!previousPositions.Contains(newDestination))
                        {
                            Move move = new Move(previousPositions[0], newDestination);
                            move.ListCaptures.Add(new Point(square.X - 1, square.Y + 1));
                            move.ListCaptures.AddRange(previousMove.ListCaptures);
                            EmptySquares.Add(move);

                            previousPositions.Add(newDestination);

                            // Kijk of er meerdere keer geslagen kan worden met behulp van recursie                     
                            EmptySquares.AddRange(GetAvailableSquares(Board, new Point(square.X - 2, square.Y + 2), move, previousPositions));
                        }
                    }
                }
            }

            // Naar rechts beneden
            if (Board[previousPositions[0].Y, previousPositions[0].X].Color != CheckerColor.Black || Board[previousPositions[0].Y, previousPositions[0].X].king)
            {
                if (IsValidPoint(square.X + 1, square.Y + 1))
                {
                    if (Board[square.Y + 1, square.X + 1].Color == CheckerColor.Empty && previousMove.NextPosition.X == -1)
                    {
                        EmptySquares.Add(new Move(previousPositions[0], square.X + 1, square.Y + 1));
                    }
                    // Controleer of slaan mogelijk is
                    else if (IsValidPoint(square.X + 2, square.Y + 2)
                        && ((square.X + 2) != previousMove.NextPosition.X || (square.Y + 2) != previousMove.NextPosition.Y)
                        && ((square.X + 2) != previousPositions[0].X || (square.Y + 2) != previousPositions[0].Y)
                        && Board[square.Y + 1, square.X + 1].Color != Board[square.Y, square.X].Color
                        && Board[square.Y + 2, square.X + 2].Color == CheckerColor.Empty)
                    {
                        Point newDestination = new Point(square.X + 2, square.Y + 2);
                        if (!previousPositions.Contains(newDestination))
                        {
                            Move move = new Move(previousPositions[0], newDestination);
                            move.ListCaptures.Add(new Point(square.X + 1, square.Y + 1));
                            move.ListCaptures.AddRange(previousMove.ListCaptures);
                            EmptySquares.Add(move);

                            previousPositions.Add(newDestination);

                            // Kijk of er meerdere keer geslagen kan worden met behulp van recursie                     
                            EmptySquares.AddRange(GetAvailableSquares(Board, new Point(square.X + 2, square.Y + 2), move, previousPositions));
                        }
                    }
                }
            }

            return EmptySquares.ToArray();
        }

        private static bool IsValidPoint(int x, int y)
        {
            if (0 <= x && x < 8 && 0 <= y && y < 8) return true;
            return false;
        }

        private static bool IsValidPoint(Point point)
        {
            return (IsValidPoint(point.X, point.Y));
        }
    }
}
