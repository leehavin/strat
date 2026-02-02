

using Prism.Mvvm;

namespace Strat.Shared.CommonViewModels
{
    /// <summary>
    /// 菜单模型
    /// </summary>
    public partial class StratMenuItem : BindableBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标名称 (适配 Avalonia 不同的图标库)
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public string[] Permissions { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        public object View { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnable { get; set; }

        public StratMenuItem(string name, object view, string[] permissions, string? icon, bool isEnable = true)
        {
            Name = name;
            Icon = icon;
            View = view;
            Permissions = permissions;
            IsEnable = isEnable;
        }
    }
}

