namespace PrizeDraw.Helpers
{
    public class TileProviderFactory : ITileProviderFactory
    {
        private readonly IMeetupComHelper _meetupComHelper;

        public TileProviderFactory(IMeetupComHelper meetupComHelper)
        {
            _meetupComHelper = meetupComHelper;
        }

        public ITileProvider CreateMeetupComTileProvider(int eventId)
        {
            return new AttendeeMeetupComTileProvider(eventId, _meetupComHelper);
        }

        public ITileProvider CreateFileTileProvider()
        {
            return new AttendeeFileListTileProvider();
        }
    }
}