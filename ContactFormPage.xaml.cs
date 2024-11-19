using MauiContactManager.ViewModels;

namespace MauiContactManager;

public partial class ContactFormPage : ContentPage
{
    public ContactFormPage(ContactFormViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}