using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AICheckers
{
    class Move
    {
        public Move()
        {
        }

        public Move(Point current, Point nextposition)
        {
            this.current = current;
            this.nextposition = nextposition;
        }

        public Move(Point current, int nextX, int nextY) : this(current, new Point(nextX, nextY))
        {
        }

        public Move(int currentX, int currentY, int nextX, int nextY) : this(new Point(currentX, currentY), new Point(nextX, nextY))
        {
        }

        private Point current = new Point(-1, -1);
        private Point nextposition = new Point(-1, -1);
        private List<Point> listcaptures = new List<Point>();
        private int score = 0;

        public Point Current
        {
            get { return this.current; }
            set { this.current = value; }
        }

        public Point NextPosition
        {
            get { return this.nextposition; }
            set { this.nextposition = value; }
        }

        public List<Point> ListCaptures
        {
            get { return listcaptures; }
        }

        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }

        public override string ToString()
        {
            return String.Format("Van: {0}, Naar: {1}", current, nextposition);
        }
    }
}
