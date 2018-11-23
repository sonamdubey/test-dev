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
        
            return Mapper.Map<List<BikeMakeModelEntity>, List<BikeMakeModel>>(entity);
        }

        public static IEnumerable<MakeModelList> Convert(IEnumerable<MakeModelListEntity> entity)
        {
           
            return Mapper.Map<IEnumerable<MakeModelListEntity>, IEnumerable<MakeModelList>>(entity);
        }
        internal static IEnumerable<MakeModelBase> Convert(IEnumerable<BikeMakeModelBase> enumerable)
        {
            if (enumerable != null)
            {
              
                return Mapper.Map<IEnumerable<BikeMakeModelBase>, IEnumerable<MakeModelBase>>(enumerable);
            }
            return null;
        }

        public static IEnumerable<ModelSpecificationsDTO> Convert(IEnumerable<BikeModelVersionsDetails> entity)
        {
           
            return Mapper.Map<IEnumerable<BikeModelVersionsDetails>, IEnumerable<ModelSpecificationsDTO>>(entity);
        }

        public static IEnumerable<MostPopularBikes> Convert(IEnumerable<MostPopularBikesBase> objModelList)
        {
            if (objModelList != null)
            {
                
                IEnumerable<MostPopularBikes> objModelListDto = Mapper.Map<IEnumerable<MostPopularBikesBase>, IEnumerable<MostPopularBikes>>(objModelList);
                if (objModelListDto != null)
                {
                    SpecsFeaturesMapper.ConvertToMostPopularBikes(objModelListDto, objModelList);
                }
                return objModelListDto;
            }
            return null;
        }

        public static IEnumerable<MostPopularBikes> ConvertWithoutMinSpec(IEnumerable<MostPopularBikesBase> objModelList)
        {
            if (objModelList != null)
            {
               
                return Mapper.Map<IEnumerable<MostPopularBikesBase>, IEnumerable<MostPopularBikes>>(objModelList);
            }
            return null;
        }
    }
}