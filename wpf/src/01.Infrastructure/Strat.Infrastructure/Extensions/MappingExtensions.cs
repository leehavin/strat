using Mapster;
using Strat.Shared.Models;
using System.Collections.ObjectModel;

namespace Strat.Infrastructure.Extensions
{
    public static class MappingExtensions
    {
        /// <summary>
        /// 将后端 PagedResult 转换为 UI 绑定的 PagedViewModel
        /// </summary>
        public static PagedViewModel<TTarget> ToUIResult<TSource, TTarget>(this PagedResult<TSource> source) 
            where TTarget : class
        {
            return new PagedViewModel<TTarget>
            {
                PageIndex = source.PageIndex,
                PageSize = source.PageSize,
                Total = source.Total,
                Items = new ObservableCollection<TTarget>(source.Items.Adapt<List<TTarget>>())
            };
        }

        /// <summary>
        /// 简化的同类型转换
        /// </summary>
        public static PagedViewModel<T> ToUIResult<T>(this PagedResult<T> source) 
            where T : class
        {
            return source.ToUIResult<T, T>();
        }
    }
}

