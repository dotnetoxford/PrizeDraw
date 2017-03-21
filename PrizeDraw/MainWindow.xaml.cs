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
        private const int WinnerTileTargetWidth = 800;
        private const int WinnerTileTargetHeight = 500;

        public MainWindow()
        {
            InitializeComponent();

            var tileProvider = new AttendeeFileListTileProvider();
            var vm = new MainWindowViewModel(tileProvider);

            InitialiseGrid(vm);

            KeyDown += MainWindow_KeyDown;
            vm.OnWinnerSelected += OnWinnerSelected;

            DataContext = vm;
        }

        private void OnWinnerSelected(object sender, WinnerSelectedEventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() => WinnerSelected(eventArgs.WinnerName));
        }

        private void WinnerSelected(string winnerName)
        {
            var selectedTileControl = (from t in TileGrid.Children.OfType<TileUserControl>()
                                       where t.AttendeeName == winnerName
                                       select t).Single();

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

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                var vm = DataContext as MainWindowViewModel;

                vm?.StartNextMode();
            }
        }

        private void InitialiseGrid(MainWindowViewModel vm)
        {
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
