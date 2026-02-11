using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Strat.Module.System.Views
{
    public partial class RoleEditDialog : UserControl
    {
        public RoleEditDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}