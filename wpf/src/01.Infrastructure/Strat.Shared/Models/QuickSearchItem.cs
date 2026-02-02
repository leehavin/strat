using System;

namespace Strat.Shared.Models
{
    /// <summary>
    /// 快捷搜索项（企业级命令面板）
    /// </summary>
    public class QuickSearchItem
    {
        /// <summary>
        /// 项目 ID
        /// </summary>
        public string Id { get; set; } = string.Empty;
        
        /// <summary>
        /// 显示标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// 描述信息
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// 项目类型：page, action, user, file
        /// </summary>
        public QuickSearchType Type { get; set; } = QuickSearchType.Page;
        
        /// <summary>
        /// 图标路径（Semi Icons）
        /// </summary>
        public string IconPath { get; set; } = string.Empty;
        
        /// <summary>
        /// 关键词（用于搜索匹配）
        /// </summary>
        public string[] Keywords { get; set; } = Array.Empty<string>();
        
        /// <summary>
        /// 目标路由（用于页面跳转）
        /// </summary>
        public string? TargetRoute { get; set; }
        
        /// <summary>
        /// 执行动作（用于命令执行）
        /// </summary>
        public Action? Action { get; set; }
        
        /// <summary>
        /// 优先级（0-10，越大越优先）
        /// </summary>
        public int Priority { get; set; } = 5;
    }
    
    /// <summary>
    /// 快捷搜索项类型
    /// </summary>
    public enum QuickSearchType
    {
        /// <summary>
        /// 页面导航
        /// </summary>
        Page,
        
        /// <summary>
        /// 功能操作
        /// </summary>
        Action,
        
        /// <summary>
        /// 用户
        /// </summary>
        User,
        
        /// <summary>
        /// 文件
        /// </summary>
        File,
        
        /// <summary>
        /// 设置
        /// </summary>
        Setting
    }
}
