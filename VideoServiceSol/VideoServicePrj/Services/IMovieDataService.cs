using VideoServicePrj.Models;

namespace VideoServicePrj.Services;

public interface IMovieDataService
{
    Task<IEnumerable<SearchResult>?> SearchMoviesAsync(string title);
    Task<Movie?> GetMovieDetailAsync(string movieId);
}