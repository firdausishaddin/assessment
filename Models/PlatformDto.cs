using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace assessment.Models
{
    public class PlatformDto
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        [JsonProperty("uniqueName")]
        public string? UniqueName { get; set; } = "";
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }
        [JsonProperty("longitude")]
        public double? Longitude { get; set; }
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; } = null;
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; } = null;
        [JsonProperty("well")]
        public List<WellDto> Wells { get; set; } = new();
    }
}