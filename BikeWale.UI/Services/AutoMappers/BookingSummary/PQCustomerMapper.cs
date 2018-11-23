using AutoMapper;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;

namespace Bikewale.Service.AutoMappers.BookingSummary
{
    public class PQCustomerMapper
    {
        internal static DTO.PriceQuote.CustomerDetails.PQCustomer Convert(Entities.BikeBooking.PQCustomerDetail objCustomer)
        {
           return Mapper.Map<PQCustomerDetail, PQCustomer>(objCustomer);
        }
    }
}