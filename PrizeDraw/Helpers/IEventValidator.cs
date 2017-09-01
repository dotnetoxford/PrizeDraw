using System.Threading.Tasks;

namespace PrizeDraw.Helpers
{
    public interface IEventValidator
    {
        Task InitAsync(int eventId);
        bool IsEventDateToday();
    }
}