using PrizeDraw.ViewModels;
using System.Collections.Generic;

namespace PrizeDraw.Helpers
{
    public interface ITileProvider
    {
        List<TileViewModel> GetTiles();
    }
}