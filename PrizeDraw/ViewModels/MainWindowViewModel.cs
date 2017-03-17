using GalaSoft.MvvmLight;
using PrizeDraw.Helpers;
using System;
using System.Collections.Generic;
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
        }

        private static readonly TimeSpan ShuffleInterval = TimeSpan.FromMilliseconds(100);

        public List<Tile> Tiles { get; set; }
        public int NumColumns { get; set; }

        private int _selectionTileRowIndex = 0;
        public int SelectionTileRowIndex
        {
            get { return _selectionTileRowIndex; }
            set { Set(nameof(SelectionTileRowIndex), ref _selectionTileRowIndex, value); }
        }

        private int _selectionTileColumnIndex = 0;
        public int SelectionTileColumnIndex
        {
            get { return _selectionTileColumnIndex; }
            set { Set(nameof(SelectionTileColumnIndex), ref _selectionTileColumnIndex, value); }
        }

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

        public bool EnableSelectionTile => Mode != ModeEnum.Idle;

        private Timer _timer;
        private DateTimeOffset _slowdownStartTime;

        public MainWindowViewModel(ITileProvider tileProvider)
        {
            Tiles = tileProvider.GetTiles();
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
            var rand = new Random();
            var numRows = (int)Math.Ceiling((decimal)Tiles.Count / NumColumns);
            SelectionTileColumnIndex = rand.Next(0, NumColumns);
            SelectionTileRowIndex = rand.Next(0, numRows);

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
    }
}
