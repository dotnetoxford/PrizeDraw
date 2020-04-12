using PrizeDraw.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;
using PrizeDraw.Helpers;
using System.Windows.Input;
using System.Windows.Media.Animation;
using PrizeDraw.Enums;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEventValidator _eventValidator;
        private const int WinnerTileTargetWidth = 800;
        private const int WinnerTileTargetHeight = 500;
        private readonly MainWindowViewModel _viewModel;
        private readonly ITileProviderFactory _tileProviderFactory;

        public MainWindow(IEventValidator eventValidator, MainWindowViewModel viewModel, ITileProviderFactory tileProviderFactory)
        {
            _eventValidator = eventValidator;
            _viewModel = viewModel;
            _tileProviderFactory = tileProviderFactory;

            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            await _viewModel.InitAsync();

            InitialiseGrid();

            KeyDown += MainWindow_KeyDown;
            _viewModel.OnWinnerSelected += OnWinnerSelected;

            base.OnInitialized(e);
        }

        private void OnWinnerSelected(object sender, WinnerSelectedEventArgs eventArgs)
        {
            Application.Current.Dispatcher?.Invoke(() => WinnerSelected(eventArgs.AttendeeId));
        }

        private void WinnerSelected(string attendeeId)
        {
            var selectedTileControl = (from t in TileGrid.Children.OfType<TileUserControl>()
                                       where t.ViewModel.AttendeeId == attendeeId
                                       select t).Single();

            _viewModel.SaveWinnerDetails(selectedTileControl.ViewModel, false);

            InitializeAndBeginAnimation(selectedTileControl);
        }

        /// <summary>
        /// Begins the animation when a tile is selected as a 'winner' of the draw.
        /// This adds a new item to the canvas.
        /// </summary>
        /// <param name="selectedTileControl"></param>
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
                case Key.Next:
                {
                    _viewModel.StartNextMode();
                    break;
                }
                case Key.Escape:
                case Key.B:
                {
                    if (_viewModel?.SelectedTile != null)
                        _viewModel.SelectedTile.IsNoShow = true;

                    _viewModel?.Restart();

                    while (Canvas.Children.Count > 0)
                        Canvas.Children.Remove(Canvas.Children[0]);

                    break;
                }
                case Key.Enter:
                case Key.PageUp:
                {
                    if (_viewModel?.SelectedTile != null)
                    {
                        _viewModel.SelectedTile.IsWinner = true;

                        // Set the winner image
                        _viewModel.SaveWinnerDetails(_viewModel.SelectedTile, true);
                    }

                    _viewModel?.Restart();

                    while (Canvas.Children.Count > 0)
                        Canvas.Children.Remove(Canvas.Children[0]);

                    break;
                }
                case Key.F5:
                {
                    Hide();

                    // Ask user for an event id
                    var requestEventIdViewModel = new RequestEventIdDialogViewModel();
                    if (new RequestEventIdDialog(requestEventIdViewModel).ShowDialog() != true)
                        return;

                    switch (requestEventIdViewModel.EventType)
                    {
                        case EventType.Meetup:
                            await _eventValidator.InitAsync(requestEventIdViewModel.EventId);

                            if (!_eventValidator.IsEventDateToday())
                                if (MessageBox.Show("This event isn't for today. Are you sure you have the correct event id?", "Event not today", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                                    return;

                            // Meetup is an offline update done before the event. This BeginUpdate will pull from Meetup and
                            // write to the file system, then require a relaunch.
                            Canvas.Children.RemoveRange(0, Canvas.Children.Count);
                            _viewModel.BeginUpdate(requestEventIdViewModel.EventId);

                            Close();
                            break;

                        case EventType.Zoom:
                            Canvas.Children.RemoveRange(0, Canvas.Children.Count);

                            _viewModel.PopulateTiles(await _tileProviderFactory.CreateZoomTileProvider(requestEventIdViewModel.EventId)
                                .GetTilesAsync());

                            InitialiseGrid();

                            Show();

                            break;
                    }

                    break;
                }
            }
        }

        private void InitialiseGrid()
        {
            if (!_viewModel.Tiles.Any())
            {
                return;
            }

            TileGrid.ColumnDefinitions.Clear();
            TileGrid.RowDefinitions.Clear();
            TileGrid.Children.Clear();

            for (var x = 0; x < _viewModel.NumColumns; x++)
                TileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var numRows = (int)Math.Ceiling((decimal)_viewModel.Tiles.Count / _viewModel.NumColumns);

            for (var y = 0; y < numRows; y++)
                TileGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var n = 0;

            foreach (var tile in _viewModel.Tiles)
            {
                var ucTile = new TileUserControl(tile);
                ucTile.SetValue(Grid.RowProperty, n / _viewModel.NumColumns);
                ucTile.SetValue(Grid.ColumnProperty, n % _viewModel.NumColumns);

                TileGrid.Children.Insert(0, ucTile);

                n++;
            }
        }

        // Explicit shutdown requires due to the Autofac setup
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
