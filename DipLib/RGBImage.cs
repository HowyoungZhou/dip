using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DipLib
{
    public class RGBImage : Image<RGBPixel>
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

        // private byte GetMaxLuminance()
        // {
        //     byte max = 0;
        //     foreach (var pixel in this)
        //     {
        //         if (pixel > max) max = pixel;

        //     }
        //     return max;
        // }

        // public void EnhanceVisibility()
        // {
        //     double logMax = Math.Log10(GetMaxLuminance() / 255.0 + 1);
        //     Pipeline((pixel) => Convert.ToByte(255 * Math.Log10(pixel / 255.0 + 1) / logMax));
        // }

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