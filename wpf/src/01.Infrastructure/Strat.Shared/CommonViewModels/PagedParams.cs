

using Prism.Mvvm;

namespace Strat.Shared.CommonViewModels
{
    public class PagedParams : BindableBase
    {
        public PagedParams()
        {
            PageSize = 10;
            PageIndex = 1;
        }

        private int _pageSize;

        public int PageSize
        {
            get { return _pageSize; }
            set { SetProperty(ref _pageSize, value); }
        }

        private int _pageIndex;

        public int PageIndex
        {
            get { return _pageIndex; }
            set { SetProperty(ref _pageIndex, value); }
        }
    }
}

