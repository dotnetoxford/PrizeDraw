using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrizeDraw.Models.MeetupCom.Event;

namespace PrizeDraw.Helpers
{
    public class MeetupComEventValidator : IEventValidator
    {
        private Event _eventInfo;

        public async Task InitAsync(int eventId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.meetup.com");

                var response = await client.GetAsync($"/dotnetoxford/events/{eventId}");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                _eventInfo = JsonConvert.DeserializeObject<Event>(content);
            }
        }

        public bool IsEventDateToday()
        {
            return UnixTimeStampToDateTime(_eventInfo.time).Date == DateTime.UtcNow.Date;
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTimeStamp);
        }
    }
}