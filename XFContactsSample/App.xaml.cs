using Xamarin.Forms;
using XFContactsSample.Services;
using XFContactsSample.ViewModels;
using XFContactsSample.Views;
using System.Linq;
using XFContactsSample.Models;
using System.Threading.Tasks;

namespace XFContactsSample
{
    public partial class App : Application
    {
        public static ContactsDatabase database;
        public static ContactsDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ContactsDatabase();
                }
                return database;
            }
        }
        private ContactsListPageViewModel ContactListContext { get; set; }
        public App()
        {
            InitializeComponent();
            ContactListContext = new ContactsListPageViewModel();
            var page = new ContactsListPage(ContactListContext);
            MainPage = new NavigationPage(page);
        }

        protected override async void OnStart()
        {
            await Task.Run(async () =>
            {
                var contacts = await Database.GetItemsAsync();
                var groupedContacts = contacts.GroupBy(d => d.NameSort).Select(d => new Grouping<string, Contact>(d.Key, d));
                foreach (var group in groupedContacts)
                {
                    ContactListContext.Contacts.Add(group);
                }
            });
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
