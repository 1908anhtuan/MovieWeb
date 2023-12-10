using System.Text.Json.Serialization;

namespace VideoServicePrj.Models;

public class YoutubeApiResponse
{
    [JsonPropertyName("items")]
    public List<YoutubeVideoItem> Items { get; set; } = new List<YoutubeVideoItem>();
}
