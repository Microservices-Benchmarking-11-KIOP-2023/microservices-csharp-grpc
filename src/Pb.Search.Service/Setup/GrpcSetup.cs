using Pb.Search.Service;

namespace Pb.Search.Setup;

public static class GrpcSetup
{
    public static IServiceCollection SetupGrpcServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddGrpc()
            .AddJsonTranscoding();

        services.AddGrpcClient<Geo.GeoClient>(o =>
        {
            o.Address = new Uri(config.GetSection("SERVICES:ADDRESSES:GEO").Value ?? throw new InvalidOperationException());
        });
        services.AddGrpcClient<Rate.RateClient>(o =>
        {
            o.Address = new Uri(config.GetSection("SERVICES:ADDRESSES:RATE").Value ?? throw new InvalidOperationException());
        });

        return services;
    }
}