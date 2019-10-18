namespace DipLib
{
    public class BinaryImage
    {
        public enum BinaryPixel
        {
            White,
            Black
        }

        public int PixelHeight { get => Pixels.GetLength(1); }

        public int PixelWidth { get => Pixels.GetLength(0); }

        public BinaryPixel this[int x, int y]
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

        public BinaryPixel this[Point point]
        {
            get
            {
                return Pixels[point.x, point.y];
            }
            set
            {
                Pixels[point.x, point.y] = value;
            }
        }

        public BinaryPixel[,] Pixels { get; set; }

        public BinaryImage(int pixelWidth, int pixelHeight)
        {
            Pixels = new BinaryPixel[pixelWidth, pixelHeight];
        }

        public BinaryImage(BinaryPixel[,] pixels)
        {
            Pixels = pixels;
        }

        public BinaryImage(byte[] grayScalePixels, int pixelWidth, int pixelHeight)
        {
            double threshold = GetThresholdByOtsu(grayScalePixels);
            Pixels = new BinaryPixel[pixelWidth, pixelHeight];
            for (int x = 0; x < pixelWidth; x++)
            {
                for (int y = 0; y < pixelHeight; y++)
                {
                    byte pixel = grayScalePixels[y * pixelWidth + x];
                    Pixels[x, y] = pixel > threshold ? BinaryPixel.White : BinaryPixel.Black;
                }
            }
        }

        public static BinaryImage operator !(BinaryImage image)
        {
            var res = new BinaryImage(image.PixelWidth, image.PixelHeight);
            for (int x = 0; x < image.PixelWidth; x++)
            {
                for (int y = 0; y < image.PixelHeight; y++)
                {
                    res[x, y] = (image[x, y] == BinaryPixel.Black ? BinaryPixel.White : BinaryPixel.Black);
                }
            }
            return res;
        }

        private static long[] GetGrayScaleHistogram(byte[] grayScalePixels)
        {
            var res = new long[256];
            foreach (var pixel in grayScalePixels)
            {
                res[pixel]++;
            }
            return res;
        }

        private static double GetThresholdByOtsu(byte[] grayScalePixels)
        {
            long pixelsCount = grayScalePixels.LongLength;
            long[] histogram = GetGrayScaleHistogram(grayScalePixels);

            long classSumTotal = 0;
            for (int i = 1; i < 256; i++) classSumTotal += i * histogram[i];

            long classSumLeft = 0;
            long classProbLeft = 0;
            double maxInnerVar = 0;
            double threshold1 = 0;
            double threshold2 = 0;

            for (int threshold = 0; threshold < 256; threshold++)
            {
                long probability = histogram[threshold];
                classProbLeft += probability;
                if (classProbLeft == 0) continue;

                long classProbRight = pixelsCount - classProbLeft;
                if (classProbRight == 0) continue;

                classSumLeft += threshold * probability;
                double classMeanDiff = (double)classSumLeft / classProbLeft - (double)(classSumTotal - classSumLeft) / classProbRight;
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
                    data[y * PixelWidth + x] = (byte)(Pixels[x, y] == BinaryPixel.Black ? 0 : 255);
                }
            }
            return data;
        }

        private bool IsPointInImage(Point point)
        {
            return point.x > 0 && point.x < PixelWidth && point.y > 0 && point.y < PixelHeight;
        }

        private bool IsOverlap(StructuringElement structElement, Point originOnImage)
        {
            for (int dx = 0; dx < structElement.PixelWidth; dx++)
            {
                for (int dy = 0; dy < structElement.PixelHeight; dy++)
                {
                    Point point = originOnImage - structElement.Origin + new Point(dx, dy);
                    //if (x1 < 0 || x1 >= PixelWidth || y1 < 0 || y1 >= PixelHeight) continue;
                    if (!IsPointInImage(point)) continue;
                    if (structElement[dx, dy] == BinaryPixel.Black && this[point] == BinaryPixel.Black) return true;
                }
            }
            return false;
        }

        private bool IsCovered(StructuringElement structElement, Point originOnImage)
        {
            for (int dx = 0; dx < structElement.PixelWidth; dx++)
            {
                for (int dy = 0; dy < structElement.PixelHeight; dy++)
                {
                    Point point = originOnImage - structElement.Origin + new Point(dx, dy);
                    if (!IsPointInImage(point)) continue;
                    if (structElement[dx, dy] == BinaryPixel.Black && this[point] != BinaryPixel.Black) return false;
                }
            }
            return true;
        }

        public BinaryImage Dilation(StructuringElement structElement)
        {
            var res = new BinaryImage(PixelWidth, PixelHeight);
            for (int x = 0; x < PixelWidth; x++)
            {
                for (int y = 0; y < PixelHeight; y++)
                {
                    if (IsOverlap(structElement, new Point(x, y))) res[x, y] = BinaryPixel.Black;
                }
            }
            return res;
        }

        public BinaryImage Erotion(StructuringElement structElement)
        {
            var res = new BinaryImage(PixelWidth, PixelHeight);
            for (int x = 0; x < PixelWidth; x++)
            {
                for (int y = 0; y < PixelHeight; y++)
                {
                    if (IsCovered(structElement, new Point(x, y))) res[x, y] = BinaryPixel.Black;
                }
            }
            return res;
        }

        public BinaryImage Open(StructuringElement structElement)
        {
            return Erotion(structElement).Dilation(structElement);
        }

        public BinaryImage Close(StructuringElement structElement)
        {
            return Dilation(structElement).Erotion(structElement);
        }
    }

    public class StructuringElement : BinaryImage
    {
        public Point Origin { get; set; }

        public StructuringElement(BinaryPixel[,] pixels, Point origin) : base(pixels)
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
                    element[x, y] = element.Origin.GetSquaredDistance(new Point(x, y)) < squaredRadis ? BinaryImage.BinaryPixel.Black : BinaryImage.BinaryPixel.White;
                }
            }
            return element;
        }
    }

    public static class BinaryPixels
    {
        public static byte Black { get => 0; }
        public static byte White { get => 255; }
    }

    public struct Point
    {
        public int x;
        public int y;

        public Point(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y + p2.y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y);
        }

        public int GetSquaredDistance(Point point)
        {
            return (x - point.x) * (x - point.x) + (y - point.y) * (y - point.y);
        }
    }
}
