using System.Text.Json.Serialization;

namespace Strat.Infrastructure.Models.Auth
{
    public class GetRoutersResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("component")]
        public string? Component { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("sort")]
        public int Sort { get; set; }

        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        [JsonPropertyName("parentId")]
        public long? ParentId { get; set; }
        
        [JsonPropertyName("children")]
        public List<GetRoutersResponse> Children { get; set; } = new List<GetRoutersResponse>();
    }
}

