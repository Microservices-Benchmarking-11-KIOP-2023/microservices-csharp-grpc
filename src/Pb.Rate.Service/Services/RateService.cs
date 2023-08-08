using Grpc.Core;
using Pb.Rate.Service.Models;

namespace Pb.Rate.Service.Services;

public class RateService : Rate.RateBase
{
    private readonly ILogger<RateService> _log;
    private readonly IDictionary<Tuple<string, string, string>, List<RatePlan>> _rateTable;

    public RateService(ILogger<RateService> logger, IRatePlansLoader ratePlansLoader)
    {
        _log = logger;
        _rateTable = InitializeRateTable(ratePlansLoader.RateTable);
    }

    public override Task<RateResult> GetRates(RateRequest request, ServerCallContext context)
    {
        var result = new RateResult();

        foreach (var hotelId in request.HotelIds)
        {
            var stay = new Tuple<string, string, string>(
                hotelId,
                request.InDate,
                request.OutDate);

            if (_rateTable.ContainsKey(stay))
            {
                result.RatePlans.AddRange(_rateTable[stay]);
            }
        }

        return Task.FromResult(result);
    }

    private IDictionary<Tuple<string, string, string>, List<RatePlan>> InitializeRateTable(IEnumerable<RatePlan> ratePlans)
    {
        var rateTable = new Dictionary<Tuple<string, string, string>, List<RatePlan>>();

        foreach (var ratePlan in ratePlans)
        {
            var stay = new Tuple<string, string, string>(
                ratePlan.HotelId,
                ratePlan.InDate,
                ratePlan.OutDate);

            if (rateTable.ContainsKey(stay))
            {
                rateTable[stay].Add(ratePlan);
            }
            else
            {
                rateTable.Add(stay, new List<RatePlan>() { ratePlan });
            }
        }

        return rateTable!;
    }
}