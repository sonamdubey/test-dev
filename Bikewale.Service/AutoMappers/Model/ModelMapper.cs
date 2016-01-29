using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Model
{
    public class ModelMapper
    {
        internal static DTO.Model.ModelDetails Convert(Entities.BikeData.BikeModelEntity objModel)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntity, ModelDetails>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            return Mapper.Map<BikeModelEntity, ModelDetails>(objModel);
        }

        internal static ModelDescription Convert(BikeDescriptionEntity objModelDesc)
        {
            Mapper.CreateMap<BikeDescriptionEntity, ModelDescription>();
            return Mapper.Map<BikeDescriptionEntity, ModelDescription>(objModelDesc);
        }

        internal static List<DTO.Version.ModelVersionList> Convert(List<BikeVersionsListEntity> mvEntityList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, ModelVersionList>();
            return Mapper.Map<List<BikeVersionsListEntity>, List<ModelVersionList>>(mvEntityList);
        }

        internal static DTO.Version.VersionSpecifications Convert(BikeSpecificationEntity objVersionSpecs)
        {
            Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
            return Mapper.Map<BikeSpecificationEntity, VersionSpecifications>(objVersionSpecs);
        }

        internal static ModelPage Convert(BikeModelPageEntity objModelPage)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();            
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<BikeDescriptionEntity, ModelDescription>();
            Mapper.CreateMap<BikeModelEntity, ModelDetails>();            
            Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
            Mapper.CreateMap<BikeVersionsListEntity, ModelVersionList>();
            Mapper.CreateMap<BikeVersionMinSpecs, VersionMinSpecs>();
            Mapper.CreateMap<BikeModelPageEntity, ModelPage>();
            Mapper.CreateMap<NewBikeModelColor,ModelColor>();            
            Mapper.CreateMap<BikeDescriptionEntity, BikeDiscription>();
            Mapper.CreateMap<UpcomingBikeEntity, UpcomingBike>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Overview, Bikewale.DTO.Model.Overview>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Features, Bikewale.DTO.Model.Features>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Specifications, Bikewale.DTO.Model.Specifications>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Specs, Bikewale.DTO.Model.Specs>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.SpecsCategory, Bikewale.DTO.Model.SpecsCategory>();
            Mapper.CreateMap<Bikewale.Entities.CMS.Photos.ModelImage, Bikewale.DTO.CMS.Photos.CMSModelImageBase>();


            return Mapper.Map<BikeModelPageEntity, ModelPage>(objModelPage);
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jan 2016
        /// Description :   Converts the Model Page entity to Version 2 ModelPage DTO
        /// </summary>
        /// <param name="objModelPage">Mode Page Entity</param>
        /// <returns></returns>
        internal static Bikewale.DTO.Model.v2.ModelPage ConvertV2(BikeModelPageEntity objModelPage)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<BikeDescriptionEntity, ModelDescription>();
            Mapper.CreateMap<BikeModelEntity, ModelDetails>();
            Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
            Mapper.CreateMap<BikeVersionsListEntity, ModelVersionList>();
            Mapper.CreateMap<BikeVersionMinSpecs, VersionMinSpecs>();
            Mapper.CreateMap<BikeModelPageEntity, Bikewale.DTO.Model.v2.ModelPage>();            
            Mapper.CreateMap<NewBikeModelColor, NewModelColor>();
            Mapper.CreateMap<BikeDescriptionEntity, BikeDiscription>();
            Mapper.CreateMap<UpcomingBikeEntity, UpcomingBike>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Overview, Bikewale.DTO.Model.Overview>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Features, Bikewale.DTO.Model.Features>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Specifications, Bikewale.DTO.Model.Specifications>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Specs, Bikewale.DTO.Model.Specs>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.SpecsCategory, Bikewale.DTO.Model.SpecsCategory>();
            Mapper.CreateMap<Bikewale.Entities.CMS.Photos.ModelImage, Bikewale.DTO.CMS.Photos.CMSModelImageBase>();
            return Mapper.Map<BikeModelPageEntity, Bikewale.DTO.Model.v2.ModelPage>(objModelPage);
        }

        internal static ModelBase Convert(BikeModelEntityBase objModel)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<BikeModelEntityBase, ModelBase>(objModel);
        }

        internal static IEnumerable<ModelBase> Convert(List<BikeModelEntityBase> objModelList)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            return Mapper.Map<List<BikeModelEntityBase>, List<ModelBase>>(objModelList);
        }

        internal static List<DTO.Widgets.MostPopularBikes> Convert(List<MostPopularBikesBase> objModelList)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            Mapper.CreateMap<MinSpecsEntity,MinSpecs>();
            Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
            return Mapper.Map<List<MostPopularBikesBase>, List<MostPopularBikes>>(objModelList);

        }
    }
}