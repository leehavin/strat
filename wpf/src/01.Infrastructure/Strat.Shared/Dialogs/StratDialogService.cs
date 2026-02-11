using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Ursa.Controls;

namespace Strat.Shared.Dialogs
{
    public class StratDialogService : IStratDialogService
    {
        private readonly Prism.Ioc.IContainerProvider _containerProvider;

        public StratDialogService(Prism.Ioc.IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
        }

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
            var top = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (top?.MainWindow != null)
            {
                var result = await MessageBox.ShowAsync(top.MainWindow, message, title, button: MessageBoxButton.OKCancel, icon: MessageBoxIcon.Question);
                return result == MessageBoxResult.OK;
            }
            return false;
        }

        public void ShowToast(string message, Layout.ToastType type = Layout.ToastType.Info)
        {
            var top = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (top?.MainWindow != null)
            {
                // TODO: 替换为 Ursa.Controls.Notification 或 Toast 组件
                // 暂时使用 MessageBox.ShowAsync (非阻塞，异步运行)
                _ = MessageBox.ShowAsync(top.MainWindow, message, "提示", button: MessageBoxButton.OK, icon: MessageBoxIcon.Information);
            }
        }

        public void ShowDialog(string dialogName, object? parameters, Action<bool, object?> callback)
        {
            // 解析视图
            var view = _containerProvider.Resolve<object>(dialogName) as Control;
            if (view == null)
            {
                ShowToast($"无法解析对话框: {dialogName}", Layout.ToastType.Error);
                return;
            }

            // 获取 ViewModel (如果是 IDialogAware 或类似，可以进行初始化)
            var vm = view.DataContext;
            
            // 如果 ViewModel 有 Initialize 方法 (自定义协议)
            if (vm != null)
            {
                var method = vm.GetType().GetMethod("Initialize");
                if (method != null)
                {
                    // 这里我们假设 parameters 是 ViewModel 需要的对象
                    method.Invoke(vm, new[] { parameters });
                }

                // 订阅关闭事件 (自定义协议)
                var closeEvent = vm.GetType().GetEvent("RequestClose");
                if (closeEvent != null)
                {
                    Action<bool>? handler = null;
                    handler = (result) =>
                    {
                        // 关闭对话框逻辑 (Ursa 尚未提供简单的全局 OverlayDialog.Close(view))
                        // 实际上 Ursa OverlayDialog 更多是 OverlayDialog.Show(view, options)
                        // 我们在这里先实现接口，具体关闭由 ViewModel 触发
                        
                        // 移除事件监听
                        // closeEvent.RemoveEventHandler(vm, handler);
                        
                        callback?.Invoke(result, vm);
                    };
                    closeEvent.AddEventHandler(vm, handler);
                }
            }

            // 显示在 MainWindow 的 OverlayLayer 上
            var top = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            if (top?.MainWindow != null)
            {
                // Ursa.Controls.OverlayDialog.Show(view, host)
                // 注意: 需要在 MainWindow.axaml 中配置 <ursa:OverlayDialogHost />
                Ursa.Controls.OverlayDialog.Show(view, top.MainWindow);
            }
        }
    }
}

