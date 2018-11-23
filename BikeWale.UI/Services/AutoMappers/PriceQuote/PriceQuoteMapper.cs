using AutoMapper;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class PriceQuoteMapper
    {
        internal static DTO.PriceQuote.BikeQuotation.PQBikePriceQuoteOutput Convert(Entities.PriceQuote.BikeQuotationEntity quotation)
        {
            return Mapper.Map<BikeQuotationEntity, PQBikePriceQuoteOutput>(quotation);
        }

        internal static DTO.PriceQuote.DealerPriceQuote.DPQuotationOutput Convert(Entities.BikeBooking.PQ_QuotationEntity objPrice)
        {
            return Mapper.Map<PQ_QuotationEntity, DPQuotationOutput>(objPrice);
        }

        internal static DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase Convert(PQ_DealerDetailEntity dealerDetailEntity)
        {
            return Mapper.Map<PQ_DealerDetailEntity, DDQDealerDetailBase>(dealerDetailEntity);
        }

        internal static DTO.PriceQuote.CustomerDetails.PQCustomer Convert(Entities.BikeBooking.PQCustomerDetail entity)
        {
            return Mapper.Map<PQCustomerDetail, PQCustomer>(entity);
        }

        internal static DTO.PriceQuote.PQOutput Convert(PQOutputEntity objPQOutput)
        {
            return Mapper.Map<PQOutputEntity, Bikewale.DTO.PriceQuote.PQOutput>(objPQOutput);
        }
    }
}