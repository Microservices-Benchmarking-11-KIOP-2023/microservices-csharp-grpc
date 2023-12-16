using Grpc.Core;
using Pb.Profile.Service.Models;
using Profile;

namespace Pb.Profile.Service.Services;

public class ProfileService : global::Profile.Profile.ProfileBase
{
    private readonly IDictionary<string, Hotel> _profiles;

    public ProfileService(IHotelLoader hotelLoader)
    {
        _profiles = InitializeProfiles(hotelLoader.Hotels);
    }

    public override Task<ProfileResult> GetProfiles(ProfileRequest request, ServerCallContext context)
    {
        var result = new ProfileResult();

        foreach (var hotelId in request.HotelIds)
        {
            if (_profiles.TryGetValue(hotelId, out var hotelProfile))
            {
                result.Hotels.Add(hotelProfile);
            }
        }

        return Task.FromResult(result);
    }

    private Dictionary<string, Hotel> InitializeProfiles(Hotel[] hotels)
    {
        var profiles = new Dictionary<string, Hotel>();

        foreach (var hotel in hotels)
        {
            profiles[hotel.Id] = hotel;
        }

        return profiles;
    }
}