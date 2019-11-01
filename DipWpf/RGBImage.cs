using System.Windows.Media;
using System.Windows.Media.Imaging;
using DipLib;

namespace DipWpf
{
    public partial class ImageHelper
    {
        private void GetRGBImage()
        {
            if (DipLibImage is RGBImage) return;
            Image = new FormatConvertedBitmap(OriginImage, PixelFormats.Bgra32, null, 0);
            byte[] pixels = new byte[Image.PixelWidth * Image.PixelHeight * 4];
            Image.CopyPixels(pixels, Image.PixelWidth * 4, 0);
            DipLibImage = new RGBImage(pixels, Image.PixelWidth, Image.PixelHeight);
        }
    }
}