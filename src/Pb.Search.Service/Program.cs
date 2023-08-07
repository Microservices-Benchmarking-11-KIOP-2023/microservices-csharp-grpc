using Pb.Search.Service.Services;
using Pb.Search.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.SetupGrpcServices(builder.Configuration);

var app = builder.Build();
app.MapGrpcService<SearchService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();