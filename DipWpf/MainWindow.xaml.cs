﻿using Microsoft.Win32;
using System.Collections.Generic;
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
            openFileDialog.Filter = "所有支持的格式|*.png;*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.jfi;*.bmp;*.dib;*.gif;*.tiff;*.tif;*.wmp|" +
                                    "PNG (*.png)|*.png|" +
                                    "JPEG (*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.jfi)|*.jpg;*.jpeg;*.jpe;*.jif;*.jfif;*.jfi|" +
                                    "BMP (*.bmp;*.dib)|*.bmp;*.dib|" +
                                    "GIF (*.gif)|*.gif|" +
                                    "TIFF (*.tiff;*.tif)|*.tiff;*.tif|" +
                                    "WMP (*.wmp)|*.wmp|" +
                                    "所有文件 (*.*)|*.*";
            if (openFileDialog.ShowDialog() != true) return;
            ImageHelper = new ImageHelper(openFileDialog.FileName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageHelper)));
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

        public void EnhanceVisibility(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.EnhanceVisibility();
        }

        public void GrayscaleHistogramEqualization(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.GrayscaleHistogramEqualization();
        }

        public void SaturationHistogramEqualization(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.SaturationHistogramEqualization();
        }

        public void LightnessHistogramEqualization(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.LightnessHistogramEqualization();
        }

        public void LightnessLinearStretch(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.LightnessLinearStretch();
        }

        public void HorizontallyMirror(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.MirrorHorizontally();
        }

        public void VerticallyMirror(object sender, ExecutedRoutedEventArgs e)
        {
            ImageHelper.MirrorVertically();
        }

        public void Translate(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new InputDialog(new List<dynamic> {
                new IntInputItem() {
                    Label = "水平分量",
                    Maximum = ImageHelper.Image.PixelWidth,
                    SmallChange = 10,
                    LargeChange = 100,
                    TickFrequency = 50,
                },
                new IntInputItem() {
                    Label = "垂直分量",
                    Maximum = ImageHelper.Image.PixelHeight,
                    SmallChange = 10,
                    LargeChange = 100,
                    TickFrequency = 50,
                },
            });
            if (dialog.ShowDialog().Value)
            {
                int dx = (dialog.InputItems[0] as IntInputItem).Value;
                int dy = (dialog.InputItems[1] as IntInputItem).Value;
                ImageHelper.Translate(dx, dy);
            }
        }

        public void Rotate(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new InputDialog(new List<dynamic> {
                new IntInputItem() {
                    Label = "中心点横坐标",
                    Maximum = ImageHelper.Image.PixelWidth - 1,
                    Value = (ImageHelper.Image.PixelWidth - 1) / 2,
                    SmallChange = 10,
                    LargeChange = 100,
                    TickFrequency = 50,
                },
                new IntInputItem() {
                    Label = "中心点纵坐标",
                    Maximum = ImageHelper.Image.PixelHeight-1,
                    Value = (ImageHelper.Image.PixelHeight - 1) / 2,
                    SmallChange = 10,
                    LargeChange = 100,
                    TickFrequency = 50,
                },
                new DoubleInputItem() {
                    Label = "旋转角度",
                    Minimum = -180,
                    Maximum = 180,
                    SmallChange = 1,
                    LargeChange = 10,
                    TickFrequency = 30,
                    FractionDigits = 2
                }
            });
            if (dialog.ShowDialog().Value)
            {
                int originX = (dialog.InputItems[0] as IntInputItem).Value;
                int originY = (dialog.InputItems[1] as IntInputItem).Value;
                double angle = (dialog.InputItems[2] as DoubleInputItem).Value;
                ImageHelper.RotateD(originX, originY, angle);
            }
        }
    }
}
