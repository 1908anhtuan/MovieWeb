using VideoServicePrj.Models;
using System.Text.Json;
using VideoServicePrj.Wrapper;

namespace VideoServicePrj.Services
{
    public class MoviesInfoService : IMovieDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MoviesInfoService> _logger;

        public MoviesInfoService(HttpClient httpClient, ILogger<MoviesInfoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<SearchResult>?> SearchMoviesAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            }
            
            var wrapper = await GetFromApiAsync<SearchResultWrapper>($"movie/imdb_id/byTitle/{title}/");
            if (wrapper == null) return Enumerable.Empty<SearchResult>(); 
            return wrapper.Results;      
        }

        public async Task<Movie?> GetMovieDetailAsync(string movieId)
        {
            if (string.IsNullOrWhiteSpace(movieId))
            {
                throw new ArgumentException("Movie ID cannot be null or empty.", nameof(movieId));
            }

            var wrapper = await GetFromApiAsync<MovieDetailWrapper>($"movie/id/{movieId}/");
            if (wrapper == null) return null; 
            return wrapper.Movie;
        }


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
    }
}
