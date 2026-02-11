using Prism.Mvvm;

namespace Strat.Infrastructure.Models.Role
{
    public class RoleResponse : BindableBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Remark { get; set; }
        public int Status { get; set; } // Added Status
    }
}

