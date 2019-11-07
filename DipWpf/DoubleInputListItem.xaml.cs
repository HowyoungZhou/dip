using System.Windows.Controls;

namespace DipWpf
{
    /// <summary>
    /// DoubleInputListItem.xaml 的交互逻辑
    /// </summary>
    public partial class DoubleInputListItem : UserControl
    {
        public DoubleInputItem InputItem { get; private set; }

        public DoubleInputListItem(DoubleInputItem inputItem)
        {
            InputItem = inputItem;
            DataContext = InputItem;
            InitializeComponent();
        }
    }
}
