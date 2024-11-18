using MauiContactManager.Interfaces;
using MauiContactManager.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiContactManager.ViewModels
{
    public class ContactDetailsViewModel
    {
        private readonly IContactDatabase _contactDatabase;

        public event PropertyChangedEventHandler PropertyChanged;

        public ContactModel Contact { get; private set; }
        public ICommand EditContactCommand { get; }
        public ICommand DeleteContactCommand { get; }

        public string FormattedPhoneNumbers => string.Join(", ", Contact?.PhoneNumbers?.Split(',') ?? new string[0]);
        public string FormattedEmails => string.Join(", ", Contact?.Emails?.Split(',') ?? new string[0]);
        public string FormattedBirthDate => Contact.BirthDate.HasValue ? Contact.BirthDate.Value.ToString("MMMM dd, yyyy") : "N/A";

        public ContactDetailsViewModel(IContactDatabase contactDatabase)
        {
            _contactDatabase = contactDatabase;

            EditContactCommand = new Command(EditContact);
            DeleteContactCommand = new Command(DeleteContact);
        }

        public void Initialize(int contactId)
        {
            Contact = _contactDatabase.GetContact(contactId);
            OnPropertyChanged(nameof(Contact));
            OnPropertyChanged(nameof(FormattedPhoneNumbers));
            OnPropertyChanged(nameof(FormattedEmails));
            OnPropertyChanged(nameof(FormattedBirthDate));
        }

        private async void EditContact()
        {
            await Shell.Current.GoToAsync($"ContactFormPage?Mode=Edit&ContactId={Contact.Id}");
        }

        private async void DeleteContact()
        {
            _contactDatabase.DeleteContact(Contact.Id);
            await Shell.Current.GoToAsync("..");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
