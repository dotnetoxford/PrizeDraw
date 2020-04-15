using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using PrizeDraw.Helpers;
using PrizeDraw.TIleProviders;

namespace PrizeDraw.ViewModels
{
    public class MeetupDotComSyncViewModel : ViewModelBase
    {
        private readonly ITileProviderFactory _tileProviderFactory;
        private readonly IDialogService _dialogService;

        public string ButtonText => _syncInProgress ? "Please wait ..." : "Download Attendees from Meetup.com";

        public string EventId { get; set; }

        private bool _syncInProgress;

        public MeetupDotComSyncViewModel(ITileProviderFactory tileProviderFactory, IDialogService dialogService)
        {
            _tileProviderFactory = tileProviderFactory;
            _dialogService = dialogService;
        }

        public RelayCommand<ICloseableWindow> SyncCommand => new RelayCommand<ICloseableWindow>(async closableWindow => await SyncAsync(closableWindow), closableWindow => !_syncInProgress);

        private async Task SyncAsync(ICloseableWindow closableWindow)
        {
            _syncInProgress = true;
            RaisePropertyChanged(nameof(ButtonText));

            try
            {
                var sourceTileProvider = _tileProviderFactory.CreateMeetupComTileProvider(EventId);
                var targetTileProvider = _tileProviderFactory.CreateFileTileProvider();

                var tiles = await sourceTileProvider.GetTilesAsync();
                await targetTileProvider.SaveTilesAsync(tiles);

                await _dialogService.ShowMessageBox("Synchronization Successful - please restart the app to use this data", "Meetup.com Download");

                closableWindow.Close();
            }
            finally
            {
                _syncInProgress = false;
                RaisePropertyChanged(nameof(ButtonText));
            }
        }
    }
}
