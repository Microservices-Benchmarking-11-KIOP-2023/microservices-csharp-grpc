using Geo;
using Grpc.Core;
using Rate;
using Search;
using RateClient = Rate.Rate.RateClient;
using GeoClient = Geo.Geo.GeoClient;
namespace Pb.Search.Service.Services;

public class SearchService : global::Search.Search.SearchBase
{
    private readonly GeoClient _geoClient;
    private readonly RateClient _rateClient;

    public SearchService(GeoClient geoClient, RateClient rateClient)
    {
        _geoClient = geoClient;
        _rateClient = rateClient;
    }

    public override async Task<SearchResult> Nearby(NearbyRequest request, ServerCallContext context)
    {
        try
        {
            var nearbyHotels = await _geoClient.NearbyAsync(new GeoRequest()
            {
                Lat = request.Lat,
                Lon = request.Lon
            }) ?? throw new RpcException(new Status(StatusCode.Internal,
                "Profile gRPC service failed to respond in time or the response was null"));

            var hotelRates = await _rateClient.GetRatesAsync(new RateRequest()
            {
                HotelIds = { nearbyHotels.HotelIds },
                InDate = request.InDate,
                OutDate = request.OutDate
            }) ?? throw new RpcException(new Status(StatusCode.Internal,
                "Profile gRPC service failed to respond in time or the response was null"));

            return new SearchResult()
            {
                HotelIds = { hotelRates.RatePlans.Select(x => x.HotelId) }
            };
        }
        catch (RpcException e)
        {
            return null!;
        }
    }
}