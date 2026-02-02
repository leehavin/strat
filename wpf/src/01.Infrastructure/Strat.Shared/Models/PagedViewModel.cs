using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Strat.Shared.Models
{
    /// <summary>
    /// UI 绑定专用分页模型（支持 INotifyPropertyChanged）
    /// 基础设施层共享模型
    /// </summary>
    public class PagedViewModel<T> : BindableBase where T : class
    {
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => SetProperty(ref _pageSize, value);
        }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set => SetProperty(ref _pageIndex, value);
        }

        private int _total;
        public int Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        private ObservableCollection<T> _items = new();
        public ObservableCollection<T> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
    }
}
