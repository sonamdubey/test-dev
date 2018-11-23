using AutoMapper;
using Bikewale.DTO.Area;
using Bikewale.DTO.City;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Location;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class PQCustomerMapper
    {
        internal static DTO.PriceQuote.CustomerDetails.PQCustomer Convert(Entities.BikeBooking.PQCustomerDetail entity)
        {
           
            return Mapper.Map<PQCustomerDetail, PQCustomer>(entity);
        }

        /// <summary>
        /// Created by : Snehal Dange on 2nd May 2018
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static Bikewale.Entities.PriceQuote.PQCustomerDetailInput Convert(Bikewale.DTO.PriceQuote.PQCustomerDetailInput entity)
        {
           
            return Mapper.Map<Bikewale.DTO.PriceQuote.PQCustomerDetailInput, Bikewale.Entities.PriceQuote.PQCustomerDetailInput>(entity);
        }

        internal static Bikewale.Entities.PriceQuote.v2.PQCustomerDetailInput Convert(Bikewale.DTO.PriceQuote.v3.PQCustomerDetailInput entity)
        {
           
            return Mapper.Map<Bikewale.DTO.PriceQuote.v3.PQCustomerDetailInput, Bikewale.Entities.PriceQuote.v2.PQCustomerDetailInput>(entity);
        }

        /// <summary>
        /// Modified by : Sanskar Gupta on 08 May 2018
        /// Description : Added mapping for `DealerDetailsDTO` to `DealerDetails` entity.
        /// </summary>
        /// <param name="outEntity"></param>
        /// <returns></returns>
        internal static DTO.PriceQuote.v2.PQCustomerDetailOutput Convertv2(Entities.PriceQuote.PQCustomerDetailOutputEntity outEntity)
        {
         
            return Mapper.Map<Entities.PriceQuote.PQCustomerDetailOutputEntity, DTO.PriceQuote.v2.PQCustomerDetailOutput>(outEntity);
        }

        internal static DTO.PriceQuote.v4.PQCustomerDetailOutput Convertv3(Entities.PriceQuote.v2.PQCustomerDetailOutputEntity outEntity)
        {
           
            return Mapper.Map<Entities.PriceQuote.v2.PQCustomerDetailOutputEntity, DTO.PriceQuote.v4.PQCustomerDetailOutput>(outEntity);
        }

        /// <summary>
        /// Modified by : Sanskar Gupta on 08 May 2018
        /// Description : Added mapping for `DealerDetailsDTO` to `DealerDetails` entity.
        /// </summary>
        internal static DTO.PriceQuote.PQCustomerDetailOutput Convert(Entities.PriceQuote.PQCustomerDetailOutputEntity outEntity)
        {
            return Mapper.Map<Entities.PriceQuote.PQCustomerDetailOutputEntity, DTO.PriceQuote.PQCustomerDetailOutput>(outEntity);
        }

        internal static DTO.PriceQuote.v3.PQCustomerDetailOutput Convert(Entities.PriceQuote.v2.PQCustomerDetailOutputEntity outEntity)
        {
            return Mapper.Map<Entities.PriceQuote.v2.PQCustomerDetailOutputEntity, DTO.PriceQuote.v3.PQCustomerDetailOutput>(outEntity);
        }

        internal static Entities.PriceQuote.PQCustomerDetailInput Convert(DTO.PriceQuote.v2.PQCustomerDetailInput input)
        {
            return Mapper.Map<DTO.PriceQuote.v2.PQCustomerDetailInput, Entities.PriceQuote.PQCustomerDetailInput>(input);
        }

        internal static Entities.PriceQuote.v2.PQCustomerDetailInput Convertv2(DTO.PriceQuote.v2.PQCustomerDetailInput input)
        {
            return Mapper.Map<DTO.PriceQuote.v2.PQCustomerDetailInput, Entities.PriceQuote.v2.PQCustomerDetailInput>(input);
        }

    }
}