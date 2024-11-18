using MauiContactManager.Models;
using MauiContactManager.ViewModels;

namespace MauiContactManager
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }

        private async void OnContactSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is ContactModel selectedContact)
            {
                await Shell.Current.GoToAsync($"ContactDetailsPage?ContactId={selectedContact.Id}");
            }
        }
    }
}
