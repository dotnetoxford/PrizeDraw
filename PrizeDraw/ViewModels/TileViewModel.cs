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
        private bool _isDrawn = false;
        private bool _isWinner = false;

        public bool IsWinner
        {
            get => _isWinner;
            set => _isWinner = value;
        }

        public bool IsDrawn
        {
            get => _isDrawn;
            set => _isDrawn = value;
        }

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

            BitmapImage = LoadBitmap(image);
        }

        public void LoadWinnerImage()
        {
            var image = @"Images/winner.png";

            BitmapImage = LoadBitmap(image);
        }

        public void LoadNoshowImage()
        {
            var image = @"Images/noshow.png";

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
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsNotSelected));
            }
        }

        public bool IsNotSelected => !IsSelected;
    }
}
