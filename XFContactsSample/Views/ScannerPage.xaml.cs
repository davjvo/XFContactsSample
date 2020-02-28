using System;
using Xamarin.Forms.Xaml;
using XFContactsSample.ViewModels;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace XFContactsSample.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerPage : ZXingScannerPage
    {
        public ScannerPage(Action<Result> contactHandler)
        {
			InitializeComponent();
			BindingContext = new ScannerPageViewModel(contactHandler);
		}
		public void Handle_OnScanResult(Result result)
		{
			(BindingContext as ScannerPageViewModel).GetContactCommand.Execute(result);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			IsScanning = false;
		}
	}
}