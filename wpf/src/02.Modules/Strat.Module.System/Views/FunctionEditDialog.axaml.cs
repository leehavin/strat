using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Strat.Module.System.Views;

public partial class FunctionEditDialog : UserControl
{
    public FunctionEditDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
