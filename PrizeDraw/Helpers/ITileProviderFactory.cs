using PrizeDraw.Enums;

namespace PrizeDraw.Helpers
{
    public interface ITileProviderFactory
    {
        ITileProvider CreateMeetupComTileProvider(string eventId);
        ITileProvider CreateZoomTileProvider(string eventId);
        ITileProvider CreateFileTileProvider();
    }
}