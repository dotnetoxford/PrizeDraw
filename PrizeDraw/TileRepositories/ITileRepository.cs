using System.Collections.Generic;
using System.Threading.Tasks;
using PrizeDraw.ViewModels;

namespace PrizeDraw.TileRepositories
{
    public interface ITileRepository
    {
        Task<List<TileViewModel>> GetTilesAsync();
        Task SaveTilesAsync(List<TileViewModel> tiles);
    }
}