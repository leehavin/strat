using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Strat.Infrastructure.Models.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "用户名不能为空"), MinLength(3, ErrorMessage = "用户名不能少于 3 位字符")]
    [JsonPropertyName("account")]
    public string Account { get; set; } = string.Empty;

    [Required(ErrorMessage = "密码不能为空"), MinLength(6, ErrorMessage = "密码不能少于 6 位字符")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
