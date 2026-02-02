using Strat.System.Application.Contracts.Organization;
using Strat.System.Application.Contracts.Organization.Dtos;
using Strat.System.Domain.Organization;

namespace Strat.System.Application.Organization;

/// <summary>
/// 组织架构服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class OrganizationService(ISqlSugarClient db) : ApplicationService, IOrganizationService
{
    private readonly ISqlSugarClient _db = db;

    public async Task<List<OrganizationResponse>> GetTreeAsync()
    {
        var list = await _db.Queryable<OrganizationEntity>()
            .OrderBy(x => x.Sort)
            .ToListAsync();

        var output = list.Adapt<List<OrganizationResponse>>();
        return output.ToTree(x => x.Children, x => x.ParentId, null);
    }

    public async Task<OrganizationResponse> GetAsync(long id)
    {
        var entity = await _db.Queryable<OrganizationEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);
        return entity.Adapt<OrganizationResponse>();
    }

    public async Task<long> AddAsync(AddOrganizationRequest input)
    {
        var entity = input.Adapt<OrganizationEntity>();
        return await _db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
    }

    public async Task UpdateAsync(long id, AddOrganizationRequest input)
    {
        var entity = await _db.Queryable<OrganizationEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        if (input.ParentId == id)
            throw BusinessException.Throw("不能将自己设为上级组织");

        entity = input.Adapt(entity);
        await _db.Updateable(entity).IgnoreColumns(x => new { x.CreateBy, x.CreateTime }).ExecuteCommandAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.Queryable<OrganizationEntity>().FirstAsync(x => x.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        var hasChildren = await _db.Queryable<OrganizationEntity>().AnyAsync(x => x.ParentId == id);
        if (hasChildren)
            throw BusinessException.Throw("该组织下存在子组织，无法删除");

        entity.IsDeleted = true;
        await _db.Updateable(entity).UpdateColumns(x => x.IsDeleted).ExecuteCommandAsync();
    }
}

