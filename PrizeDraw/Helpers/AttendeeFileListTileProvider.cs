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
                                Color = new SolidColorBrush(Color.FromRgb(
                                    (byte)rand.Next(0, 256),
                                    (byte)rand.Next(0, 256),
                                    (byte)rand.Next(0, 256)))
                            }).ToList();
        }
    }
}