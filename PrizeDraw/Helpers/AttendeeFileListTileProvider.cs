using PrizeDraw.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PrizeDraw.Helpers
{
    public class AttendeeFileListTileProvider : ITileProvider
    {
        private static string FileName => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PrizeDraw", "Attendees.txt");

        public async Task<List<TileViewModel>> GetTilesAsync()
        {
            await Task.Yield();

            var rand = new Random();

            if (!File.Exists(FileName))
            {
                return new List<TileViewModel>();
            }

            return File.ReadAllLines(FileName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x)
                .Select(x => x.Split('\t'))
                .Select(x => new TileViewModel
                {
                    Name = x[0],
                    AttendeeId = int.Parse(x[1]),
                    ImageUri = null,
                    LocalImageFileName = string.IsNullOrWhiteSpace(x[2]) ? null : x[2],
                    // Bias the randomized colours so it's more purple, and less chance of
                    // a white conflicting with the selected tile. Longer term, this needs
                    // to be made more configurable.
                    Color = new SolidColorBrush(Color.FromRgb(
                        (byte)rand.Next(0, 200),
                        (byte)rand.Next(0, 200),
                        (byte)rand.Next(50, 256)))
                }).ToList();
        }

        public async Task SaveTilesAsync(List<TileViewModel> tiles)
        {
            var fullPath = Path.GetDirectoryName(FileName);
            if (fullPath == null)
            {
                throw new NullReferenceException("Invalid filename");
            }

            var imagePath = Path.Combine(fullPath, "Images");

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            var remoteImages = tiles.Where(t => !string.IsNullOrWhiteSpace(t.ImageUri));

            foreach (var remoteImage in remoteImages)
            {
                using (var client = new HttpClient())
                {
                    var imageBytes = await client.GetByteArrayAsync(remoteImage.ImageUri);
                    var localImagePath = Path.Combine(imagePath, $"{remoteImage.AttendeeId}.jpg");
                    File.WriteAllBytes(localImagePath, imageBytes);
                    remoteImage.LocalImageFileName = localImagePath;
                }
            }

            using (var fs = new FileStream(FileName, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs))
                {
                    foreach (var tile in tiles)
                    {
                        await sw.WriteLineAsync($"{tile.Name}\t{tile.AttendeeId}\t{tile.LocalImageFileName}");
                    }
                }
            }
        }
    }
}