using Grpc.Core;
using Grpc.Net.Client;
using SearchClient = Search.Search.SearchClient;
using ProfileClient = Profile.Profile.ProfileClient;

namespace Pb.ApiGateway.Setup;
public static class GrpcSetup
{
    public static IServiceCollection SetupGrpcServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddGrpc()
            .AddJsonTranscoding();

        services.AddGrpcClient<SearchClient>(o =>
        {
            o.Address = new Uri(config.GetSection("SERVICES:ADDRESSES:SEARCH").Value ??
                                throw new InvalidOperationException());
            o.ChannelOptionsActions.Add(options =>
                options.Credentials = ChannelCredentials.Insecure);
        }).ConfigureChannel(options =>
        {
            options.HttpHandler = new SocketsHttpHandler()
            {
                EnableMultipleHttp2Connections = true,
            };
        });
        
        services.AddGrpcClient<ProfileClient>(o =>
        {
            o.Address = new Uri(config.GetSection("SERVICES:ADDRESSES:PROFILE").Value ?? throw new InvalidOperationException());
            o.ChannelOptionsActions.Add(options =>
                options.Credentials = ChannelCredentials.Insecure);
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