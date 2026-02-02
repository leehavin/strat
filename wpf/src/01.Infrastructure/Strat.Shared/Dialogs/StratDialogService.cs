using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Ursa.Controls;

namespace Strat.Shared.Dialogs
{
    public class StratDialogService : IStratDialogService
    {
        public async Task ShowMessageAsync(string message, string title = "提示")
        {
            var top = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (top?.MainWindow != null)
            {
                await MessageBox.ShowAsync(top.MainWindow, message, title);
            }
            else
            {
                await MessageBox.ShowAsync(message, title);
            }
        }

        public async Task ShowErrorAsync(string message, string title = "错误")
        {
            var top = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (top?.MainWindow != null)
            {
                await MessageBox.ShowAsync(top.MainWindow, message, title, button: MessageBoxButton.OK, icon: MessageBoxIcon.Error);
            }
            else
            {
                await MessageBox.ShowAsync(message, title, button: MessageBoxButton.OK, icon: MessageBoxIcon.Error);
            }
        }

        public async Task<bool> ShowConfirmAsync(string message, string title = "确认")
        {
            MessageBoxResult result;
            var top = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (top?.MainWindow != null)
            {
                result = await MessageBox.ShowAsync(top.MainWindow, message, title, button: MessageBoxButton.OKCancel, icon: MessageBoxIcon.Question);
            }
            else
            {
                result = await MessageBox.ShowAsync(message, title, button: MessageBoxButton.OKCancel, icon: MessageBoxIcon.Question);
            }
            return result == MessageBoxResult.OK;
        }
    }
}

