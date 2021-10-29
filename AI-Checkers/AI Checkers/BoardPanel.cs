using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using AICheckers.Properties;

namespace AICheckers
{
    public class BoardPanel : Panel
    {
        InterfaceAI AI = null;

        // Afbeeldingen voor de schijven/dammen
        Image WhiteChecker = Resources.checkerwhite;
        Image WhiteKing = Resources.checkerwhiteking;
        Image BlackChecker = Resources.checkerblack;
        Image BlackKing = Resources.checkerblackking;

        Color blackSquare = Color.DarkGray;
        Color whiteSquare = Color.Gainsboro;

        bool animate = false;
        const int Duration = 1000;
        Square animationPiece;
        Point previousPoint = new Point(-1, -1);
        Point Point = new Point(-1, -1);
        Point nextPoint = new Point(-1, -1);
        int delta = 10;
        int baseWidth = 0;
        int Score = 0; // Score van de speler
        Point selectedPiece = new Point(-1, -1);
        List<Move> availableMoves = new List<Move>();

        CheckerColor Turn = CheckerColor.Black;
        private System.ComponentModel.IContainer component;

        Square[,] board = new Square[8,8];
        
        public BoardPanel()
            : base()
        {
            this.ResizeRedraw = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint, true);

            // Genereren van het spel bord
            for (int k = 0; k < 8; k++)
            {
                for (int i = 0; i < 8; i++)
                {
                    board[k, i] = new Square();
                    board[k, i].Color = CheckerColor.Empty;
                }
            }

            // Positioneren van de schijven
            for (int j = 0; j < 8; j += 1)
            {
                int offset = 0;
                if (j % 2 != 0)
                {
                    offset++;
                }
                for (int l = offset; l < 8; l += 2)
                {
                    if (j < 3) board[j, l].Color = CheckerColor.White;
                    if (j > 4) board[j, l].Color = CheckerColor.Black;
                }
            }

            AI = new AI_BTree();
            AI.Color = CheckerColor.Black;

            Advanced();
        }

        protected override void OnPaint(PaintEventArgs p)
        {
            // Teken tool
            base.OnPaint(p);                        
            p.Graphics.Clear(whiteSquare);

            // Teken het spelbord
            baseWidth = (base.Width) / 8;
            for (int c = 0; c < base.Width; c += baseWidth)
            {
                int set = 0;
                if ((c / baseWidth) % 2 != 0)
                {
                    set += baseWidth;
                }
                for (int i = set; i < base.Width; i += (baseWidth * 2))
                {
                    p.Graphics.FillRectangle(Brushes.DarkGray, c, i, baseWidth, baseWidth);
                }
            }

            // Visualiseren van mogelijke moves
            foreach (Move move in availableMoves)
            {
                p.Graphics.FillRectangle(Brushes.PaleTurquoise, move.NextPosition.X * baseWidth, move.NextPosition.Y * baseWidth, baseWidth, baseWidth);
            }

            // Tekenen van de geselecteerde schijf
            if (selectedPiece.X >= 0 && selectedPiece.Y >= 0)
            {
                p.Graphics.FillRectangle(Brushes.PeachPuff, selectedPiece.X * baseWidth, selectedPiece.Y * baseWidth, baseWidth, baseWidth);
            }

            // Het tekenen van de rand
            p.Graphics.DrawRectangle(Pens.DarkGray,
            p.ClipRectangle.Left,
            p.ClipRectangle.Top,
            p.ClipRectangle.Width - 1,
            p.ClipRectangle.Height - 1);

            // Zorgen dat alles goed samen mee scaled
            p.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            // Teken schijf afbeelding
            for (int a = 0; a < 8; a++)
            {
                for (int b = 0; b < 8; b++)
                {
                    if (board[a,b].Color == CheckerColor.White)
                    {
                        if (board[a, b].king)
                        {
                            // Teken de witte dam / koning
                            p.Graphics.DrawImage(WhiteKing, new Rectangle(b * baseWidth, a * baseWidth, baseWidth, baseWidth));
                        }
                        else
                        {
                            // Teken de witte schijf
                            p.Graphics.DrawImage(WhiteChecker, new Rectangle(b * baseWidth, a * baseWidth, baseWidth, baseWidth));
                        }
                    }
                    else if (board[a, b].Color == CheckerColor.Black)
                    {
                        if (board[a, b].king)
                        {
                            // Teken de zwarte dam / koning
                            p.Graphics.DrawImage(BlackKing, new Rectangle(b * baseWidth, a * baseWidth, baseWidth, baseWidth));
                        }
                        else
                        {
                            // Teken de zwarte schijf
                            p.Graphics.DrawImage(BlackChecker, new Rectangle(b * baseWidth, a * baseWidth, baseWidth, baseWidth));
                        }
                    }
                }
            }
        }
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            int X = (int)(((double)e.X / (double)base.Width) * 8.0d);
            int Y = (int)(((double)e.Y / (double)Height) * 8.0d);

