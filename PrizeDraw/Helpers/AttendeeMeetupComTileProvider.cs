using PrizeDraw.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;
using PrizeDraw.Models.MeetupCom.Rsvps;

namespace PrizeDraw.Helpers
{
    public class AttendeeMeetupComTileProvider : ITileProvider
    {
        public async Task<List<TileViewModel>> GetTilesAsync()
        {
            using (var client = new HttpClient())
            {
                //var eventId = _settings.MeetupDotComEventId;
                var eventId = "241049545";

                client.BaseAddress = new Uri("https://api.meetup.com");

                var response = await client.GetAsync($"/dotnetoxford/events/{eventId}/rsvps");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var rsvps = JsonConvert.DeserializeObject<List<Rsvp>>(content);

                var rand = new Random();

                return rsvps.Where(x => x.response == "yes").Select(x => new TileViewModel
                {
                    Name = x.member.name,
                    AttendeeId = x.member.id,
                    ImageUri = x.member.photo?.highres_link ?? x.member.photo?.photo_link,
                    Color = new SolidColorBrush(Color.FromRgb((byte)rand.Next(100, 256),
                                                              (byte)rand.Next(100, 256),
                                                              (byte)rand.Next(100, 256)))
                }).ToList();
            }
        }

        public Task SaveTilesAsync(List<TileViewModel> tiles)
        {
            throw new NotSupportedException();
        }
    }
}