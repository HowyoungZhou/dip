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

        public static RoutedCommand Erotion { get; } = new RoutedCommand();

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

        public BitmapSource Grayscale { get; set; }

        public BinaryImage BinaryImage { get; set; }

        public string FileName { get; private set; }

        public ImageHelper(string fileName)
        {
            FileName = fileName;
            Image = new BitmapImage(new Uri(fileName));
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

        public void ConvertToGrayscale()
        {
            Grayscale = Image = new FormatConvertedBitmap(Image, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
            NotifyPropertyChanged("Image");
        }

        private void RefreshBinaryImage()
        {
            Image = BitmapSource.Create(Image.PixelWidth, Image.PixelHeight, Image.DpiX, Image.DpiY, PixelFormats.Gray8, BitmapPalettes.Gray256, BinaryImage.ToPixelsData(), BinaryImage.PixelWidth);
            NotifyPropertyChanged("Image");
        }

        private void GetBinaryImage()
        {
            if (BinaryImage != null) return;
            if (Grayscale == null)
            {
                Grayscale = new FormatConvertedBitmap(Image, PixelFormats.Gray8, BitmapPalettes.Gray256, 0);
            }
            byte[] pixels = new byte[Grayscale.PixelWidth * Grayscale.PixelHeight];
            Grayscale.CopyPixels(pixels, Grayscale.PixelWidth, 0);
            BinaryImage = new BinaryImage(pixels, Grayscale.PixelWidth, Grayscale.PixelHeight);
        }

        public void Binarize()
        {
            GetBinaryImage();
            RefreshBinaryImage();
        }

        public void Dilation()
        {
            GetBinaryImage();
            BinaryImage = BinaryImage.Dilation(StructuringElements.Cross(3));
            RefreshBinaryImage();
        }

        public void Erotion()
        {
            GetBinaryImage();
            BinaryImage = BinaryImage.Erotion(StructuringElements.Cross(3));
            RefreshBinaryImage();
        }

        public void MorphologyOpen()
        {
            GetBinaryImage();
            BinaryImage = BinaryImage.Open(StructuringElements.Cross(3));
            RefreshBinaryImage();
        }

        public void MorphologyClose()
        {
            GetBinaryImage();
            BinaryImage = BinaryImage.Close(StructuringElements.Cross(3));
            RefreshBinaryImage();
        }
    }
}
