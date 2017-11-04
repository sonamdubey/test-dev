using AutoMapper;
using Bikewale.DTO.PriceQuote.City;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.PriceQuote.City
{
    public class PQCityListMapper
    {
        internal static IEnumerable<DTO.PriceQuote.City.PQCityBase> Convert(IEnumerable<Entities.Location.CityEntityBase> objCityList)
        {
            Mapper.CreateMap<CityEntityBase, PQCityBase>();
            return Mapper.Map<IEnumerable<CityEntityBase>, IEnumerable<PQCityBase>>(objCityList);
        }

        internal static IEnumerable<DTO.PriceQuote.City.v2.PQCityBase> ConvertV2(IEnumerable<CityEntityBase> objCityList)
        {
            Mapper.CreateMap<CityEntityBase, Bikewale.DTO.PriceQuote.City.v2.PQCityBase>();
            return Mapper.Map<IEnumerable<CityEntityBase>, IEnumerable<Bikewale.DTO.PriceQuote.City.v2.PQCityBase>>(objCityList);
            
        }
    }
}