using System;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;

namespace XFContactsSample.ViewModels
{
    public class ScannerPageViewModel
    {
        public ICommand GetContactCommand;

        public ScannerPageViewModel(Action<Result> resultHandler)
        {
            GetContactCommand = new Command<Result>(resultHandler);
        }
    }
}
