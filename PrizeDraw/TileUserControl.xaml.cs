using System.Windows.Controls;
using System.Windows.Media;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for TileUserControl.xaml
    /// </summary>
    public partial class TileUserControl : UserControl
    {
        public string Text { get; private set; }
        public SolidColorBrush BackgroundColor { get; private set; }

        public TileUserControl(string text, Color backgroundColor)
        {
            Text = text;
            BackgroundColor = new SolidColorBrush(backgroundColor);

            InitializeComponent();
        }
    }
}
