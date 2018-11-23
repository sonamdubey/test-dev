using AutoMapper;
using Bikewale.DTO.City;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.ServiceCenter
{
    /// <summary>
    /// Created By:- Subodh Jain 09 Nov 2016
    /// Summary:- For mapping Service Center Locator Dto and Enities
    /// </summary>
    public class ServiceCenterMapper
    {
        internal static IEnumerable<CityBase> Convert(IEnumerable<CityEntityBase> objCityList)
        {
           return Mapper.Map<IEnumerable<CityEntityBase>, IEnumerable<CityBase>>(objCityList);
        }
    }
}