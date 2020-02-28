using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFContactsSample.ViewModels;

namespace XFContactsSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsListPage : ContentPage
    {
        public ContactsListPage(ContactsListPageViewModel context)
        {
            InitializeComponent();
            BindingContext = context;
        }
    }
}