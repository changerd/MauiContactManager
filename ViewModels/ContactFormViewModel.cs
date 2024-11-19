using System.ComponentModel;
using System.Windows.Input;
using System.Text.RegularExpressions;
using MauiContactManager.Interfaces;
using MauiContactManager.Models;

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

        public string PhoneValidationError { get; set; }
        public string EmailValidationError { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ContactFormViewModel(IContactDatabase contactDatabase, INavigationService navigationService)
        {
            _contactDatabase = contactDatabase;
            _navigationService = navigationService;

            Contact = new ContactModel();

            SaveContactCommand = new Command(SaveContact, CanSaveContact);
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

        private bool CanSaveContact()
        {
            return !HasValidationErrors();
        }

        private bool HasValidationErrors()
        {
            return !string.IsNullOrEmpty(PhoneValidationError) || !string.IsNullOrEmpty(EmailValidationError);
        }

        private async void SaveContact()
        {
            if (!ValidatePhoneNumber(Contact.PhoneNumbers))
            {
                PhoneValidationError = "Phone number must start with '+' and contain 10 to 15 digits.";
                OnPropertyChanged(nameof(PhoneValidationError));
                return;
            }
            else
            {
                PhoneValidationError = null;
                OnPropertyChanged(nameof(PhoneValidationError));
            }

            if (!ValidateEmail(Contact.Emails))
            {
                EmailValidationError = "Email must be in a valid format (e.g., user@example.com).";
                OnPropertyChanged(nameof(EmailValidationError));
                return;
            }
            else
            {
                EmailValidationError = null;
                OnPropertyChanged(nameof(EmailValidationError));
            }

            _contactDatabase.SaveContact(Contact);
            await _navigationService.GoBackAsync();
        }

        private void Cancel()
        {
            _navigationService.GoBackAsync();
        }

        private void DeleteContact()
        {
            if (_isEditMode)
            {
                _contactDatabase.DeleteContact(_contactId);
            }
            _navigationService.GoBackAsync();
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;
            var regex = new Regex(@"^\+([0-9]{10,15})$");
            return regex.IsMatch(phoneNumber);
        }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            var regex = new Regex(@"^[^@]+@[^@]+\.[^@]+$");
            return regex.IsMatch(email);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
