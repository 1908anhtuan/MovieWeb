using System.Text.Json.Serialization;

namespace VideoServicePrj.Models;

public class Genre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("genre")]
    public string? GenreName { get; set; }
}