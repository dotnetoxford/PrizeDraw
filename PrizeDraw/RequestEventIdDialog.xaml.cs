using System.Windows;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for RequestEventIdDialog.xaml
    /// </summary>
    public partial class RequestEventIdDialog : Window
    {
        public RequestEventIdDialog()
        {
            InitializeComponent();
        }

        private void OkayButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
