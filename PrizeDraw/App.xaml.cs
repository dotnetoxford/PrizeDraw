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

            mainWindow.DataContext = rootViewModel;

            mainWindow.Show();
        }
   }
}
