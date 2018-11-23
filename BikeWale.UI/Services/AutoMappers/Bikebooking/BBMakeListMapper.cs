using AutoMapper;
using Bikewale.DTO.BikeBooking.Make;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Bikebooking
{
    public class BBMakeListMapper
    {
        /// <summary>
        /// Created By : Vivek Gupta on 04-07-2016
        /// Desc : mapper to map make list in bike booking
        /// </summary>
        /// <param name="lstCity"></param>
        /// <returns></returns>
        internal static IEnumerable<BBMakeBase> Convert(List<BikeMakeEntityBase> lstMake)
        {
           return Mapper.Map<List<BikeMakeEntityBase>, IEnumerable<BBMakeBase>>(lstMake);
        }
    }
}