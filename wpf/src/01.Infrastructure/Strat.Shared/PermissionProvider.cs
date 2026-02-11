using Avalonia;

namespace Strat.Shared
{
    /// <summary>
    /// 权限控制依附属性
    /// </summary>
    public class PermissionProvider : AvaloniaObject
    {
        public static readonly AttachedProperty<string> CodeProperty =
            AvaloniaProperty.RegisterAttached<PermissionProvider, AvaloniaObject, string>("Code");

        public static string GetCode(AvaloniaObject element) => element.GetValue(CodeProperty);
        public static void SetCode(AvaloniaObject element, string value) => element.SetValue(CodeProperty, value);

        static PermissionProvider()
        {
            CodeProperty.Changed.AddClassHandler<AvaloniaObject>((sender, e) => 
            {
                if (sender is Avalonia.Controls.Control control && e.NewValue is string code)
                {
                    if (string.IsNullOrEmpty(code)) return;
                    CheckPermission(control, code);
                }
            });
        }

        public static void CheckPermission(Avalonia.Controls.Control control, string code)
        {
            // 这里我们先做一个占位，实际需要获取容器中的 IPermissionService
            // 在真正的 Prism 应用中，我们可以通过 Application.Current.GetContainer() 获取
            // 这里我们采用一种折中的方式：让 PermissionService 暴露一个静态实例（虽然不那么纯粹，但在 WPF/Avalonia 依附属性中很高效）
        }
    }
}
