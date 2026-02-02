using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Strat.Identity.Domain.Function;
using Strat.Infrastructure.Persistence;

namespace Strat.System.Application.Interface;

/// <summary>
/// 接口服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class InterfaceService(
    ISqlSugarClient db,
    IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider)
    : ApplicationService, IInterfaceService
{
    private readonly ISqlSugarClient _db = db;
    private readonly IApiDescriptionGroupCollectionProvider _apiDescriptionGroupCollectionProvider = apiDescriptionGroupCollectionProvider;

    /// <summary>
    /// 分页查询
    /// </summary>
    public async Task<PagedList<InterfaceGroupResponse>> GetPagedListAsync(GetInterfacePagedRequest input)
    {
        // 按接口地址搜索
        if (!string.IsNullOrWhiteSpace(input.Path))
        {
            var interfaces = await _db.Queryable<InterfaceEntity>()
                .Where(x => x.Path.Contains(input.Path))
                .ToListAsync();

            var groupIds = interfaces.Select(x => x.GroupId).Distinct().ToList();

            RefAsync<int> total = 0;
            var groups = await _db.Queryable<InterfaceGroupEntity>()
                .Where(x => groupIds.Contains(x.Id))
                .ToPageListAsync(input.PageIndex, input.PageSize, total);

            var result = groups.Adapt<List<InterfaceGroupResponse>>();
            foreach (var item in result)
            {
                item.Interfaces = interfaces.Where(i => i.GroupId == item.Id)
                    .ToList()
                    .Adapt<List<InterfaceResponse>>();
            }

            return result.ToPagedList(total, input.PageIndex, input.PageSize);
        }

        // 按分组名称搜索或全量查询
        RefAsync<int> totalCount = 0;
        var groupList = await _db.Queryable<InterfaceGroupEntity>()
            .WhereIF(!string.IsNullOrWhiteSpace(input.GroupName), x => x.Name.Contains(input.GroupName!))
            .Includes(x => x.Interfaces)
            .ToPageListAsync(input.PageIndex, input.PageSize, totalCount);

        return groupList.Adapt<List<InterfaceGroupResponse>>()
            .ToPagedList(totalCount, input.PageIndex, input.PageSize);
    }

    /// <summary>
    /// 同步API接口
    /// </summary>
    public async Task SyncApiAsync()
    {
        var apiDescriptionGroupsItems = _apiDescriptionGroupCollectionProvider.ApiDescriptionGroups.Items
            .Where(x => x.GroupName != null && !x.GroupName.StartsWith("Abp", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var apiDescriptions = apiDescriptionGroupsItems.SelectMany(x => x.Items).ToList();
        var controllerGroups = apiDescriptions
            .GroupBy(x => ((ControllerActionDescriptor)x.ActionDescriptor).ControllerName)
            .ToList();

        foreach (var controllerGroup in controllerGroups)
        {
            if (!controllerGroup.Any()) continue;

            var controllerName = controllerGroup.Key;
            var groupEntity = await _db.Queryable<InterfaceGroupEntity>()
                .FirstAsync(x => x.Code == controllerName);

            long groupId;
            List<InterfaceEntity> savedInterfaces = [];
            List<InterfaceEntity> newInterfaces = [];

            if (groupEntity == null)
            {
                // 新建分组
                var firstAction = (ControllerActionDescriptor)controllerGroup.First().ActionDescriptor;
                var summary = GetControllerSummary(firstAction.ControllerTypeInfo) ?? controllerName;

                groupId = await _db.Insertable(new InterfaceGroupEntity
                {
                    Name = summary,
                    Code = controllerName
                }).ExecuteReturnSnowflakeIdAsync();
            }
            else
            {
                groupId = groupEntity.Id;
                savedInterfaces = await _db.Queryable<InterfaceEntity>()
                    .Where(x => x.GroupId == groupId)
                    .ToListAsync();
            }

            // 处理接口
            foreach (var apiDescription in controllerGroup)
            {
                var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
                if (actionDescriptor == null) continue;

                // 跳过允许匿名的接口
                var allowAnonymous = actionDescriptor.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();
                if (allowAnonymous != null) continue;

                var path = apiDescription.RelativePath;
                var method = apiDescription.HttpMethod;

                if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(method)) continue;

                // 检查是否已存在
                if (!savedInterfaces.Any(x => x.Path == path && x.RequestMethod == method))
                {
                    var summary = GetMethodSummary(actionDescriptor.MethodInfo) ?? actionDescriptor.ActionName;
                    newInterfaces.Add(new InterfaceEntity
                    {
                        GroupId = groupId,
                        Name = summary,
                        Path = path,
                        RequestMethod = method
                    });
                }
            }

            // 批量插入新接口
            if (newInterfaces.Count > 0)
            {
                await _db.Insertable(newInterfaces).ExecuteReturnSnowflakeIdListAsync();
            }

            // 移除已删除的接口
            var removeInterfaces = savedInterfaces.Where(saved =>
                !controllerGroup.Any(api =>
                    api.RelativePath == saved.Path && api.HttpMethod == saved.RequestMethod))
                .ToList();

            if (removeInterfaces.Count > 0)
            {
                var removeIds = removeInterfaces.Select(x => x.Id).ToList();

                // 先删除功能接口关联
                await _db.Deleteable<FunctionInterfaceEntity>()
                    .Where(x => removeIds.Contains(x.InterfaceId))
                    .ExecuteCommandAsync();

                // 再删除接口
                await _db.Deleteable(removeInterfaces).ExecuteCommandAsync();
            }
        }
    }

    /// <summary>
    /// 获取控制器的 XML 注释摘要
    /// </summary>
    private static string? GetControllerSummary(Type controllerType)
    {
        // 简化实现：返回控制器名称（移除 Service 后缀）
        var name = controllerType.Name;
        if (name.EndsWith("Service"))
            name = name[..^7];
        return name;
    }

    /// <summary>
    /// 获取方法的 XML 注释摘要
    /// </summary>
    private static string? GetMethodSummary(MethodInfo methodInfo)
    {
        // 简化实现：返回方法名称
        return methodInfo.Name;
    }
}

