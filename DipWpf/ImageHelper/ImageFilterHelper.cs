using DipLib;

namespace DipWpf
{
    public partial class ImageHelper
    {
        public void MeanFilter(int size)
        {
            DipLibImage = (IBitmapSource) ((IFilterableImage) DipLibImage).MeanFilter(size);
            RefreshImage();
        }
        
        public void LaplacianFilter()
        {
            DipLibImage = (IBitmapSource) ((IFilterableImage) DipLibImage).LaplacianFilter();
            RefreshImage();
        }
        
        public void ExtendedLaplacianFilter()
        {
            DipLibImage = (IBitmapSource) ((IFilterableImage) DipLibImage).ExtendedLaplacianFilter();
            RefreshImage();
        }
    }
}