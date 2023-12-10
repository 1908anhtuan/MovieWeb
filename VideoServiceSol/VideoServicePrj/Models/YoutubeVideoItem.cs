using System.Text.Json.Serialization;

namespace VideoServicePrj.Models;

public class YoutubeVideoItem
{
    [JsonPropertyName("id")]
    public YoutubeVideoId Id { get; set; }
}