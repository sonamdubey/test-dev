using AutoMapper;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Location;

namespace Bikewale.Service.AutoMappers.BookingSummary
{
    public class BookingSummaryMapper
    {
        internal static DTO.PriceQuote.CustomerDetails.PQCustomer Convert(Entities.BikeBooking.PQCustomerDetail objCustomer)
        {
            return Mapper.Map<PQCustomerDetail, PQCustomer>(objCustomer);
        }

        internal static DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase Convert(Entities.BikeBooking.PQ_DealerDetailEntity dealerDetailEntity)
        {
            return Mapper.Map<PQ_DealerDetailEntity, DDQDealerDetailBase>(dealerDetailEntity);
        }
    }
}