using Grpc.Core;
using Pb.Profile.Service;

namespace Pb.Profile.Service.Services;

public class ProfileService : Profile.ProfileBase
{
    private readonly ILogger<ProfileService> _log;

    public ProfileService(ILogger<ProfileService> log)
    {
        _log = log;
    }

    // public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    // {
    //     return Task.FromResult(new HelloReply
    //     {
    //         Message = "Hello " + request.Name
    //     });
    // }
}