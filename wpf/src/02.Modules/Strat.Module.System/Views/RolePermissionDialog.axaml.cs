using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Strat.Module.System.Views;

public partial class RolePermissionDialog : UserControl
{
    public RolePermissionDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
