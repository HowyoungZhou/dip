using DipLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public ImageHelper ImageHelper { get; set; }

        public MainWindow()
        {
            DataContext = this;

            // var image = new BitmapImage(new Uri(@"E:\sample3.jpg"));
            // var grayScale = new FormatConvertedBitmap(image, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
            // byte[] pixels = new byte[grayScale.PixelWidth * grayScale.PixelHeight];
            // grayScale.CopyPixels(pixels, grayScale.PixelWidth, 0);
            // //var binaryImageData = new BinaryImage(pixels, grayScale.PixelWidth, grayScale.PixelHeight);
            // var binaryImageData = new BinaryImage(pixels, grayScale.PixelWidth, grayScale.PixelHeight).Close(StructuringElements.Circle(2));
            // CurrentImage = BitmapSource.Create(grayScale.PixelWidth, grayScale.PixelHeight, grayScale.DpiX, grayScale.DpiY, PixelFormats.Gray8, BitmapPalettes.Gray256, binaryImageData.ToPixelsData(), binaryImageData.PixelWidth);
            // //CurrentImage = image;

            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void IsImageOpened(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ImageHelper != null;
        }

        private void OpenImage(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            ImageHelper = new ImageHelper(openFileDialog.FileName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageHelper"));
        }

        private void SaveImage(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.Save();
        }

        private void SaveImageAs(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.SaveAs();
        }

        private void Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void ConvertToGrayscale(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.ConvertToGrayscale();
        }

        private void Binarize(object sender, ExecutedRoutedEventArgs e){
            ImageHelper.Binarize();
        }
    }
}
