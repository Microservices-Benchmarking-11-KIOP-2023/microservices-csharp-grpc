using Grpc.Core;
using Pb.Geo.Service.Models;

namespace Pb.Geo.Service.Services;

public class GeoService : Geo.GeoBase
{
    private readonly IEnumerable<Point> _points;
    private const int MaxSearchRadius = 10;
    private const double EarthRadius = 6371;
    private const double RadianConst = Math.PI / 180;

    public GeoService(IPointLoader pointLoader)
    {
        _points = pointLoader.Points;
    }

    public override Task<GeoResult> Nearby(GeoRequest request, ServerCallContext context)
    {
        var center = new Point
        {
            Lat = request.Lat,
            Lon = request.Lon
        };
        
        var kNearestNeighbours = GetNeighbors(center, _points);

        return Task.FromResult(new GeoResult()
        {
            HotelIds = { kNearestNeighbours.Select(x => x.HotelId) }
        });
    }
    

    private IEnumerable<Point> GetNeighbors(Point point, IEnumerable<Point> points)
    {
        return points
            .Select(p => new { Point = p, Distance = CalculateDistanceBetweenPoints(point, p) })
            .Where(pointAndDistance => pointAndDistance.Distance <= Math.Pow(MaxSearchRadius, 2))
            .OrderBy(pointAndDistance => pointAndDistance.Distance)
            .Select(pointAndDistance => pointAndDistance.Point);
    }
    
    private double CalculateDistanceBetweenPoints(Point originPoint, Point destinationPoint)
    {
        var distanceLat = ToRadians(originPoint.Lat - destinationPoint.Lat);
        var distanceLon = ToRadians(originPoint.Lon - originPoint.Lon);

        var inverseHaversine = Math.Pow(Math.Sin(distanceLat / 2), 2) +
                Math.Cos(originPoint.Lat) * Math.Cos(destinationPoint.Lat) *
                Math.Pow(Math.Sin(distanceLon / 2), 2);
        var centralAngle = 2 * Math.Asin(Math.Sqrt(inverseHaversine));

        return centralAngle * EarthRadius;
    }

    static double ToRadians(double degrees)
    {
        return degrees * RadianConst;
    }
}