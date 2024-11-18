using MauiContactManager.Interfaces;
using MauiContactManager.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiContactManager.ViewModels
{
    public class ContactFormViewModel
    {
        private readonly IContactDatabase _contactDatabase;
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

        public ContactFormViewModel(IContactDatabase contactDatabase)
        {
            _contactDatabase = contactDatabase;

            Contact = new ContactModel();

            SaveContactCommand = new Command(SaveContact);
            CancelCommand = new Command(Cancel);
            DeleteContactCommand = new Command(DeleteContact);
        }

        private async void SaveContact()
        {
            _contactDatabase.SaveContact(Contact);
            await Shell.Current.GoToAsync("..");
        }

        private async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void DeleteContact()
        {
            if (_isEditMode)
            {
                _contactDatabase.DeleteContact(_contactId);
            }
            await Shell.Current.GoToAsync("..");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
