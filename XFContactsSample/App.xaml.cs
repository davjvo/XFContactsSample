using Xamarin.Forms;
using XFContactsSample.Services;
using XFContactsSample.Views;

namespace XFContactsSample
{
    public partial class App : Application
    {
        private static ContactsDatabase database;
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
        public App()
        {
            InitializeComponent();
            var page = new ContactsListPage();
            MainPage = new NavigationPage(page);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
