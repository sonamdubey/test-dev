using AutoMapper;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class DPQuotationOutputMapper
    {
        internal static DTO.PriceQuote.DealerPriceQuote.DPQuotationOutput Convert(Entities.BikeBooking.PQ_QuotationEntity objPrice)
        {
          return Mapper.Map<PQ_QuotationEntity, DPQuotationOutput>(objPrice);
        }
    }
}