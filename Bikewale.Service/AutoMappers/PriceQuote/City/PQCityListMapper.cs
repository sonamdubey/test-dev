using AutoMapper;
using Bikewale.DTO.PriceQuote.City;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.PriceQuote.City
{
    public class PQCityListMapper
    {
        internal static IEnumerable<DTO.PriceQuote.City.PQCityBase> Convert(IEnumerable<Entities.Location.CityEntityBase> objCityList)
        {
            Mapper.CreateMap<CityEntityBase, PQCityBase>();
            return Mapper.Map<IEnumerable<CityEntityBase>, IEnumerable<PQCityBase>>(objCityList);
        }
    }
}