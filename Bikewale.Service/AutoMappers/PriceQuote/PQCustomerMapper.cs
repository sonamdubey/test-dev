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
            Mapper.CreateMap<CustomerEntityBase, PQCustomerBase>();
            Mapper.CreateMap<VersionColor, PQColor>();
            Mapper.CreateMap<PQCustomerDetail, PQCustomer>();
            Mapper.CreateMap<CityEntityBase, CityBase>();
            Mapper.CreateMap<Bikewale.Entities.Location.AreaEntityBase, AreaBase>();
            return Mapper.Map<PQCustomerDetail, PQCustomer>(entity);
        }

        /// <summary>
        /// Created by : Snehal Dange on 2nd May 2018
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static Bikewale.Entities.PriceQuote.PQCustomerDetailInput Convert(Bikewale.DTO.PriceQuote.PQCustomerDetailInput entity)
        {
            Mapper.CreateMap<Bikewale.DTO.PriceQuote.PQCustomerDetailInput, Bikewale.Entities.PriceQuote.PQCustomerDetailInput>();
            return Mapper.Map<Bikewale.DTO.PriceQuote.PQCustomerDetailInput, Bikewale.Entities.PriceQuote.PQCustomerDetailInput>(entity);
        }

    }
}