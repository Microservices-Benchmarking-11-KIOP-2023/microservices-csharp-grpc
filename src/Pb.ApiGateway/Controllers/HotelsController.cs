using Microsoft.AspNetCore.Mvc;
using Pb.ApiGateway.Models;
using Pb.ApiGateway.Providers;

namespace Pb.ApiGateway.Controllers;

public class HotelsController : Controller
{
    private readonly ILogger<HotelsController> _log;
    private readonly IHotelProvider _hotelProvider;
    
    public HotelsController(ILogger<HotelsController> log, IHotelProvider hotelProvider)
    {
        _log = log;
        _hotelProvider = hotelProvider;
    }
    
    [HttpGet]
    [Route("/hotels")]
    public async Task<GeoJsonResponse?> FetchHotels([FromQuery] HotelParameters hotelParameters)
    {
        return await _hotelProvider.FetchHotels(hotelParameters);
    }
}