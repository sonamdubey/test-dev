using AutoMapper;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class DDQDealerDetailBaseMapper
    {
        internal static DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase Convert(Entities.BikeBooking.PQ_DealerDetailEntity dealerDetailEntity)
        {
            return Mapper.Map<PQ_DealerDetailEntity, DDQDealerDetailBase>(dealerDetailEntity);
        }
    }
}