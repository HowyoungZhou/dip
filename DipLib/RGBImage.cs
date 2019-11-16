using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DipLib
{
    public class RGBImage : Image<RGBPixel?>, ITransformableImage
    {
        public RGBImage(RGBPixel?[,] pixels) : base(pixels)
        {
        }

        public RGBImage(int pixelWidth, int pixelHeight) : base(pixelWidth, pixelHeight)
        {
        }

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
                    Pixels[x, y] = new RGBPixel(bgraPixels[offset + 2], bgraPixels[offset + 1], bgraPixels[offset + 0],
                        bgraPixels[offset + 3]);
                }
            }
        }

        private float GetMaxLuminance()
        {
            float max = 0;
            foreach (var pixel in this)
            {
                Debug.Assert(pixel != null, nameof(pixel) + " != null");
                float l = pixel.Value.ToHSL().L;
                if (l > max) max = l;
            }

            return max;
        }

        private float GetMinLuminance()
        {
            float min = 1;
            foreach (var pixel in this)
            {
                Debug.Assert(pixel != null, nameof(pixel) + " != null");
                float l = pixel.Value.ToHSL().L;
                if (l < min) min = l;
            }

            return min;
        }

        public void EnhanceVisibility()
        {
            double logMax = Math.Log10(GetMaxLuminance() + 1);
            Pipeline((pixel) =>
            {
                Debug.Assert(pixel != null, nameof(pixel) + " != null");
                var hsl = pixel.Value.ToHSL();
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
                Debug.Assert(pixel != null, nameof(pixel) + " != null");
                var hsl = pixel.Value.ToHSL();
                hsl.L = (hsl.L - min) / k;
                return hsl.ToRGB();
            });
        }

        private long[] GetHistogramOfSaturation(int levels)
        {
            var res = new long[levels];
            foreach (var pixel in this)
            {
                Debug.Assert(pixel != null, nameof(pixel) + " != null");
                int level = (int) Math.Round(pixel.Value.ToHSL().S * (levels - 1));
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
            var minCDF = cdf.Min();
            double denominator = PixelWidth * PixelHeight - minCDF;
            Pipeline((pixel) =>
            {
                Debug.Assert(pixel != null, nameof(pixel) + " != null");
                var hsl = pixel.Value.ToHSL();
                int level = (int) Math.Round(hsl.S * (levels - 1));
                hsl.S = Convert.ToSingle((cdf[level] - minCDF) / denominator);
                return hsl.ToRGB();
            });
        }

        private long[] GetHistogramOfLightness(int levels)
        {
            var res = new long[levels];
            foreach (var pixel in this)
            {
                Debug.Assert(pixel != null, nameof(pixel) + " != null");
                int level = (int) Math.Round(pixel.Value.ToHSL().L * (levels - 1));
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
            var minCDF = cdf.Min();
            double denominator = PixelWidth * PixelHeight - minCDF;
            Pipeline((pixel) =>
            {
                Debug.Assert(pixel != null, nameof(pixel) + " != null");
                var hsl = pixel.Value.ToHSL();
                int level = (int) Math.Round(hsl.L * (levels - 1));
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

        public ITransformableImage Translate(int dx, int dy)
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

        public ITransformableImage RotateD(Point origin, double angle)
        {
            var vertices = new Point[]
            {
                Point.Zeros.RotateD(origin, angle),
                new Point(PixelWidth - 1, 0).RotateD(origin, angle),
                new Point(PixelWidth - 1, PixelHeight - 1).RotateD(origin, angle),
                new Point(0, PixelHeight - 1).RotateD(origin, angle)
            };

            var xs = vertices.Select((point) => point.X).ToArray();
            var ys = vertices.Select((point) => point.Y).ToArray();

            int left = xs.Min(), right = xs.Max(), top = ys.Min(), bottom = ys.Max();

            var newOrigin = new Point(left, top);
            var res = Transform(
                right - left + 1,
                bottom - top + 1,
                (point) => point.RotateD(origin, angle) - newOrigin
            );

            var borders = new Line[]
            {
                new Line(vertices[3], vertices[0]),
                new Line(vertices[0], vertices[1]),
                new Line(vertices[1], vertices[2]),
                new Line(vertices[2], vertices[3])
            };

            for (var y = 0; y < res.PixelHeight; y++)
            {
                var inters = new List<int>();
                foreach (var border in borders)
                {
                    var inter = Line.Intersection(border, new Line(y, Line.Axis.Y));
                    if (inter == null) continue;
                    var x = inter.Value.X;
                    if (x < 0 || x >= res.PixelWidth) continue;
                    inters.Add(x);
                }

                res.RowInterpolation(y, inters.Min(), inters.Max());
            }

            res.Pipeline((pixel) => pixel ?? RGBColors.Transparent);
            return res;
        }

        private void RowInterpolation(int y, int start, int end)
        {
            for (int x = start; x <= end; x++)
            {
                if (Pixels[x, y] != null) continue;
                Pixels[x, y] = FindNearestInRow(x, y, start, end);
            }
        }

        private RGBPixel FindNearestInRow(int x, int y, int start, int end)
        {
            for (int cursor = 1;; cursor++)
            {
                if (x - cursor >= start && Pixels[x - cursor, y] != null) return Pixels[x - cursor, y].Value;
                if (x + cursor <= end && Pixels[x + cursor, y] != null) return Pixels[x + cursor, y].Value;
            }
        }

        public byte[] ToBrgaPixelsData()
        {
            byte[] data = new byte[PixelHeight * PixelWidth * 4];
            for (int x = 0; x < PixelWidth; x++)
            {
                for (int y = 0; y < PixelHeight; y++)
                {
                    Debug.Assert(Pixels[x, y] != null);
                    var pixel = Pixels[x, y].Value;
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
            return BitmapSource.Create(PixelWidth, PixelHeight, dpiX, dpiY, PixelFormats.Bgra32, null,
                ToBrgaPixelsData(), PixelWidth * 4);
        }
    }
}