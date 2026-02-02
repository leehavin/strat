using Strat.Identity.Application.Contracts.Function.Dtos;
using Strat.Shared.Models;
using Volo.Abp.Application.Services;

namespace Strat.Identity.Application.Contracts.Function;

/// <summary>
/// 功能服务接口
/// </summary>
public interface IFunctionService : IApplicationService
{
    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PagedList<FunctionResponse>> GetPagedListAsync(GetFunctionPagedRequest input);

    /// <summary>
    /// 获取单条记录
    /// </summary>
    Task<FunctionResponse> GetAsync(long id);

    /// <summary>
    /// 获取功能树
    /// </summary>
    Task<List<FunctionResponse>> GetTreeAsync();

    /// <summary>
    /// 添加功能
    /// </summary>
    Task<long> AddAsync(AddFunctionRequest input);

    /// <summary>
    /// 更新功能
    /// </summary>
    Task UpdateAsync(long id, AddFunctionRequest input);

    /// <summary>
    /// 删除功能
    /// </summary>
    Task DeleteAsync(long id);

    /// <summary>
    /// 获取功能绑定的接口
    /// </summary>
    Task<List<BindedInterfaceResponse>> GetInterfacesAsync(long functionId);

    /// <summary>
    /// 分配/取消分配接口
    /// </summary>
    Task AssignInterfaceAsync(AssignInterfaceRequest input);

    /// <summary>
    /// 删除功能接口绑定
    /// </summary>
    Task DeleteFunctionInterfaceAsync(long id);
}

