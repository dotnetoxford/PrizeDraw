using PrizeDraw.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PrizeDraw.Helpers
{
    public class TestTileProvider : ITileProvider
    {
        public Task<List<TileViewModel>> GetTilesAsync()
        {
            return Task.FromResult(new List<TileViewModel>
            {
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(255, 0, 0)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(0, 255, 0)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(0, 0, 255)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(0, 255, 255)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(255, 255, 0)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(0, 255, 0)) },
                new TileViewModel { Color = new SolidColorBrush(Color.FromRgb(128, 255, 0)) },
            });
        }

        public Task SaveTilesAsync(List<TileViewModel> tiles)
        {
            throw new System.NotSupportedException();
        }
    }
}