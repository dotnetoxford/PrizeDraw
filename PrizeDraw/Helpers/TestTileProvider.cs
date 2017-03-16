using PrizeDraw.ViewModels;
using System.Collections.Generic;
using System.Windows.Media;

namespace PrizeDraw.Helpers
{
    public class TestTileProvider : ITileProvider
    {
        public List<Tile> GetTiles()
        {
            return new List<Tile>
            {
                new Tile { Color = Color.FromRgb(255, 0, 0) },
                new Tile { Color = Color.FromRgb(0, 255, 0) },
                new Tile { Color = Color.FromRgb(0, 0, 255) },
                new Tile { Color = Color.FromRgb(0, 255, 255) },
                new Tile { Color = Color.FromRgb(255, 255, 0) },
                new Tile { Color = Color.FromRgb(0, 255, 0) },
                new Tile { Color = Color.FromRgb(128, 255, 0) },
            };
        }
    }
}