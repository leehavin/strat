using System.Text.Json.Serialization;

namespace Strat.Infrastructure.Models.Auth;

public class LoginResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; } = new();
}
