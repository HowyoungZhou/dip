using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;
namespace DipLib
{
    public class BinaryImage
    {
        public int PixelHeight { get => Pixels.GetLength(1); }

        public int PixelWidth { get => Pixels.GetLength(0); }

        public byte[,] Pixels { get; set; }

        public BinaryImage(byte[,] pixels)
        {
            Pixels = pixels;
        }

        public BinaryImage(byte[] grayScalePixels)
        {

        }

        private static int[] GetGrayScaleHistogram(byte[] grayScalePixels)
        {
            var res = new int[256];
            foreach (var pixel in grayScalePixels)
            {
                res[pixel]++;
            }
            return res;
        }

        private static int GetThresholdByOtsu(byte[] grayScalePixels)
        {
            int pixelsCount = grayScalePixels.Length;
            int[] histogram = GetGrayScaleHistogram(grayScalePixels);

            int classSumTotal = 0;
            for (int i = 0; i < 256; i++)
            {
                classSumTotal += histogram[i] * i;
            }

            int classProbLeft = 0;
            int classSumLeft = 0;
            double maxInnerVar = 0;
            int threshold1 = 0, threshold2 = 0;

            for (int threshold = 0; threshold < 256; threshold++)
            {
                int probability = histogram[threshold];
                classProbLeft += probability;
                if (classProbLeft == 0) continue;

                int classProbRight = pixelsCount - classProbLeft;
                if (classProbRight == 0) break;

                classSumLeft += threshold * probability;
                double classMeanLeft = classSumLeft / classProbLeft;
                double classMeanRight = (classSumTotal - classSumLeft) / classProbRight;
                double innerClassVar = classProbLeft * classProbRight * (classMeanLeft - classMeanRight) * (classMeanLeft - classMeanRight);
                if (innerClassVar >= maxInnerVar)
                {
                    threshold1 = threshold;
                    if (innerClassVar > maxInnerVar)
                    {
                        threshold2 = threshold;
                    }
                    maxInnerVar = innerClassVar;
                }
            }

            return (threshold1 + threshold2) / 2;
        }
    }


}
