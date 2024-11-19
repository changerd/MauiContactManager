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
        private readonly INavigationService _navigationService;
        private string _searchQuery;
        private bool _isContactsEmpty;

        public ObservableCollection<ContactModel> Contacts { get; set; }
        public ObservableCollection<ContactModel> FilteredContacts { get; set; }

        public ICommand AddContactCommand { get; }
        public ICommand LoadContactsCommand { get; }
        public ICommand ContactSelectedCommand { get; }
        public ICommand GoToSettingsCommand { get; }


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

        public MainViewModel(IContactDatabase contactDatabase, INavigationService navigationService)
        {
            _contactDatabase = contactDatabase;
            _navigationService = navigationService;
            Contacts = new ObservableCollection<ContactModel>(_contactDatabase.GetContacts());
            FilteredContacts = new ObservableCollection<ContactModel>(Contacts);

            AddContactCommand = new Command(async () => await AddContact());
            LoadContactsCommand = new Command(LoadContacts);
            ContactSelectedCommand = new Command<ContactModel>(async (selectedContact) => await OnContactSelected(selectedContact));
            GoToSettingsCommand = new Command(async () => await GoToSettings());
        }

        public void LoadContacts()
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

            FilteredContacts = new ObservableCollection<ContactModel>(FilteredContacts.OrderBy(item => item.Name));

            IsContactsEmpty = FilteredContacts == null || !FilteredContacts.Any();
            OnPropertyChanged(nameof(FilteredContacts));
        }

        private async Task AddContact()
        {
            var parameters = new Dictionary<string, object>
            {
                { "Mode", "Add" }
            };

            await _navigationService.NavigateToAsync<ContactFormPage>(parameters);
        }

        public async Task OnContactSelected(ContactModel selectedContact)
        {
            if (selectedContact != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "ContactId", selectedContact.Id }
                };

                await _navigationService.NavigateToAsync<ContactDetailsPage>(parameters);
            }
        }

        private async Task GoToSettings()
        {
            await _navigationService.NavigateToAsync<SettingsPage>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
