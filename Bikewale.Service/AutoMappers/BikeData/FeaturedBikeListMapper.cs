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
    public class FeaturedBikeListMapper
    {
        internal static IEnumerable<DTO.BikeData.FeaturedBike> Convert(List<Entities.BikeData.FeaturedBikeEntity> objFeature)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<FeaturedBikeEntity, FeaturedBike>();

            return Mapper.Map<List<FeaturedBikeEntity>, List<FeaturedBike>>(objFeature);
        }
    }
}