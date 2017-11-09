using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrizeDraw.Helpers
{
    public class MeetupComHelper : IMeetupComHelper
    {
        public async Task<string> GetEventApiPathAsync(HttpClient httpClient, int eventId)
        {
            var eventDetailsResponse = await httpClient.GetAsync($"/2/events?event_id={eventId}");
            eventDetailsResponse.EnsureSuccessStatusCode();
            var eventDetailsResponseContent = await eventDetailsResponse.Content.ReadAsStringAsync();
            var eventDetails = JsonConvert.DeserializeAnonymousType(eventDetailsResponseContent, new
            {
                results = new[] {
                    new {
                        group = new {
                            urlname = "",
                        }
                    }
                }
            });

            return $"/{eventDetails.results[0].group.urlname}/events/{eventId}";
        }
    }
}