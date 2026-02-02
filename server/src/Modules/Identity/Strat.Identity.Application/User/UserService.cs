using Strat.Identity.Domain.Events;
using Strat.Identity.Domain.Shared;
using Strat.Shared.Abstractions;
using Strat.Shared.Extensions;

namespace Strat.Identity.Application.User;

/// <summary>
/// 用户服务
/// </summary>
[ApiExplorerSettings(GroupName = ApiGroupConst.System)]
[Authorize]
public class UserService(
    ISqlSugarClient db, 
    ICache cache, 
    IEventPublisher eventPublisher,
    IUnitOfWork unitOfWork) : ApplicationService, IUserService
{
    private readonly ISqlSugarClient _db = db;
    private readonly ICache _cache = cache;
    private readonly IEventPublisher _eventPublisher = eventPublisher;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private const string PermissionCacheKey = "Auth:Permissions:{0}";
    private const string RouterCacheKey = "Auth:Routers:{0}";

    /// <summary>
    /// 分页查询用户
    /// </summary>
    public async Task<PagedList<UserResponse>> GetPagedListAsync(GetUserPagedRequest input)
    {
        RefAsync<int> total = 0;

        var list = await _db.Queryable<UserEntity>()
            .LeftJoin<UserRoleEntity>((u, ur) => u.Id == ur.UserId)
            .LeftJoin<RoleEntity>((u, ur, r) => ur.RoleId == r.Id)
            .WhereIF(input.Name.IsNotNullOrWhiteSpace(), u => u.Name.Contains(input.Name!))
            .WhereIF(input.Account.IsNotNullOrWhiteSpace(), u => u.Account.Contains(input.Account!))
            .WhereIF(input.Telephone.IsNotNullOrWhiteSpace(), u => u.Telephone!.Contains(input.Telephone!))
            .WhereIF(input.Email.IsNotNullOrWhiteSpace(), u => u.Email!.Contains(input.Email!))
            .WhereIF(input.Status.HasValue, u => u.Status == input.Status)
            .WhereIF(input.OrganizationId.HasValue, u => u.OrganizationId == input.OrganizationId)
            .Select((u, ur, r) => new UserEntity
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                Telephone = u.Telephone,
                Email = u.Email,
                Avatar = u.Avatar,
                Status = u.Status,
                OrganizationId = u.OrganizationId,
                CreateTime = u.CreateTime,
                Remark = u.Remark,
                RoleId = r.Id,
                RoleName = r.Name
            })
            .OrderBy(u => u.Status)
            .OrderByDescending(u => u.CreateTime)
            .ToPageListAsync(input.PageIndex, input.PageSize, total);

        var items = list.Adapt<List<UserResponse>>();
        return PagedList<UserResponse>.Create(items, total, input.PageIndex, input.PageSize);
    }

    /// <summary>
    /// 获取用户详情
    /// </summary>
    public async Task<UserResponse> GetAsync(long id)
    {
        var entity = await _db.Queryable<UserEntity>()
            .LeftJoin<UserRoleEntity>((u, ur) => u.Id == ur.UserId)
            .LeftJoin<RoleEntity>((u, ur, r) => ur.RoleId == r.Id)
            .Where(u => u.Id == id)
            .Select((u, ur, r) => new UserEntity
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                Telephone = u.Telephone,
                Email = u.Email,
                Avatar = u.Avatar,
                Status = u.Status,
                OrganizationId = u.OrganizationId,
                CreateTime = u.CreateTime,
                Remark = u.Remark,
                RoleId = r.Id,
                RoleName = r.Name
            })
            .FirstAsync();

        return entity == null ? throw BusinessException.Throw(ErrorTipsEnum.NoResult) : entity.Adapt<UserResponse>();
    }

    /// <summary>
    /// 添加用户
    /// </summary>
    public async Task<long> AddAsync(AddUserRequest input)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // 检查账号是否存在
            var exists = await _db.Queryable<UserEntity>()
                .AnyAsync(u => u.Account == input.Account);
            if (exists)
                throw BusinessException.Throw("账号已存在");

            var entity = input.Adapt<UserEntity>();
            entity.Password = BCryptHelper.HashPassword(input.Password ?? "123456");

            var id = await _db.Insertable(entity).ExecuteReturnSnowflakeIdAsync();

            // 添加用户角色关联
            await _db.Insertable(new UserRoleEntity
            {
                UserId = id,
                RoleId = input.RoleId
            }).ExecuteReturnSnowflakeIdAsync();

            return id;
        });
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    public async Task UpdateAsync(long id, UpdateUserRequest input)
    {
        await _unitOfWork.ExecuteAsync(async () =>
        {
            var entity = await _db.Queryable<UserEntity>().FirstAsync(u => u.Id == id);
            if (entity == null)
                throw BusinessException.Throw(ErrorTipsEnum.NoResult);

            // 更新用户信息
            if (input.Name != null) entity.Name = input.Name;
            if (input.Telephone != null) entity.Telephone = input.Telephone;
            if (input.Email != null) entity.Email = input.Email;
            if (input.Avatar != null) entity.Avatar = input.Avatar;
            if (input.OrganizationId.HasValue) entity.OrganizationId = input.OrganizationId.Value;
            if (input.Remark != null) entity.Remark = input.Remark;

            await _db.Updateable(entity)
                .IgnoreColumns(u => new { u.Account, u.Password, u.Status, u.CreateBy, u.CreateTime })
                .ExecuteCommandAsync();

            // 更新角色关联
            if (input.RoleId.HasValue)
            {
                await _db.Deleteable<UserRoleEntity>().Where(ur => ur.UserId == id).ExecuteCommandAsync();
                await _db.Insertable(new UserRoleEntity
                {
                    UserId = id,
                    RoleId = input.RoleId.Value
                }).ExecuteReturnSnowflakeIdAsync();

                // 发送变更事件，由 Handler 处理缓存清理
                await _eventPublisher.PublishAsync(new UserChangedEvent(id));
            }
        });
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    public async Task DeleteAsync(long id)
    {
        var entity = await _db.Queryable<UserEntity>().FirstAsync(u => u.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        if (entity.Account == "admin")
            throw BusinessException.Throw("初始账户禁止删除");

        // 删除用户角色关联
        await _db.Deleteable<UserRoleEntity>().Where(ur => ur.UserId == id).ExecuteCommandAsync();

        // 软删除用户
        entity.IsDeleted = true;
        await _db.Updateable(entity).UpdateColumns(u => u.IsDeleted).ExecuteCommandAsync();

        // 发送变更事件
        await _eventPublisher.PublishAsync(new UserChangedEvent(id));
    }

    /// <summary>
    /// 停用用户
    /// </summary>
    public async Task DisableAsync(long id)
    {
        var entity = await _db.Queryable<UserEntity>().FirstAsync(u => u.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        if (entity.Account == "admin")
            throw BusinessException.Throw("初始账户不能停用");

        entity.Status = (int)UserStatusEnum.Stop;
        await _db.Updateable(entity).UpdateColumns(u => u.Status).ExecuteCommandAsync();

        // 发送变更事件
        await _eventPublisher.PublishAsync(new UserChangedEvent(id));
    }

    /// <summary>
    /// 启用用户
    /// </summary>
    public async Task EnableAsync(long id)
    {
        var entity = await _db.Queryable<UserEntity>().FirstAsync(u => u.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        entity.Status = (int)UserStatusEnum.Normal;
        await _db.Updateable(entity).UpdateColumns(u => u.Status).ExecuteCommandAsync();
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    public async Task<string> ResetPasswordAsync(long id)
    {
        var entity = await _db.Queryable<UserEntity>().FirstAsync(u => u.Id == id);
        if (entity == null)
            throw BusinessException.Throw(ErrorTipsEnum.NoResult);

        var newPassword = new Random().Next(100000, 999999).ToString();
        entity.Password = BCryptHelper.HashPassword(newPassword);

        await _db.Updateable(entity).UpdateColumns(u => u.Password).ExecuteCommandAsync();

        return newPassword;
    }
}

