using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Strat.Module.System.Views
{
    public partial class UserEditDialog : UserControl
    {
        public UserEditDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}