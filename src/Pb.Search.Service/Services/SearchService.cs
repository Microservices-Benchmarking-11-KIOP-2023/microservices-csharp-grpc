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
            }) ?? throw new RpcException(new Status(StatusCode.Internal,
                "Profile gRPC service failed to respond in time or the response was null"));
            
            _log.LogInformation("Successfully retrieved data from Geo Service");

            var hotelRates = await _rateClient.GetRatesAsync(new RateRequest()
            {
                HotelIds = { nearbyHotels.HotelIds },
                InDate = request.InDate,
                OutDate = request.OutDate
            }) ?? throw new RpcException(new Status(StatusCode.Internal,
                "Profile gRPC service failed to respond in time or the response was null"));
            
            _log.LogInformation("Successfully retrieved data from Rates Service");
            
            return new SearchResult()
            {
                HotelIds = { hotelRates.RatePlans.Select(x => x.HotelId) }
            };
        }
        catch (RpcException e)
        {
            _log.LogError("One of gRPC services returned null: {Exception}", e);
            return null!;
        }
    }
}