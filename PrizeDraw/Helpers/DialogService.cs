using System;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Views;

namespace PrizeDraw.Helpers
{
    class DialogService : IDialogService
    {
        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public async Task ShowMessage(string message, string title)
        {
            await Task.Yield();
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }

        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        public async Task ShowMessageBox(string message, string title)
        {
            await Task.Yield();
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }
    }
}
