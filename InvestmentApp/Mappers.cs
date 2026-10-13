using Google.Protobuf.WellKnownTypes;
using Tinkoff.InvestApi.V1;

namespace InvestmentApp
{
    public static class Mappers
    {
        public static Decimal MoneyValueToDeciaml(MoneyValue value) => value.Units + value.Nano / 1000000000m;
        public static DateTime GrpcTimestampToDateTime(Timestamp? ts) => ts?.ToDateTime() ?? new DateTime();
        public static Decimal QuotationToDecimal(Quotation? value) => value.Units + value.Nano / 1000000000m;
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

    }
}
