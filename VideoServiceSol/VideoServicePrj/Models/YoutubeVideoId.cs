using System.Text.Json.Serialization;

namespace VideoServicePrj.Models;

public class YoutubeVideoId
{
    [JsonPropertyName("videoId")]
    public string VideoId { get; set; }
}