using Avalonia.Controls;
using Avalonia.Input;
using Strat.Shared.AutoRegisterAttributes;
using Strat.Module.Dashboard.ViewModels;

namespace Strat.Module.Dashboard.Views
{
    [NavigationView]
    public partial class Main : UserControl
    {
        public Main()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 点击遮罩层关闭面板
        /// </summary>
        private void OnOverlayPressed(object? sender, PointerPressedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                // 关闭通知面板
                if (viewModel.NotificationPanel.IsOpen)
                {
                    viewModel.NotificationPanel.IsOpen = false;
                }
                
                // 关闭快捷搜索面板
                if (viewModel.QuickSearchPanel.IsOpen)
                {
                    viewModel.QuickSearchPanel.IsOpen = false;
                }
            }
            
            e.Handled = true;
        }
    }
}
