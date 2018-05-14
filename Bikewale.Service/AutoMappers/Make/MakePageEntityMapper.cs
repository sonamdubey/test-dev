using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using Bikewale.Service.AutoMappers.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Make
{
    public class MakePageEntityMapper
    {
        public static MakePage Convert(BikeMakePageEntity entity)
        {
            if (entity != null)
            {
                Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
                Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
                Mapper.CreateMap<BikeDescriptionEntity, BikeDescription>();
                Mapper.CreateMap<BikeMakePageEntity, MakePage>();
                MakePage objDto = Mapper.Map<BikeMakePageEntity, MakePage>(entity);
                if (objDto != null && objDto.PopularBikes != null)
                {
                    SpecsFeaturesMapper.ConvertToMostPopularBikes(objDto.PopularBikes, entity.PopularBikes);
                }
                return objDto;
            }
            return null;
        }
    }
}