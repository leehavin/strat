using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Strat.Module.System.Views;

public partial class UserManagementView : UserControl
{
    public UserManagementView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
