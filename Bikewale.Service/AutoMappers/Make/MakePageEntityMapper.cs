﻿using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;

namespace Bikewale.Service.AutoMappers.Make
{
    public class MakePageEntityMapper
    {
        public static MakePage Convert(BikeMakePageEntity entity)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
            Mapper.CreateMap<BikeDescriptionEntity,BikeDescription>();
            Mapper.CreateMap<BikeMakePageEntity, MakePage>();
            Mapper.CreateMap<MinSpecsEntity, MinSpecs>();
            return Mapper.Map<BikeMakePageEntity, MakePage>(entity);
        }
    }
}