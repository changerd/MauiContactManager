using MauiContactManager.Interfaces;

namespace MauiContactManager.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToAsync<TPage>(Dictionary<string, object>? parameters = null) where TPage : Page
        {
            var page = _serviceProvider.GetService<TPage>();

            if (page != null)
            {
                if (page.BindingContext is IParameterizedViewModel viewModel && parameters != null)
                {
                    viewModel.ApplyParameters(parameters);
                }

                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(page);
                }
            }
        }

        public async Task GoBackAsync()
        {
            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                await navigationPage.PopAsync();
            }
        }
    }
}
