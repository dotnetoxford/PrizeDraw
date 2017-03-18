using System.Windows.Controls;
using PrizeDraw.ViewModels;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for TileUserControl.xaml
    /// </summary>
    public partial class TileUserControl : UserControl
    {
        public TileUserControl(TileViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();
        }
    }
}
