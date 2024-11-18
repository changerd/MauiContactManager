using MauiContactManager.ViewModels;

namespace MauiContactManager;

[QueryProperty(nameof(Mode), "Mode")]
[QueryProperty(nameof(ContactId), "ContactId")]
public partial class ContactFormPage : ContentPage
{
    public string Mode
    {
        set
        {
            if (BindingContext is ContactFormViewModel viewModel)
            {
                viewModel.Mode = value;
            }
        }
    }

    public int ContactId
    {
        set
        {
            if (BindingContext is ContactFormViewModel viewModel)
            {
                viewModel.ContactId = value;
            }
        }
    }

    public ContactFormPage(ContactFormViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}