using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DipLib
{
    public class GrayscaleImage : Image<byte>
    {
        public GrayscaleImage(byte[,] pixels) : base(pixels) { }

        public GrayscaleImage(int pixelWidth, int pixelHeight) : base(pixelWidth, pixelHeight) { }

        public GrayscaleImage(byte[] grayScalePixels, int pixelWidth, int pixelHeight) : base(pixelWidth, pixelHeight)
        {
            for (int x = 0; x < pixelWidth; x++)
            {
                for (int y = 0; y < pixelHeight; y++)
                {
                    Pixels[x, y] = grayScalePixels[y * pixelWidth + x];
                }
            }
        }

        private byte GetMaxLuminance()
        {
            byte max = 0;
            foreach (var pixel in this)
            {
                if (pixel > max) max = pixel;

            }
            return max;
        }

        public void EnhanceVisibility()
        {
            double logMax = Math.Log10(GetMaxLuminance() + 1);
            Pipeline((pixel) => Convert.ToByte(Math.Log10(pixel + 1) / logMax));
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

        public override BitmapSource ToBitmapSource(double dpiX, double dpiY)
        {
            return BitmapSource.Create(PixelWidth, PixelHeight, dpiX, dpiY, PixelFormats.Gray8, BitmapPalettes.Gray256, ToPixelsData(), PixelWidth);
        }
    }
}