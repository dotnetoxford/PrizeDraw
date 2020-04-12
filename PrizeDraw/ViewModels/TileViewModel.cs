using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace PrizeDraw.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public string Name { get; }
        public string AttendeeId { get; }
        public SolidColorBrush Color { get; }
        public string RemoteImageUri;
        public BitmapImage BitmapImage { get; private set; }

        public bool IsAvailable => !IsSelected;
        public bool IsAvailableAndNotSelected => IsAvailable && !IsNoShow && !IsWinner;

        public TileViewModel(string name, string attendeeId, string remoteImageUri, SolidColorBrush color)
        {
            Name = name;
            AttendeeId = attendeeId;
            Color = color;
            RemoteImageUri = remoteImageUri;
        }

        public void LoadImage(string imageFolder)
        {
            var imagePath = Path.Combine(imageFolder, AttendeeId + ".jpg");
            var image = File.Exists(imagePath) ? imagePath : @"Images/NoPhoto.png";

            BitmapImage = LoadBitmap(image);
        }

        private BitmapImage LoadBitmap(string image)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmap.UriSource = new Uri(image, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            return bitmap;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                Set(nameof(IsSelected), ref _isSelected, value);
                RaisePropertyChanged(nameof(IsAvailableAndNotSelected));
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        private bool _isWinner;

        public bool IsWinner
        {
            get => _isWinner;
            set
            {
                Set(nameof(IsWinner), ref _isWinner, value);
                RaisePropertyChanged(nameof(IsAvailableAndNotSelected));
                RaisePropertyChanged(nameof(IsSelected));
                RaisePropertyChanged(nameof(IsNoShow));
                RaisePropertyChanged(nameof(IsWinner));
            }
        }

        private bool _isNoShow;

        public bool IsNoShow
        {
            get => _isNoShow;
            set
            {
                Set(nameof(IsNoShow), ref _isNoShow, value);
                RaisePropertyChanged(nameof(IsAvailableAndNotSelected));
                RaisePropertyChanged(nameof(IsSelected));
                RaisePropertyChanged(nameof(IsNoShow));
                RaisePropertyChanged(nameof(IsWinner));
            }
        }
    }
}
