using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace PrizeDraw.ViewModels
{
    public class RequestEventIdDialogViewModel : ViewModelBase
    {
        public int EventId { get; set; }

        //public RelayCommand OkayCommand { get; private set; }

        public RequestEventIdDialogViewModel()
        {
            //OkayCommand = new RelayCommand(
            //    () => { c },
            //    () => true); // This is a bool specifying if the command can be executed
        }
    }
}
