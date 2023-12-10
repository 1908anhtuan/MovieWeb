using Microsoft.AspNetCore.Mvc;
using VideoServicePrj.Services;

namespace VideoServicePrj.Controllers;

[ApiController]
[Route("[controller]")]
public class TrailerController : ControllerBase
{
    private readonly ITrailerService _trailerService;
    private readonly ILogger<TrailerController> _logger;

    public TrailerController(ITrailerService trailerService, ILogger<TrailerController> logger)
    {
        _trailerService = trailerService;
        _logger = logger;
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> GetTrailer(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest("Title is required.");
        }

        try
        {
            var trailer = await _trailerService.GetTrailerAsync(title);
            if (trailer.VideoId == null) 
            {
                return NotFound($"Trailer for '{title}' not found.");
            }

            return Ok(trailer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching trailer for '{title}': {ex.Message}");
            return StatusCode(500, "An error occurred while fetching the trailer.");
        }
    }

}