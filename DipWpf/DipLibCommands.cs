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

        public static RoutedCommand HistogramEqualization { get; } = new RoutedCommand();

        public static RoutedCommand LightnessLinearStretch { get; } = new RoutedCommand();
    }
}