namespace DipLib
{
    public enum BinaryPixel
    {
        White,
        Black
    }

    public struct RGBPixel
    {
        public byte A { get; set; }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public RGBPixel(byte b, byte r, byte g, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }

    public struct HSVPixel
    {
        public byte A { get; set; }

        public byte H { get; set; }

        public byte S { get; set; }

        public byte V { get; set; }
    }
}