using System.Net.Http;
using System.Threading.Tasks;

namespace PrizeDraw.Helpers
{
    public interface IMeetupComHelper
    {
        Task<string> GetEventApiPathAsync(HttpClient httpClient, int eventId);
    }
}