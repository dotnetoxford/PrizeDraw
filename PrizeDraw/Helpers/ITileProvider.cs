using PrizeDraw.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrizeDraw.Helpers
{
    //(todo) Rename me to ITileRepository, now that we're saving aswell as retrieving
    public interface ITileProvider
    {
        Task<List<TileViewModel>> GetTilesAsync();
        Task SaveTilesAsync(List<TileViewModel> tiles);
    }
}