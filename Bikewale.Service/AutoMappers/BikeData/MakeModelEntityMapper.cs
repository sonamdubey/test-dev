﻿using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
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
    }
}