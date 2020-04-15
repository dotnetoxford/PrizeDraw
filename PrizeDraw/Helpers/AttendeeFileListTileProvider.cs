using PrizeDraw.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;
using RandomColorGenerator;

namespace PrizeDraw.Helpers
{
    public class AttendeeFileListTileProvider : ITileProvider
    {
        private static string AppFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PrizeDraw");
        private static string AttendeesFileName => Path.Combine(AppFolder, "Attendees.txt");
        private static string ImageFolder => Path.Combine(AppFolder, "Images");

        public async Task<List<TileViewModel>> GetTilesAsync()
        {
            await Task.Yield();

            if (!File.Exists(AttendeesFileName))
                return new List<TileViewModel>();

            var tiles = File.ReadAllLines(AttendeesFileName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x)
                .Select(x => x.Split('\t'))
                .Select(x => new TileViewModel(
                    name: x[0],
                    attendeeId: x[1],
                    remoteImageUri: null,
                    // Randomize colour value
                    color: new SolidColorBrush(RandomColor.GetColor(ColorScheme.Random, Luminosity.Bright))
                )).ToList();

            foreach (var tile in tiles)
                tile.LoadImage(ImageFolder);

            return tiles;
        }

        public async Task SaveTilesAsync(List<TileViewModel> tiles)
        {
            var fullPath = Path.GetDirectoryName(AttendeesFileName);

            if (fullPath == null)
                throw new NullReferenceException("Invalid filename");

            var imagePath = Path.Combine(fullPath, "Images");

            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);

            var remoteImages = tiles.Where(t => !string.IsNullOrWhiteSpace(t.RemoteImageUri));

            foreach (var remoteImage in remoteImages)
                using (var client = new HttpClient())
                {
                    var localImagePath = Path.Combine(imagePath, $"{remoteImage.AttendeeId}.jpg");
                    var imageBytes = await client.GetByteArrayAsync(remoteImage.RemoteImageUri);
                    File.WriteAllBytes(localImagePath, imageBytes);
                }

            using (var fs = new FileStream(AttendeesFileName, FileMode.Create, FileAccess.Write))
                using (var sw = new StreamWriter(fs))
                    foreach (var tile in tiles)
                        await sw.WriteLineAsync($"{tile.Name}\t{tile.AttendeeId}");
        }
    }
}