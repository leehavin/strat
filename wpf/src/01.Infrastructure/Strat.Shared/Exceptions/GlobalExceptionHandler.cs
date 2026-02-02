using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Strat.Shared.Dialogs;
using Strat.Shared.Logging;
using Ursa.Controls;

namespace Strat.Shared.Exceptions
{
    /// <summary>
    /// 全局异常处理器（企业级三层防护）
    /// 
    /// 防护层级：
    /// 1. AppDomain.UnhandledException - 捕获所有未处理异常（包括非UI线程）
    /// 2. Dispatcher.UnhandledException - 捕获UI线程异常
    /// 3. TaskScheduler.UnobservedTaskException - 捕获异步任务未观察异常
    /// 
    /// 企业级特性：
    /// - 三层异常网络覆盖所有可能的崩溃点
    /// - 用户友好的错误提示（隐藏技术细节）
    /// - 完整的错误日志记录（便于排查）
    /// - 优雅降级（程序继续运行，而非崩溃）
    /// </summary>
    public static class GlobalExceptionHandler
    {
        private static IStratDialogService? _dialogService;
        private static bool _isInitialized;

        /// <summary>
        /// 初始化全局异常捕获器
        /// </summary>
        /// <param name="dialogService">对话框服务（可选，用于显示错误提示）</param>
        public static void Initialize(IStratDialogService? dialogService = null)
        {
            if (_isInitialized)
            {
                StratLogger.Warning("[GlobalExceptionHandler] 已经初始化，跳过重复初始化");
                return;
            }

            _dialogService = dialogService;

            // 第一层防护：AppDomain 未处理异常（非UI线程）
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;

            // 第二层防护：Avalonia UI 线程异常
            if (Application.Current != null)
            {
                // 注意：Avalonia 11 中需要通过 Dispatcher 订阅
                Avalonia.Threading.Dispatcher.UIThread.UnhandledException += OnDispatcherUnhandledException;
            }

            // 第三层防护：Task 异步异常（未被 await 捕获的异常）
            TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedTaskException;

            _isInitialized = true;
            StratLogger.Information("[GlobalExceptionHandler] 全局异常捕获器初始化成功（三层防护）");
        }

        /// <summary>
        /// 更新对话框服务（延迟注入）
        /// </summary>
        public static void SetDialogService(IStratDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        #region 第一层防护：AppDomain 未处理异常

        private static void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            var isCritical = e.IsTerminating;

            StratLogger.Error(
                exception,
                $"[GlobalExceptionHandler] AppDomain未处理异常（Critical: {isCritical}）"
            );

            // 尝试在 UI 线程显示错误
            _ = ShowErrorDialogAsync(
                title: isCritical ? "严重错误" : "应用程序错误",
                exception: exception,
                isCritical: isCritical
            );
        }

        #endregion

        #region 第二层防护：Dispatcher UI 线程异常

        private static void OnDispatcherUnhandledException(object sender, Avalonia.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            StratLogger.Error(e.Exception, "[GlobalExceptionHandler] UI线程未处理异常");

            // 标记异常已处理（阻止应用崩溃）
            e.Handled = true;

            // 显示用户友好的错误提示
            _ = ShowErrorDialogAsync(
                title: "界面操作错误",
                exception: e.Exception,
                isCritical: false
            );
        }

        #endregion

        #region 第三层防护：TaskScheduler 异步任务异常

        private static void OnTaskSchedulerUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            StratLogger.Error(e.Exception, "[GlobalExceptionHandler] 异步任务未观察异常");

            // 标记异常已观察（阻止程序终止）
            e.SetObserved();

            // 显示错误提示
            _ = ShowErrorDialogAsync(
                title: "后台任务错误",
                exception: e.Exception,
                isCritical: false
            );
        }

        #endregion

        #region 错误提示显示

        /// <summary>
        /// 显示错误对话框（线程安全）
        /// </summary>
        private static async Task ShowErrorDialogAsync(string title, Exception? exception, bool isCritical)
        {
            try
            {
                // 构造用户友好的错误消息
                var userMessage = BuildUserFriendlyMessage(exception, isCritical);

                // 在 UI 线程显示对话框
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    try
                    {
                        if (_dialogService != null)
                        {
                            await _dialogService.ShowErrorAsync(userMessage, title);
                        }
                        else
                        {
                            // 降级方案：直接使用 Ursa MessageBox
                            var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                            if (lifetime?.MainWindow != null)
                            {
                                await MessageBox.ShowAsync(
                                    lifetime.MainWindow,
                                    userMessage,
                                    title,
                                    button: MessageBoxButton.OK,
                                    icon: MessageBoxIcon.Error
                                );
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        // 如果显示对话框也失败，至少记录日志
                        StratLogger.Error(innerEx, "[GlobalExceptionHandler] 显示错误对话框失败");
                    }
                });
            }
            catch (Exception ex)
            {
                // 防御性编程：即使异常处理器也可能出错
                StratLogger.Error(ex, "[GlobalExceptionHandler] ShowErrorDialogAsync 执行失败");
            }
        }

        /// <summary>
        /// 构造用户友好的错误消息
        /// </summary>
        private static string BuildUserFriendlyMessage(Exception? exception, bool isCritical)
        {
            if (exception == null)
            {
                return isCritical
                    ? "应用程序遇到严重错误，可能需要重启。\n详细信息已记录到日志。"
                    : "操作失败，请重试。\n如果问题持续存在，请联系技术支持。";
            }

            // 根据异常类型返回不同的提示
            var friendlyMessage = exception switch
            {
                System.Net.Http.HttpRequestException => "网络连接失败，请检查网络设置后重试。",
                UnauthorizedAccessException => "您没有权限执行此操作。",
                System.IO.FileNotFoundException => "找不到所需的文件，请重新安装应用程序。",
                InvalidOperationException when exception.Message.Contains("XAML") => "界面加载失败，请重启应用程序。",
                TaskCanceledException => "操作已取消。",
                TimeoutException => "操作超时，请检查网络连接后重试。",
                ArgumentException => "操作参数无效，请检查输入后重试。",
                _ => "操作失败，请重试。"
            };

            // 在开发环境显示详细错误
            #if DEBUG
            friendlyMessage += $"\n\n【调试信息】\n{exception.GetType().Name}: {exception.Message}";
            if (exception.InnerException != null)
            {
                friendlyMessage += $"\n内部异常: {exception.InnerException.Message}";
            }
            #endif

            if (isCritical)
            {
                friendlyMessage += "\n\n建议：请保存当前工作并重启应用程序。";
            }

            return friendlyMessage;
        }

        #endregion
    }
}
