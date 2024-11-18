using MauiContactManager.Interfaces;
using MauiContactManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiContactManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IContactDatabase _contactDatabase;
        private string _searchQuery;
        private bool _isContactsEmpty;

        public ObservableCollection<ContactModel> Contacts { get; set; }
        public ObservableCollection<ContactModel> FilteredContacts { get; set; }

        public ICommand AddContactCommand { get; }
        public ICommand LoadContactsCommand { get; }
        public ICommand ContactSelectedCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                FilterContacts();
            }
        }

        public bool IsContactsEmpty
        {
            get => _isContactsEmpty;
            set
            {
                _isContactsEmpty = value;
                OnPropertyChanged(nameof(IsContactsEmpty));
            }
        }

        public MainViewModel(IContactDatabase contactDatabase)
        {
            _contactDatabase = contactDatabase;
            Contacts = new ObservableCollection<ContactModel>(_contactDatabase.GetContacts());
            FilteredContacts = new ObservableCollection<ContactModel>(Contacts);

            AddContactCommand = new Command(AddContact);
            LoadContactsCommand = new Command(LoadContacts);
            ContactSelectedCommand = new Command<ContactModel>(OnContactSelected);
        }

        private void LoadContacts()
        {
            Contacts.Clear();
            foreach (var contact in _contactDatabase.GetContacts())
            {
                Contacts.Add(contact);
            }
            FilterContacts();
        }

        private void FilterContacts()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredContacts = new ObservableCollection<ContactModel>(Contacts);
            }
            else
            {
                FilteredContacts = new ObservableCollection<ContactModel>(
                    Contacts.Where(c => c.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                );
            }

            IsContactsEmpty = FilteredContacts == null || !FilteredContacts.Any();
            OnPropertyChanged(nameof(FilteredContacts));
        }

        private async void AddContact()
        {
            await Shell.Current.GoToAsync($"///{nameof(ContactFormPage)}?Mode=Add");
        }

        private async void OnContactSelected(ContactModel selectedContact)
        {
            if (selectedContact != null)
            {
                await Shell.Current.GoToAsync($"ContactDetailsPage?ContactId={selectedContact.Id}");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
