using PrizeDraw.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrizeDraw.Helpers
{
    public interface ITileProvider
    {
        Task<List<TileViewModel>> GetTilesAsync();
    }
}