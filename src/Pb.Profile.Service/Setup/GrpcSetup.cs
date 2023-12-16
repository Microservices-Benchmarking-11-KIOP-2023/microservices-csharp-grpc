using Pb.Profile.Service.Services;

namespace Pb.Profile.Service.Setup;

public static class GrpcSetup
{
    public static IServiceCollection SetupGrpcServices(this IServiceCollection services)
    {
        services
            .AddGrpc()
            .AddJsonTranscoding();
        services.AddSingleton<ProfileService>();
        return services;
    }
}