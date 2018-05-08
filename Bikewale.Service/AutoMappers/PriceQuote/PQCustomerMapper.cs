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




        internal static DTO.PriceQuote.v2.PQCustomerDetailOutput Convertv2(Entities.PriceQuote.PQCustomerDetailOutputEntity outEntity)
        {
            Mapper.CreateMap<Entities.PriceQuote.PQCustomerDetailOutputEntity, DTO.PriceQuote.v2.PQCustomerDetailOutput>();
            return Mapper.Map<Entities.PriceQuote.PQCustomerDetailOutputEntity, DTO.PriceQuote.v2.PQCustomerDetailOutput>(outEntity);
        }

        internal static DTO.PriceQuote.PQCustomerDetailOutput Convert(Entities.PriceQuote.PQCustomerDetailOutputEntity outEntity)
        {
            Mapper.CreateMap<Entities.PriceQuote.PQCustomerDetailOutputEntity, DTO.PriceQuote.PQCustomerDetailOutput>();
            Mapper.CreateMap<Entities.PriceQuote.DealerDetails, DTO.PriceQuote.BikeBooking.DealerDetailsDTO>();
            return Mapper.Map<Entities.PriceQuote.PQCustomerDetailOutputEntity, DTO.PriceQuote.PQCustomerDetailOutput>(outEntity);
        }

        internal static Entities.PriceQuote.PQCustomerDetailInput Convert(DTO.PriceQuote.v2.PQCustomerDetailInput input)
        {
            Mapper.CreateMap<DTO.PriceQuote.v2.PQCustomerDetailInput, Entities.PriceQuote.PQCustomerDetailInput>();
            return Mapper.Map<DTO.PriceQuote.v2.PQCustomerDetailInput, Entities.PriceQuote.PQCustomerDetailInput>(input);
        }
    }
}