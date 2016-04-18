using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Model.v3;
using Bikewale.DTO.Series;
using Bikewale.DTO.Version;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Model
{
    /// <summary>
    /// Modified By : Lucky Rathore on 15 Apr 2016
    /// Description : Add BikeSpecs ConvertToBikeSpecs(BikeModelPageEntity objModelPage).
    /// </summary>
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

        internal static Bikewale.DTO.Model.ModelPage Convert(BikeModelPageEntity objModelPage)
        {
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<BikeDescriptionEntity, ModelDescription>();
            Mapper.CreateMap<BikeModelEntity, ModelDetails>();
            Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
            Mapper.CreateMap<BikeVersionsListEntity, ModelVersionList>();
            Mapper.CreateMap<BikeVersionMinSpecs, VersionMinSpecs>();
            Mapper.CreateMap<BikeModelPageEntity, Bikewale.DTO.Model.ModelPage>();
            Mapper.CreateMap<NewBikeModelColor, ModelColor>();
            Mapper.CreateMap<BikeDescriptionEntity, BikeDiscription>();
            Mapper.CreateMap<UpcomingBikeEntity, UpcomingBike>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Overview, Bikewale.DTO.Model.Overview>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Features, Bikewale.DTO.Model.Features>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Specifications, Bikewale.DTO.Model.Specifications>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.Specs, Bikewale.DTO.Model.Specs>();
            Mapper.CreateMap<Bikewale.Entities.BikeData.SpecsCategory, Bikewale.DTO.Model.SpecsCategory>();
            Mapper.CreateMap<Bikewale.Entities.CMS.Photos.ModelImage, Bikewale.DTO.CMS.Photos.CMSModelImageBase>();


            return Mapper.Map<BikeModelPageEntity, Bikewale.DTO.Model.ModelPage>(objModelPage);
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
            Mapper.CreateMap<MinSpecsEntity, MinSpecs>();
            Mapper.CreateMap<MostPopularBikesBase, MostPopularBikes>();
            return Mapper.Map<List<MostPopularBikesBase>, List<MostPopularBikes>>(objModelList);

        }

        /// <summary>
        /// Created By : Lucky Rathore on 15 Apr 2016
        /// Description : Mapper for BikeSpecs DTO and BikeModelPageEntity Entity
        /// </summary>
        /// <param name="objModelPage">object of BikeModelPageEntity</param>
        /// <returns>BikeSpecs DTO</returns>
        internal static BikeSpecs ConvertToBikeSpecs(BikeModelPageEntity objModelPage, Entities.PriceQuote.PQByCityAreaEntity pqEntity)
        {
            try
            {
                Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
                Mapper.CreateMap<BikeDescriptionEntity, ModelDescription>();
                Mapper.CreateMap<BikeModelEntity, ModelDetails>();
                Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
                Mapper.CreateMap<BikeVersionsListEntity, ModelVersionList>();
                Mapper.CreateMap<BikeVersionMinSpecs, Bikewale.DTO.Model.v3.VersionDetail>();
                Mapper.CreateMap<BikeModelPageEntity, BikeSpecs>();
                Mapper.CreateMap<NewBikeModelColor, NewModelColor>();
                Mapper.CreateMap<BikeDescriptionEntity, BikeDiscription>();
                Mapper.CreateMap<UpcomingBikeEntity, UpcomingBike>();
                Mapper.CreateMap<Bikewale.Entities.BikeData.Overview, Bikewale.DTO.Model.Overview>();
                Mapper.CreateMap<Bikewale.Entities.BikeData.Features, Bikewale.DTO.Model.Features>();
                Mapper.CreateMap<Bikewale.Entities.BikeData.Specifications, Bikewale.DTO.Model.v2.Specifications>();
                Mapper.CreateMap<Bikewale.Entities.BikeData.Specs, Bikewale.DTO.Model.Specs>();
                Mapper.CreateMap<Bikewale.Entities.BikeData.SpecsCategory, Bikewale.DTO.Model.v2.SpecsCategory>();
                var bikespecs = Mapper.Map<BikeSpecs>(objModelPage);
                bikespecs.IsAreaExists = pqEntity.IsAreaExists;
                bikespecs.IsExShowroomPrice = pqEntity.IsExShowroomPrice;
                bikespecs.ModelVersions = Convert(pqEntity.VersionList);
                return bikespecs;            
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.AutoMappers.Model.ModelMapper.ConvertToBikeSpecs");
                objErr.SendMail();
                return default(BikeSpecs);
            }
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 15 Apr 2016
        /// Summary:To map Object for V3 model entity and PQ entity
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        internal static DTO.Model.v3.ModelPage ConvertV3(BikeModelPageEntity objModelPage, Entities.PriceQuote.PQByCityAreaEntity pqEntity)
        {
            Bikewale.DTO.Model.v3.ModelPage objDTOModelPage = null;
            try
            {
                objDTOModelPage = new DTO.Model.v3.ModelPage();
                objDTOModelPage.SmallDescription = objModelPage.ModelDesc.SmallDescription;
                objDTOModelPage.MakeId = objModelPage.ModelDetails.MakeBase.MakeId;
                objDTOModelPage.MakeName = objModelPage.ModelDetails.MakeBase.MakeName;
                objDTOModelPage.ModelId = objModelPage.ModelDetails.ModelId;
                objDTOModelPage.ModelName = objModelPage.ModelDetails.ModelName;
                objDTOModelPage.ReviewCount = objModelPage.ModelDetails.ReviewCount;
                objDTOModelPage.ReviewRate = objModelPage.ModelDetails.ReviewRate;
                objDTOModelPage.IsDiscontinued = !objModelPage.ModelDetails.New;

                if (objModelPage.objOverview != null)
                {
                    foreach (var spec in objModelPage.objOverview.OverviewList)
                    {
                        switch (spec.DisplayText)
                        {
                            case "Capacity":
                                objDTOModelPage.Capacity = spec.DisplayValue;
                                break;
                            case "Mileage":
                                objDTOModelPage.Mileage = spec.DisplayValue;
                                break;
                            case "Max power":
                                objDTOModelPage.MaxPower = spec.DisplayValue;
                                break;
                            case "Weight":
                                objDTOModelPage.Weight = spec.DisplayValue;
                                break;
                        }
                    }
                }
                if (objModelPage.Photos != null)
                {
                    var photos = new List<DTO.Model.v3.CMSModelImageBase>();
                    foreach (var photo in objModelPage.Photos)
                    {
                        var addPhoto = new DTO.Model.v3.CMSModelImageBase()
                        {
                            HostUrl = photo.HostUrl,
                            OriginalImgPath = photo.OriginalImgPath
                        };
                        photos.Add(addPhoto);
                    }
                    objDTOModelPage.Photos = photos;
                }
                if (pqEntity != null)
                {
                    objDTOModelPage.IsCityExists = pqEntity.IsCityExists; //soo
                    objDTOModelPage.IsAreaExists = pqEntity.IsAreaExists; //soo
                    objDTOModelPage.IsExShowroomPrice = pqEntity.IsExShowroomPrice; //soo
                    objDTOModelPage.ModelVersions = Convert(pqEntity.VersionList);
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return objDTOModelPage;
        }

        /// <summary>
        /// Created by: Sumit Kate on 15 Apr 2016
        /// Summary: Map List List<VersionDetail> from IEnumerable<BikeVersionMinSpecs> enumerable
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        private static List<VersionDetail> Convert(IEnumerable<BikeVersionMinSpecs> enumerable)
        {
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            Mapper.CreateMap<BikeVersionMinSpecs, VersionDetail>();
            return Mapper.Map<IEnumerable<BikeVersionMinSpecs>, List<VersionDetail>>(enumerable);
        }
    }
}