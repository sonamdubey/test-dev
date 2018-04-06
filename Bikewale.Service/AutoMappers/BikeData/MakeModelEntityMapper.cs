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
            if (objModelList != null)
            {
                Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
                Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
                IEnumerable<MostPopularBikes> objModelListDto = Mapper.Map<IEnumerable<MostPopularBikesBase>, IEnumerable<MostPopularBikes>>(objModelList);
                if (objModelListDto != null)
                {
                    SpecsFeaturesMapper.ConvertToMostPopularBikes(objModelListDto, objModelList);
                }
                return objModelListDto;
            }
            return null;
        }
    }
}