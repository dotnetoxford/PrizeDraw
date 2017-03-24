using GalaSoft.MvvmLight;
using PrizeDraw.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace PrizeDraw.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public enum ModeEnum
        {
            Idle,
            Shuffling,
            Slowdown,
            WinnerSelected
        }

        private static readonly TimeSpan ShuffleInterval = TimeSpan.FromMilliseconds(100);

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
                {
                    previousSelectedTile.IsSelected = false;
                }

                SelectedTile.IsSelected = true;
            }
        }

        private readonly Timer _timer;
        private DateTimeOffset _slowdownStartTime;

        public MainWindowViewModel(ITileProvider tileProvider)
        {
            Tiles = tileProvider.GetTiles();

            // Randomly shuffle the list
            var rnd = new Random();
            Tiles = Tiles.OrderBy(item => rnd.Next()).ToList();

            NumColumns = (int)(Math.Sqrt(Tiles.Count) + 0.5);

            _timer = new Timer();
            _timer.Elapsed += (sender, e) => HandleTimer();
        }

        public void StartNextMode()
        {
            switch(Mode)
            {
                case ModeEnum.Idle:
                {
                    StartShuffle();
                    break;
                }
                case ModeEnum.Shuffling:
                {
                    StartSlowdown();
                    break;
                }
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

        private void HandleTimer()
        {
            if (_mode == ModeEnum.Slowdown && _timer.Interval > MaxSlowdownInterval.TotalMilliseconds) // Winning tile!
            {
                _timer.Stop();
                _mode = ModeEnum.WinnerSelected;
                OnWinnerSelected?.Invoke(this, new WinnerSelectedEventArgs { WinningTile = SelectedTile, WinnerName = SelectedTile.Name });
                return;
            }

            var rand = new Random();

            var randomTileIndex = rand.Next(0, Tiles.Count);

            SelectedTile = Tiles[randomTileIndex];

            _timer.Interval = GetCurrentTimerInterval();
        }

        private double GetCurrentTimerInterval()
        {
            switch(_mode)
            {
                case ModeEnum.Idle: { return -1; }
                case ModeEnum.Shuffling: { return ShuffleInterval.TotalMilliseconds; }
                case ModeEnum.Slowdown:
                {
                    var ms = (DateTime.UtcNow - _slowdownStartTime).TotalMilliseconds;
                    return Math.Max(ShuffleInterval.TotalMilliseconds, ms);
                }
            }

            throw new NotSupportedException($"Unhandled mode: {_mode}");
        }

        public void Restart()
        {
            _mode = ModeEnum.Idle;
            _timer.Stop();

            var selectedTile = Tiles.FirstOrDefault(x => x.IsSelected);

            if (selectedTile != null)
            {
                selectedTile.IsSelected = false;
            }
        }
    }
}
