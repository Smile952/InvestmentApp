using InvestmentApp.Models;
using InvestmentApp.Providers;
using Tinkoff.InvestApi.V1;

namespace InvestmentApp.Services
{
    public class BondAnalyzerService
    {
        public BondProvider provider;
        public BondAnalyzerService(BondProvider provider)
        {
            this.provider = provider;
        }

        public async Task<List<BondAnalyzerModel>> GetBondAnalyzerModels()
        {
            BondsResponse bonds = await provider.GetBonds();
            var analyzerModels =
                bonds.Instruments
                    .Where(x => x.ForQualInvestorFlag == false
                        && ((x.MaturityDate != null
                            && x.MaturityDate.ToDateTime() > DateTime.UtcNow)
                            || x.MaturityDate == null)
                        && (x.RiskLevel == RiskLevel.Moderate || x.RiskLevel == RiskLevel.Low)
                        && x.BlockedTcaFlag == false
                        && x.CouponQuantityPerYear >= 2
                        && x.Nominal.Currency == "rub"
                    )
                    .Select(x => BondAnalyzerModel.FromBondResponse(x))
                    .ToList();
            return analyzerModels;
        } 
        public void InjectLastPrice(ref List<BondAnalyzerModel> analyzerModels, GetLastPricesResponse lastPrices)
        {
            var bondWithPrices = analyzerModels.Where(am => lastPrices.LastPrices.Select(lp => lp.InstrumentUid).Contains(am.Uid));
            Dictionary<string, Decimal> bondPrices =
                bondWithPrices.ToDictionary(bwp => bwp.Uid,
                    bwp => Mappers.QuotationToDecimal(lastPrices.LastPrices
                        .FirstOrDefault(lp => lp.InstrumentUid == bwp.Uid).Price)
            );
            bondPrices = bondPrices.Where(bwp => bwp.Value <= 20000m).ToDictionary();
            analyzerModels = analyzerModels
                .Select(am => 
                {
                    if (bondPrices.ContainsKey(am.Uid)) 
                    { 
                        var percentOfBondPrice = bondPrices[am.Uid];
                        var bondPrice = am.InitNominal * percentOfBondPrice / 100;
                        am.LastPrice = bondPrice;
                    }
                    return am;
                }).Where(am => am.LastPrice != null)
                .ToList();
        }

        public async Task<List<BondModel>> GetFinalSum(List<BondAnalyzerModel> analyzerModels)
        {
            Dictionary<string, GetBondCouponsResponse> coupons = new Dictionary<string, GetBondCouponsResponse>();
            for (int i = 0; i < analyzerModels.Count() / 10; i++)
            {
                var tasks = analyzerModels.Take(new Range(i*10, (i+1)*10)).Select(async am =>
                {
                    coupons[am.Uid] = await provider.GetBondCoupons(am.Uid, DateTime.UtcNow, DateTime.MaxValue);
                });
                await Task.WhenAll(tasks.ToList().Take(new Range(i, (i+1)*10)));
                await Task.Delay(2000);
            }
            Dictionary<string, Decimal> couponSumPerBond = 
                analyzerModels.ToDictionary(
                    x => x.Uid, 
                    x => coupons.ContainsKey(x.Uid) ? coupons[x.Uid].Events
                        .Select(x => Mappers.MoneyValueToDeciaml(x.PayOneBond))
                        .Sum() : 0m
            );
            analyzerModels = analyzerModels.Where(am => coupons.ContainsKey(am.Uid)).ToList();

            List<BondModel> bonds = analyzerModels
                .Select(x => new BondModel { 
                    Uid = x.Uid, 
                    CouponSum = couponSumPerBond[x.Uid], 
                    Name = x.Name, 
                    Price = x.LastPrice ?? 0m,
                    Nominal = x.Nominal,
                    StartDate = DateTime.UtcNow, 
                    EndDate = Mappers.GrpcTimestampToDateTime(coupons[x.Uid].Events.OrderByDescending(x => x.CouponDate)?.FirstOrDefault().CouponDate)
                }).ToList();
            return bonds;
        }
    }
}
