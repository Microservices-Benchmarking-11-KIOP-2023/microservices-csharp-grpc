using System.Diagnostics;
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
    private readonly SearchClient _searchClient;
    private readonly ProfileClient _profileClient;

    public HotelProvider(ProfileClient profileClient, SearchClient searchClient)
    {
        _profileClient = profileClient;
        _searchClient = searchClient;
    }

    public async Task<GeoJsonResponse?> FetchHotels(HotelParameters parameters)
    {
        try
        {
            var searchResponse = await _searchClient.NearbyAsync(
                new NearbyRequest
                {
                    Lon = parameters.Lon!.Value,
                    Lat = parameters.Lat!.Value,
                    InDate = parameters.InDate,
                    OutDate = parameters.OutDate
                }) ?? throw new RpcException(new Status(StatusCode.Unavailable,
                "Search gRPC service failed to respond in time"));
            
            var profileResponse = await _profileClient.GetProfilesAsync(
                new ProfileRequest
                {
                    HotelIds = { searchResponse.HotelIds }
                }) ?? throw new RpcException(new Status(StatusCode.Unavailable,
                "Profile gRPC service failed to respond in time"));

            var hotels = CreateGeoJsonResponse(profileResponse.Hotels);
            return hotels;
        }
        catch (RpcException e)
        {
            return new GeoJsonResponse();
        }
        catch (Exception e)
        {
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
            return true;
        }

        if (parameters is { Lon: not null, Lat: not null })
        {
            return false;
        }
        
        return true;
    }
}