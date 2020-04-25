using System.Linq;
using System.Windows;
using Autofac;
using PrizeDraw.ViewModels;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = Bootstrapper.Init();

            var rootViewModel = container.Resolve<MainWindowViewModel>();
            var mainWindow = container.Resolve<MainWindow>();

            //(todo) Handling of arguments needs tidying up and doing in a proper manner.
            // This was a quick attempt for our first Virtual .NET Oxford. Pushing it
            // for now, as another usergroup wants to use this functionality.
            var indexOfZoomMeetingId = e.Args
                .Select((x, n) => new {x, n})
                .SingleOrDefault(x => x.x == "-zoommeetingid") ?.n + 1;

            if (indexOfZoomMeetingId != null && e.Args.Length > indexOfZoomMeetingId)
                rootViewModel.ForceZoomMeetingId = e.Args[indexOfZoomMeetingId.Value];

            mainWindow.DataContext = rootViewModel;

            mainWindow.Show();
        }
   }
}
