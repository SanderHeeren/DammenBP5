using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AICheckers
{
    class AI_BTree : InterfaceAI
    {
        int LEVEL_AI = 2;

        // Waardes van aanvallen (hoger nummber heeft voorrang over een ander als het om een keuze gaat)
        int PIECE_CAPTURE = 4;
        int KING_CAPTURE = 2;
        int DOUBLE_CAPTURE = 10;
        int MULTIPLE_CAPTURES = 20;

        // Waardes van verdediging/vluchten (hoger nummber heeft voorrang over een ander als het om een keuze gaat)
        int CHECKERRISK = 6;
        int KINGRISK = 8;

        // Strategische waarde
        int TURNKING = 2;
        
        CheckerColor color;

        BinaryTree<Move> gameBTree;

        public CheckerColor Color
        {
            get { return color; }
            set { color = value; }
        }

        public Move ProcessI(Square[,] Board)
        {
            gameBTree = new BinaryTree<Move>(new Move());

            for (int e = 0; e < 8; e++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (Board[e, i].Color == Color)
                    {
                        foreach (Move myMove in Utility.GetAvailableSquares(Board, new Point(i, e)))
                        {
                            CalcChildMoves(0, gameBTree.Add(myMove), myMove, Copy(Board));
                        }
                    }
                }
            }

            ScoreMoves(Board);
            return SumMoves();
        }

        private Square[,] Copy(Square[,] sourceB)
        {
            Square[,] Sresults = new Square[8, 8];

            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    Sresults[j, i] = new Square();
                    Sresults[j, i].Color = sourceB[j, i].Color;
                    Sresults[j, i].king = sourceB[j, i].king;
                }
            }

            return Sresults;
        }

        private void CalcChildMoves(int recursion, BinaryTree<Move> branch, Move move, Square[,] vBoard)
        {
            if (recursion >= LEVEL_AI)
            {
                return;
            }

            CheckerColor moveColour = vBoard[move.Current.Y, move.Current.X].Color;

            // Voer de move uit
            vBoard = ExecuteAIMove(move, vBoard);

            // Reken de moves uit van de tegenstander
            for (int e = 0; e < 8; e++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (vBoard[e, i].Color != moveColour)
                    {
                        foreach (Move otherPlayerMove in Utility.GetAvailableSquares(vBoard, new Point(i, e)))
                        {
                            if (vBoard[e, i].Color != CheckerColor.Empty)
                            {
                                CalcChildMoves(++recursion, branch.Add(otherPlayerMove), otherPlayerMove, Copy(vBoard));
                            }
                        }
                    }
                }
            }
        }

        private Square[,] ExecuteAIMove(Move move, Square[,] Board)
        {
            Board[move.NextPosition.Y, move.NextPosition.X].Color = Board[move.Current.Y, move.Current.X].Color;
            Board[move.NextPosition.Y, move.NextPosition.X].king = Board[move.Current.Y, move.Current.X].king;
            Board[move.Current.Y, move.Current.X].Color = CheckerColor.Empty;
            Board[move.Current.Y, move.Current.X].king = false;

            // Movement voor de koning/dam
            if ((move.NextPosition.Y == 7 && Board[move.NextPosition.Y, move.NextPosition.X].Color == CheckerColor.White)
                || (move.NextPosition.Y == 0 && Board[move.NextPosition.Y, move.NextPosition.X].Color == CheckerColor.Black))
            {
                Board[move.NextPosition.Y, move.NextPosition.X].king = true;
            }

            return Board;
        }
        private void ScoreMoves(Square[,] Board)
        {
            // Loop over het hoogste niveau mogelijk moves 
            Action<Move> scoreMove = (Move move) => move.Score = ScoreSingleMove(move, Board);

            foreach (BinaryTree<Move> possibleMove in gameBTree.GetChildren)
            {
                possibleMove.Traversal(scoreMove);
            }
        }

        private Move SumMoves()
        {
            //Loop over het hoogste niveau mogelijk moves

            int SumBranch = 0;
            Action<Move> sumScores = (Move move) => SumBranch += move.Score;

            foreach (BinaryTree<Move> possibleMove in gameBTree.GetChildren)
            {
                possibleMove.Traversal(sumScores);
                possibleMove.GetValue.Score += SumBranch;
                SumBranch = 0;
            }

            // return de hoogste score
            return gameBTree.GetChildren.OrderByDescending(o => o.GetValue.Score).ToList()[0].GetValue;
        }

        private int ScoreSingleMove(Move move, Square[,] board)
        {
            int score = 0;

            score += move.ListCaptures.Count * PIECE_CAPTURE;

            if (move.ListCaptures.Count == 2) score += DOUBLE_CAPTURE;
            if (move.ListCaptures.Count > 2) score += MULTIPLE_CAPTURES;

            // Controleer de king captures
            foreach (Point point in move.ListCaptures)
            {
                if (board[point.Y, point.X].king) score += KING_CAPTURE;
            }

            // Controleer of een van de eigen schijven geslagen kan worden
            for (int e = 0; e < 8; e++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (board[e, i].Color == Color)
                    {
                        foreach (Move MoveOpponent in Utility.GetAvailableSquares(board, new Point(i, e)))
                        {
                            if (MoveOpponent.ListCaptures.Contains(move.Current))
                            {
                                if (board[move.Current.Y, move.Current.X].king)
                                {
                                    score += KINGRISK;
                                }
                                else
                                {
                                    score += CHECKERRISK;
                                }
                            }
                        }
                    }
                }
            }
            if (board[move.Current.Y, move.Current.X].Color != color) score *= -1;
            return score;
        }
    }
}
