using Grpc.Net.Client;

namespace Pb.ApiGateway.Setup;

public static class GrpcSetup
{
    public static IServiceCollection SetupGrpcServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddGrpc()
            .AddJsonTranscoding();

        services.AddGrpcClient<Search.SearchClient>(o =>
        {
            o.Address = new Uri(config.GetSection("SERVICES:ADDRESSES:SEARCH").Value ?? throw new InvalidOperationException());
        }).ConfigureChannel(options =>
        {
            options.HttpHandler = new SocketsHttpHandler()
            {
                EnableMultipleHttp2Connections = true,
            };
        });
        
        services.AddGrpcClient<Profile.ProfileClient>(o =>
        {
            o.Address = new Uri(config.GetSection("SERVICES:ADDRESSES:PROFILE").Value ?? throw new InvalidOperationException());
        }).ConfigureChannel(options =>
        {
            options.HttpHandler = new SocketsHttpHandler()
            {
                EnableMultipleHttp2Connections = true,
            };
        });;

        return services;
    }
}