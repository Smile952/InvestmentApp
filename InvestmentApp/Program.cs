using Grpc.Core;
using Grpc.Net.Client;
using InvestmentApp;
using InvestmentApp.Models;
using InvestmentApp.Providers;
using InvestmentApp.Services;
using Tinkoff.InvestApi.V1;

using var channel = GrpcChannel.ForAddress(SettingsReader.GetProdAddr(), new GrpcChannelOptions() { MaxReceiveMessageSize = int.MaxValue });
var meta = new Metadata
{
    { "Authorization", SettingsReader.GetProdAuthToken() }
};


TinkofRequestManager manager = new TinkofRequestManager(25, 5000);

var instrumentsClient = new InstrumentsService.InstrumentsServiceClient(channel);
var marketClient = new MarketDataService.MarketDataServiceClient(channel);

BondProvider bondProvider = new BondProvider(instrumentsClient, meta);
MarkerdataProvider markerdataProvider = new MarkerdataProvider(marketClient, meta);

BondAnalyzerService bondAnalyzerService = new BondAnalyzerService(bondProvider);

List<BondAnalyzerModel> analyzerModels = new List<BondAnalyzerModel>();
GetLastPricesResponse lastPrices = new GetLastPricesResponse();

await manager.AppendRequestAsync(async () => analyzerModels = await bondAnalyzerService.GetBondAnalyzerModels());
await manager.AppendRequestAsync(async () => lastPrices = await markerdataProvider.GetLastPricesAsync());
bondAnalyzerService.InjectLastPrice(ref analyzerModels, lastPrices);

Dictionary<string, GetBondCouponsResponse> coupons = new Dictionary<string, GetBondCouponsResponse>();
foreach(var am in analyzerModels)
{
    await manager.AppendRequestAsync(async () =>
    {
        var coupon = await bondProvider.GetBondCoupons(am.Uid, DateTime.UtcNow, DateTime.MaxValue);
        coupons[am.Uid] = coupon;
    });
}
var final = bondAnalyzerService.GetFinalSum(analyzerModels, coupons).DistinctBy(x => x.Name);
var result = 
    final.ToDictionary(
        x => x.Name, 
        x => new { 
            value = x.CouponSum + x.Nominal - x.Price, 
            forTime = new DateTime(x.EndDate.Ticks).ToString("D")
        }).Where(x => x.Value.value > 0).OrderByDescending(x => x.Value.value);
foreach(var r in result)
{
    Console.WriteLine($"{r.Key}: {r.Value.value} to {r.Value.forTime}");
}
