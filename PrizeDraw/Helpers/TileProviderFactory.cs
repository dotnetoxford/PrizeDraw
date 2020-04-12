namespace PrizeDraw.Helpers
{
    public class TileProviderFactory : ITileProviderFactory
    {
        private readonly IMeetupComHelper _meetupComHelper;

        public TileProviderFactory(IMeetupComHelper meetupComHelper) =>
            _meetupComHelper = meetupComHelper;

        public ITileProvider CreateMeetupComTileProvider(int eventId) =>
             new AttendeeMeetupComTileProvider(eventId, _meetupComHelper);

        public ITileProvider CreateZoomTileProvider(int eventId) =>
            new AttendeeZoomTileProvider(eventId);

        public ITileProvider CreateFileTileProvider() =>
            new AttendeeFileListTileProvider();
    }
}