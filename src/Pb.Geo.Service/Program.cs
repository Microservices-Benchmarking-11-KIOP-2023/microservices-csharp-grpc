using Pb.Geo.Service.Models;
using Pb.Geo.Service.Services;
using Pb.Geo.Service.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.SetupGrpcServices();
builder.Services.AddSingleton<IPointLoader>(new PointLoader(builder.Configuration["DATA:GEO"]));

var app = builder.Build();

app.MapGrpcService<GeoService>();

app.Run();