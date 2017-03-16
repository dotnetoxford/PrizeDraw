using PrizeDraw.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace PrizeDraw.Helpers
{
    public class AttendeeFileListTileProvider : ITileProvider
    {
        public List<Tile> GetTiles()
        {
            var rand = new Random();

            return File.ReadAllLines(@"c:\dump\dotnetoxford-attendies.txt")
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .OrderBy(x => x)
                            .Select((x, n) => new Tile
                            {
                                Name = x,
                                Color = Color.FromRgb(
                                    (byte)rand.Next(0, 256),
                                    (byte)rand.Next(0, 256),
                                    (byte)rand.Next(0, 256))
                            }).ToList();
        }
    }
}