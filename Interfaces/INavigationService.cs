namespace MauiContactManager.Interfaces
{
    public interface INavigationService
    {
        Task NavigateToAsync<TPage>(Dictionary<string, object>? parameters = null) where TPage : Page;
        Task GoBackAsync();
    }
}
