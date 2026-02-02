namespace Strat.Shared.Events
{
    public enum MessageType
    {
        Info,
        Success,
        Warning,
        Error
    }

    public class AppMessagePayload
    {
        public string Title { get; set; } = "提示";
        public string Content { get; set; } = string.Empty;
        public MessageType Type { get; set; } = MessageType.Info;
    }

    /// <summary>
    /// 全局应用消息事件（用于展示弹窗或通知）
    /// </summary>
    public class AppMessageEvent : Prism.Events.PubSubEvent<AppMessagePayload>
    {
    }
}

