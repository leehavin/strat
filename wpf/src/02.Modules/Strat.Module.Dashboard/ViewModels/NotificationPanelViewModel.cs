using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Strat.Shared.CommonViewModels;
using Strat.Shared.Models;
using Strat.Shared.Services;
using Strat.Shared.Logging;

namespace Strat.Module.Dashboard.ViewModels
{
    /// <summary>
    /// 通知中心面板 ViewModel（企业级实现）
    /// </summary>
    public class NotificationPanelViewModel : StratViewModelBase
    {
        private readonly INotificationService _notificationService;
        
        public NotificationPanelViewModel(
            IEventAggregator eventAggregator,
            INotificationService notificationService) 
            : base(eventAggregator)
        {
            _notificationService = notificationService;
            Notifications = new ObservableCollection<NotificationItem>();
            
            // 订阅未读数量变化事件
            _notificationService.UnreadCountChanged += OnUnreadCountChanged;
            
            // 初始化加载
            _ = LoadNotificationsAsync();
        }
        
        #region Properties
        
        private int _unreadCount;
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnreadCount
        {
            get => _unreadCount;
            set => SetProperty(ref _unreadCount, value);
        }
        
        private bool _isOpen;
        /// <summary>
        /// 面板是否打开
        /// </summary>
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (SetProperty(ref _isOpen, value) && value)
                {
                    // 打开时刷新数据
                    _ = LoadNotificationsAsync();
                }
            }
        }
        
        private bool _onlyUnread;
        /// <summary>
        /// 只显示未读
        /// </summary>
        public bool OnlyUnread
        {
            get => _onlyUnread;
            set
            {
                if (SetProperty(ref _onlyUnread, value))
                {
                    _ = LoadNotificationsAsync();
                }
            }
        }
        
        /// <summary>
        /// 通知列表
        /// </summary>
        public ObservableCollection<NotificationItem> Notifications { get; }
        
        #endregion
        
        #region Commands
        
        private DelegateCommand? _togglePanelCommand;
        /// <summary>
        /// 切换面板显示/隐藏
        /// </summary>
        public DelegateCommand TogglePanelCommand => 
            _togglePanelCommand ??= new DelegateCommand(() => IsOpen = !IsOpen);
        
        private DelegateCommand? _refreshCommand;
        /// <summary>
        /// 刷新通知列表
        /// </summary>
        public DelegateCommand RefreshCommand => 
            _refreshCommand ??= new DelegateCommand(async () => await LoadNotificationsAsync());
        
        private DelegateCommand<NotificationItem>? _markAsReadCommand;
        /// <summary>
        /// 标记为已读
        /// </summary>
        public DelegateCommand<NotificationItem> MarkAsReadCommand => 
            _markAsReadCommand ??= new DelegateCommand<NotificationItem>(
                async item => await ExecuteMarkAsReadAsync(item),
                item => item != null && !item.IsRead)
            .ObservesProperty(() => Notifications);
        
        private DelegateCommand? _markAllAsReadCommand;
        /// <summary>
        /// 全部标记为已读
        /// </summary>
        public DelegateCommand MarkAllAsReadCommand => 
            _markAllAsReadCommand ??= new DelegateCommand(
                async () => await ExecuteMarkAllAsReadAsync(),
                () => Notifications.Any(n => !n.IsRead))
            .ObservesProperty(() => UnreadCount);
        
        private DelegateCommand<NotificationItem>? _deleteCommand;
        /// <summary>
        /// 删除通知
        /// </summary>
        public DelegateCommand<NotificationItem> DeleteCommand => 
            _deleteCommand ??= new DelegateCommand<NotificationItem>(
                async item => await ExecuteDeleteAsync(item),
                item => item != null);
        
        private DelegateCommand<NotificationItem>? _itemClickCommand;
        /// <summary>
        /// 点击通知项
        /// </summary>
        public DelegateCommand<NotificationItem> ItemClickCommand => 
            _itemClickCommand ??= new DelegateCommand<NotificationItem>(
                async item => await ExecuteItemClickAsync(item),
                item => item != null);
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// 加载通知列表
        /// </summary>
        private async Task LoadNotificationsAsync()
        {
            try
            {
                // 更新未读数量
                UnreadCount = await _notificationService.GetUnreadCountAsync();
                
                // 加载通知列表
                var notifications = await _notificationService.GetNotificationsAsync(
                    pageIndex: 1, 
                    pageSize: 20, 
                    onlyUnread: OnlyUnread);
                
                // 更新UI（确保在UI线程）
                await global::Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Notifications.Clear();
                    foreach (var notification in notifications)
                    {
                        Notifications.Add(notification);
                    }
                });
                
                StratLogger.Information($"[NotificationPanel] 加载通知成功，共 {notifications.Count} 条");
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[NotificationPanel] 加载通知失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 标记为已读
        /// </summary>
        private async Task ExecuteMarkAsReadAsync(NotificationItem item)
        {
            try
            {
                await _notificationService.MarkAsReadAsync(item.Id);
                item.IsRead = true;
                MarkAsReadCommand.RaiseCanExecuteChanged();
                MarkAllAsReadCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[NotificationPanel] 标记已读失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 全部标记为已读
        /// </summary>
        private async Task ExecuteMarkAllAsReadAsync()
        {
            try
            {
                await _notificationService.MarkAllAsReadAsync();
                
                // 更新UI
                await global::Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var notification in Notifications)
                    {
                        notification.IsRead = true;
                    }
                });
                
                MarkAllAsReadCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[NotificationPanel] 全部标记已读失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 删除通知
        /// </summary>
        private async Task ExecuteDeleteAsync(NotificationItem item)
        {
            try
            {
                await _notificationService.DeleteAsync(item.Id);
                Notifications.Remove(item);
                MarkAllAsReadCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[NotificationPanel] 删除通知失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 点击通知项
        /// </summary>
        private async Task ExecuteItemClickAsync(NotificationItem item)
        {
            // 如果未读，先标记为已读
            if (!item.IsRead)
            {
                await ExecuteMarkAsReadAsync(item);
            }
            
            // 如果有链接，导航到目标页面
            if (!string.IsNullOrEmpty(item.LinkUrl))
            {
                StratLogger.Information($"[NotificationPanel] 导航到: {item.LinkUrl}");
                // TODO: 实现页面导航逻辑
            }
            
            // 关闭面板
            IsOpen = false;
        }
        
        /// <summary>
        /// 未读数量变化事件处理
        /// </summary>
        private void OnUnreadCountChanged(object? sender, int count)
        {
            global::Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                UnreadCount = count;
            });
        }
        
        #endregion
        
        /// <summary>
        /// 清理资源
        /// </summary>
        ~NotificationPanelViewModel()
        {
            _notificationService.UnreadCountChanged -= OnUnreadCountChanged;
        }
    }
}
