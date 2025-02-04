using System.Windows.Input;

namespace DipWpf
{
    public static class DipLibCommands
    {
        public static RoutedCommand ConvertToGrayscale { get; } = new RoutedCommand();

        public static RoutedCommand Binarize { get; } = new RoutedCommand();

        public static RoutedCommand Erosion { get; } = new RoutedCommand();

        public static RoutedCommand Dilation { get; } = new RoutedCommand();

        public static RoutedCommand MorphologyOpen { get; } = new RoutedCommand();

        public static RoutedCommand MorphologyClose { get; } = new RoutedCommand();

        public static RoutedCommand EnhanceVisibility { get; } = new RoutedCommand();

        public static RoutedCommand GrayscaleHistogramEqualization { get; } = new RoutedCommand();

        public static RoutedCommand LightnessHistogramEqualization { get; } = new RoutedCommand();

        public static RoutedCommand SaturationHistogramEqualization { get; } = new RoutedCommand();

        public static RoutedCommand LightnessLinearStretch { get; } = new RoutedCommand();

        public static RoutedCommand MirrorHorizontally { get; } = new RoutedCommand();

        public static RoutedCommand MirrorVertically { get; } = new RoutedCommand();

        public static RoutedCommand Translate { get; } = new RoutedCommand();

        public static RoutedCommand Rotate { get; } = new RoutedCommand();

        public static RoutedCommand Shear { get; } = new RoutedCommand();

        public static RoutedCommand ScaleWithNni { get; } = new RoutedCommand();

        public static RoutedCommand ScaleWithBi { get; } = new RoutedCommand();

        public static RoutedCommand MeanFilter { get; } = new RoutedCommand();

        public static RoutedCommand LaplacianFilter { get; } = new RoutedCommand();

        public static RoutedCommand ExtendedLaplacianFilter { get; } = new RoutedCommand();

        public static object BilateralFilter { get; } = new RoutedCommand();
    }
}