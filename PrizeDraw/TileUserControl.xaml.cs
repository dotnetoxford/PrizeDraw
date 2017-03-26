using System.Windows.Controls;
using PrizeDraw.ViewModels;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for TileUserControl.xaml
    /// </summary>
    public partial class TileUserControl : UserControl
    {
        public TileViewModel ViewModel { get; private set; }

        public TileUserControl(TileViewModel viewModel)
        {
            ViewModel = viewModel;

            DataContext = viewModel;

            InitializeComponent();
        }
    }
}
