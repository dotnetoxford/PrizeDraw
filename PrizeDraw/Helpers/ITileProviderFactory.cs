using PrizeDraw.Enums;

namespace PrizeDraw.Helpers
{
    public interface ITileProviderFactory
    {
        ITileProvider CreateMeetupComTileProvider(int eventId);
        ITileProvider CreateZoomTileProvider(int eventId);
        ITileProvider CreateFileTileProvider();
    }
}