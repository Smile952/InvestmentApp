using Tinkoff.InvestApi.V1;

namespace InvestmentApp.Models
{
    public class BondAnalyzerModel
    {
        public string Figi { get; set; } = string.Empty;

        public string Uid { get; set; } = string.Empty;
        public string Tiker { get; set; } = string.Empty;
        public string Isin { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public Decimal Nominal { get; set; }
        public Decimal InitNominal { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int Lot { get; set; }

        public long IssueSize { get; set; }
        public long IssueSizePlan { get; set; }
        public string IssueKind { get; set; } = string.Empty;

        public string Sector { get; set; } = string.Empty;
        public string CountryOfRisk { get; set; } = string.Empty;
        public DateTime? MaturityDate { get; set; }

        public DateTime? CallDate { get; set; }
        public int CouponQuantityPerYear { get; set; }
        public Decimal AciValue { get; set; }
        public bool AmortizationFlag { get; set; }
        public bool FloatingCouponFlag { get; set; }
        public Decimal PlacementPrice { get; set; }
        public Quotation MinPriceIncrement { get; set; }

        public bool SubardinateFlag { get; set; }
        public BondType Type { get; set; }

        public RealExchange RealExchange { get; set; }
        public string Exchange { get; set; } = string.Empty;
        public Decimal? LastPrice { get; set; }
        public static BondAnalyzerModel FromBondResponse(Bond bond)
        {
            return new BondAnalyzerModel
            {
                Figi = bond.Figi,
                Uid = bond.Uid,
                Tiker = bond.Ticker,
                Isin = bond.Isin,
                Name = bond.Name,
                Nominal = Mappers.MoneyValueToDeciaml(bond.Nominal),
                InitNominal = Mappers.MoneyValueToDeciaml(bond.InitialNominal),
                Currency = bond.Currency,
                Lot = bond.Lot,
                IssueSize = bond.IssueSize,
                IssueSizePlan = bond.IssueSizePlan,
                IssueKind = bond.IssueKind,
                Sector = bond.Sector,
                CountryOfRisk = bond.CountryOfRisk,
                MaturityDate = bond.MaturityDate?.ToDateTime(),
                CallDate = bond.CallDate?.ToDateTime(),
                CouponQuantityPerYear = bond.CouponQuantityPerYear,
                AciValue = Mappers.MoneyValueToDeciaml(bond.AciValue),
                AmortizationFlag = bond.AmortizationFlag,
                FloatingCouponFlag = bond.FloatingCouponFlag,
                PlacementPrice = Mappers.MoneyValueToDeciaml(bond.PlacementPrice),
                MinPriceIncrement = bond.MinPriceIncrement,
                SubardinateFlag = bond.SubordinatedFlag,
                Type = bond.BondType,
                RealExchange = bond.RealExchange,
                Exchange = bond.Exchange
            };
        }
    }
}
