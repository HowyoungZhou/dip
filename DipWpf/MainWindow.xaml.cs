using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace DipWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ImageHelper ImageHelper { get; set; }

        public MainWindow()
        {
            DataContext = this;
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
            openFileDialog.Filter = "所有支持的格式|*.png;*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.jfi;*.bmp;*.dib;*.gif;*.tiff;*.tif;*.wmp|"+
                                    "PNG (*.png)|*.png|" +
                                    "JPEG (*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.jfi)|*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.jfi|" +
                                    "BMP (*.bmp;*.dib)|*.bmp;*.dib|" +
                                    "GIF (*.gif)|*.gif|" +
                                    "TIFF (*.tiff;*.tif)|*.tiff;*.tif|" +
                                    "WMP (*.wmp)|*.wmp|" +
                                    "所有文件 (*.*)|*.*";
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

        private void Binarize(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.Binarize();
        }

        public void Dilation(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.Dilation();
        }

        public void Erosion(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.Erosion();
        }

        public void MorphologyOpen(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.MorphologyOpen();
        }

        public void MorphologyClose(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.MorphologyClose();
        }

        public void GrayscaleEnhanceVisibility(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.GrayscaleEnhanceVisibility();
        }

        public void GrayscaleHistogramEqualization(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.GrayscaleHistogramEqualization();
        }
    }
}
