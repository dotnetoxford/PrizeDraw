using PrizeDraw.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System;

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

            var vm = new MainWindowViewModel();

            InitialiseGrid(vm);

            DataContext = vm;
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
                var rect = new Rectangle();

                rect.SetValue(Grid.RowProperty, n / vm.NumColumns);
                rect.SetValue(Grid.ColumnProperty, n % vm.NumColumns);

                rect.Fill = new SolidColorBrush(tile.Color);

                TileGrid.Children.Add(rect);

                n++;
            }
        }
    }
}
