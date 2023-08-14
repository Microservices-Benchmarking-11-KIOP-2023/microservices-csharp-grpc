using GeoClient = Geo.Geo.GeoClient;
using RateClient = Rate.Rate.RateClient;

namespace Pb.Search.Service.Setup;

public static class GrpcSetup
{
    public static IServiceCollection SetupGrpcServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddGrpc()
            .AddJsonTranscoding();

        services.AddGrpcClient<GeoClient>(o =>
        {
            o.Address = new Uri(config.GetSection("SERVICES:ADDRESSES:GEO").Value ?? throw new InvalidOperationException());
        });
        services.AddGrpcClient<RateClient>(o =>
        {
            o.Address = new Uri(config.GetSection("SERVICES:ADDRESSES:RATE").Value ?? throw new InvalidOperationException());
        });

        return services;
    }
}