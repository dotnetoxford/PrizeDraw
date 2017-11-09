namespace PrizeDraw.Helpers
{
    public interface ITileProviderFactory
    {
        ITileProvider CreateMeetupComTileProvider(int eventId);
        ITileProvider CreateFileTileProvider();
    }
}