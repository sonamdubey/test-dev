using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}