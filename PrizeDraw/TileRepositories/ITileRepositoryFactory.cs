namespace PrizeDraw.TileRepositories
{
    public interface ITileRepositoryFactory
    {
        ITileRepository CreateMeetupComTileRepository(string eventId);
        ITileRepository CreateZoomTileRepository(string eventId);
        ITileRepository CreateFileTileRepository();
    }
}