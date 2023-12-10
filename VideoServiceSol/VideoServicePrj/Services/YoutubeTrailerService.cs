using VideoServicePrj.Models;
using System.Text.Json;


namespace VideoServicePrj.Services
{
    public class YoutubeTrailerService : ITrailerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<YoutubeTrailerService> _logger;
        private readonly IConfiguration _configuration;
        
        public YoutubeTrailerService(HttpClient httpClient, ILogger<YoutubeTrailerService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Trailer> GetTrailerAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            }

            var apiKey = _configuration["YouTubeApi:Key"]; 
            var searchQuery = UrlEncode($"{title} Trailer");
            var endpoint = $"search?part=id&q={searchQuery}&type=video&maxResults=1&key={apiKey}";
            var responseWrapper = await GetFromApiAsync<YoutubeApiResponse>(endpoint);
            var videoId = responseWrapper?.Items.FirstOrDefault()?.Id.VideoId;
            return new Trailer { VideoId = videoId };}


        private async Task<T?> GetFromApiAsync<T>(string endpoint)
        {
            try
            {
                _logger.LogInformation($"Sending request to {_httpClient.BaseAddress}{endpoint}");
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while making a request to the API.");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error occurred during response deserialization.");
                throw;
            }
        }

        private static string UrlEncode(string value)
        {
            return System.Net.WebUtility.UrlEncode(value);
        }
    }
}
