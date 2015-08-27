﻿using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Model
{
    public class ModelEntityToDTO
    {
        internal static DTO.Model.ModelDetails ConvertModelEntity(Entities.BikeData.BikeModelEntity objModel)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntity, ModelDetails>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            return Mapper.Map<BikeModelEntity, ModelDetails>(objModel);
        }

        internal static ModelDescription ConvertModelDescription(BikeDescriptionEntity objModelDesc)
        {
            Mapper.CreateMap<BikeDescriptionEntity, ModelDescription>();
            return Mapper.Map<BikeDescriptionEntity, ModelDescription>(objModelDesc);
        }

        internal static List<DTO.Version.ModelVersionList> ConvertVersionList(List<BikeVersionsListEntity> mvEntityList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, ModelVersionList>();
            return Mapper.Map<List<BikeVersionsListEntity>, List<ModelVersionList>>(mvEntityList);
        }

        internal static DTO.Version.VersionSpecifications ConvertSpecificationEntity(BikeSpecificationEntity objVersionSpecs)
        {
            Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
            return Mapper.Map<BikeSpecificationEntity, VersionSpecifications>(objVersionSpecs);
        }

        internal static ModelPage ConvertModelPage(BikeModelPageEntity objModelPage)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelPageEntity, ModelPage>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<BikeDescriptionEntity, ModelDescription>();
            Mapper.CreateMap<BikeModelEntity, ModelDetails>();
            Mapper.CreateMap<BikeVersionsListEntity, ModelVersionList>();
            Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
            return Mapper.Map<BikeModelPageEntity, ModelPage>(objModelPage);
        }

        internal static ModelBase ConvertModelEntityBase(BikeModelEntityBase objModel)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<BikeModelEntityBase, ModelBase>(objModel);
        }

        internal static IEnumerable<ModelBase> ConvertModelEntityBaseList(List<BikeModelEntityBase> objModelList)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<List<BikeModelEntityBase>, List<ModelBase>>(objModelList);
        }
    }
}