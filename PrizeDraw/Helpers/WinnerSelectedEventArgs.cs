using PrizeDraw.ViewModels;

namespace PrizeDraw.Helpers
{
    public class WinnerSelectedEventArgs
    {
        public TileViewModel WinningTile { get; set; }
        public string WinnerName { get; set; }
        public string AttendeeId { get; set; }
    }
}