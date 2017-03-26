using PrizeDraw.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;
using PrizeDraw.Properties;

namespace PrizeDraw.Helpers
{
    public class AttendeeMeetupComTileProvider : ITileProvider
    {
        private readonly Settings _settings;

        public AttendeeMeetupComTileProvider(Settings settings)
        {
            _settings = settings;
        }

        public async Task<List<TileViewModel>> GetTilesAsync()
        {
            using (var client = new HttpClient())
            {
                var eventId = _settings.MeetupDotComEventId;

                client.BaseAddress = new Uri("https://api.meetup.com");

                var response = await client.GetAsync($"/dotnetoxford/events/{eventId}/rsvps");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var rsvps = JsonConvert.DeserializeObject<List<Class1>>(content);

                var rand = new Random();

                return rsvps.Select(x => new TileViewModel
                {
                    Name = x.member.name,
                    AttendeeId = x.member.id,
                    ImageUri = x.member.photo?.highres_link ?? x.member.photo?.photo_link,
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

        public Task SaveTilesAsync(List<TileViewModel> tiles)
        {
            throw new NotSupportedException();
        }
    }

    //(todo) Tidy up these classes (as they were just pasted from json)
    public class Class1
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string response { get; set; }
        public int guests { get; set; }
        public Event _event { get; set; }
        public Group group { get; set; }
        public Member member { get; set; }
        public Venue venue { get; set; }
    }

    public class Event
    {
        public string id { get; set; }
        public string name { get; set; }
        public int yes_rsvp_count { get; set; }
        public long time { get; set; }
        public int utc_offset { get; set; }
    }

    public class Group
    {
        public int id { get; set; }
        public string urlname { get; set; }
        public string name { get; set; }
        public string who { get; set; }
        public int members { get; set; }
        public string join_mode { get; set; }
        public Group_Photo group_photo { get; set; }
    }

    public class Group_Photo
    {
        public int id { get; set; }
        public string highres_link { get; set; }
        public string photo_link { get; set; }
        public string thumb_link { get; set; }
        public string type { get; set; }
        public string base_url { get; set; }
    }

    public class Member
    {
        public int id { get; set; }
        public string name { get; set; }
        public Photo photo { get; set; }
        public Event_Context event_context { get; set; }
        public string role { get; set; }
        public string bio { get; set; }
    }

    public class Photo
    {
        public int id { get; set; }
        public string highres_link { get; set; }
        public string photo_link { get; set; }
        public string thumb_link { get; set; }
        public string type { get; set; }
        public string base_url { get; set; }
    }

    public class Event_Context
    {
        public bool host { get; set; }
    }

    public class Venue
    {
        public int id { get; set; }
        public string name { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public bool repinned { get; set; }
        public string address_1 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string localized_country_name { get; set; }
    }


}