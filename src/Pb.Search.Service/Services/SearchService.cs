using Grpc.Core;
using Pb.Search.Service;

namespace Pb.Search.Service.Services;

public class SearchService : Search.SearchBase
{
    private readonly ILogger<SearchService> _log;
    private readonly Geo.GeoClient _geoClient;
    private readonly Rate.RateClient _rateClient;

    public SearchService(ILogger<SearchService> log, Geo.GeoClient geoClient, Rate.RateClient rateClient)
    {
        _log = log;
        _geoClient = geoClient;
        _rateClient = rateClient;
    }
    
    public override async Task<SearchResult> Nearby(NearbyRequest request, ServerCallContext context)
    {
        _log.LogInformation("Search service called with parameters: {Request}", request);

        try
        {
            var nearbyHotels = await _geoClient.NearbyAsync(new GeoRequest()
            {
                Lat = request.Lat,
                Lon = request.Lon
            }) ?? throw NullReferenceException("Invalid Geo service request parameters");
        }
        catch (NullReferenceException e)
        {
            
        }

        _log.LogInformation("");
        
        var hotelRates = await _rateClient.GetRatesAsync()
    }
}