namespace Pb.Geo.Service.Setup;

public static class GrpcSetup
{
    public static IServiceCollection SetupGrpcServices(this IServiceCollection services)
    {
        services
            .AddGrpc()
            .AddJsonTranscoding();
        
        return services;
    }
}