using PrizeDraw.Helpers;

namespace PrizeDraw.TileRepositories.Impl
{
    public class TileRepositoryFactory : ITileRepositoryFactory
    {
        private readonly IMeetupComHelper _meetupComHelper;

        public TileRepositoryFactory(IMeetupComHelper meetupComHelper) =>
            _meetupComHelper = meetupComHelper;

        public ITileRepository CreateMeetupComTileRepository(string eventId) =>
             new AttendeeMeetupComTileRepository(eventId, _meetupComHelper);

        public ITileRepository CreateZoomTileRepository(string eventId) =>
            new AttendeeZoomTileRepository(eventId);

        public ITileRepository CreateFileTileRepository() =>
            new AttendeeFileListTileRepository();
    }
}