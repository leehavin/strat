using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Strat.Shared.Events;
using Strat.Shared.Logging;
using System;
using System.Threading.Tasks;

namespace Strat.Shared.CommonViewModels
{
    /// <summary>
    /// 企业级基础 ViewModel
    /// </summary>
    public abstract class StratViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected IEventAggregator EventAggregator { get; }

        private bool _isBusy;
        /// <summary>
        /// 全局加载状态映射到本地
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected StratViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            
            // 订阅全局 Loading 事件
            EventAggregator.GetEvent<LoadingEvent>().Subscribe(status => IsBusy = status);
        }

        /// <summary>
        /// 安全执行异步操作（自动处理异常和Loading状态）
        /// </summary>
        protected async Task ExecuteAsync(Func<Task> operation)
        {
            try
            {
                IsBusy = true;
                await operation();
            }
            catch (Exception ex)
            {
                StratLogger.Error($"[{GetType().Name}] 操作失败: {ex.Message}");
                // 异常会被全局拦截器处理，这里只记录日志
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        #region 生命周期方法

        /// <summary>
        /// 视图加载时调用 (Avalonia Loaded)
        /// </summary>
        public virtual void OnLoaded() { }

        /// <summary>
        /// 视图卸载时调用 (Avalonia Unloaded)
        /// </summary>
        public virtual void OnUnloaded() { }

        public virtual void OnNavigatedTo(NavigationContext navigationContext) { }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }

        public virtual void Destroy()
        {
            // 默认取消所有事件订阅
            EventAggregator.GetEvent<LoadingEvent>().Unsubscribe(null);
        }

        #endregion
    }
}

