using PrizeDraw.Helpers;

namespace PrizeDraw.TIleProviders.Impl
{
    public class TileProviderFactory : ITileProviderFactory
    {
        private readonly IMeetupComHelper _meetupComHelper;

        public TileProviderFactory(IMeetupComHelper meetupComHelper) =>
            _meetupComHelper = meetupComHelper;

        public ITileProvider CreateMeetupComTileProvider(string eventId) =>
             new AttendeeMeetupComTileProvider(eventId, _meetupComHelper);

        public ITileProvider CreateZoomTileProvider(string eventId) =>
            new AttendeeZoomTileProvider(eventId);

        public ITileProvider CreateFileTileProvider() =>
            new AttendeeFileListTileProvider();
    }
}