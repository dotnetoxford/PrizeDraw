using System.Collections.Generic;
using System.Threading.Tasks;
using PrizeDraw.ViewModels;

namespace PrizeDraw.TIleProviders
{
    //(todo) Rename me to ITileRepository, now that we're saving aswell as retrieving
    public interface ITileProvider
    {
        Task<List<TileViewModel>> GetTilesAsync();
        Task SaveTilesAsync(List<TileViewModel> tiles);
    }
}