using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace assessment.Responses
{
    public class WellApiResponse
    {
        public int Id { get; set; }
        [JsonProperty("platformId")]
        public int PlatformId { get; set; }
        [JsonProperty("uniqueName")]
        public string? UniqueName { get; set; } = "";
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }
        [JsonProperty("longitude")]
        public double? Longitude { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedAt { get; set; } = null;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
