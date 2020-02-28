using System;
using System.Collections.Generic;
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
        public ObservableCollection<Grouping<string, Contact>> Contacts { get; set; }
        public ICommand NavigateToNewContact =>
            new Command(GoToNew);
        public ICommand DeleteContactCommand =>
            new Command<Contact>(DeleteContact);
        public ICommand ShowMoreOptionsCommand =>
            new Command<Contact>(ShowMoreOptions);
        private const string Call = "Call";
        private const string Edit = "Edit";

        public ContactsListPageViewModel()
        {
            App.Database.GetItemsAsync().SafeFireAndForget(false);
            Contacts = new ObservableCollection<Grouping<string, Contact>>();
        }
        public async void GoToNew()
        {
            var newContactPageViewModel = new NewContactPageViewModel
            {
                SaveContact = new Action<Contact>(AddContact)
            };

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
                    var contactPageContext = new NewContactPageViewModel(selected)
                    {
                        SaveContact = new Action<Contact>(AddContact)
                    };
                    var editPage = new NewContactPage(contactPageContext);
                    await Application.Current.MainPage.Navigation.PushAsync(editPage);
                    break;
                default:
                    break;
            }
        }
        public async void AddContact(Contact contact)
        {
            //TODO: Actualizar directamente desde la base de datos  
            if (ValidContact(contact) && contact.Id == 0)
            {
                var group = Contacts.FirstOrDefault(d => d.Key == contact.NameSort);

                if (group != null)
                {
                    group.Add(contact);
                }
                else
                {
                    var newGroup = new Grouping<string, Contact>(contact.NameSort, new List<Contact> { contact });
                    Contacts.Add(newGroup);
                }

                await App.Database.SaveItemAsync(contact);
            }

            await Application.Current.MainPage.Navigation.PopAsync(true);
        }
        public async void DeleteContact(Contact selected)
        {
            //TODO: Actualizar directamente desde la base de datos  
            var contact = await App.Database.GetItemAsync(selected.Id);
            await App.Database.DeleteItemAsync(contact);
            Contacts.FirstOrDefault(d => d.Key == selected.NameSort).Remove(selected);
        }
        public bool ValidContact(Contact contact)
        {
            if (!string.IsNullOrEmpty(contact.FirstName) || !string.IsNullOrEmpty(contact.LastName)
            || !string.IsNullOrEmpty(contact.Phone) || !string.IsNullOrEmpty(contact.Email))
            {
                return true;
            }
            return false;
        }
    }
}
