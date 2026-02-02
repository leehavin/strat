using System.Linq.Expressions;
using System.Reflection;

namespace Strat.Shared.Extensions;

/// <summary>
/// 树形结构扩展方法
/// </summary>
public static class TreeExtensions
{
    /// <summary>
    /// 转换为树结构
    /// </summary>
    /// <typeparam name="T">节点类型</typeparam>
    /// <param name="list">平铺列表</param>
    /// <param name="childrenSelector">子节点选择器</param>
    /// <param name="parentIdSelector">父级ID选择器</param>
    /// <param name="rootId">根节点ID</param>
    /// <returns>树形结构列表</returns>
    public static List<T> ToTree<T>(
        this List<T> list,
        Expression<Func<T, List<T>?>> childrenSelector,
        Expression<Func<T, object?>> parentIdSelector,
        object? rootId = null) where T : class
    {
        if (list == null || list.Count == 0)
            return [];

        var childrenProperty = (childrenSelector.Body as MemberExpression)?.Member as PropertyInfo
            ?? throw new ArgumentException("Invalid children selector");

        var parentIdFunc = parentIdSelector.Compile();

        // 找出根节点
        var rootItems = list.Where(x =>
        {
            var parentId = parentIdFunc(x);
            if (parentId == null) return rootId == null;
            return parentId.ToString() == rootId?.ToString();
        }).ToList();

        // 递归设置子节点
        foreach (var item in rootItems)
        {
            SetChildren(item, list, childrenProperty, parentIdFunc);
        }

        return rootItems;
    }

    private static void SetChildren<T>(
        T parent,
        List<T> allItems,
        PropertyInfo childrenProperty,
        Func<T, object?> parentIdFunc) where T : class
    {
        // 获取当前节点ID
        var idProperty = typeof(T).GetProperty("Id");
        if (idProperty == null) return;

        var parentId = idProperty.GetValue(parent);
        if (parentId == null) return;

        // 找出子节点
        var children = allItems.Where(x =>
        {
            var pId = parentIdFunc(x);
            return pId?.ToString() == parentId.ToString();
        }).ToList();

        // 设置子节点
        childrenProperty.SetValue(parent, children);

        // 递归处理子节点
        foreach (var child in children)
        {
            SetChildren(child, allItems, childrenProperty, parentIdFunc);
        }
    }
}

