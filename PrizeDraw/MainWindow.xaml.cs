using PrizeDraw.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;
using PrizeDraw.Helpers;
using System.Windows.Input;
using System.Windows.Media.Animation;

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

        public MainWindow(IEventValidator eventValidator, MainWindowViewModel viewModel)
        {
            _eventValidator = eventValidator;
            _viewModel = viewModel;

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
            Application.Current.Dispatcher.Invoke(() => WinnerSelected(eventArgs.AttendeeId));
        }

        private void WinnerSelected(int attendeeId)
        {
            var selectedTileControl = (from t in TileGrid.Children.OfType<TileUserControl>()
                                       where t.ViewModel.AttendeeId == attendeeId
                                       select t).Single();

            _viewModel.SaveWinnerDetails(selectedTileControl.ViewModel, false);

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
                case Key.Next:
                {
                    _viewModel.StartNextMode();
                    break;
                }
                case Key.Escape:
                case Key.B:
                {                  
                    _viewModel.Restart();

                    while (Canvas.Children.Count > 0)
                    {
                        Canvas.Children.Remove(Canvas.Children[0]);
                    }

                    break;
                }
                case Key.Enter:
                case Key.PageUp:
                {
                    if (_viewModel?.SelectedTile != null)
                    {
                        _viewModel.SaveWinnerDetails(_viewModel.SelectedTile, true);
                    }

                    break;
                }
                case Key.F5:
                {
                    Hide();

                    // Ask user for an event id
                    var dlg = new RequestEventIdDialog();
                    if (dlg.ShowDialog() != true)
                    {
                        return;
                    }

                    await _eventValidator.InitAsync(dlg.EventId);

                    if (!_eventValidator.IsEventDateToday())
                    {
                        if (MessageBox.Show("This event isn't for today. Are you sure you have the correct event id?", "Event not today", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            return;
                        }
                    }

                    Canvas.Children.RemoveRange(0, Canvas.Children.Count);

                    _viewModel.BeginUpdate(dlg.EventId);

                    Close();

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

            for(var x = 0; x < _viewModel.NumColumns; x++)
            {
                TileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            var numRows = (int)Math.Ceiling((decimal)_viewModel.Tiles.Count / _viewModel.NumColumns);

            for (var y = 0; y < numRows; y++)
            {
                TileGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

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
