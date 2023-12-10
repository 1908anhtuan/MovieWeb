using System.Text.Json.Serialization;

namespace VideoServicePrj.Models;

public class Trailer
{
    [JsonPropertyName("videoId")]
    public string? VideoId { get; set; }

}