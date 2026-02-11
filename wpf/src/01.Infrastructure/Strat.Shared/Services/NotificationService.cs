using Strat.Shared.Assets;
using Strat.Shared.Models;

namespace Strat.Shared.Services
{
    /// <summary>
    /// 通知服务实现（Mock版本，后续可替换为真实API）
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly List<NotificationItem> _notifications = new();
        
        public event EventHandler<int>? UnreadCountChanged;
        
        public NotificationService()
        {
            InitializeMockData();
        }
        
        private void InitializeMockData()
        {
            _notifications.AddRange(new[]
            {
                new NotificationItem
                {
                    Id = 1,
                    Title = "系统更新通知",
                    Content = "系统将于今晚 22:00 进行升级维护，预计持续 2 小时",
                    Type = "info",
                    IsRead = false,
                    CreateTime = DateTime.Now.AddMinutes(-30),
                    IconPath = SemiIcons.Info
                },
                new NotificationItem
                {
                    Id = 2,
                    Title = "新用户注册审批",
                    Content = "张三提交的账号注册申请等待您的审批",
                    Type = "warning",
                    IsRead = false,
                    CreateTime = DateTime.Now.AddHours(-2),
                    IconPath = SemiIcons.Warning,
                    LinkUrl = "/user/审批"
                },
                new NotificationItem
                {
                    Id = 3,
                    Title = "数据备份完成",
                    Content = "数据库定时备份任务已成功完成",
                    Type = "success",
                    IsRead = true,
                    CreateTime = DateTime.Now.AddHours(-5),
                    IconPath = SemiIcons.CheckCircle
                },
                new NotificationItem
                {
                    Id = 4,
                    Title = "异常登录警告",
                    Content = "检测到您的账号在北京有异常登录尝试",
                    Type = "error",
                    IsRead = false,
                    CreateTime = DateTime.Now.AddDays(-1),
                    IconPath = SemiIcons.Error
                },
                new NotificationItem
                {
                    Id = 5,
                    Title = "任务执行提醒",
                    Content = "您创建的批量导入任务已完成，共导入 1250 条数据",
                    Type = "success",
                    IsRead = true,
                    CreateTime = DateTime.Now.AddDays(-2),
                    IconPath = SemiIcons.CheckCircle
                }
            });
        }
        
        public Task<int> GetUnreadCountAsync()
        {
            var count = _notifications.Count(n => !n.IsRead);
            return Task.FromResult(count);
        }
        
        public Task<List<NotificationItem>> GetNotificationsAsync(int pageIndex = 1, int pageSize = 20, bool onlyUnread = false)
        {
            var query = onlyUnread
                ? _notifications.Where(n => !n.IsRead)
                : _notifications;
            
            var result = query
                .OrderByDescending(n => n.CreateTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            return Task.FromResult(result);
        }
        
        public Task MarkAsReadAsync(long notificationId)
        {
            var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                NotifyUnreadCountChanged();
            }
            
            return Task.CompletedTask;
        }
        
        public Task MarkAllAsReadAsync()
        {
            var hasChanges = false;
            foreach (var notification in _notifications.Where(n => !n.IsRead))
            {
                notification.IsRead = true;
                hasChanges = true;
            }
            
            if (hasChanges)
            {
                NotifyUnreadCountChanged();
            }
            
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(long notificationId)
        {
            var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                var wasUnread = !notification.IsRead;
                _notifications.Remove(notification);
                
                if (wasUnread)
                {
                    NotifyUnreadCountChanged();
                }
            }
            
            return Task.CompletedTask;
        }
        
        private void NotifyUnreadCountChanged()
        {
            var count = _notifications.Count(n => !n.IsRead);
            UnreadCountChanged?.Invoke(this, count);
        }
    }
}
