using AutoMapper;
using Bikewale.DTO.BikeBooking.City;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Bikebooking
{
    /// <summary>
    /// Created By : Vivek Gupta on 04-07-2016
    /// Desc : mapper to map city list in bike booking
    /// </summary>
    public class BBCityListMapper
    {
        /// <summary>
        /// Created By : Vivek Gupta on 04-07-2016
        /// Desc : mapper to map city list in bike booking
        /// </summary>
        /// <param name="lstCity"></param>
        /// <returns></returns>
        internal static IEnumerable<BBCityBase> Convert(List<CityEntityBase> lstCity)
        {
            Mapper.CreateMap<CityEntityBase, BBCityBase>();
            return Mapper.Map<List<CityEntityBase>, IEnumerable<BBCityBase>>(lstCity);
        }
    }
}