            Point clickPosition = new Point(X, Y);

            // Controleer of deze positie van de speler is
            if (board[Y, X].Color != CheckerColor.Empty
                && board[Y, X].Color != Turn)
                return;
          
            // Controleer of het mogelijk is om hem hier naar te verplaatsen
            List<Move> posibilities = availableMoves.Where(m => m.NextPosition == clickPosition).ToList<Move>();
            if (posibilities.Count > 0)
            {
                // verplaats naar de geselecteerde positie
                MovePiece(posibilities[0]);
            }
            else if (board[Y, X].Color != CheckerColor.Empty)
            {
                // De huidige geselecteerde schijf
                selectedPiece.X = X;
                selectedPiece.Y = Y;
                availableMoves.Clear();

                Console.WriteLine("Geselecteerde schijf: {0}",selectedPiece.ToString());
                
                Move[] OpenSquares = Utility.GetAvailableSquares(board, selectedPiece);
                availableMoves.AddRange(OpenSquares);

                this.Invalidate();
            }            
        }

        private void MovePiece(Move move)
        {
            Console.WriteLine(move.ToString());

            board[move.NextPosition.Y, move.NextPosition.X].Color = board[move.Current.Y, move.Current.X].Color;
            board[move.NextPosition.Y, move.NextPosition.X].king = board[move.Current.Y, move.Current.X].king;
            Reset(move.Current);

            foreach (Point point in move.ListCaptures)
            {
                if (Turn == CheckerColor.White)
                {
                    Score++;
                    Score++;
                    Console.WriteLine("Huidige Score: " + Score);
                }
                Reset(point);
            }

            selectedPiece.X = -1;
            selectedPiece.Y = -1;

            // Voor de koning / dam
            if ((move.NextPosition.Y == 7 && board[move.NextPosition.Y, move.NextPosition.X].Color == CheckerColor.White)
                || (move.NextPosition.Y == 0 && board[move.NextPosition.Y, move.NextPosition.X].Color == CheckerColor.Black))
            {
                board[move.NextPosition.Y, move.NextPosition.X].king = true;
            }

            availableMoves.Clear();

            previousPoint.X = move.Current.Y * baseWidth;
            previousPoint.Y = move.Current.X * baseWidth;
            nextPoint.X = move.NextPosition.Y * baseWidth;
            nextPoint.Y = move.NextPosition.X * baseWidth;
            Point = previousPoint;
            
            animate = true;
            this.Invalidate();
            Advanced();
        }

        private void Reset(Point square)
        {
            // Verwijderen van een schijf
            board[square.Y, square.X].Color = CheckerColor.Empty;
            
            board[square.Y, square.X].king = false;
        }

        private void Advanced()
        {
            if (Turn == CheckerColor.White)
            {
                Turn = CheckerColor.Black;
            }
            else
            {
                Turn = CheckerColor.White;
            }

            if (AI != null && AI.Color == Turn)
            {
                Move MoveAI = AI.ProcessI(board);
                MovePiece(MoveAI);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
