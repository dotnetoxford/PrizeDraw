using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace PrizeDraw.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public string Name { get; set; }
        private static readonly SolidColorBrush SelectedColor
            = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                Set(nameof(IsSelected), ref _isSelected, value);
                RaisePropertyChanged("Color");
            }
        }

        private SolidColorBrush _color;
        public SolidColorBrush Color
        {
            get { return IsSelected ? SelectedColor : _color; }
            set { _color = value; }
        }
    }
}
