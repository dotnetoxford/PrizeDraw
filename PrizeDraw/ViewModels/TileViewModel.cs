using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace PrizeDraw.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public int AttendeeId { get; set; }
        public string ImageUri { get; set; }
        public string LocalImageFileName { get; set; }

        public string Image => LocalImageFileName ?? ImageUri ?? @"Images\NoPhoto.png";

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                Set(nameof(IsSelected), ref _isSelected, value);
                RaisePropertyChanged("IsNotSelected");
            }
        }

        public bool IsNotSelected => !IsSelected;

        public SolidColorBrush Color { get; set; }
    }
}
