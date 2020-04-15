namespace PrizeDraw.TIleProviders
{
    public interface ITileProviderFactory
    {
        ITileProvider CreateMeetupComTileProvider(string eventId);
        ITileProvider CreateZoomTileProvider(string eventId);
        ITileProvider CreateFileTileProvider();
    }
}