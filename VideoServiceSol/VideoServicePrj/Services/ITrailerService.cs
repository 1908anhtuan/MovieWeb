using VideoServicePrj.Models;

namespace VideoServicePrj.Services;

public interface ITrailerService
{
    public Task<Trailer> GetTrailerAsync(string title);
}