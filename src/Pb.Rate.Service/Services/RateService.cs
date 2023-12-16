using Grpc.Core;
using Pb.Rate.Service.Loaders;
using Rate;

namespace Pb.Rate.Service.Services
{
    public class RateService : global::Rate.Rate.RateBase
    {
        private readonly IDictionary<(string, string, string), List<RatePlan>> _rateTable;

        public RateService(IRatePlansLoader ratePlansLoader)
        {
            _rateTable = InitializeRateTable(ratePlansLoader.RateTable);
        }

        public override async Task<RateResult> GetRates(RateRequest request, ServerCallContext context)
        {
            var result = new RateResult();
            foreach (var hotelId in request.HotelIds)
            {
                var stay = (hotelId, request.InDate, request.OutDate);
            
                if (_rateTable.TryGetValue(stay, out var ratePlans))
                    result.RatePlans.AddRange(ratePlans);
            }
            
            return result; 
        }

        private IDictionary<(string, string, string), List<RatePlan>> InitializeRateTable(IEnumerable<RatePlan> ratePlans)
        {
            var rateTable = new Dictionary<(string, string, string), List<RatePlan>>();

            foreach (var ratePlan in ratePlans)
            {
                var stay = (ratePlan.HotelId, ratePlan.InDate, ratePlan.OutDate);

                if (rateTable.TryGetValue(stay, out var existingRatePlans))
                    existingRatePlans.Add(ratePlan);
                else
                    rateTable[stay] = new List<RatePlan> { ratePlan };
            }

            return rateTable;
        }
    }
}