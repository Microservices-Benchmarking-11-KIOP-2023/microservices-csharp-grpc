using Grpc.Core;
using Pb.Rate.Service.Models;

namespace Pb.Rate.Service.Services;

public class RateService : Rate.RateBase
{
    private readonly ILogger<RateService> _logger;
    private readonly RatePlan[] _ratePlans;
    private readonly Dictionary<Stay, List<RatePlan>> _rateTable;

    public RateService(ILogger<RateService> logger, IRatePlansLoader ratePlansLoader)
    {
        _logger = logger;
        _rateTable = InitializeRateTable(ratePlansLoader.RateTable);
    }

    public override Task<RateResult> GetRates(RateRequest request, ServerCallContext context)
    {
        var result = new RateResult();

        foreach (var hotelId in request.HotelIds)
        {
            var stay = new Stay()
            {
                HotelId = hotelId,
                InDate = request.InDate,
                OutDate = request.OutDate
            };

            if (_rateTable.TryGetValue(stay, out var value))
                result.RatePlans.AddRange(value);
        }

        return Task.FromResult(result);
    }

    private Dictionary<Stay, List<RatePlan>> InitializeRateTable(IEnumerable<RatePlan> ratePlans)
    {
        var rateTable = new Dictionary<Stay, List<RatePlan>>();

        foreach (var ratePlan in ratePlans)
        {
            var stay = new Stay()
            {
                HotelId = ratePlan.HotelId,
                InDate = ratePlan.InDate,
                OutDate = ratePlan.OutDate
            };
            
            if (rateTable.TryGetValue(stay, out var value))
            {
                value.Add(ratePlan);
            }
        }

        return rateTable!;
    }
}