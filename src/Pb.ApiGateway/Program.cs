using Pb.ApiGateway.Providers;
using Pb.ApiGateway.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.SetupGrpcServices(builder.Configuration);
builder.Services.AddSingleton<IHotelProvider, HotelProvider>();

var app = builder.Build();
app.MapControllers();

app.Run();
