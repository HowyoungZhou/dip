using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DipWpf
{
    /// <summary>
    /// InputDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InputDialog : Window
    {
        public string Caption { get; set; }

        public List<dynamic> InputItems { get; private set; }

        public InputDialog(List<dynamic> inputItems, string caption = "设置以下参数的值")
        {
            DataContext = this;
            Caption = caption;
            InputItems = inputItems;
            Title = caption;
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            foreach (var item in InputItems)
            {
                if (item is IntInputItem)
                {
                    inputList.Items.Add(new ListViewItem()
                    {
                        Content = new IntInputListItem(item),
                        HorizontalContentAlignment = HorizontalAlignment.Stretch
                    });
                }
                else if (item is DoubleInputItem)
                {
                    inputList.Items.Add(new ListViewItem()
                    {
                        Content = new DoubleInputListItem(item),
                        HorizontalContentAlignment = HorizontalAlignment.Stretch
                    });
                }
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
