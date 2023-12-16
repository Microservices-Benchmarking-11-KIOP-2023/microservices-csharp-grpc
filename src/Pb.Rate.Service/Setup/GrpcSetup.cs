using Pb.Rate.Service.Services;

namespace Pb.Rate.Service.Setup;

public static class GrpcSetup
{
    public static IServiceCollection SetupGrpcServices(this IServiceCollection services)
    {
        services
            .AddGrpc()
            .AddJsonTranscoding();
        services.AddSingleton<RateService>();
        return services;
    }
}