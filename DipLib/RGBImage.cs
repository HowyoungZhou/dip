using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DipLib
{
    public class RGBImage : Image<RGBPixel>, ITransformableImage
    {
        public RGBImage(RGBPixel[,] pixels) : base(pixels) { }

        public RGBImage(int pixelWidth, int pixelHeight) : base(pixelWidth, pixelHeight) { }

        public RGBImage(byte[] bgraPixels, int pixelWidth, int pixelHeight) : base(pixelWidth, pixelHeight)
        {
            for (int x = 0; x < pixelWidth; x++)
            {
                for (int y = 0; y < pixelHeight; y++)
                {
                    int offset = y * PixelWidth * 4 + x * 4;

                    // offset + 0: B
                    // offset + 1: G
                    // offset + 2: R
                    // offset + 3: A
                    Pixels[x, y] = new RGBPixel(bgraPixels[offset + 2], bgraPixels[offset + 1], bgraPixels[offset + 0], bgraPixels[offset + 3]);
                }
            }
        }

        private float GetMaxLuminance()
        {
            float max = 0;
            foreach (var pixel in this)
            {
                float l = pixel.ToHSL().L;
                if (l > max) max = l;

            }
            return max;
        }

        private float GetMinLuminance()
        {
            float min = 1;
            foreach (var pixel in this)
            {
                float l = pixel.ToHSL().L;
                if (l < min) min = l;

            }
            return min;
        }

        public void EnhanceVisibility()
        {
            double logMax = Math.Log10(GetMaxLuminance() + 1);
            Pipeline((pixel) =>
            {
                var hsl = pixel.ToHSL();
                hsl.L = Convert.ToSingle(Math.Log10(hsl.L + 1) / logMax);
                return hsl.ToRGB();
            });
        }

        public void LightnessLinearStretch()
        {
            float min = GetMinLuminance();
            float k = GetMaxLuminance() - min;
            Pipeline((pixel) =>
            {
                var hsl = pixel.ToHSL();
                hsl.L = (hsl.L - min) / k;
                return hsl.ToRGB();
            });
        }

        private long[] GetHistogramOfSaturation(int levels)
        {
            var res = new long[levels];
            foreach (var pixel in this)
            {
                int level = (int)Math.Round(pixel.ToHSL().S * (levels - 1));
                res[level]++;
            }
            return res;
        }

        private long[] GetHistogramCDFOfSaturation(int levels)
        {
            var histogram = GetHistogramOfSaturation(levels);
            var res = new long[levels];
            long sum = 0;
            for (int i = 0; i < levels; i++)
            {
                res[i] = histogram[i] + sum;
                sum += histogram[i];
            }
            return res;
        }

        public void SaturationHistogramEqualization(int levels)
        {
            var cdf = GetHistogramCDFOfSaturation(levels);
            var minCDF = cdf.GetMin();
            double denominator = PixelWidth * PixelHeight - minCDF;
            Pipeline((pixel) =>
            {
                var hsl = pixel.ToHSL();
                int level = (int)Math.Round(hsl.S * (levels - 1));
                hsl.S = Convert.ToSingle((cdf[level] - minCDF) / denominator);
                return hsl.ToRGB();
            });
        }

        private long[] GetHistogramOfLightness(int levels)
        {
            var res = new long[levels];
            foreach (var pixel in this)
            {
                int level = (int)Math.Round(pixel.ToHSL().L * (levels - 1));
                res[level]++;
            }
            return res;
        }

        private long[] GetHistogramCDFOfLightness(int levels)
        {
            var histogram = GetHistogramOfLightness(levels);
            var res = new long[levels];
            long sum = 0;
            for (int i = 0; i < levels; i++)
            {
                res[i] = histogram[i] + sum;
                sum += histogram[i];
            }
            return res;
        }

        public void LightnessHistogramEqualization(int levels)
        {
            var cdf = GetHistogramCDFOfLightness(levels);
            var minCDF = cdf.GetMin();
            double denominator = PixelWidth * PixelHeight - minCDF;
            Pipeline((pixel) =>
            {
                var hsl = pixel.ToHSL();
                int level = (int)Math.Round(hsl.L * (levels - 1));
                hsl.L = Convert.ToSingle((cdf[level] - minCDF) / denominator);
                return hsl.ToRGB();
            });
        }

        public RGBImage Transform(int newWidth, int newHeight, TransformDelegate transform)
        {
            var res = new RGBImage(newWidth, newHeight);
            for (int y = 0; y < PixelHeight; y++)
            {
                for (int x = 0; x < PixelWidth; x++)
                {
                    var newPos = transform(new Point(x, y));
                    if (newPos.X < 0 || newPos.X >= newWidth) continue;
                    if (newPos.Y < 0 || newPos.Y >= newHeight) continue;
                    res[newPos] = this[x, y];
                }
            }
            return res;
        }

        public RGBImage Translate(int dx, int dy)
        {
            return Transform(PixelWidth + dx, PixelHeight + dy, (point) => point + new Point(dx, dy));
        }

        public void MirrorHorizontally()
        {
            for (int x = 0; x < PixelWidth / 2; x++)
            {
                for (int y = 0; y < PixelHeight; y++)
                {
                    var temp = this[x, y];
                    this[x, y] = this[PixelWidth - 1 - x, y];
                    this[PixelWidth - 1 - x, y] = temp;
                }
            }
        }

        public void MirrorVertically()
        {
            for (int x = 0; x < PixelWidth; x++)
            {
                for (int y = 0; y < PixelHeight / 2; y++)
                {
                    var temp = this[x, y];
                    this[x, y] = this[x, PixelHeight - 1 - y];
                    this[x, PixelHeight - 1 - y] = temp;
                }
            }
        }

        public byte[] ToBrgaPixelsData()
        {
            byte[] data = new byte[PixelHeight * PixelWidth * 4];
            for (int x = 0; x < PixelWidth; x++)
            {
                for (int y = 0; y < PixelHeight; y++)
                {
                    var pixel = Pixels[x, y];
                    int offset = y * PixelWidth * 4 + x * 4;
                    data[offset] = pixel.B;
                    data[offset + 1] = pixel.G;
                    data[offset + 2] = pixel.R;
                    data[offset + 3] = pixel.A;
                }
            }
            return data;
        }

        public override BitmapSource ToBitmapSource(double dpiX, double dpiY)
        {
            return BitmapSource.Create(PixelWidth, PixelHeight, dpiX, dpiY, PixelFormats.Bgra32, null, ToBrgaPixelsData(), PixelWidth * 4);
        }
    }
}