using MauiContactManager.ViewModels;

namespace MauiContactManager;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}