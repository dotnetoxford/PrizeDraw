using PrizeDraw.ViewModels;
using System.Collections.Generic;
using System.Windows.Media;

namespace PrizeDraw.Helpers
{
    public class TestTileProvider : ITileProvider
    {
        public List<TileViewModel> GetTiles()
        {
            return new List<TileViewModel>
            {
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(255, 0, 0)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(0, 255, 0)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(0, 0, 255)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(0, 255, 255)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(255, 255, 0)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(0, 255, 0)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(128, 255, 0)) },
            };
        }
    }
}