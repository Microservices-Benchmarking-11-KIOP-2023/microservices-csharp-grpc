
namespace Pb.Geo.Service.Services;

public class GeoService : Geo.GeoBase
{
    private readonly ILogger<GeoService> _logger;

    public GeoService(ILogger<GeoService> logger)
    {
        _logger = logger;
    }
}