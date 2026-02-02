using System.Collections.Generic;
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
        
        [JsonPropertyName("parentId")]
        public long? ParentId { get; set; }
        
        [JsonPropertyName("children")]
        public List<GetRoutersResponse> Children { get; set; } = new List<GetRoutersResponse>();
    }
}

