using System.Windows.Media;
using System.Windows.Media.Imaging;
using DipLib;

namespace DipWpf
{
    public partial class ImageHelper
    {
        private void GetRgbImage()
        {
            if (DipLibImage is RgbImage) return;
            Image = new FormatConvertedBitmap(OriginImage, PixelFormats.Bgra32, null, 0);
            byte[] pixels = new byte[Image.PixelWidth * Image.PixelHeight * 4];
            Image.CopyPixels(pixels, Image.PixelWidth * 4, 0);
            DipLibImage = new RgbImage(pixels, Image.PixelWidth, Image.PixelHeight);
        }

        private void GetGrayscaleImage()
        {
            if (DipLibImage is GrayscaleImage) return;
            Image = new FormatConvertedBitmap(OriginImage, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
            byte[] pixels = new byte[Image.PixelWidth * Image.PixelHeight];
            Image.CopyPixels(pixels, Image.PixelWidth, 0);
            DipLibImage = new GrayscaleImage(pixels, Image.PixelWidth, Image.PixelHeight);
        }

        public void ConvertToGrayscale()
        {
            GetGrayscaleImage();
            NotifyPropertyChanged(nameof(Image));
        }

        public void EnhanceVisibility()
        {
            if (DipLibImage is GrayscaleImage) (DipLibImage as GrayscaleImage).EnhanceVisibility();
            else
            {
                if (!(DipLibImage is RgbImage)) GetRgbImage();
                (DipLibImage as RgbImage).EnhanceVisibility();
            }
            RefreshImage();
        }

        public void LightnessLinearStretch()
        {
            if (DipLibImage is GrayscaleImage) (DipLibImage as GrayscaleImage).LightnessLinearStretch();
            else
            {
                if (!(DipLibImage is RgbImage)) GetRgbImage();
                (DipLibImage as RgbImage).LightnessLinearStretch();
            }
            RefreshImage();
        }

        public void GrayscaleHistogramEqualization()
        {
            GetGrayscaleImage();
            (DipLibImage as GrayscaleImage).HistogramEqualization();
            RefreshImage();
        }

        public void SaturationHistogramEqualization()
        {
            GetRgbImage();
            (DipLibImage as RgbImage).SaturationHistogramEqualization(101);
            RefreshImage();
        }

        public void LightnessHistogramEqualization()
        {
            GetRgbImage();
            (DipLibImage as RgbImage).LightnessHistogramEqualization(101);
            RefreshImage();
        }
    }
}