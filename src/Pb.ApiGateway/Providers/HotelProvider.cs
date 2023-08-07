using Pb.ApiGateway.Models;

namespace Pb.ApiGateway.Providers;

public interface IHotelProvider
{
    Task<GeoJsonResponse?> FetchHotels(HotelParameters parameters);
}

public class HotelProvider : IHotelProvider
{
    private readonly ILogger<HotelProvider> _log;
    private readonly Search.SearchClient _searchClient;
    private readonly Profile.ProfileClient _profileClient;

    public HotelProvider(ILogger<HotelProvider> log, Profile.ProfileClient profileClient,
        Search.SearchClient searchClient)
    {
        _log = log;
        _profileClient = profileClient;
        _searchClient = searchClient;
    }

    public async Task<GeoJsonResponse?> FetchHotels(HotelParameters parameters)
    {
        if (CheckParameters(parameters)) return null;

        var searchResponse = await _searchClient.NearbyAsync(
            new NearbyRequest
            {
                Lon = parameters.Lon!.Value,
                Lat = parameters.Lat!.Value,
                InDate = parameters.InDate,
                OutDate = parameters.OutDate
            });

        var profileResponse = await _profileClient.GetProfilesAsync(
            new ProfileRequest
            {
                HotelIds = { searchResponse.HotelIds }
            });

        var hotels = CreateGeoJsonResponse(profileResponse.Hotels);

        return hotels;
    }

    private static GeoJsonResponse? CreateGeoJsonResponse(IEnumerable<Hotel> hotels)
    {
        var features = new List<Feature>();

        foreach (var hotel in hotels)
        {
            features.Add(new Feature
            {
                Type = "Feature",
                Id = hotel.Id,
                Properties = hotel,
                Geometry = new Geometry
                {
                    Type = "Point",
                    Coordinates = new List<float> { hotel.Address.Lon, hotel.Address.Lat }
                }
            });
        }

        return new GeoJsonResponse
        {
            Type = "FeatureCollection",
            Features = features
        };
    }

    private bool CheckParameters(HotelParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(parameters.InDate) || string.IsNullOrWhiteSpace(parameters.OutDate))
        {
            _log.LogError("Please specify proper inDate/outDate params");
            return true;
        }

        if (parameters is { Lon: not null, Lat: not null }) return false;
        
        _log.LogError("Please specify proper lon/lat params");
        
        return true;

    }
}