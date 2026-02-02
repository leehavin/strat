using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Strat.Shared.Models;

namespace Strat.Shared.Services
{
    /// <summary>
    /// 通知服务接口（企业级通知中心）
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        Task<int> GetUnreadCountAsync();
        
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="onlyUnread">只显示未读</param>
        Task<List<NotificationItem>> GetNotificationsAsync(int pageIndex = 1, int pageSize = 20, bool onlyUnread = false);
        
        /// <summary>
        /// 标记消息为已读
        /// </summary>
        Task MarkAsReadAsync(long notificationId);
        
        /// <summary>
        /// 全部标记为已读
        /// </summary>
        Task MarkAllAsReadAsync();
        
        /// <summary>
        /// 删除消息
        /// </summary>
        Task DeleteAsync(long notificationId);
        
        /// <summary>
        /// 通知数量变化事件
        /// </summary>
        event EventHandler<int>? UnreadCountChanged;
    }
}
