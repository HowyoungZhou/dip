using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DipLib;
using Microsoft.Win32;

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
    }

    public class ImageHelper : INotifyPropertyChanged
    {
        public enum FileType
        {
            Png,
            Jpeg,
            Bmp,
            Gif,
            Tiff,
            Wmp
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapSource Image { get; set; }

        // public BitmapSource Grayscale { get; set; }

        // public BinaryImage BinaryImage { get; set; }

        public BitmapImage OriginImage { get; set; }

        public IBitmapSource DipLibImage { get; set; }

        public string FileName { get; private set; }

        public ImageHelper(string fileName)
        {
            FileName = fileName;
            Image = OriginImage = new BitmapImage(new Uri(fileName));
        }

        private void NotifyPropertyChanged(String name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private FileType? GetFileTypeByExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".png":
                    return FileType.Png;
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                case ".jif":
                case ".jfif":
                case ".jfi":
                    return FileType.Jpeg;
                case ".bmp":
                case ".dib":
                    return FileType.Bmp;
                case ".gif":
                    return FileType.Gif;
                case ".tiff":
                case ".tif":
                    return FileType.Tiff;
                case ".wmp":
                    return FileType.Wmp;
                default:
                    return null;
            }
        }

        private BitmapEncoder GetEncoder(string fileName)
        {
            switch (GetFileTypeByExtension(Path.GetExtension(fileName)))
            {
                case FileType.Png:
                    return new PngBitmapEncoder();
                case FileType.Jpeg:
                    return new JpegBitmapEncoder();
                case FileType.Bmp:
                    return new BmpBitmapEncoder();
                case FileType.Gif:
                    return new GifBitmapEncoder();
                case FileType.Tiff:
                    return new TiffBitmapEncoder();
                case FileType.Wmp:
                    return new WmpBitmapEncoder();
                default:
                    return new PngBitmapEncoder();
            }
        }

        private void SaveImage(String fileName)
        {
            var encoder = GetEncoder(fileName);
            encoder.Frames.Add(BitmapFrame.Create(Image));
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public void Save()
        {
            SaveImage(FileName);
        }

        public void SaveAs()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG (*.png)|*.png|" +
                                    "JPEG (*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.jfi)|*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.jfi|" +
                                    "BMP (*.bmp;*.dib)|*.bmp;*.dib|" +
                                    "GIF (*.gif)|*.gif|" +
                                    "TIFF (*.tiff;*.tif)|*.tiff;*.tif|" +
                                    "WMP (*.wmp)|*.wmp|" +
                                    "所有文件 (*.*)|*.*";
            if (saveFileDialog.ShowDialog() != true) return;
            SaveImage(saveFileDialog.FileName);
            FileName = saveFileDialog.FileName;
        }

        private void GetGrayscaleImage()
        {
            Image = new FormatConvertedBitmap(OriginImage, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
            byte[] pixels = new byte[Image.PixelWidth * Image.PixelHeight];
            Image.CopyPixels(pixels, Image.PixelWidth, 0);
            DipLibImage = new GrayscaleImage(pixels, Image.PixelWidth, Image.PixelHeight);
        }

        public void ConvertToGrayscale()
        {
            GetGrayscaleImage();
            NotifyPropertyChanged("Image");
        }

        private void RefreshImage()
        {
            Image = DipLibImage.ToBitmapSource(Image.DpiX, Image.DpiY);
            NotifyPropertyChanged("Image");
        }

        private void GetBinaryImage()
        {
            if (DipLibImage is BinaryImage) return;
            if (!(DipLibImage is GrayscaleImage)) GetGrayscaleImage();
            var grayscale = DipLibImage as GrayscaleImage;
            DipLibImage = new BinaryImage(grayscale.ToPixelsData(), grayscale.PixelWidth, grayscale.PixelHeight);
        }

        public void Binarize()
        {
            GetBinaryImage();
            RefreshImage();
        }

        public void Dilation()
        {
            GetBinaryImage();
            DipLibImage = (DipLibImage as BinaryImage).Dilation(StructuringElements.Cross(3));
            RefreshImage();
        }

        public void Erosion()
        {
            GetBinaryImage();
            DipLibImage = (DipLibImage as BinaryImage).Erosion(StructuringElements.Cross(3));
            RefreshImage();
        }

        public void MorphologyOpen()
        {
            GetBinaryImage();
            DipLibImage = (DipLibImage as BinaryImage).Open(StructuringElements.Cross(3));
            RefreshImage();
        }

        public void MorphologyClose()
        {
            GetBinaryImage();
            DipLibImage = (DipLibImage as BinaryImage).Close(StructuringElements.Cross(3));
            RefreshImage();
        }
    }
}
