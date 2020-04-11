using System.Windows;
using PrizeDraw.ViewModels;

namespace PrizeDraw
{
    /// <summary>
    /// Interaction logic for RequestEventIdDialog.xaml
    /// </summary>
    public partial class RequestEventIdDialog : Window
    {
        public RequestEventIdDialog(RequestEventIdDialogViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();
        }

        private void OkayButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
