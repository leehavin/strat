using System.Threading.Tasks;

namespace Strat.Shared.Dialogs
{
    public interface IStratDialogService
    {
        Task ShowMessageAsync(string message, string title = "提示");
        Task ShowErrorAsync(string message, string title = "错误");
        Task<bool> ShowConfirmAsync(string message, string title = "确认");
    }
}

