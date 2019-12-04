using DipLib;

namespace DipWpf
{
    public partial class ImageHelper
    {
        public void MirrorHorizontally()
        {
            (DipLibImage as ITransformableImage).MirrorHorizontally();
            RefreshImage();
        }

        public void MirrorVertically()
        {
            (DipLibImage as ITransformableImage).MirrorVertically();
            RefreshImage();
        }

        public void Translate(int dx, int dy)
        {
            DipLibImage = (IBitmapSource) (DipLibImage as ITransformableImage).Translate(dx, dy);
            RefreshImage();
        }

        public void RotateD(int originX, int originY, double angle)
        {
            DipLibImage =
                (IBitmapSource) (DipLibImage as ITransformableImage).RotateD(new Point(originX, originY), angle);
            RefreshImage();
        }

        public void Shear(double dx, double dy)
        {
            DipLibImage = (IBitmapSource) (DipLibImage as ITransformableImage).Shear(dx, dy);
            RefreshImage();
        }

        public void ScaleWithNni(double kx, double ky)
        {
            DipLibImage =
                (IBitmapSource) (DipLibImage as ITransformableImage).Scale(kx, ky,
                    Interpolation.NearestNeighborInterpolation);
            RefreshImage();
        }
        
        public void ScaleWithBi(double kx, double ky)
        {
            DipLibImage =
                (IBitmapSource) (DipLibImage as ITransformableImage).Scale(kx, ky,
                    Interpolation.BilinearInterpolation);
            RefreshImage();
        }
    }
}