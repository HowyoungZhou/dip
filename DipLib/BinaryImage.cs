namespace DipLib
{
    public class BinaryImage
    {
        public int PixelHeight { get => Pixels.GetLength(1); }

        public int PixelWidth { get => Pixels.GetLength(0); }

        public byte[,] Pixels { get; set; }

        public BinaryImage(int pixelWidth, int pixelHeight)
        {
            Pixels = new byte[pixelWidth, pixelHeight];
        }

        public BinaryImage(byte[,] pixels)
        {
            Pixels = pixels;
        }

        public BinaryImage(byte[] grayScalePixels, int pixelWidth, int pixelHeight)
        {
            double threshold = GetThresholdByOtsu(grayScalePixels);
            Pixels = new byte[pixelWidth, pixelHeight];
            for (int x = 0; x < pixelWidth; x++)
            {
                for (int y = 0; y < pixelHeight; y++)
                {
                    byte pixel = grayScalePixels[y * pixelWidth + x];
                    Pixels[x, y] = (byte)(pixel > threshold ? 255 : 0);
                }
            }
        }

        private static int[] GetGrayScaleHistogram(byte[] grayScalePixels)
        {
            var res = new int[256];
            foreach (var pixel in grayScalePixels)
            {
                res[pixel]++;
            }
            return res;
        }

        private static double GetThresholdByOtsu(byte[] grayScalePixels)
        {
            int pixelsCount = grayScalePixels.Length;
            int[] histogram = GetGrayScaleHistogram(grayScalePixels);

            int classSumTotal = 0;
            for (int i = 1; i < 256; i++) classSumTotal += i * histogram[i];

            int classSumLeft = 0;
            int classProbLeft = 0;
            double maxInnerVar = 0;
            double threshold1 = 0;
            double threshold2 = 0;

            for (int threshold = 0; threshold < 256; threshold++)
            {
                int probability = histogram[threshold];
                classProbLeft += probability;
                if (classProbLeft == 0) continue;

                int classProbRight = pixelsCount - classProbLeft;
                if (classProbRight == 0) continue;

                classSumLeft += threshold * probability;
                int classMeanDiff = classSumLeft / classProbLeft - (classSumTotal - classSumLeft) / classProbRight;
                double innerClassVar = classProbLeft * classProbRight * classMeanDiff * classMeanDiff;

                if (innerClassVar >= maxInnerVar)
                {
                    threshold1 = threshold;
                    if (innerClassVar > maxInnerVar) threshold2 = threshold;
                    maxInnerVar = innerClassVar;
                }
            }
            return (threshold1 + threshold2) / 2;
        }

        public byte[] ToPixelsData()
        {
            byte[] data = new byte[PixelHeight * PixelWidth];
            for (int x = 0; x < PixelWidth; x++)
            {
                for (int y = 0; y < PixelHeight; y++)
                {
                    data[y * PixelWidth + x] = Pixels[x, y];
                }
            }
            return data;
        }
    }

    public class StructuringElement : BinaryImage
    {
        public Point Origin { get; set; }

        public StructuringElement(byte[,] pixels, Point origin) : base(pixels)
        {
            Origin = origin;
        }

        public StructuringElement(int pixelWidth, int pixelHeight, Point origin) : base(pixelWidth, pixelHeight)
        {
            Origin = origin;
        }
    }

    public static class StructuringElements
    {
        public static StructuringElement Circle(int radius)
        {
            var origin = new Point(radius, radius);
            var element = new StructuringElement(radius * 2, radius * 2, origin);
            int squaredRadis = radius * radius;
            for (int x = 0; x < element.PixelWidth; x++)
            {
                for (int y = 0; y < element.PixelHeight; y++)
                {
                    element.Pixels[x, y] = (byte)(element.Origin.GetSquaredDistance(origin) < squaredRadis ? 255 : 0);
                }
            }
            return element;
        }
    }

    public struct Point
    {
        int x;
        int y;

        public Point(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public int GetSquaredDistance(Point point)
        {
            return (x - point.x) * (x - point.x) + (y - point.y) * (y - point.y);
        }
    }
}
