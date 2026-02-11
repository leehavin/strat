namespace Strat.Shared.Models
{
    /// <summary>
    /// 通知消息项（企业级通知中心）
    /// </summary>
    public class NotificationItem
    {
        /// <summary>
        /// 消息 ID
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; } = string.Empty;
        
        /// <summary>
        /// 消息类型：info, success, warning, error
        /// </summary>
        public string Type { get; set; } = "info";
        
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        
        /// <summary>
        /// 跳转链接（可选）
        /// </summary>
        public string? LinkUrl { get; set; }
        
        /// <summary>
        /// 图标路径（Semi Icons）
        /// </summary>
        public string IconPath { get; set; } = string.Empty;
    }
}
