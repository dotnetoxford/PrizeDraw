using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace PrizeDraw.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public List<Tile> Tiles { get; set; }
        public int NumColumns { get; set; }

        public MainWindowViewModel()
        {
            Tiles = new List<Tile>
            {
                new Tile { Color = Color.FromRgb(255, 0, 0) },
                new Tile { Color = Color.FromRgb(0, 255, 0) },
                new Tile { Color = Color.FromRgb(0, 0, 255) },
                new Tile { Color = Color.FromRgb(0, 255, 255) },
                new Tile { Color = Color.FromRgb(255, 255, 0) },
                new Tile { Color = Color.FromRgb(0, 255, 0) },
                new Tile { Color = Color.FromRgb(128, 255, 0) },
            };

            NumColumns = (int)(Math.Sqrt(Tiles.Count) + 0.5);
        }
    }
}
