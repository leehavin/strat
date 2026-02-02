using Strat.Shared.CommonViewModels;
using System.ComponentModel.DataAnnotations;

namespace Strat.Module.Identity.Models
{
    public class LoginModel : StratValidateModel
    {
        private string _account = string.Empty;
        [Required(ErrorMessage = "账号不能为空")]
        public string Account
        {
            get => _account;
            set => SetProperty(ref _account, value);
        }

        private string _password = string.Empty;
        [Required(ErrorMessage = "密码不能为空")]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
    }
}

