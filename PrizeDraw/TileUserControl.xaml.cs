using System.Windows.Controls;
using PrizeDraw.ViewModels;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for TileUserControl.xaml
    /// </summary>
    public partial class TileUserControl : UserControl
    {
        public string AttendeeName { get; set; }

        public TileUserControl(TileViewModel viewModel)
        {
            DataContext = viewModel;

            AttendeeName = viewModel.Name;

            InitializeComponent();
        }
    }
}
