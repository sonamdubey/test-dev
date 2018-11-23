using AutoMapper;
using Bikewale.DTO.City;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.City
{
    /// <summary>
    /// Modified By :   Sumit Kate on 22 Mar 2016
    /// Description :   Added Mapper for IEnumerable of CityEntityBase
    /// </summary>
    public class CityListMapper
    {
        internal static IEnumerable<DTO.City.CityBase> Convert(List<Entities.Location.CityEntityBase> objCityList)
        {
            return Mapper.Map<List<CityEntityBase>, List<CityBase>>(objCityList);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 22 Mar 2016
        /// Converts Entity IEnumerable to DTO IEnumerable
        /// </summary>
        /// <param name="objCityList"></param>
        /// <returns></returns>
        internal static IEnumerable<DTO.City.CityBase> Convert(IEnumerable<Entities.Location.CityEntityBase> objCityList)
        {
            return Mapper.Map<IEnumerable<CityEntityBase>, IEnumerable<CityBase>>(objCityList);
        }

		internal static IEnumerable<CityFinance> MapCity(IEnumerable<CityEntityBase> objCityList)
		{
			return Mapper.Map<IEnumerable<CityEntityBase>, IEnumerable<CityFinance>>(objCityList);
		}
    }
}