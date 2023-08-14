using Microsoft.AspNetCore.Server.Kestrel.Core;
using Pb.Search.Service.Services;
using Pb.Search.Service.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.SetupGrpcServices(builder.Configuration);

var app = builder.Build();
app.MapGrpcService<SearchService>();

app.Run();