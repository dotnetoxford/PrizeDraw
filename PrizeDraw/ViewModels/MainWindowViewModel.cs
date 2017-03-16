using GalaSoft.MvvmLight;
using PrizeDraw.Helpers;
using System;
using System.Collections.Generic;
using System.Timers;

namespace PrizeDraw.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
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

        public MainWindowViewModel(ITileProvider tileProvider)
        {
            Tiles = tileProvider.GetTiles();
            NumColumns = (int)(Math.Sqrt(Tiles.Count) + 0.5);

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => HandleTimer();
            timer.Start();
        }

        private void HandleTimer()
        {
            var rand = new Random();
            var numRows = (int)Math.Ceiling((decimal)Tiles.Count / NumColumns);
            SelectionTileColumnIndex = rand.Next(0, NumColumns);
            SelectionTileRowIndex = rand.Next(0, numRows);
        }
    }
}
