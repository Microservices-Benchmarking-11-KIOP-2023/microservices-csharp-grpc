using Grpc.Core;
using Pb.Geo.Service.Models;

namespace Pb.Geo.Service.Services;

public class GeoService : Geo.GeoBase
{
    private readonly ILogger<GeoService> _log;
    private readonly IPointLoader _pointLoader;
    private const int MaxSearchRadius = 10;
    private const int MaxSearchResults = Int32.MaxValue;
    private const double EarthRadius = 6371;

    public GeoService(ILogger<GeoService> log, IPointLoader pointLoader)
    {
        _log = log;
        _pointLoader = pointLoader;
    }

    public override Task<GeoResult> Nearby(GeoRequest request, ServerCallContext context)
    {
        var center = new Point
        {
            Lat = request.Lat,
            Lon = request.Lon
        };
        
        var kNearestNeighbours = GetNeighbors(center, _pointLoader.Points);
        
        return Task.FromResult(new GeoResult()
        {
            HotelIds = { kNearestNeighbours.Select(x => x.HotelId) }
        });
    }
    
    //TODO: Validate if returns proper values.
    //Source: https://stackoverflow.com/questions/65256053/find-k-nearest-neighbor-in-c-sharp
    private IEnumerable<Point> GetNeighbors(Point point, params Point[] points)
    {
        return points
            .Select(p => new { Point = p, Distance = CalculateDistanceBetweenPoints(point, p) })
            .Where(pointAndDistance => pointAndDistance.Distance <= Math.Pow(MaxSearchRadius, 2))
            .OrderBy(pointAndDistance => pointAndDistance.Distance)
            .Take(MaxSearchResults)
            .Select(pointAndDistance => pointAndDistance.Point);
    }
    
    private double CalculateDistanceBetweenPoints(Point originPoint, Point destinationPoint)
    {
        var dlat = ToRadians(originPoint.Lat - destinationPoint.Lat);
        var dlon = ToRadians(originPoint.Lon - originPoint.Lon);

        var a = Math.Pow(Math.Sin(dlat / 2), 2) +
                Math.Cos(originPoint.Lat) * Math.Cos(destinationPoint.Lat) *
                Math.Pow(Math.Sin(dlon / 2), 2);
        var c = 2 * Math.Asin(Math.Sqrt(a));

        return (c * EarthRadius);
    }

    static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}