using System;

namespace DipLib
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Point Zeros { get; } = new Point(0, 0);

        public Point(int x = 0, int y = 0)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point operator +(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);

        public static Point operator -(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);

        public static Point operator -(Point p) => new Point(-p.X, -p.Y);

        public int GetSquaredDistance(Point point)
        {
            return (X - point.X) * (X - point.X) + (Y - point.Y) * (Y - point.Y);
        }

        public Point RotateR(Point origin, double angle)
        {
            return new Point(
                (int)Math.Round(origin.X + (X - origin.X) * Math.Cos(angle) - (Y - origin.Y) * Math.Sin(angle)),
                (int)Math.Round(origin.Y + (X - origin.X) * Math.Sin(angle) + (Y - origin.Y) * Math.Cos(angle))
            );
        }

        public Point RotateD(Point origin, double angle)
        {
            return RotateR(origin, angle / 180 * Math.PI);
        }
    }
}