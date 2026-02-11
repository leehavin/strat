using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Strat.Shared.AutoRegisterAttributes;

namespace Strat.Module.System.Views;

[NavigationView("RoleManagementView")]
public partial class RoleManagementView : UserControl
{
    public RoleManagementView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}