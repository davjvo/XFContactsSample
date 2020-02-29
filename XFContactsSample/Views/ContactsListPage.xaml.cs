using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFContactsSample.ViewModels;

namespace XFContactsSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactsListPage : ContentPage
    {
        public ContactsListPageViewModel Context { get; set; }
        public ContactsListPage()
        {
            InitializeComponent();
            BindingContext = Context = new ContactsListPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Context.SetContactsCommand.Execute(null);
        }
    }
}