using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Strat.Module.System.Views;

public partial class OrganizationManagementView : UserControl
{
    public OrganizationManagementView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
