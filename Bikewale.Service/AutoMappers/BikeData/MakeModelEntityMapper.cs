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
    public class MakeModelEntityMapper
    {
        public static List<BikeMakeModel> Convert(List<BikeMakeModelEntity> entity)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeMakeModelEntity, BikeMakeModel>();
            return Mapper.Map<List<BikeMakeModelEntity>, List<BikeMakeModel>>(entity);
        }
    }
}