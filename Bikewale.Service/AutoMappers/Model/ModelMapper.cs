using AutoMapper;
using Bikewale.BAL.PriceQuote;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.DealerLocator;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Model.v3;
using Bikewale.DTO.PriceQuote.v2;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.DTO.PriceQuote.Version.v2;
using Bikewale.DTO.Series;
using Bikewale.DTO.Version;
using Bikewale.DTO.Videos;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.DTO;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.Videos;
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.AutoMappers.Model.ModelMapper.ConvertToBikeSpecs");
                objErr.SendMail();
                return default(BikeSpecs);
            }
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 15 Apr 2016
        /// Summary:To map Object for V3 model entity and PQ entity
        /// updated by: Sangram Nandkhile on 05 May 2016 
        /// Summary: Added upcoming section
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
                objDTOModelPage.IsUpcoming = objModelPage.ModelDetails.Futuristic;

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
                    objDTOModelPage.DealerId = pqEntity.DealerId;
                    objDTOModelPage.PQId = pqEntity.PqId;
                }
                // Upcoming section
                if (objModelPage.ModelDetails.Futuristic && objModelPage.UpcomingBike != null && objModelPage.ModelDetails != null)
                {
                    objDTOModelPage.ExpectedLaunchDate = objModelPage.UpcomingBike.ExpectedLaunchDate;
                    objDTOModelPage.ExpectedMinPrice = objModelPage.UpcomingBike.EstimatedPriceMin;
                    objDTOModelPage.ExpectedMaxPrice = objModelPage.UpcomingBike.EstimatedPriceMax;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            return objDTOModelPage;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 17 June 2016
        /// Descritpion : Mapping for V4 version of ModelpageEntity.
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        internal static DTO.Model.v4.ModelPage ConvertV4(BikeModelPageEntity objModelPage, Entities.PriceQuote.PQByCityAreaEntity pqEntity, DetailedDealerQuotationEntity dealers)
        {
            Bikewale.DTO.Model.v4.ModelPage objDTOModelPage = null;
            try
            {
                objDTOModelPage = new DTO.Model.v4.ModelPage();
                objDTOModelPage.SmallDescription = objModelPage.ModelDesc.SmallDescription;
                objDTOModelPage.MakeId = objModelPage.ModelDetails.MakeBase.MakeId;
                objDTOModelPage.MakeName = objModelPage.ModelDetails.MakeBase.MakeName;
                objDTOModelPage.ModelId = objModelPage.ModelDetails.ModelId;
                objDTOModelPage.ModelName = objModelPage.ModelDetails.ModelName;
                objDTOModelPage.ReviewCount = objModelPage.ModelDetails.ReviewCount;
                objDTOModelPage.ReviewRate = objModelPage.ModelDetails.ReviewRate;
                objDTOModelPage.IsUpcoming = objModelPage.ModelDetails.Futuristic;
                if (!objDTOModelPage.IsUpcoming)
                {
                    objDTOModelPage.IsDiscontinued = !objModelPage.ModelDetails.New;
                }

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
                    objDTOModelPage.IsCityExists = pqEntity.IsCityExists; 
                    objDTOModelPage.IsAreaExists = pqEntity.IsAreaExists; 
                    objDTOModelPage.IsExShowroomPrice = pqEntity.IsExShowroomPrice; 
                    objDTOModelPage.ModelVersions = Convert(pqEntity.VersionList);
                    objDTOModelPage.DealerId = pqEntity.DealerId;
                    objDTOModelPage.PQId = pqEntity.PqId;
                }
                // Upcoming section
                if (objModelPage.ModelDetails.Futuristic && objModelPage.UpcomingBike != null && objModelPage.ModelDetails != null)
                {
                    objDTOModelPage.ExpectedLaunchDate = objModelPage.UpcomingBike.ExpectedLaunchDate;
                    objDTOModelPage.ExpectedMinPrice = objModelPage.UpcomingBike.EstimatedPriceMin;
                    objDTOModelPage.ExpectedMaxPrice = objModelPage.UpcomingBike.EstimatedPriceMax;
                }
                if (dealers != null)
                {
                    if (dealers.PrimaryDealer != null )
                    {
                        var dealerOffer = new List<DPQOffer>();
                        foreach (var offer in dealers.PrimaryDealer.OfferList)
                        {
                            var addOffer = new DPQOffer()
                            {
                                Id = (int)offer.OfferId,
                                OfferCategoryId = (int)offer.OfferCategoryId,
                                Text = offer.OfferText
                            };
                            dealerOffer.Add(addOffer);
                        }
                        objDTOModelPage.PrimaryDealerOffers = dealerOffer;
                        if (dealers.PrimaryDealer.DealerDetails != null)
                        {
                            objDTOModelPage.PrimaryDealer = new DealerBase();
                            objDTOModelPage.PrimaryDealer.Name = dealers.PrimaryDealer.DealerDetails.Organization;
                            objDTOModelPage.PrimaryDealer.MaskingNumber = dealers.PrimaryDealer.DealerDetails.MaskingNumber;
                            objDTOModelPage.PrimaryDealer.Area = dealers.PrimaryDealer.DealerDetails.objArea.AreaName;
                            objDTOModelPage.PrimaryDealer.DealerId = dealers.PrimaryDealer.DealerDetails.DealerId;
                            objDTOModelPage.PrimaryDealer.DealerPkgType = (Bikewale.DTO.PriceQuote.DealerPackageType) dealers.PrimaryDealer.DealerDetails.DealerPackageType;
                        }
                        
                    }
                    if((pqEntity.IsAreaExists && pqEntity.IsAreaSelected) || !pqEntity.IsAreaExists)
                    {
                        objDTOModelPage.SecondaryDealerCount = (ushort)dealers.SecondaryDealerCount;
                    }
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

        /// <summary>
        /// Created by: Sangram Nandkhile on 20 Apr 2016
        /// Summary: Map PQByCityArea from PQByCityAreaEntity
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>

        internal static PQByCityAreaDTO Convert(PQByCityAreaEntity pqEntity)
        {
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            Mapper.CreateMap<BikeVersionMinSpecs, VersionDetail>();
            Mapper.CreateMap<PQByCityAreaEntity, PQByCityAreaDTO>();
            return Mapper.Map<PQByCityAreaEntity, PQByCityAreaDTO>(pqEntity);
        }

        /// <summary>
        /// Created by: Vivek Gupta on 5-5-2016
        /// Summary: Map  BikeModelContent and BikeModelContentDTO
        /// </summary>
        /// <param name="objContent"></param>
        /// <returns></returns>

        internal static BikeModelContentDTO Convert(BikeModelContent objContent)
        {
            Mapper.CreateMap<BikeVersionsListEntity, VersionBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<ReviewRatingEntityBase, ReviewRatingBase>();
            Mapper.CreateMap<ReviewTaggedBikeEntity, ReviewTaggedBike>();
            Mapper.CreateMap<ReviewEntity, Review>();
            Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
            Mapper.CreateMap<BikeVideoEntity, VideoBase>();
            Mapper.CreateMap<BikeModelContent, BikeModelContentDTO>();
            return Mapper.Map<BikeModelContent, BikeModelContentDTO>(objContent);
        }

        /// <summary>
        /// Created by: Vivek Gupta on 17-06-2016
        /// Summary: Map   Map PQByCityArea from PQByCityAreaDTOV2
        /// </summary>//DetailedDealerQuotationEntity
        /// <param name="pqCityAea"></param>
        /// <returns></returns>
        internal static PQByCityAreaDTOV2 ConvertV2(PQByCityAreaEntity pqCityAea)
        {
            Mapper.CreateMap<BikeVersionMinSpecs, VersionDetail>();
            Mapper.CreateMap<DealerQuotationEntity, DealerBase>().ForMember(d => d.Name, opt => opt.MapFrom(s => s.DealerDetails.Organization));
            Mapper.CreateMap<DealerQuotationEntity, DealerBase>().ForMember(d => d.DealerId, opt => opt.MapFrom(s => s.DealerDetails.DealerId));
            Mapper.CreateMap<DealerQuotationEntity, DealerBase>().ForMember(d => d.Area, opt => opt.MapFrom(s => s.DealerDetails.objArea.AreaName));
            Mapper.CreateMap<DealerQuotationEntity, DealerBase>().ForMember(d => d.MaskingNumber, opt => opt.MapFrom(s => s.DealerDetails.MaskingNumber));
            Mapper.CreateMap<DealerQuotationEntity, DealerBase>().ForMember(d => d.DealerPkgType, opt => opt.MapFrom(s => s.DealerDetails.DealerPackageType));
            Mapper.CreateMap<PQByCityAreaEntity, PQByCityAreaDTOV2>();
            //Mapper.CreateMap<DealerQuotationEntity, DPQOffer>();
            var versionPrices = Mapper.Map<PQByCityAreaEntity, PQByCityAreaDTOV2>(pqCityAea);

            if (pqCityAea.PrimaryDealer != null && pqCityAea.PrimaryDealer.OfferList != null)
            {
                List<DPQOffer> objOffers = new List<DPQOffer>();

                foreach (var offer in pqCityAea.PrimaryDealer.OfferList)
                {
                    var addOffer = new DPQOffer()
                    {
                        Id = System.Convert.ToInt32(offer.OfferId),
                        OfferCategoryId = System.Convert.ToInt32(offer.OfferCategoryId),
                        Text = offer.OfferText
                    };
                    objOffers.Add(addOffer);
                }

                versionPrices.PrimaryDealerOffers = objOffers;
            }
            return versionPrices;
        }
    }
}