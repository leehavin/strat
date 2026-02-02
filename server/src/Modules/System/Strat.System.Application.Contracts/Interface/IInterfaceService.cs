using Strat.Shared.Models;
using Strat.System.Application.Contracts.Interface.Dtos;
using Volo.Abp.Application.Services;

namespace Strat.System.Application.Contracts.Interface;

/// <summary>
/// 接口服务接口
/// </summary>
public interface IInterfaceService : IApplicationService
{
    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PagedList<InterfaceGroupResponse>> GetPagedListAsync(GetInterfacePagedRequest input);

    /// <summary>
    /// 同步API接口
    /// </summary>
    Task SyncApiAsync();
}

