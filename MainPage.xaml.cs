using MauiContactManager.Interfaces;
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

        private async void OnContactSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedContact = e.CurrentSelection.FirstOrDefault() as ContactModel;
        if (selectedContact != null)
        {
            // Вызываем команду из ViewModel
            var viewModel = BindingContext as MainViewModel;
            if (viewModel != null)
            {
                await viewModel.OnContactSelected(selectedContact);
            }
        }
    }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is MainViewModel viewModel)
            {
                viewModel.LoadContacts();
            }
        }
    }
}
