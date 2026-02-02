
namespace Strat.Shared.HttpService
{
    public interface IStratHttpService
    {
        Task DeleteAsync(string url, object data);
        Task<T> DeleteAsync<T>(string url, object data);
        Task GetAsync(string url, object? data = null);
        Task<T> GetAsync<T>(string url, object? data = null);
        Task PostAsync(string url, object data);
        Task<T> PostAsync<T>(string url, object data);
        Task PutAsync(string url, object data);
        Task<T> PutAsync<T>(string url, object data);
    }
}

