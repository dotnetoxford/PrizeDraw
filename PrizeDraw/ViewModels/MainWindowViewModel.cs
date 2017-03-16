using GalaSoft.MvvmLight;
using PrizeDraw.Helpers;
using System;
using System.Collections.Generic;

namespace PrizeDraw.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public List<Tile> Tiles { get; set; }
        public int NumColumns { get; set; }

        public MainWindowViewModel(ITileProvider tileProvider)
        {
            Tiles = tileProvider.GetTiles();

            NumColumns = (int)(Math.Sqrt(Tiles.Count) + 0.5);
        }
    }
}
