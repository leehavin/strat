namespace Strat.Shared.Dialogs
{
    public interface IStratDialogService
    {
        Task ShowMessageAsync(string message, string title = "提示");
        Task ShowErrorAsync(string message, string title = "错误");
        Task<bool> ShowConfirmAsync(string message, string title = "确认");
        void ShowToast(string message, Layout.ToastType type = Layout.ToastType.Info);
        void ShowDialog(string dialogName, object? parameters, Action<bool, object?> callback);
    }
}

namespace Strat.Shared.Layout
{
    public enum ToastType
    {
        Info,
        Success,
        Warning,
        Error
    }
}

