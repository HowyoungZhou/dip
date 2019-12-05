using System;
using System.Diagnostics;

namespace DipLib
{
    public class HslImage : Image<HslPixel>, IFilterableImage
    {
        public HslImage(int pixelWidth, int pixelHeight) : base(pixelWidth, pixelHeight)
        {
        }

        public HslImage(HslPixel[,] pixels) : base(pixels)
        {
        }

        public HslImage Map(PixelPositionPipelineDelegate process)
        {
            var res = new HslImage(PixelWidth, PixelHeight);
            ForEach((pixel, position) => res[position] = process(pixel, position));
            return res;
        }

        public HslImage ParallelMap(PixelPositionPipelineDelegate process)
        {
            var res = new HslImage(PixelWidth, PixelHeight);
            ParallelForEach((pixel, position) => res[position] = process(pixel, position));
            return res;
        }

        public RgbImage ToRgbImage()
        {
            var res = new RgbImage(PixelWidth, PixelHeight);
            ForEach((pixel, position) => res[position] = pixel.ToRgb());
            return res;
        }

        public IFilterableImage Filter(Filter filter)
        {
            return ParallelMap((pixel, position) =>
            {
                double res = 0;
                for (int x = 0; x < filter.PixelWidth; x++)
                {
                    for (int y = 0; y < filter.PixelHeight; y++)
                    {
                        var targetPos = position - filter.Origin + new Point(x, y);
                        targetPos.X = targetPos.X.LimitTo(0, PixelWidth - 1);
                        targetPos.Y = targetPos.Y.LimitTo(0, PixelHeight - 1);
                        res += filter[x, y] * this[targetPos].L;
                    }
                }

                return new HslPixel(pixel.H, pixel.S, Convert.ToSingle(res).LimitTo(0, 1), pixel.A);
            });
        }

        public IFilterableImage MeanFilter(int size) => Filter(Filters.Mean(size));

        public IFilterableImage LaplacianFilter() => Filter(Filters.Laplacian);

        public IFilterableImage ExtendedLaplacianFilter() => Filter(Filters.ExtendedLaplacian);

        public IFilterableImage BilateralFilter(double sigmaD, double sigmaR)
        {
            return ParallelMap((pixel, position) =>
            {
                double weight = 0, l = 0;
                ForEach((neighbor, neighborPos) =>
                {
                    double w = Math.Exp(-position.GetSquaredDistance(neighborPos) / (2 * sigmaD.Squared()) -
                                        (pixel.L - neighbor.L).Squared() / (2 * sigmaR.Squared()));
                    weight += w;
                    l += w * neighbor.L;
                });
                return new HslPixel(pixel.H, pixel.S, Convert.ToSingle(l / weight).LimitTo(0, 1), pixel.A);
            });
        }
    }
}