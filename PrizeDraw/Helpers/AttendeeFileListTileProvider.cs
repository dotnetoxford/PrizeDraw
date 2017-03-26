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
        public List<TileViewModel> GetTiles()
        {
            var rand = new Random();

            return File.ReadAllLines(@"TestData\TestAttendeeData.txt")
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .OrderBy(x => x)
                            .Select((x, n) => new TileViewModel
                            {
                                Name = x,
                                AttendeeId = n,
                                // Bias the randomized colours so it's more purple, and less chance of
                                // a white conflicting with the selected tile. Longer term, this needs
                                // to be made more configurable.
                                Color = new SolidColorBrush(Color.FromRgb(
                                    (byte)rand.Next(0, 200),
                                    (byte)rand.Next(0, 200),
                                    (byte)rand.Next(50, 256)))
                            }).ToList();
        }
    }
}