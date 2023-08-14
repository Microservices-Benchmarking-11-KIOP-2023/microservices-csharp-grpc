using Pb.Profile.Service.Models;
using Pb.Profile.Service.Services;
using Pb.Profile.Service.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.SetupGrpcServices();
builder.Services.AddSingleton<IHotelLoader>(new HotelLoader(builder.Configuration["DATA:HOTELS"]));
var app = builder.Build();

app.MapGrpcService<ProfileService>();

app.Run();