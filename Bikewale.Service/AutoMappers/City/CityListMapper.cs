using AutoMapper;
using Bikewale.DTO.City;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.City
{
    public class CityListMapper
    {
        internal static IEnumerable<DTO.City.CityBase> Convert(List<Entities.Location.CityEntityBase> objCityList)
        {
            Mapper.CreateMap<CityEntityBase, CityBase>();
            return Mapper.Map<List<CityEntityBase>, List<CityBase>>(objCityList);
        }
    }
}