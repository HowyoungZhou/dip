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
            DipLibImage = (DipLibImage as ITransformableImage).Translate(dx, dy);
            RefreshImage();
        }
    }
}