using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;

namespace PrizeDraw.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public string Name { get; }
        public int AttendeeId { get; }
        public SolidColorBrush Color { get; }
        public string RemoteImageUri;
        public BitmapImage BitmapImage { get; private set; }

        public TileViewModel(string name, int attendeeId, string remoteImageUri, SolidColorBrush color)
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

            BitmapImage = new BitmapImage();
            BitmapImage.BeginInit();
            BitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            BitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            BitmapImage.UriSource = new Uri(image, UriKind.RelativeOrAbsolute);
            BitmapImage.EndInit();
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                Set(nameof(IsSelected), ref _isSelected, value);
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsNotSelected));
            }
        }

        public bool IsNotSelected => !IsSelected;
    }
}
