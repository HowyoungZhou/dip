﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using DipLib;
using Microsoft.Win32;

namespace DipWpf
{
    public partial class ImageHelper : INotifyPropertyChanged
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

        public BitmapImage OriginImage { get; set; }

        public IBitmapSource DipLibImage { get; set; }

        public string FileName { get; private set; }

        public ImageHelper(string fileName)
        {
            FileName = fileName;
            Image = OriginImage = new BitmapImage(new Uri(fileName));
            GetRgbImage();
        }

        private void NotifyPropertyChanged(String name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private static FileType? GetFileTypeByExtension(string extension)
        {
            switch (extension.ToLower(new CultureInfo("en-US")))
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

        private static BitmapEncoder GetEncoder(string fileName)
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

        private void RefreshImage()
        {
            Image = DipLibImage.ToBitmapSource(Image.DpiX, Image.DpiY);
            NotifyPropertyChanged(nameof(Image));
        }
    }
}
