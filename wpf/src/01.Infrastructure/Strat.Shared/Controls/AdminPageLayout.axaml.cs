using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Strat.Shared.Controls
{
    public partial class AdminPageLayout : UserControl
    {
        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<AdminPageLayout, string>(nameof(Title));

        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly StyledProperty<object?> FilterContentProperty =
            AvaloniaProperty.Register<AdminPageLayout, object?>(nameof(FilterContent));

        public object? FilterContent
        {
            get => GetValue(FilterContentProperty);
            set => SetValue(FilterContentProperty, value);
        }

        public static readonly StyledProperty<object?> ActionContentProperty =
            AvaloniaProperty.Register<AdminPageLayout, object?>(nameof(ActionContent));

        public object? ActionContent
        {
            get => GetValue(ActionContentProperty);
            set => SetValue(ActionContentProperty, value);
        }

        public static readonly StyledProperty<object?> MainContentProperty =
            AvaloniaProperty.Register<AdminPageLayout, object?>(nameof(MainContent));

        public object? MainContent
        {
            get => GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }

        public static readonly StyledProperty<object?> FooterContentProperty =
            AvaloniaProperty.Register<AdminPageLayout, object?>(nameof(FooterContent));

        public object? FooterContent
        {
            get => GetValue(FooterContentProperty);
            set => SetValue(FooterContentProperty, value);
        }

        public AdminPageLayout()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
