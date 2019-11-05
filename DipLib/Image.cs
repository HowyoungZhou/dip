using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace DipLib
{
    public interface IBitmapSource
    {
        public int PixelHeight { get; }

        public int PixelWidth { get; }

        public BitmapSource ToBitmapSource(double dpiX, double dpiY);
    }

    public interface ITransformableImage
    {
        public RGBImage Translate(int dx, int dy);

        public void MirrorHorizontally();

        public void MirrorVertically();
    }

    public abstract class Image<T> : IEnumerable<T>, IBitmapSource
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

        public delegate T PipelineDelegate(T pixel);

        public delegate Point TransformDelegate(Point point);

        public Image(int pixelWidth, int pixelHeight)
        {
            Pixels = new T[pixelWidth, pixelHeight];
        }

        public Image(T[,] pixels)
        {
            Pixels = pixels;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int y = 0; y < PixelHeight; y++)
            {
                for (int x = 0; x < PixelWidth; x++)
                {
                    yield return Pixels[x, y];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Pipeline(PipelineDelegate process)
        {
            for (int y = 0; y < PixelHeight; y++)
            {
                for (int x = 0; x < PixelWidth; x++)
                {
                    Pixels[x, y] = process(Pixels[x, y]);
                }
            }
        }

        public abstract BitmapSource ToBitmapSource(double dpiX, double dpiY);
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