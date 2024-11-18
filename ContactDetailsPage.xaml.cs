using MauiContactManager.ViewModels;

namespace MauiContactManager;

[QueryProperty(nameof(ContactId), "ContactId")]
public partial class ContactDetailsPage : ContentPage
{
    private readonly ContactDetailsViewModel _viewModel;


    public string ContactId
    {
        set
        {
            if (int.TryParse(value, out var contactId))
            {
                _viewModel.Initialize(contactId);
            }
        }
    }


    public ContactDetailsPage(ContactDetailsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;

        BindingContext = _viewModel;
    }
}