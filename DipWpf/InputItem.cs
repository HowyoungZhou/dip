using System;
using System.ComponentModel;

namespace DipWpf
{
    public class InputItem<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private T _value;

        public string Label { get; set; }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
    }

    public class NumberInputItem<T> : InputItem<T>
    {
        public T Minimum { get; set; }

        public T Maximum { get; set; }

        public T LargeChange { get; set; }

        public T SmallChange { get; set; }

        public T TickFrequency { get; set; }
    }

    public class IntInputItem : NumberInputItem<int>
    {
        public IntInputItem()
        {
            Minimum = 0;
            Maximum = 100;
            LargeChange = 10;
            SmallChange = 1;
            TickFrequency = 1;
        }
    }

    public class DoubleInputItem : NumberInputItem<double>
    {
        public int? FractionDigits { get; set; } = null;

        public new double Value
        {
            get => base.Value;
            set
            {
                if (FractionDigits.HasValue) value = Math.Round(value, FractionDigits.Value);
                base.Value = value;
            }
        }

        public DoubleInputItem()
        {
            Minimum = 0;
            Maximum = 10;
            LargeChange = 1;
            SmallChange = 0.1;
            TickFrequency = 0.1;
        }
    }
}