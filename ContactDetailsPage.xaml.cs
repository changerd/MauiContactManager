using MauiContactManager.ViewModels;

namespace MauiContactManager;

public partial class ContactDetailsPage : ContentPage
{
    private readonly ContactDetailsViewModel _viewModel;

    public ContactDetailsPage(ContactDetailsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;

        BindingContext = _viewModel;
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