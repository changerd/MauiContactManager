using MauiContactManager.Interfaces;
using MauiContactManager.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiContactManager.ViewModels
{
    public class ContactDetailsViewModel : IParameterizedViewModel, INotifyPropertyChanged
    {
        private readonly IContactDatabase _contactDatabase;
        private readonly INavigationService _navigationService;
        private int _contactId;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public ICommand EditContactCommand { get; }
        public ICommand DeleteContactCommand { get; }

        private ContactModel _contact;
        public ContactModel Contact
        {
            get => _contact;
            private set
            {
                _contact = value;
                OnPropertyChanged(nameof(Contact));
                OnPropertyChanged(nameof(FormattedPhoneNumbers));
                OnPropertyChanged(nameof(FormattedEmails));
                OnPropertyChanged(nameof(FormattedBirthDate));
            }
        }

        public int ContactId
        {
            get => _contactId;
            set
            {
                _contactId = value;
                Initialize(value);
                OnPropertyChanged(nameof(ContactId));
            }
        }

        public string FormattedPhoneNumbers => string.Join(", ", Contact?.PhoneNumbers?.Split(',') ?? new string[0]);
        public string FormattedEmails => string.Join(", ", Contact?.Emails?.Split(',') ?? new string[0]);
        public string FormattedBirthDate => Contact != null && Contact.BirthDate.HasValue ? Contact.BirthDate.Value.ToString("MMMM dd, yyyy") : "N/A";

        public ContactDetailsViewModel(IContactDatabase contactDatabase, INavigationService navigationService)
        {
            _contactDatabase = contactDatabase;
            _navigationService = navigationService;

            EditContactCommand = new Command(EditContact);
            DeleteContactCommand = new Command(DeleteContact);            
        }

        public void ApplyParameters(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("ContactId") && parameters["ContactId"] is int contactId)
            {
                ContactId = contactId;
            }
        }

        public void Initialize()
        {
            if (ContactId != 0)
            {
                Initialize(ContactId);
            }
        }

        private void Initialize(int contactId)
        {            
            Contact = _contactDatabase.GetContact(contactId);
            
            if (Contact == null)
            {                
                Contact = new ContactModel { Name = "Unknown", PhoneNumbers = "N/A", Emails = "N/A", BirthDate = null };
            }            
        }

        private async void EditContact()
        {
            var parameters = new Dictionary<string, object>
            {
                { "Mode", "Edit" },
                { "ContactId", Contact.Id }
            };

            await _navigationService.NavigateToAsync<ContactFormPage>(parameters);
        }

        private async void DeleteContact()
        {
            _contactDatabase.DeleteContact(Contact.Id);
            await _navigationService.GoBackAsync();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Console.WriteLine($"Property {propertyName} changed");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
