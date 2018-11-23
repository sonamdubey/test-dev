using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.BikeData
{
    public class FeaturedBikeListMapper
    {
        internal static IEnumerable<DTO.BikeData.FeaturedBike> Convert(List<Entities.BikeData.FeaturedBikeEntity> objFeature)
        {

            return Mapper.Map<List<FeaturedBikeEntity>, List<FeaturedBike>>(objFeature);
        }
    }
}