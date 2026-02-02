using Prism.Mvvm;

namespace Strat.Shared.CommonViewModels
{
    /// <summary>
    /// 面包屑导航项
    /// </summary>
    public class BreadcrumbItem : BindableBase
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _route = string.Empty;
        public string Route
        {
            get => _route;
            set => SetProperty(ref _route, value);
        }

        private bool _isLast;
        public bool IsLast
        {
            get => _isLast;
            set => SetProperty(ref _isLast, value);
        }
    }
}
