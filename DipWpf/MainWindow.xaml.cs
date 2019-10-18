using DipLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DipWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ImageSource CurrentImage { get; set; }

        public MainWindow()
        {
            DataContext = this;

            var image = new BitmapImage(new Uri(@"E:\sample2.jpg"));
            var grayScale = new FormatConvertedBitmap(image, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
            byte[] pixels = new byte[grayScale.PixelWidth * grayScale.PixelHeight];
            grayScale.CopyPixels(pixels, grayScale.PixelWidth, 0);
            var binaryImageData = new BinaryImage(pixels, grayScale.PixelWidth, grayScale.PixelHeight).Close(StructuringElements.Circle(2));
            CurrentImage = BitmapSource.Create(grayScale.PixelWidth, grayScale.PixelHeight, grayScale.DpiX, grayScale.DpiY, PixelFormats.Gray8, BitmapPalettes.Gray256, binaryImageData.ToPixelsData(), binaryImageData.PixelWidth);
            //CurrentImage = binaryImageData;

            InitializeComponent();
        }
    }
}
