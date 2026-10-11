using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tinkoff.InvestApi.V1;

namespace InvestmentApp.Providers
{
    public class BondProvider(InstrumentsService.InstrumentsServiceClient client, Metadata metadata)
    {
        public async Task<BondsResponse>GetBonds()
        {
            var request = new InstrumentsRequest { InstrumentStatus = InstrumentStatus.All };
            var response = await client.BondsAsync(request, metadata);
            return response;
        }

        public async Task<BondResponse> GetBondByUidAsync(string bondUid)
        {
            var request = new InstrumentRequest() { Id = bondUid, IdType = InstrumentIdType.Uid };
            var response = await client.BondByAsync(request, metadata);
            return response;
        }
        public async Task<GetBondCouponsResponse> GetBondCoupons(string bondUid, DateTime from, DateTime to)
        {
            var request = new GetBondCouponsRequest() { 
                InstrumentId = bondUid, 
                From = Timestamp.FromDateTimeOffset(from), 
                To = Timestamp.FromDateTimeOffset(to)
            };
            var response = await client.GetBondCouponsAsync(request, metadata);
            return response;
        }
    }
}