using Pb.Geo.Service.Services;

namespace Pb.Geo.Service.Setup;

public static class GrpcSetup
{
    public static IServiceCollection SetupGrpcServices(this IServiceCollection services)
    {
        services
            .AddGrpc()
            .AddJsonTranscoding();
        services.AddSingleton<GeoService>();
        
        return services;
    }
}