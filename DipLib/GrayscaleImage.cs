using System;

namespace DipLib
{
    public class GrayscaleImage : Image<byte>
    {
        public GrayscaleImage(byte[,] pixels) : base(pixels) { }

        public GrayscaleImage(int pixelWidth, int pixelHeight) : base(pixelWidth, pixelHeight) { }

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
    }
}