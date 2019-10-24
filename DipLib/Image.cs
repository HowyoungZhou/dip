namespace DipLib
{
    public abstract class Image<T>
    {
        public int PixelHeight { get => Pixels.GetLength(1); }

        public int PixelWidth { get => Pixels.GetLength(0); }

        public T this[int x, int y]
        {
            get
            {
                return Pixels[x, y];
            }
            set
            {
                Pixels[x, y] = value;
            }
        }

        public T this[Point point]
        {
            get
            {
                return Pixels[point.X, point.Y];
            }
            set
            {
                Pixels[point.X, point.Y] = value;
            }
        }

        public T[,] Pixels { get; set; }

        public Image(int pixelWidth, int pixelHeight)
        {
            Pixels = new T[pixelWidth, pixelHeight];
        }

        public Image(T[,] pixels)
        {
            Pixels = pixels;
        }
    }

    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x = 0, int y = 0)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public int GetSquaredDistance(Point point)
        {
            return (X - point.X) * (X - point.X) + (Y - point.Y) * (Y - point.Y);
        }
    }
}