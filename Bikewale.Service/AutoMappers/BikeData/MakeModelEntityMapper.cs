using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.BikeData
{
    public class MakeModelEntityMapper
    {
        public static List<BikeMakeModel> Convert(List<BikeMakeModelEntity> entity)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeMakeModelEntity, BikeMakeModel>();
            return Mapper.Map<List<BikeMakeModelEntity>, List<BikeMakeModel>>(entity);
        }

        public static IEnumerable<MakeModelList> Convert(IEnumerable<MakeModelListEntity> entity)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<MakeModelListEntity, MakeModelList>();
            return Mapper.Map<IEnumerable<MakeModelListEntity>, IEnumerable<MakeModelList>>(entity);
        }
        internal static IEnumerable<MakeModelBase> Convert(IEnumerable<BikeMakeModelBase> enumerable)
        {
            if (enumerable != null)
            {
                Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                Mapper.CreateMap<BikeMakeModelBase, MakeModelBase>();
                return Mapper.Map<IEnumerable<BikeMakeModelBase>, IEnumerable<MakeModelBase>>(enumerable);
            }
            return null;
        }

        public static IEnumerable<ModelSpecificationsDTO> Convert(IEnumerable<BikeModelVersionsDetails> entity)
        {
            Mapper.CreateMap<BikeVersionSegmentDetails, VersionSegmentDTO>();
            Mapper.CreateMap<BikeModelVersionsDetails, ModelSpecificationsDTO>();
            return Mapper.Map<IEnumerable<BikeModelVersionsDetails>, IEnumerable<ModelSpecificationsDTO>>(entity);
        }

        public static IEnumerable<MostPopularBikes> Convert(IEnumerable<MostPopularBikesBase> objModelList)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            Mapper.CreateMap<MinSpecsEntity, MinSpecs>();
            Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
            return Mapper.Map<IEnumerable<MostPopularBikesBase>, IEnumerable<MostPopularBikes>>(objModelList);
        }
    }
}