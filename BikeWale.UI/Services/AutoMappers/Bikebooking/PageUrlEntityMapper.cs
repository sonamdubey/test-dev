using AutoMapper;

namespace Bikewale.Service.AutoMappers.Bikebooking
{
    /// <summary>
    /// 
    /// </summary>
    public class PageUrlEntityMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageUrlEntity"></param>
        /// <returns></returns>
        internal static DTO.BikeBooking.PagingUrl Convert(Entities.BikeBooking.PagingUrl PageUrlEntity)
        {

            return Mapper.Map<Entities.BikeBooking.PagingUrl, DTO.BikeBooking.PagingUrl>(PageUrlEntity);
        }
    }
}