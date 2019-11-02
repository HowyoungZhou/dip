using System;

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

        public RGBPixel(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public HSLPixel ToHSV()
        {
            float r = R / 255f, g = G / 255f, b = B / 255f;
            float min = Math.Min(r, Math.Min(g, b));
            float max = Math.Max(r, Math.Max(g, b));
            float l = (max + min) / 2;

            float s = 0;
            if (l == 0 || max == min) s = 0;
            else if (l <= 0.5) s = (max - min) / (2 * l);
            else s = (max - min) / (2 - 2 * l);

            float h = 0;
            if (max != min)
            {
                if (max == r) h = 60 * (g - b) / (max - min) + (g >= b ? 0 : 360);
                else if (max == g) h = 60 * (b - r) / (max - min) + 120;
                else h = 60 * (r - g) / (max - min) + 240;
            }

            return new HSLPixel(h, s, l, A);
        }
    }

    public struct HSLPixel
    {
        public byte A { get; set; }

        public float H { get; set; }

        public float S { get; set; }

        public float L { get; set; }

        public HSLPixel(float h, float s, float l, byte a)
        {
            H = h;
            S = s;
            L = l;
            A = a;
        }

        public RGBPixel ToRGB()
        {
            float a = S * Math.Min(L, 1 - L);
            HSLPixel hsl = this;
            float f(float n)
            {
                float k = (n + hsl.H / 30) % 12;
                return hsl.L - a * Math.Max(Math.Min(Math.Min(k - 3, 9 - k), 1), -1);
            }

            return new RGBPixel((byte)Math.Round(f(0) * 255), (byte)Math.Round(f(8) * 255), (byte)Math.Round(f(4) * 255), A);
        }
    }
}