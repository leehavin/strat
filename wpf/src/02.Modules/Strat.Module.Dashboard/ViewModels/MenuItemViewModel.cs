using Strat.Shared.Assets;
using System.Collections.ObjectModel;

namespace Strat.Module.Dashboard.ViewModels
{
    /// <summary>
    /// 菜单项 ViewModel（企业级重构版）
    /// </summary>
    public class MenuItemViewModel : BindableBase
    {
        public long Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Code { get; set; } = string.Empty;
        
        public long ParentId { get; set; }
        
        /// <summary>
        /// Semi Design Icons 资源键路径数据
        /// </summary>
        public string IconPath { get; set; } = SemiIcons.Description;
        
        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }
        
        public ObservableCollection<MenuItemViewModel> Children { get; set; } = new();
        
        public bool HasChildren => Children.Any();
    }
}
