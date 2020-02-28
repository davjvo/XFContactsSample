using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFContactsSample.ViewModels;

namespace XFContactsSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewContactPage : ContentPage
    {
        public NewContactPage()
        {
            InitializeComponent();
            BindingContext = new NewContactPageViewModel();
        }

        public NewContactPage(NewContactPageViewModel Context)
        {
            InitializeComponent();
            BindingContext = Context;
        }
    }
}