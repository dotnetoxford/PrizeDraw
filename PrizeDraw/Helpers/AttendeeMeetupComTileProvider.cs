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
        private readonly int _eventId;

        public AttendeeMeetupComTileProvider(int eventId)
        {
            _eventId = eventId;
        }

        public async Task<List<TileViewModel>> GetTilesAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.meetup.com");

                var response = await client.GetAsync($"/dotnetoxford/events/{_eventId}/rsvps");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var rsvps = JsonConvert.DeserializeObject<List<Rsvp>>(content);

                var rand = new Random();

                return rsvps.Where(x => x.response == "yes").Select(x => new TileViewModel(
                    name: x.member.name,
                    attendeeId: x.member.id,
                    remoteImageUri: x.member.photo?.highres_link ?? x.member.photo?.photo_link,
                    color: new SolidColorBrush(Color.FromRgb((byte)rand.Next(100, 256),
                                                              (byte)rand.Next(100, 256),
                                                              (byte)rand.Next(100, 256)))
                )).ToList();
            }
        }

        public Task SaveTilesAsync(List<TileViewModel> tiles)
        {
            throw new NotSupportedException();
        }
    }
}