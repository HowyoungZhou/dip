namespace DipLib
{
    public class HslImage:Image<HslPixel>
    {
        public HslImage(int pixelWidth, int pixelHeight) : base(pixelWidth, pixelHeight)
        {
        }

        public HslImage(HslPixel[,] pixels) : base(pixels)
        {
        }
    }
}