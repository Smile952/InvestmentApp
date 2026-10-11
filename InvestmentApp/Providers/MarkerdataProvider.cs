using Grpc.Core;
using Tinkoff.InvestApi.V1;

namespace InvestmentApp.Providers
{
    public class MarkerdataProvider(MarketDataService.MarketDataServiceClient client, Metadata metadata)
    {

        public async Task<GetLastPricesResponse> GetLastPricesAsync()
        {
            var request = new GetLastPricesRequest();
            var response = await client.GetLastPricesAsync(request, metadata);
            return response;
        }

    }
}
