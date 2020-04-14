using System.Threading.Tasks;

namespace PrizeDraw.Helpers
{
    public interface IEventValidator
    {
        Task InitAsync(string eventId);
        bool IsEventDateToday();
    }
}