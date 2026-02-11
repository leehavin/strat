using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Ursa.Controls;
using System.Threading.Tasks;
using System;

namespace Strat.Shared.Dialogs
{
    public class StratDialogService : IStratDialogService
    {
        private readonly Prism.Ioc.IContainerProvider _containerProvider;
        private Ursa.Controls.WindowNotificationManager? _notificationManager;

        public StratDialogService(Prism.Ioc.IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
        }

        public async Task ShowMessageAsync(string message, string title = "提示")
        {
            await Ursa.Controls.MessageBox.ShowOverlayAsync(message, title, hostId: "GlobalHost");
        }

        public async Task ShowErrorAsync(string message, string title = "错误")
        {
            await Ursa.Controls.MessageBox.ShowOverlayAsync(message, title, icon: Ursa.Controls.MessageBoxIcon.Error, hostId: "GlobalHost");
        }

        public async Task<bool> ShowConfirmAsync(string message, string title = "确认")
        {
            var result = await Ursa.Controls.MessageBox.ShowOverlayAsync(message, title, button: Ursa.Controls.MessageBoxButton.OKCancel, icon: Ursa.Controls.MessageBoxIcon.Question, hostId: "GlobalHost");
            return result == Ursa.Controls.MessageBoxResult.OK;
        }

        public void ShowToast(string message, Layout.ToastType type = Layout.ToastType.Info)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                var top = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                if (top?.MainWindow == null) return;

                if (_notificationManager == null)
                {
                    _notificationManager = new Ursa.Controls.WindowNotificationManager(top.MainWindow)
                    {
                        Position = NotificationPosition.TopRight,
                        MaxItems = 3
                    };
                }

                Avalonia.Controls.Notifications.NotificationType notificationType = type switch
                {
                    Layout.ToastType.Success => Avalonia.Controls.Notifications.NotificationType.Success,
                    Layout.ToastType.Warning => Avalonia.Controls.Notifications.NotificationType.Warning,
                    Layout.ToastType.Error => Avalonia.Controls.Notifications.NotificationType.Error,
                    _ => Avalonia.Controls.Notifications.NotificationType.Information
                };

                ((Avalonia.Controls.Notifications.IManagedNotificationManager)_notificationManager).Show(
                    new Avalonia.Controls.Notifications.Notification("提示", message, notificationType)
                );
            });
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
                        // 手动关闭 Ursa OverlayDialog
                        // 通过可视树向上查找 OverlayDialogHost 并移除当前视图（或其包装器）
                        var current = view as Control;
                        while (current != null)
                        {
                            var parent = current.Parent as Control;
                            if (parent is Ursa.Controls.OverlayDialogHost host)
                            {
                                host.Children.Remove(current);
                                break;
                            }
                            current = parent;
                        }
                        
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

