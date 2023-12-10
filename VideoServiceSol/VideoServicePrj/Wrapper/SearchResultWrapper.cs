using System.Text.Json.Serialization;
using VideoServicePrj.Models;

namespace VideoServicePrj.Wrapper;

public class SearchResultWrapper
{
    [JsonPropertyName("results")]
    public IEnumerable<SearchResult> Results { get; set; }
}