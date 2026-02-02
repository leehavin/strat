

using System.Reflection;
using Prism.Ioc;
using Strat.Shared.AutoRegisterAttributes;

namespace Strat.Shared
{
    public static class AutoRegisterViewExtensions
    {
        public static void AutoRegisterViewForNavigation(this IContainerRegistry containerRegistry)
        {
            var assembly = Assembly.GetCallingAssembly();
            
            // 1. 基于特性的显式注册
            var attributedTypes = assembly.GetExportedTypes()
                .Where(t => t.IsDefined(typeof(NavigationViewAttribute))).ToList();
                
            foreach (var item in attributedTypes)
            {
                var navigationViewAttribute = item.GetCustomAttribute<NavigationViewAttribute>();
                containerRegistry.RegisterForNavigation(item, navigationViewAttribute?.Name ?? item.Name);
            }

            // 2. 基于约定的自动注册 (Views 命名空间下的所有类)
            var viewTypes = assembly.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null && t.Namespace.EndsWith(".Views"))
                .Where(t => !attributedTypes.Contains(t)) // 避免重复注册
                .ToList();

            foreach (var viewType in viewTypes)
            {
                // 查找对应的 ViewModel (约定：ViewName + ViewModel)
                var viewModelName = viewType.Name + "ViewModel";
                var viewModelType = assembly.GetExportedTypes()
                    .FirstOrDefault(t => t.IsClass && !t.IsAbstract && t.Name == viewModelName && 
                                       t.Namespace != null && t.Namespace.EndsWith(".ViewModels"));

                if (viewModelType != null)
                {
                    // 使用反射调用泛型方法 RegisterForNavigation<TView, TViewModel>(name)
                    var method = typeof(IContainerRegistry).GetMethods()
                        .FirstOrDefault(m => m.Name == "RegisterForNavigation" && 
                                           m.IsGenericMethod && 
                                           m.GetGenericArguments().Length == 2 &&
                                           m.GetParameters().Length == 1);
                    
                    if (method != null)
                    {
                        var genericMethod = method.MakeGenericMethod(viewType, viewModelType);
                        genericMethod.Invoke(containerRegistry, new object[] { viewType.Name });
                    }
                }
                else
                {
                    containerRegistry.RegisterForNavigation(viewType, viewType.Name);
                }
            }
        }
    }
}


