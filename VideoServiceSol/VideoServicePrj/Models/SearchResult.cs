using System.Text.Json.Serialization;

namespace VideoServicePrj.Models
{
    public class SearchResult
    {
        [JsonPropertyName("imdb_id")]
        public string? MovieId { get; set; }
        [JsonPropertyName("title")]
        public string? Name { get; set; }
        
        
    }
}