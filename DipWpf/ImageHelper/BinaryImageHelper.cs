using DipLib;

namespace DipWpf
{
    public partial class ImageHelper
    {
        private void GetBinaryImage()
        {
            if (DipLibImage is BinaryImage) return;
            if (!(DipLibImage is GrayscaleImage)) GetGrayscaleImage();
            var grayscale = DipLibImage as GrayscaleImage;
            DipLibImage = new BinaryImage(grayscale.ToPixelsData(), grayscale.PixelWidth, grayscale.PixelHeight);
        }

        public void Binarize()
        {
            GetBinaryImage();
            RefreshImage();
        }

        public void Dilation()
        {
            GetBinaryImage();
            DipLibImage = (DipLibImage as BinaryImage).Dilation(StructuringElements.Cross(3));
            RefreshImage();
        }

        public void Erosion()
        {
            GetBinaryImage();
            DipLibImage = (DipLibImage as BinaryImage).Erosion(StructuringElements.Cross(3));
            RefreshImage();
        }

        public void MorphologyOpen()
        {
            GetBinaryImage();
            DipLibImage = (DipLibImage as BinaryImage).Open(StructuringElements.Cross(3));
            RefreshImage();
        }

        public void MorphologyClose()
        {
            GetBinaryImage();
            DipLibImage = (DipLibImage as BinaryImage).Close(StructuringElements.Cross(3));
            RefreshImage();
        }
    }
}