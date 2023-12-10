using Microsoft.AspNetCore.Mvc;
using VideoServicePrj.Models;
using VideoServicePrj.Services;


namespace VideoServicePrj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieDataService _movieDataService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMovieDataService movieDataService, ILogger<MoviesController> logger)
        {
            _movieDataService = movieDataService;
            _logger = logger;
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<SearchResult>>> Search(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                _logger.LogInformation("Search called with null or empty title.");
                return BadRequest("Title is required.");
            }

            try
            {
                _logger.LogInformation($"Searching movies with title: {title}");
                var results = await _movieDataService.SearchMoviesAsync(title);
                if (results == null) return NotFound($"No results found for '{title}'.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching movies: {ex.Message}");
                return StatusCode(500, "Error fetching movies from external service");
            }
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Movie>> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("Get called with null or empty ID.");
                return BadRequest("ID is required.");
            }

            try
            {
                _logger.LogInformation($"Getting movie details with ID: {id}");
                var movie = await _movieDataService.GetMovieDetailAsync(id);

                if (movie != null) return Ok(movie);
                _logger.LogInformation($"Movie not found for ID: {id}");
                return NotFound($"Movie with ID '{id}' not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching movie details: {ex.Message}");
                return StatusCode(500, "Error fetching movie details from external service");
            }
        }
    }
}
