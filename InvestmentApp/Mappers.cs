using Google.Protobuf.WellKnownTypes;
using Tinkoff.InvestApi.V1;

namespace InvestmentApp
{
    public static class Mappers
    {
        public static Decimal MoneyValueToDeciaml(MoneyValue value) => value.Units + value.Nano / 1000000000m;
        public static DateTime GrpcTimestampToDateTime(Timestamp? ts) => ts?.ToDateTime() ?? new DateTime();
        public static Decimal QuotationToDecimal(Quotation? value) => value.Units + value.Nano / 1000000000m;

    }
}
