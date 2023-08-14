using Pb.Rate.Service.Loaders;
using Pb.Rate.Service.Services;
using Pb.Rate.Service.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.SetupGrpcServices();
builder.Services.AddSingleton<IRatePlansLoader>(new RatePlansLoader(builder.Configuration["DATA:INVENTORY"]));

var app = builder.Build();
app.MapGrpcService<RateService>();

app.Run();