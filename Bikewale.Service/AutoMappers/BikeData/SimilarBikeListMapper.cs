using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.BikeData
{
    public class SimilarBikeListMapper
    {
        internal static IEnumerable<DTO.BikeData.SimilarBike> Convert(IEnumerable<Entities.BikeData.SimilarBikeEntity> objSimilarBikes)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
            Mapper.CreateMap<SimilarBikeEntity, SimilarBike>();

            return Mapper.Map<IEnumerable<SimilarBikeEntity>, IEnumerable<SimilarBike>>(objSimilarBikes);
        }
    }
}