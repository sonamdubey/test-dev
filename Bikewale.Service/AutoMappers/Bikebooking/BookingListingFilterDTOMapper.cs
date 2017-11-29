using AutoMapper;

namespace Bikewale.Service.AutoMappers.Bikebooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Feb 2016
    /// Description :   BookingListingFilterDTO Mapper
    /// </summary>
    public class BookingListingFilterDTOMapper
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 05 Feb 2016
        /// Description :   Converts DTO to entity
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal static Entities.BikeBooking.BookingListingFilterEntity Convert(DTO.BikeBooking.BookingListingFilterDTO filter)
        {
            Mapper.CreateMap<DTO.BikeBooking.BookingListingFilterDTO, Entities.BikeBooking.BookingListingFilterEntity>();
            return Mapper.Map<DTO.BikeBooking.BookingListingFilterDTO, Entities.BikeBooking.BookingListingFilterEntity>(filter);
        }
    }
}