using MauiContactManager.Interfaces;
using MauiContactManager.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiContactManager.ViewModels
{
    public class ContactFormViewModel : IParameterizedViewModel, INotifyPropertyChanged
    {
        private readonly IContactDatabase _contactDatabase;
        private readonly INavigationService _navigationService;
        private bool _isEditMode;
        private int _contactId;

        private string _mode;
        public string Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                _isEditMode = _mode == "Edit";
                OnPropertyChanged(nameof(Mode));

            }
        }

        public int ContactId
        {
            get => _contactId;
            set
            {
                _contactId = value;
                if (_isEditMode && _contactId > 0)
                {
                    Contact = _contactDatabase.GetContact(_contactId);
                    OnPropertyChanged(nameof(Contact));
                }
                OnPropertyChanged(nameof(ContactId));
            }
        }

        public ContactModel Contact { get; set; }
        public ICommand SaveContactCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteContactCommand { get; }

        public string PageTitle => _isEditMode ? "Edit Contact" : "Add Contact";
        public bool IsEditMode => _isEditMode;

        public event PropertyChangedEventHandler PropertyChanged;

        public ContactFormViewModel(IContactDatabase contactDatabase, INavigationService navigationService)
        {
            _contactDatabase = contactDatabase;
            _navigationService = navigationService;

            Contact = new ContactModel();

            SaveContactCommand = new Command(SaveContact);
            CancelCommand = new Command(Cancel);
            DeleteContactCommand = new Command(DeleteContact);
        }

        public void ApplyParameters(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("Mode"))
            {
                Mode = parameters["Mode"] as string;
            }

            if (parameters.ContainsKey("ContactId") && parameters["ContactId"] is int contactId)
            {
                ContactId = contactId;
            }
        }

        private async void SaveContact()
        {
            _contactDatabase.SaveContact(Contact);
            await _navigationService.GoBackAsync();
        }

        private async void Cancel()
        {
            await _navigationService.GoBackAsync();
        }

        private async void DeleteContact()
        {
            if (_isEditMode)
            {
                _contactDatabase.DeleteContact(_contactId);
            }
            await _navigationService.GoBackAsync();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
