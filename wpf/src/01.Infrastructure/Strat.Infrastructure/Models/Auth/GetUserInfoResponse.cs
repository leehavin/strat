using System.Text.Json.Serialization;

namespace Strat.Infrastructure.Models.Auth
{
    public class GetUserInfoResponse
    {
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("telephone")]
        public string Telephone { get; set; } = string.Empty;
        
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
}

