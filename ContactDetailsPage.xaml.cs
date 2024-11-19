using MauiContactManager.ViewModels;

namespace MauiContactManager;

public partial class ContactDetailsPage : ContentPage
{
    public ContactDetailsPage(ContactDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ContactDetailsViewModel viewModel)
        {
            viewModel.Initialize();
        }
    }
}