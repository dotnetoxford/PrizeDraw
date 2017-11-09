using PrizeDraw.ViewModels;

namespace PrizeDraw.Helpers
{
    public class MeetupComSyncDialog : IMeetupComSyncDialog
    {
        private readonly MeetupDotComSync _window;

        public MeetupComSyncDialog(MeetupDotComSync window, MeetupDotComSyncViewModel viewModel)
        {
            _window = window;

            _window.DataContext = viewModel;
        }

        public void BeginUpdate(int eventId)
        {
            _window.ShowDialog();
        }
    }
}