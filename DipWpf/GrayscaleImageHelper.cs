using System.Windows.Media;
using System.Windows.Media.Imaging;
using DipLib;

namespace DipWpf
{
    public partial class ImageHelper
    {
        private void GetGrayscaleImage()
        {
            if(DipLibImage is GrayscaleImage) return;
            Image = new FormatConvertedBitmap(OriginImage, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
            byte[] pixels = new byte[Image.PixelWidth * Image.PixelHeight];
            Image.CopyPixels(pixels, Image.PixelWidth, 0);
            DipLibImage = new GrayscaleImage(pixels, Image.PixelWidth, Image.PixelHeight);
        }

        public void ConvertToGrayscale()
        {
            GetGrayscaleImage();
            NotifyPropertyChanged("Image");
        }

        public void GrayscaleEnhanceVisibility(){
            GetGrayscaleImage();
            (DipLibImage as GrayscaleImage).EnhanceVisibility();
            RefreshImage();
        }

        public void GrayscaleHistogramEqualization(){
            GetGrayscaleImage();
            (DipLibImage as GrayscaleImage).HistogramEqualization();
            RefreshImage();
        }
    }
}