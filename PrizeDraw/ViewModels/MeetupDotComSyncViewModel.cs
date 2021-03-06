﻿using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using PrizeDraw.Helpers;
using PrizeDraw.TileRepositories;

namespace PrizeDraw.ViewModels
{
    public class MeetupDotComSyncViewModel : ViewModelBase
    {
        private readonly ITileRepositoryFactory _tileRepositoryFactory;
        private readonly IDialogService _dialogService;

        public string ButtonText => _syncInProgress ? "Please wait ..." : "Download Attendees from Meetup.com";

        public string EventId { get; set; }

        private bool _syncInProgress;

        public MeetupDotComSyncViewModel(ITileRepositoryFactory tileRepositoryFactory, IDialogService dialogService)
        {
            _tileRepositoryFactory = tileRepositoryFactory;
            _dialogService = dialogService;
        }

        public RelayCommand<ICloseableWindow> SyncCommand => new RelayCommand<ICloseableWindow>(async closableWindow => await SyncAsync(closableWindow), closableWindow => !_syncInProgress);

        private async Task SyncAsync(ICloseableWindow closableWindow)
        {
            _syncInProgress = true;
            RaisePropertyChanged(nameof(ButtonText));

            try
            {
                var sourceTileRepository = _tileRepositoryFactory.CreateMeetupComTileRepository(EventId);
                var targetFileRepository = _tileRepositoryFactory.CreateFileTileRepository();

                var tiles = await sourceTileRepository.GetTilesAsync();
                await targetFileRepository.SaveTilesAsync(tiles);

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
