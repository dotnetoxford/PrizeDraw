using GalaSoft.MvvmLight;
using PrizeDraw.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace PrizeDraw.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ITileProviderFactory _tileProviderFactory;
        private readonly ISoundEffects _soundEffects;
        private readonly IMeetupComSyncDialog _meetupComSyncDialog;

        public enum ModeEnum
        {
            Idle,
            Shuffling,
            Slowdown,
            WinnerSelected
        }

        private static readonly TimeSpan ShuffleInterval = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// If this value is 1.0, the timer interval will decrease by the number of seconds passed since
        /// the start of the slowdown. If it's 0.5, then it'll slowdown at half the speed, etc.
        /// </summary>
        private static readonly double SlowdownTimeCoefficient = 0.25;

        // When the slowdown interval exceeds this, we have a winner!
        private static readonly TimeSpan MaxSlowdownInterval = TimeSpan.FromMilliseconds(1000);

        public List<TileViewModel> Tiles { get; set; }
        public int NumColumns { get; set; }

        public event EventHandler<WinnerSelectedEventArgs> OnWinnerSelected;

        private ModeEnum _mode = ModeEnum.Idle;
        public ModeEnum Mode
        {
            get { return _mode; }
            set
            {
                Set(nameof(Mode), ref _mode, value);
                RaisePropertyChanged("EnableSelectionTile");
            }
        }

        private TileViewModel _selectedTile;
        public TileViewModel SelectedTile
        {
            get { return _selectedTile; }
            set
            {
                Set(nameof(SelectedTile), ref _selectedTile, value);

                var previousSelectedTile = Tiles.SingleOrDefault(x => x.IsSelected);

                if (previousSelectedTile != null)
                    previousSelectedTile.IsSelected = false;

                SelectedTile.IsSelected = true;
            }
        }

        private readonly Timer _timer;
        private DateTimeOffset _slowdownStartTime;

        public MainWindowViewModel(ITileProviderFactory tileProviderFactory, ISoundEffects soundEffects, IMeetupComSyncDialog meetupComSyncDialog)
        {
            _tileProviderFactory = tileProviderFactory;
            _soundEffects = soundEffects;
            _meetupComSyncDialog = meetupComSyncDialog;

            _timer = new Timer();
        }

        public async Task InitAsync()
        {
            PopulateTiles(await _tileProviderFactory.CreateFileTileProvider().GetTilesAsync());
            _timer.Elapsed += (sender, e) => HandleTimer();
        }

        public void PopulateTiles(List<TileViewModel> tiles)
        {
            var rnd = new Random();

            // Randomly shuffle the list
            Tiles = tiles.OrderBy(item => rnd.Next()).ToList();

            NumColumns = (int)(Math.Sqrt(Tiles.Count) + 0.5);
        }

        public void StartNextMode()
        {
            switch(Mode)
            {
                case ModeEnum.Idle:
                    StartShuffle();
                    break;
                case ModeEnum.Shuffling:
                    StartSlowdown();
                    break;
            }
        }

        public void StartShuffle()
        {
            Mode = ModeEnum.Shuffling;
            _timer.Interval = GetCurrentTimerInterval();
            _timer.Start();
        }

        public void StartSlowdown()
        {
            Mode = ModeEnum.Slowdown;
            _slowdownStartTime = DateTimeOffset.UtcNow;
            _timer.Interval = GetCurrentTimerInterval();
        }

        public void SaveWinnerDetails(TileViewModel winningTileViewModel, bool flagged)
        {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PrizeDraw", "Winners.txt");

            using (var stream = File.AppendText(fileName))
            {
                stream.WriteLineAsync($"Time: {DateTime.UtcNow}");
                stream.WriteLineAsync($"    Name: {winningTileViewModel.Name}");
                stream.WriteLineAsync($"    Id: {winningTileViewModel.AttendeeId}");

                if (flagged)
                    stream.WriteLineAsync("    Flagged as genuine winner");
            }
        }

        private void HandleTimer()
        {
            if (_mode == ModeEnum.Slowdown && _timer.Interval > MaxSlowdownInterval.TotalMilliseconds) // Winning tile!
            {
                _soundEffects.PlayWinnerSound();
                _timer.Stop();
                _mode = ModeEnum.WinnerSelected;
                OnWinnerSelected?.Invoke(this, new WinnerSelectedEventArgs
                                               {
                                                   WinningTile = SelectedTile,
                                                   WinnerName = SelectedTile.Name,
                                                   AttendeeId = SelectedTile.AttendeeId
                                               });
                return;
            }

            var rand = new Random();
            var availableTiles = Tiles.Where(t => t.IsAvailableAndNotSelected).ToList();
            var randomTileIndex = rand.Next(0, availableTiles.Count);

            SelectedTile = availableTiles[randomTileIndex];
            _timer.Interval = GetCurrentTimerInterval();
            _soundEffects.PlayTileChangeSound();
        }

        private double GetCurrentTimerInterval()
        {
            switch(_mode)
            {
                case ModeEnum.Idle: { return -1; }
                case ModeEnum.Shuffling: { return ShuffleInterval.TotalMilliseconds; }
                case ModeEnum.Slowdown:
                    var ms = (DateTime.UtcNow - _slowdownStartTime).TotalMilliseconds * SlowdownTimeCoefficient;
                    return Math.Max(ShuffleInterval.TotalMilliseconds, ms);
            }

            throw new NotSupportedException($"Unhandled mode: {_mode}");
        }

        public void Restart()
        {
            _mode = ModeEnum.Idle;
            _timer.Stop();

            var selectedTile = Tiles.FirstOrDefault(x => x.IsSelected);

            if (selectedTile != null)
                selectedTile.IsSelected = false;
        }

        public void BeginUpdate(string eventId) =>
            _meetupComSyncDialog.BeginUpdate(eventId);
    }
}
