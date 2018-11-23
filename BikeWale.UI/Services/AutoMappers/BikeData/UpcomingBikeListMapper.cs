using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.BikeData.Upcoming;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.BikeData
{
    public class UpcomingBikeListMapper
    {
        internal static IEnumerable<DTO.BikeData.UpcomingBike> Convert(IEnumerable<Entities.BikeData.UpcomingBikeEntity> objUpcoming)
        {
            return Mapper.Map<IEnumerable<UpcomingBikeEntity>, List<UpcomingBike>>(objUpcoming);
        }

        internal static UpcomingBikesListInputEntity Convert(InputFilterDTO objFilter)
        {
            return Mapper.Map<InputFilterDTO, UpcomingBikesListInputEntity>(objFilter);
        }

        internal static UpcomingBikeResultDTO Convert(UpcomingBikeResult objResult)
        {
            return Mapper.Map<UpcomingBikeResult, UpcomingBikeResultDTO>(objResult);
        }
    }
}