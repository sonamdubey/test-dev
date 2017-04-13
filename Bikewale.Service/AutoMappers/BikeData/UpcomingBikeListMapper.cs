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
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeDescriptionEntity, BikeDiscription>();
            Mapper.CreateMap<UpcomingBikeEntity, UpcomingBike>();

            return Mapper.Map<IEnumerable<UpcomingBikeEntity>, List<UpcomingBike>>(objUpcoming);
        }

        internal static UpcomingBikesListInputEntity Convert(InputFilterDTO objFilter)
        {
            Mapper.CreateMap<InputFilterDTO, UpcomingBikesListInputEntity>();
            return Mapper.Map<InputFilterDTO, UpcomingBikesListInputEntity>(objFilter);
        }

        internal static UpcomingBikeResultDTO Convert(UpcomingBikeResult objResult)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<UpcomingBikeEntity, UpcomingBikeDTOBase>();
            Mapper.CreateMap<UpcomingBikesListInputEntity, InputFilterDTO>();
            Mapper.CreateMap<UpcomingBikeResult, UpcomingBikeResultDTO>();
            return Mapper.Map<UpcomingBikeResult, UpcomingBikeResultDTO>(objResult);
        }
    }
}