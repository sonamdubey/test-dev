using AutoMapper;
using Bikewale.DTO.BikeBooking;
using Bikewale.DTO.BikeBooking.Make;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.DTO.BikeBooking.Version;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Bikebooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Feb 2016
    /// Description :   BikeBookingListingEntity Mapper
    /// </summary>
    public class BikeBookingListingEntityMapper
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 05 Feb 2016
        /// Description :   Converts Entity to DTO
        /// </summary>
        /// <param name="lstEntity"></param>
        /// <returns></returns>
        internal static IEnumerable<DTO.BikeBooking.BikeBookingListingDTO> Convert(IEnumerable<Entities.BikeBooking.BikeBookingListingEntity> lstEntity)
        {
           return Mapper.Map<IEnumerable<Entities.BikeBooking.BikeBookingListingEntity>, IEnumerable<DTO.BikeBooking.BikeBookingListingDTO>>(lstEntity);
        }
    }
}