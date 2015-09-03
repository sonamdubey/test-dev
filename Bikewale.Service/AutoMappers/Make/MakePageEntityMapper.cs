using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            return Mapper.Map<BikeMakePageEntity, MakePage>(entity);
        }
    }
}