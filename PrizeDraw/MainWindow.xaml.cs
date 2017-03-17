using PrizeDraw.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System;
using PrizeDraw.Helpers;
using System.Windows.Input;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var tileProvider = new AttendeeFileListTileProvider();
            var vm = new MainWindowViewModel(tileProvider);

            InitialiseGrid(vm);

            KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            DataContext = vm;
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                var vm = DataContext as MainWindowViewModel;

                if(vm != null)
                {
                    vm.StartNextMode();
                }
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
                var ucTile = new TileUserControl(tile.Name, tile.Color);
                ucTile.SetValue(Grid.RowProperty, n / vm.NumColumns);
                ucTile.SetValue(Grid.ColumnProperty, n % vm.NumColumns);

                TileGrid.Children.Insert(0, ucTile);

                n++;
            }
        }
    }
}
