using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Strat.Module.System.Views;

public partial class FunctionManagementView : UserControl
{
    public FunctionManagementView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
