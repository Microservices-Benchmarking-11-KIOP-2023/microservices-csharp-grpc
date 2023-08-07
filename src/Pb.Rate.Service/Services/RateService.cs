using Grpc.Core;
using Pb.Rate.Service;

namespace Pb.Rate.Service.Services;

public class RateService : Rate.RateBase
{
    private readonly ILogger<RateService> _logger;

    public RateService(ILogger<RateService> logger)
    {
        _logger = logger;
    }

    // public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    // {
    //     return Task.FromResult(new HelloReply
    //     {
    //         Message = "Hello " + request.Name
    //     });
    // }
}