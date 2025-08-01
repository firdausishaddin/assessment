﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace assessment.Models
{
    public class WellDto
    {
        public int Id { get; set; }
        public int WellId { get; set; }
        [JsonProperty("platformId")]
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
        public PlatformDto? Platform { get; set; }
    }
}