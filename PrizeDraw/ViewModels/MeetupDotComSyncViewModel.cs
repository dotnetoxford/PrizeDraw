using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using PrizeDraw.Helpers;

namespace PrizeDraw.ViewModels
{
    public class MeetupDotComSyncViewModel : ViewModelBase
    {
        private readonly ITileProvider _sourceTileProvider;
        private readonly ITileProvider _targetTileProvider;
        private readonly IDialogService _dialogService;

        public string ButtonText => _syncInProgress ? "Please wait ..." : "Download Attendees from Meetup.com";

        private bool _syncInProgress;

        public MeetupDotComSyncViewModel(ITileProvider sourceTileProvider, ITileProvider targetTileProvider, IDialogService dialogService)
        {
            _sourceTileProvider = sourceTileProvider;
            _targetTileProvider = targetTileProvider;
            _dialogService = dialogService;
        }

        public RelayCommand SyncCommand => new RelayCommand(async () => await SyncAsync(), () => !_syncInProgress);

        private async Task SyncAsync()
        {
            _syncInProgress = true;
            RaisePropertyChanged(nameof(ButtonText));

            try
            {
                var tiles = await _sourceTileProvider.GetTilesAsync();
                await _targetTileProvider.SaveTilesAsync(tiles);

                await _dialogService.ShowMessageBox("Synchronization Successful - please restart the app to use this data", "Meetup.com Download");
            }
            finally
            {
                _syncInProgress = false;
                RaisePropertyChanged(nameof(ButtonText));
            }
        }
    }
}
