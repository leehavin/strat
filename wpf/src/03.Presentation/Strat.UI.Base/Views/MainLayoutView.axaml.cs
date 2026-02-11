using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Strat.UI.Base.Views;

public partial class MainLayoutView : UserControl
{
    public MainLayoutView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
