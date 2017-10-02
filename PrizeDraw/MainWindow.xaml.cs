using PrizeDraw.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;
using PrizeDraw.Helpers;
using System.Windows.Input;
using System.Windows.Media.Animation;
using PrizeDraw.Properties;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WinnerTileTargetWidth = 800;
        private const int WinnerTileTargetHeight = 500;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            var tileProvider = new AttendeeFileListTileProvider();
            var vm = new MainWindowViewModel(tileProvider);

            await vm.InitAsync();

            InitialiseGrid(vm);

            KeyDown += MainWindow_KeyDown;
            vm.OnWinnerSelected += OnWinnerSelected;

            DataContext = vm;

            base.OnInitialized(e);
        }

        private void OnWinnerSelected(object sender, WinnerSelectedEventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() => WinnerSelected(eventArgs.AttendeeId));
        }

        private void WinnerSelected(int attendeeId)
        {
            var selectedTileControl = (from t in TileGrid.Children.OfType<TileUserControl>()
                                       where t.ViewModel.AttendeeId == attendeeId
                                       select t).Single();

            var vm = DataContext as MainWindowViewModel;

            vm?.SaveWinnerDetails(selectedTileControl.ViewModel, false);

            InitializeAndBeginAnimation(selectedTileControl);
        }

        private void InitializeAndBeginAnimation(TileUserControl selectedTileControl)
        {
            var absoluteTilePosition = selectedTileControl.TransformToAncestor(this).Transform(new Point(0, 0));

            var tileViewModel = selectedTileControl.DataContext as TileViewModel;

            var selectedTile = new TileUserControl(tileViewModel)
                               {
                                   Width = selectedTileControl.ActualWidth,
                                   Height = selectedTileControl.ActualHeight
                               };

            Canvas.SetLeft(selectedTile, absoluteTilePosition.X);
            Canvas.SetTop(selectedTile, absoluteTilePosition.Y);

            var targetXPos = ActualWidth * 0.5d - WinnerTileTargetWidth * 0.5d;
            var targetYPos = ActualHeight * 0.5d - WinnerTileTargetHeight * 0.5d;

            var animWidth = new DoubleAnimation
                       {
                           From = selectedTile.Width,
                           To = 800,
                           Duration = new Duration(TimeSpan.FromSeconds(2))
                       };

            var animHeight = new DoubleAnimation
                       {
                           From = selectedTile.Height,
                           To = 500,
                           Duration = new Duration(TimeSpan.FromSeconds(2))
                       };

            var animXPos = new DoubleAnimation
                       {
                           From = absoluteTilePosition.X,
                           To = targetXPos,
                           Duration = new Duration(TimeSpan.FromSeconds(2))
                       };

            var animYPos = new DoubleAnimation
                       {
                           From = absoluteTilePosition.Y,
                           To = targetYPos,
                           Duration = new Duration(TimeSpan.FromSeconds(2))
                       };

            Storyboard.SetTarget(animWidth, selectedTile);
            Storyboard.SetTarget(animHeight, selectedTile);
            Storyboard.SetTarget(animXPos, selectedTile);
            Storyboard.SetTarget(animYPos, selectedTile);
            Storyboard.SetTargetProperty(animWidth, new PropertyPath(WidthProperty));
            Storyboard.SetTargetProperty(animHeight, new PropertyPath(HeightProperty));
            Storyboard.SetTargetProperty(animXPos, new PropertyPath(LeftProperty));
            Storyboard.SetTargetProperty(animYPos, new PropertyPath(TopProperty));

            var storyboard = new Storyboard();
            storyboard.Children.Add(animWidth);
            storyboard.Children.Add(animHeight);
            storyboard.Children.Add(animXPos);
            storyboard.Children.Add(animYPos);

            Canvas.Children.Add(selectedTile);

            storyboard.Begin(this);
        }

        async void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                {
                    var vm = DataContext as MainWindowViewModel;
                    vm?.StartNextMode();
                    break;
                }
                case Key.Escape:
                {
                    var vm = DataContext as MainWindowViewModel;
                    vm?.Restart();

                    while (Canvas.Children.Count > 0)
                    {
                        Canvas.Children.Remove(Canvas.Children[0]);
                    }

                    break;
                }
                case Key.Enter:
                {
                    var vm = DataContext as MainWindowViewModel;
                    vm?.SaveWinnerDetails(vm.SelectedTile, true);
                    break;
                }
                case Key.F5:
                {
                    // Ask user for an event id
                    var vmEventIdDlg = new RequestEventIdDialogViewModel();
                    var dlg = new RequestEventIdDialog {DataContext = vmEventIdDlg};
                    if (dlg.ShowDialog() != true)
                    {
                        return;
                    }

                    var eventValidator = new MeetupComEventValidator();
                    await eventValidator.InitAsync(vmEventIdDlg.EventId);

                    if (!eventValidator.IsEventDateToday())
                    {
                        if (MessageBox.Show("This event isn't for today. Are you sure you have the correct event id?", "Event not today", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            return;
                        }
                    }

                    var sourceTileProvider = new AttendeeMeetupComTileProvider();
                    var targetTileProvider = new AttendeeFileListTileProvider();
                    var dialogServer = new DialogService();
                    var wnd = new MeetupDotComSync {DataContext = new MeetupDotComSyncViewModel(sourceTileProvider, targetTileProvider, dialogServer)};
                    wnd.ShowDialog();
                    break;
                }
            }
        }

        private void InitialiseGrid(MainWindowViewModel vm)
        {
            if (!vm.Tiles.Any())
            {
                return;
            }

            for(var x = 0; x < vm.NumColumns; x++)
            {
                TileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            var numRows = (int)Math.Ceiling((decimal)vm.Tiles.Count / vm.NumColumns);

            for (var y = 0; y < numRows; y++)
            {
                TileGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            var n = 0;

            foreach (var tile in vm.Tiles)
            {
                var ucTile = new TileUserControl(tile);
                ucTile.SetValue(Grid.RowProperty, n / vm.NumColumns);
                ucTile.SetValue(Grid.ColumnProperty, n % vm.NumColumns);

                TileGrid.Children.Insert(0, ucTile);

                n++;
            }
        }
    }
}
