using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;
using XFContactsSample.Models;
using XFContactsSample.Utils;
using XFContactsSample.Views;
using ZXing;

namespace XFContactsSample.ViewModels
{
    public class NewContactPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Contact Contact { get; set; }

        #region States
        public string ArrowImage { get { return ShowMoreNameInfo ? "arrowUp" : "arrowDown"; } }
        public bool ShowMoreInfo { get; set; }
        public bool ShowMoreNameInfo { get; set; }
        public bool MoreFieldsNotShowing { get; set; }
        #endregion

        #region Properties
        public ImageSource ContactImage { get; set; }
        public List<string> MobileTypes { get { return Constants.MobileTypes; } }
        public List<string> AddressTypes { get { return Constants.AddressTypes; } }
        public List<string> AIMTypes { get { return Constants.AIMTypes; } }
        public List<string> DateTypes { get { return Constants.DateTypes; } }
        public List<string> RelationshipTypes { get { return Constants.RelationshipTypes; } }
        public string SelectedMobileType { get; set; }
        public string SelectedEmailType { get; set; }
        public string SelectedAddressType { get; set; }
        public string SelectedAIMType { get; set; }
        public string SelectedDateType { get; set; }
        public string SelectedRelationShipType { get; set; }
        #endregion

        #region Actions
        public Action<Contact> SaveContact;
        public ICommand ChangeShownInfoCommand { get; set; }
        public ICommand ChangeNameShownInfoCommand { get; set; }
        public ICommand SetContactImageCommand { get; set; }
        public ICommand SaveContactCommand =>
            new Command<Contact>(SaveContact);
        public ICommand ScanContactCommand =>
            new Command(ScanContact);
        #endregion
        
        #region Constants
        private const string TakePhotoAction = "Take photo";
        private const string ChoosePhotoAction = "Choose photo";
        #endregion

        public NewContactPageViewModel()
        {
            SetUp();
        }

        public NewContactPageViewModel(Contact contact)
        {
            Contact = contact;
            SetUp();
        }

        public void SetUp()
        {
            //TODO: Agregar escaneo de imagenes

            if (Contact == null)
            {
                Contact = new Contact();
            }

            if (string.IsNullOrEmpty(Contact.Image))
            {
                ContactImage = ImageSource.FromFile("camera");
            }
            else
            {
                ContactImage = ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(Contact.Image))
                );
            }

            MoreFieldsNotShowing = true;

            ChangeShownInfoCommand = new Command(ShowInfo);

            ChangeNameShownInfoCommand = new Command(ShowNameInfo);

            SetContactImageCommand = new Command(SetContactImage);
        }

        public void ShowInfo()
        {
            ShowMoreInfo = !ShowMoreInfo;
            MoreFieldsNotShowing = false;
        }

        public void ShowNameInfo()
        {
            ShowMoreNameInfo = !ShowMoreNameInfo;
        }

        public async void SetContactImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }

            var selected = await Application.Current.MainPage.DisplayActionSheet("Change photo", "Cancel", null, TakePhotoAction, ChoosePhotoAction);

            MediaFile file = null;

            switch (selected)
            {
                case TakePhotoAction:
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { 
                        SaveToAlbum = true,
                        AllowCropping = true,
                        DefaultCamera = CameraDevice.Front
                    });
                    break;
                case ChoosePhotoAction:
                    file = await CrossMedia.Current.PickPhotoAsync();
                    break;
                default:
                    break;
            }

            if (file == null)
                return;
            
            ContactImage = ImageSource.FromStream(() =>
            {
                Stream stream = file.GetStream();
                var ms = new MemoryStream();
                stream.CopyTo(ms);
                Contact.Image = Convert.ToBase64String(ms.ToArray());
                return stream;
            });
        }

        public async void ScanContact()
        {
            var scannedContactHandler = new Action<Result>(GetScannedContact);
            var page = new ScannerPage(scannedContactHandler);

            await Application.Current.MainPage.Navigation.PushAsync(page, true);
        }

        public void GetScannedContact(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var contact = result.Text.Split('*');
                Contact.FirstName = contact[0];
                Contact.LastName = contact[1];
                Contact.Phone = contact[2];
                await Application.Current.MainPage.Navigation.PopAsync(true);
                SaveContactCommand.Execute(Contact);
            });
        }
    }
}
