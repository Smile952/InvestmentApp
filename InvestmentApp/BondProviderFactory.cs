using Grpc.Core;
using InvestmentApp.Providers;
using Tinkoff.InvestApi.V1;

namespace InvestmentApp
{
    public class BondProviderFactory
    {

        public int RequestCount { get; set; }
        public int MillisecondsDelay { get; set; }
        public BondProvider GetBondProvider(InstrumentsService.InstrumentsServiceClient client, Metadata metadata)
        {
            return new BondProvider(client, metadata);
        }

    }
}
