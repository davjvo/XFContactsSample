using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFContactsSample.Models;
using XFContactsSample.Utils;
using XFContactsSample.Views;

namespace XFContactsSample.ViewModels
{
    public class ContactsListPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsRefreshing { get; set; }
        public ObservableCollection<Grouping<string, Contact>> Contacts { get; private set; }
        public ICommand NavigateToNewContact =>
            new Command(GoToNew);
        public ICommand DeleteContactCommand =>
            new Command<Contact>(DeleteContact);
        public ICommand ShowMoreOptionsCommand =>
            new Command<Contact>(ShowMoreOptions);
        public ICommand SetContactsCommand => 
            new Command(async () =>
            {
                IsRefreshing = true;
                var databaseData = await App.Database.GetItemsAsync();
                var groupedData = databaseData
                                    .GroupBy(d => d.NameSort)
                                    .Select(d => new Grouping<string, Contact>(d.Key, d))
                                    .OrderBy(d => d.Key)
                                    .ToList();
                Contacts = new ObservableCollection<Grouping<string, Contact>>(groupedData);
                IsRefreshing = false;
            });
        private const string Call = "Call";
        private const string Edit = "Edit";

        public ContactsListPageViewModel()
        {
            App.Database.GetItemsAsync().SafeFireAndForget(false);
        }
        public async void GoToNew()
        {
            var newContactPageViewModel = new NewContactPageViewModel();

            var newContactPage = new NewContactPage(newContactPageViewModel);
            await Application.Current.MainPage.Navigation.PushAsync(newContactPage);
        }
        public async void ShowMoreOptions(Contact selected)
        {
            var selectedACtion = await Application.Current.MainPage.DisplayActionSheet("Options", "Cancel", null, Call, Edit);

            switch (selectedACtion)
            {
                case Call:
                    if (string.IsNullOrEmpty(selected.Phone))
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Contact has no phone number", "Ok");
                        return;
                    }
                    PhoneDialer.Open(selected.Phone);
                    break;
                case Edit:
                    var contactPageContext = new NewContactPageViewModel(selected);
                    var editPage = new NewContactPage(contactPageContext);
                    await Application.Current.MainPage.Navigation.PushAsync(editPage);
                    break;
                default:
                    break;
            }
        }
        public async void DeleteContact(Contact selected)
        {
            await App.Database.DeleteItemAsync(selected);
            SetContactsCommand.Execute(null);
        }
    }
}
