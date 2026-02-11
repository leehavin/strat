using Strat.Shared.Models;

namespace Strat.Shared.Services
{
    /// <summary>
    /// 快捷搜索服务接口（企业级命令面板）
    /// </summary>
    public interface IQuickSearchService
    {
        /// <summary>
        /// 搜索项目
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="type">搜索类型过滤（null=全部）</param>
        /// <param name="maxResults">最大结果数量</param>
        Task<List<QuickSearchItem>> SearchAsync(string keyword, QuickSearchType? type = null, int maxResults = 10);
        
        /// <summary>
        /// 获取所有可用项目（用于命令面板）
        /// </summary>
        Task<List<QuickSearchItem>> GetAllItemsAsync();
        
        /// <summary>
        /// 获取最近使用的项目
        /// </summary>
        Task<List<QuickSearchItem>> GetRecentItemsAsync(int count = 5);
        
        /// <summary>
        /// 记录使用历史
        /// </summary>
        Task RecordUsageAsync(string itemId);
    }
    
    /// <summary>
    /// 快捷搜索服务实现（Mock版本，后续可替换为真实API）
    /// </summary>
    public class QuickSearchService : IQuickSearchService
    {
        private readonly List<QuickSearchItem> _allItems = new();
        private readonly List<string> _recentIds = new();
        
        public QuickSearchService()
        {
            InitializeDefaultItems();
        }
        
        private void InitializeDefaultItems()
        {
            _allItems.AddRange(new[]
            {
                new QuickSearchItem 
                { 
                    Id = "page.user", 
                    Title = "用户管理", 
                    Description = "管理系统用户", 
                    Type = QuickSearchType.Page, 
                    IconPath = Strat.Shared.Assets.SemiIcons.People,
                    Keywords = new[] { "user", "用户", "yhgl" },
                    TargetRoute = "User",
                    Priority = 10
                },
                new QuickSearchItem 
                { 
                    Id = "page.role", 
                    Title = "角色管理", 
                    Description = "管理系统角色", 
                    Type = QuickSearchType.Page, 
                    IconPath = Strat.Shared.Assets.SemiIcons.Badge,
                    Keywords = new[] { "role", "角色", "jsgl" },
                    TargetRoute = "Role",
                    Priority = 9
                },
                new QuickSearchItem 
                { 
                    Id = "page.home", 
                    Title = "首页", 
                    Description = "返回首页", 
                    Type = QuickSearchType.Page, 
                    IconPath = Strat.Shared.Assets.SemiIcons.Home,
                    Keywords = new[] { "home", "首页", "sy" },
                    TargetRoute = "Home",
                    Priority = 8
                },
                new QuickSearchItem 
                { 
                    Id = "action.refresh", 
                    Title = "刷新页面", 
                    Description = "刷新当前页面数据", 
                    Type = QuickSearchType.Action, 
                    IconPath = Strat.Shared.Assets.SemiIcons.Refresh,
                    Keywords = new[] { "refresh", "刷新", "sx" },
                    Priority = 7
                },
                new QuickSearchItem 
                { 
                    Id = "action.logout", 
                    Title = "退出登录", 
                    Description = "退出当前账号", 
                    Type = QuickSearchType.Action, 
                    IconPath = Strat.Shared.Assets.SemiIcons.Logout,
                    Keywords = new[] { "logout", "退出", "tc" },
                    Priority = 6
                },
                new QuickSearchItem 
                { 
                    Id = "setting.profile", 
                    Title = "个人设置", 
                    Description = "编辑个人信息", 
                    Type = QuickSearchType.Setting, 
                    IconPath = Strat.Shared.Assets.SemiIcons.Settings,
                    Keywords = new[] { "profile", "设置", "sz", "grsz" },
                    Priority = 5
                }
            });
        }
        
        public Task<List<QuickSearchItem>> SearchAsync(string keyword, QuickSearchType? type = null, int maxResults = 10)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return Task.FromResult(_allItems.Take(maxResults).ToList());
            }
            
            var lowerKeyword = keyword.ToLower();
            var results = _allItems
                .Where(item => type == null || item.Type == type)
                .Where(item => 
                    item.Title.ToLower().Contains(lowerKeyword) ||
                    item.Keywords.Any(k => k.ToLower().Contains(lowerKeyword)) ||
                    (item.Description?.ToLower().Contains(lowerKeyword) ?? false))
                .OrderByDescending(item => item.Priority)
                .Take(maxResults)
                .ToList();
            
            return Task.FromResult(results);
        }
        
        public Task<List<QuickSearchItem>> GetAllItemsAsync()
        {
            return Task.FromResult(_allItems.OrderByDescending(x => x.Priority).ToList());
        }
        
        public Task<List<QuickSearchItem>> GetRecentItemsAsync(int count = 5)
        {
            var recentItems = _recentIds
                .Take(count)
                .Select(id => _allItems.FirstOrDefault(x => x.Id == id))
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
            
            return Task.FromResult(recentItems);
        }
        
        public Task RecordUsageAsync(string itemId)
        {
            _recentIds.Remove(itemId);
            _recentIds.Insert(0, itemId);
            
            if (_recentIds.Count > 20)
            {
                _recentIds.RemoveAt(_recentIds.Count - 1);
            }
            
            return Task.CompletedTask;
        }
    }
}
