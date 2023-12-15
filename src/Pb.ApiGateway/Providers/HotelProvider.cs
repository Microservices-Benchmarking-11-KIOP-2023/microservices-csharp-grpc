using System.Globalization;
using Grpc.Core;
using Pb.ApiGateway.Models;
using Profile;
using Search;
using SearchClient = Search.Search.SearchClient;
using ProfileClient = Profile.Profile.ProfileClient;
using Hotel = Profile.Hotel;

namespace Pb.ApiGateway.Providers;

public interface IHotelProvider
{
    Task<GeoJsonResponse?> FetchHotels(HotelParameters parameters);
}

public class HotelProvider : IHotelProvider
{
    private readonly ILogger<HotelProvider> _log;
    private readonly SearchClient _searchClient;
    private readonly ProfileClient _profileClient;

    public HotelProvider(ILogger<HotelProvider> log, ProfileClient profileClient,
        SearchClient searchClient)
    {
        _log = log;
        _profileClient = profileClient;
        _searchClient = searchClient;
    }

    public async Task<GeoJsonResponse?> FetchHotels(HotelParameters parameters)
    {
        _log.LogInformation("Processing request. Checking ");
        if (AreParametersInvalid(parameters))
            throw new Exception(
                $"Invalid Parameters: {parameters.Lon!.Value}, {parameters.Lat!.Value},{parameters.InDate},{parameters.OutDate}");

        try
        {
            _log.LogInformation("Calling Search with params: {ParametersLon}, {ParametersLat},{ParametersInDate},{ParametersOutDate}",
                parameters.Lon!.Value, parameters.Lat!.Value, parameters.InDate, parameters.OutDate);

            var searchResponse = await _searchClient.NearbyAsync(
                new NearbyRequest
                {
                    Lon = parameters.Lon!.Value,
                    Lat = parameters.Lat!.Value,
                    InDate = parameters.InDate,
                    OutDate = parameters.OutDate
                }) ?? throw new RpcException(new Status(StatusCode.Unavailable,
                "Search gRPC service failed to respond in time"));

            _log.LogInformation("Calling profile with number of hotels {HotelIds}:",searchResponse.HotelIds.Count);
            var profileResponse = await _profileClient.GetProfilesAsync(
                new ProfileRequest
                {
                    HotelIds = { searchResponse.HotelIds }
                }) ?? throw new RpcException(new Status(StatusCode.Unavailable,
                "Profile gRPC service failed to respond in time"));
            
            _log.LogInformation("Retrieved data from both search and profile service");

            var hotels = CreateGeoJsonResponse(profileResponse.Hotels);
            return hotels;
        }
        catch (RpcException e)
        {
            _log.LogError("One of gRPC services responded with Unavailable status code : {Exception}", e);
            return new GeoJsonResponse();
        }
        catch (Exception e)
        {
            _log.LogError("Unknown exception: {Exception}", e);
            return new GeoJsonResponse();
        }
    }

    private static GeoJsonResponse? CreateGeoJsonResponse(IEnumerable<Hotel> hotels)
    {
        var features = new List<Feature?>();

        foreach (var hotel in hotels)
        {
            features.Add(new Feature
            {
                Type = "Feature",
                Id = hotel.Id,
                Properties = new Properties()
                {
                    Name = hotel.Name,
                    PhoneNumber = hotel.PhoneNumber
                },
                Geometry = new Geometry
                {
                    Type = "Point",
                    Coordinates = new double[]
                    {
                        hotel.Address.Lon,
                        hotel.Address.Lat
                    }
                }
            });
        }

        return new GeoJsonResponse
        {
            Type = "FeatureCollection",
            Features = features!
        };
    }

    private bool AreParametersInvalid(HotelParameters parameters)
    {
        var isValidFormatDateIn = DateTime.TryParseExact(parameters.InDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateIn);
        var isValidFormatDateOut = DateTime.TryParseExact(parameters.OutDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOut);

        if (!isValidFormatDateIn && !isValidFormatDateOut)
        {
            _log.LogError("Please specify proper inDate/outDate params {Parameters}", parameters);
            return true;
        }

        if (parameters is { Lon: not null, Lat: not null })
        {
            return false;
        }
        
        _log.LogError("Please specify proper lon/lat params {Parameters}", parameters);
        return true;
    }
}