using System.Text.Json.Serialization;
using VideoServicePrj.Models;

namespace VideoServicePrj.Wrapper;

public class MovieDetailWrapper
{
    [JsonPropertyName("results")]
    public Movie Movie { get; set; }
}
