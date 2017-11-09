using System.Windows;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for RequestEventIdDialog.xaml
    /// </summary>
    public partial class RequestEventIdDialog : Window
    {
        public int EventId { get; set; }

        public RequestEventIdDialog()
        {
            InitializeComponent();

            tbEventId.SelectionLength = 1;
        }

        private void OkayButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
