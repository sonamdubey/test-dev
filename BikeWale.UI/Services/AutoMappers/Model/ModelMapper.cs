using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Campaign;
using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.DealerLocator;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Model.v3;
using Bikewale.DTO.PriceQuote.v2;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.DTO.PriceQuote.Version.v2;
using Bikewale.DTO.Series;
using Bikewale.DTO.UserReviews;
using Bikewale.DTO.Version;
using Bikewale.DTO.Videos;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.DTO;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using Bikewale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Model
{
    /// <summary>
    /// Modified By : Lucky Rathore on 15 Apr 2016
    /// Description : Add BikeSpecs ConvertToBikeSpecs(BikeModelPageEntity objModelPage).
    /// </summary>
    public class ModelMapper
    {
        private static IList<string> _categoryDisplayNameList = new List<string> { "Summary", "Engine & Transmission", "Brakes, Wheels and Suspension", "Dimensions and Chassis", "Fuel efficiency and Performance" };
        internal static DTO.Model.ModelDetails Convert(Entities.BikeData.BikeModelEntity objModel)
        {
          return Mapper.Map<Entities.BikeData.BikeModelEntity, ModelDetails>(objModel);
        }

        internal static ModelDescription Convert(BikeDescriptionEntity objModelDesc)
        {
           return Mapper.Map<BikeDescriptionEntity, ModelDescription>(objModelDesc);
        }

        internal static List<DTO.Version.ModelVersionList> Convert(List<BikeVersionsListEntity> mvEntityList)
        {
           return Mapper.Map<List<BikeVersionsListEntity>, List<ModelVersionList>>(mvEntityList);
        }

        internal static DTO.Version.VersionSpecifications Convert(BikeSpecificationEntity objVersionSpecs)
        {
           return Mapper.Map<BikeSpecificationEntity, VersionSpecifications>(objVersionSpecs);
        }

        internal static Bikewale.DTO.Model.ModelPage Convert(BikeModelPageEntity objModelPage)
        {
            
            var dto = Mapper.Map<BikeModelPageEntity, Bikewale.DTO.Model.ModelPage>(objModelPage);

            if (objModelPage.AllPhotos != null)
            {

                dto.Photos = objModelPage.AllPhotos.Select(
                    m =>
                        new Bikewale.DTO.CMS.Photos.CMSModelImageBase()
                        {
                            HostUrl = m.HostUrl,
                            OriginalImgPath = m.OriginalImgPath
                        }
                    );
            }

            if (objModelPage.ModelVersions != null)
            {
                IList<VersionMinSpecs> versionMinSpecsList = new List<VersionMinSpecs>();
                foreach (BikeVersionMinSpecs bikeVersion in objModelPage.ModelVersions)
                {
                    versionMinSpecsList.Add(SpecsFeaturesMapper.ConvertToVersionMinSpecs(bikeVersion));
                }
                dto.ModelVersions = versionMinSpecsList;
            }
            return dto;
        }
        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jan 2016
        /// Description :   Converts the Model Page entity to Version 2 ModelPage DTO
        /// </summary>
        /// <param name="objModelPage">Mode Page Entity</param>
        /// <returns></returns>
        internal static Bikewale.DTO.Model.v2.ModelPage ConvertV2(BikeModelPageEntity objModelPage)
        {
           
            var dto = Mapper.Map<BikeModelPageEntity, Bikewale.DTO.Model.v2.ModelPage>(objModelPage);

            if (objModelPage.AllPhotos != null)
            {

                dto.Photos = objModelPage.AllPhotos.Select(
                    m =>
                        new Bikewale.DTO.CMS.Photos.CMSModelImageBase()
                        {
                            HostUrl = m.HostUrl,
                            OriginalImgPath = m.OriginalImgPath
                        }
                    );
            }

            if (objModelPage.ModelVersions != null)
            {
                IList<VersionMinSpecs> versionMinSpecsList = new List<VersionMinSpecs>();
                foreach (BikeVersionMinSpecs bikeVersion in objModelPage.ModelVersions)
                {
                    versionMinSpecsList.Add(SpecsFeaturesMapper.ConvertToVersionMinSpecs(bikeVersion));
                }
                dto.ModelVersions = versionMinSpecsList.ToList();
            }
            return dto;
        }

        internal static ModelBase Convert(BikeModelEntityBase objModel)
        {
            return Mapper.Map<BikeModelEntityBase, ModelBase>(objModel);
        }

        internal static IEnumerable<ModelBase> Convert(List<BikeModelEntityBase> objModelList)
        {
            return Mapper.Map<List<BikeModelEntityBase>, List<ModelBase>>(objModelList);
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 20 Mar 2018
        /// Description : Mapping SpecsFeaturesItem List to Bikewale.DTO.ModelSpecs
        /// </summary>
        /// <param name="specFeatureItemList"></param>
        /// <returns></returns>
        internal static IEnumerable<DTO.Model.Specs> Convert(IEnumerable<SpecsFeaturesItem> specFeatureItemList)
        {
            IList<DTO.Model.Specs> specsList = null;
            try
            {
                if (specFeatureItemList != null)
                {
                    specsList = new List<DTO.Model.Specs>();
                    foreach (SpecsFeaturesItem specsFeaturesItem in specFeatureItemList)
                    {
                        string itemValue = FormatMinSpecs.ShowAvailable(specsFeaturesItem.ItemValues.FirstOrDefault(), specsFeaturesItem.UnitTypeText, specsFeaturesItem.DataType, specsFeaturesItem.Id);
                        specsList.Add(new DTO.Model.Specs()
                        {
                            DisplayText = specsFeaturesItem.DisplayText,
                            DisplayValue = itemValue
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.AutoMappers.Model.ModelMapper.Convert( IEnumerable<SpecsFeaturesItem> {0})", specFeatureItemList));
            }
            return specsList;
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 30 Mar 2018
        /// Description : Convertor from SpecsItem to DTO.Model.Specs
        /// </summary>
        /// <param name="specSummaryList"></param>
        /// <returns></returns>
        internal static IEnumerable<DTO.Model.Specs> Convert(IEnumerable<SpecsItem> specSummaryList)
        {
            IList<DTO.Model.Specs> specsList = null;
            try
            {
                if (specSummaryList != null)
                {
                    specsList = new List<DTO.Model.Specs>();
                    foreach (SpecsItem specsFeaturesItem in specSummaryList)
                    {
                        string itemValue = FormatMinSpecs.ShowAvailable(specsFeaturesItem.Value, specsFeaturesItem.UnitType, specsFeaturesItem.DataType, specsFeaturesItem.Id);
                        specsList.Add(new DTO.Model.Specs()
                        {
                            DisplayText = specsFeaturesItem.Name,
                            DisplayValue = itemValue
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.AutoMappers.Model.ModelMapper.Convert( IEnumerable<SpecsItem> {0})", specSummaryList));
            }
            return specsList;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 15 Apr 2016
        /// Description : Mapper for BikeSpecs DTO and BikeModelPageEntity Entity
        /// Modified by : Pratibha Verma on 19 Mar 2018
        /// Description : Added logic to integrate CW specs and features to BW
        /// Modified by : Rajan Chauhan on 20 Apr 2018
        /// Description : Added Summary under Specification
        /// </summary>
        /// <param name="objModelPage">object of BikeModelPageEntity</param>
        /// <returns>BikeSpecs DTO</returns>
        internal static BikeSpecs ConvertToBikeSpecs(BikeModelPageEntity objModelPage, PQByCityAreaEntity pqEntity)
        {
            try
            {
               
                IEnumerable<DTO.Model.Specs> featuresList = null;
                IList<DTO.Model.v2.SpecsCategory> specsCategory = null;
                if (objModelPage != null && objModelPage.VersionSpecsFeatures != null)
                {
                    if (objModelPage.VersionSpecsFeatures.Features != null)
                    {
                        featuresList = Convert(objModelPage.VersionSpecsFeatures.Features);
                    }
                    if (objModelPage.VersionSpecsFeatures.Specs != null)
                    {
                        specsCategory = new List<DTO.Model.v2.SpecsCategory>();
                        specsCategory.Add(new DTO.Model.v2.SpecsCategory()
                        {
                            DisplayName = "Summary",
                            Specs = Convert(objModelPage.SpecsSummaryList.Reverse().Skip(1).Reverse())
                        });
                        foreach (var specsCat in objModelPage.VersionSpecsFeatures.Specs)
                        {
                            specsCategory.Add(new DTO.Model.v2.SpecsCategory()
                            {
                                DisplayName = GetCategoryDisplayName(specsCategory.Count),
                                Specs = Convert(specsCat.SpecsItemList)
                            });
                        }
                    }
                }

                var bikespecs = Mapper.Map<BikeSpecs>(objModelPage);
                bikespecs.IsAreaExists = pqEntity.IsAreaExists;
                bikespecs.IsExShowroomPrice = pqEntity.IsExShowroomPrice;
                bikespecs.ModelVersions = ConvertBikeVersionToVersionDetail(pqEntity.VersionList);
                bikespecs.FeaturesList = featuresList;
                bikespecs.SpecsCategory = specsCategory;
                return bikespecs;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.AutoMappers.Model.ModelMapper.ConvertToBikeSpecs");
                return default(BikeSpecs);
            }
        }

        /// <summary>
        /// Author  : Kartik Raahtod on 20 jun 2018 pricequote changes
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <param name="pqEntity"></param>
        /// <returns></returns>
        internal static BikeSpecs ConvertToBikeSpecsV2(BikeModelPageEntity objModelPage, Bikewale.Entities.PriceQuote.v3.PQByCityAreaEntity pqEntity)
        {
            try
            {
                IEnumerable<DTO.Model.Specs> featuresList = null;
                IList<DTO.Model.v2.SpecsCategory> specsCategory = null;
                if (objModelPage != null && objModelPage.VersionSpecsFeatures != null)
                {
                    if (objModelPage.VersionSpecsFeatures.Features != null)
                    {
                        featuresList = Convert(objModelPage.VersionSpecsFeatures.Features);
                    }
                    if (objModelPage.VersionSpecsFeatures.Specs != null)
                    {
                        specsCategory = new List<DTO.Model.v2.SpecsCategory>();
                        specsCategory.Add(new DTO.Model.v2.SpecsCategory()
                        {
                            DisplayName = "Summary",
                            Specs = Convert(objModelPage.SpecsSummaryList.Reverse().Skip(1).Reverse())
                        });
                        foreach (var specsCat in objModelPage.VersionSpecsFeatures.Specs)
                        {
                            specsCategory.Add(new DTO.Model.v2.SpecsCategory()
                            {
                                DisplayName = GetCategoryDisplayName(specsCategory.Count),
                                Specs = Convert(specsCat.SpecsItemList)
                            });
                        }
                    }
                }

                var bikespecs = Mapper.Map<BikeSpecs>(objModelPage);
                bikespecs.IsAreaExists = pqEntity.IsAreaExists;
                bikespecs.IsExShowroomPrice = pqEntity.IsExShowroomPrice;
                bikespecs.ModelVersions = ConvertBikeVersionToVersionDetail(pqEntity.VersionList);
                bikespecs.FeaturesList = featuresList;
                bikespecs.SpecsCategory = specsCategory;
                return bikespecs;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.AutoMappers.Model.ModelMapper.ConvertToBikeSpecsV2");
                return default(BikeSpecs);
            }
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 15 Apr 2016
        /// Summary:To map Object for V3 model entity and PQ entity
        /// updated by: Sangram Nandkhile on 05 May 2016 
        /// Summary: Added upcoming section
        /// Modified By : Rajan Chauhan on 10 Apr 2018
        /// Description : Changed spec binding from overView to specSummaryList
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

                if (objModelPage.SpecsSummaryList != null)
                {
                    string displayValue;
                    foreach (var spec in objModelPage.SpecsSummaryList)
                    {
                        displayValue = FormatMinSpecs.ShowAvailable(spec.Value, spec.UnitType, spec.DataType, spec.Id);
                        switch ((EnumSpecsFeaturesItems)spec.Id)
                        {
                            case EnumSpecsFeaturesItems.Displacement:
                                objDTOModelPage.Capacity = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.FuelEfficiencyOverall:
                                objDTOModelPage.Mileage = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.MaxPowerBhp:
                                objDTOModelPage.MaxPower = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.KerbWeight:
                                objDTOModelPage.Weight = displayValue;
                                break;
                        }
                    }
                }


                if (objModelPage.AllPhotos != null)
                {
                    var photos = new List<DTO.Model.v3.CMSModelImageBase>();
                    foreach (var photo in objModelPage.AllPhotos)
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
                    objDTOModelPage.ModelVersions = ConvertBikeVersionToVersionDetail(pqEntity.VersionList);
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


        internal static DTO.Model.v3.ModelPage ConvertV4(BikeModelPageEntity objModelPage, Entities.PriceQuote.PQByCityAreaEntity pqEntity)
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

                if (objModelPage.SpecsSummaryList != null)
                {
                    string displayValue;
                    foreach (var spec in objModelPage.SpecsSummaryList)
                    {
                        displayValue = FormatMinSpecs.ShowAvailable(spec.Value, spec.UnitType, spec.DataType, spec.Id);
                        switch ((EnumSpecsFeaturesItems)spec.Id)
                        {
                            case EnumSpecsFeaturesItems.Displacement:
                                objDTOModelPage.Capacity = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.FuelEfficiencyOverall:
                                objDTOModelPage.Mileage = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.MaxPowerBhp:
                                objDTOModelPage.MaxPower = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.KerbWeight:
                                objDTOModelPage.Weight = displayValue;
                                break;
                        }
                    }
                }


                if (objModelPage.AllPhotos != null)
                {
                    var photos = new List<DTO.Model.v3.CMSModelImageBase>();
                    foreach (var photo in objModelPage.AllPhotos)
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
                    objDTOModelPage.ModelVersions = ConvertBikeVersionToVersionDetail(pqEntity.VersionList);
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
        /// Modified By : Rajan Chauhan on 2 April 2018
        /// Description : ModelVersions convertor changed to ConvertBikeVersionToVersionDetail
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        internal static DTO.Model.v4.ModelPage ConvertV4(BikeModelPageEntity objModelPage, Entities.PriceQuote.PQByCityAreaEntity pqEntity, Entities.PriceQuote.v2.DetailedDealerQuotationEntity dealers)
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

                if (objModelPage.SpecsSummaryList != null)
                {
                    string displayValue;
                    foreach (var spec in objModelPage.SpecsSummaryList)
                    {
                        displayValue = FormatMinSpecs.ShowAvailable(spec.Value, spec.UnitType, spec.DataType, spec.Id);
                        switch ((EnumSpecsFeaturesItems)spec.Id)
                        {
                            case EnumSpecsFeaturesItems.Displacement:
                                objDTOModelPage.Capacity = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.FuelEfficiencyOverall:
                                objDTOModelPage.Mileage = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.MaxPowerBhp:
                                objDTOModelPage.MaxPower = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.KerbWeight:
                                objDTOModelPage.Weight = displayValue;
                                break;
                        }
                    }
                }

                if (objModelPage.AllPhotos != null)
                {
                    var photos = new List<DTO.Model.v3.CMSModelImageBase>();
                    foreach (var photo in objModelPage.AllPhotos)
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
                    objDTOModelPage.ModelVersions = ConvertBikeVersionToVersionDetail(pqEntity.VersionList);
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
                    if (dealers.PrimaryDealer != null)
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
                            objDTOModelPage.PrimaryDealer.DealerPkgType = (Bikewale.DTO.PriceQuote.DealerPackageType)dealers.PrimaryDealer.DealerDetails.DealerPackageType;
                            objDTOModelPage.IsPremium = dealers.PrimaryDealer.IsPremiumDealer;
                        }

                    }
                    objDTOModelPage.SecondaryDealerCount = (ushort)dealers.SecondaryDealerCount;

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
           
            return Mapper.Map<IEnumerable<BikeVersionMinSpecs>, List<VersionDetail>>(enumerable);
        }

        /// <summary>
        /// Created by: Sangram Nandkhile on 20 Apr 2016
        /// Summary: Map PQByCityArea from PQByCityAreaEntity
        /// </summary>
        /// <param name="pqEntity"></param>
        /// <returns></returns>

        internal static PQByCityAreaDTO Convert(PQByCityAreaEntity pqEntity)
        {
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
           return Mapper.Map<BikeModelContent, BikeModelContentDTO>(objContent);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 6th September 2017
        /// Desc : Map BikeModelContent entity to dto BikeModelContentDTO
        /// </summary>
        /// <param name="objContent"></param>
        /// <returns></returns>
        internal static Bikewale.DTO.Model.v2.BikeModelContentDTO ConvertV2(Bikewale.Entities.BikeData.v2.BikeModelContent objContent)
        {
            return Mapper.Map<Bikewale.Entities.BikeData.v2.BikeModelContent, Bikewale.DTO.Model.v2.BikeModelContentDTO>(objContent);
        }

        /// <summary>
        /// Created by: Vivek Gupta on 17-06-2016
        /// Summary: Map   Map PQByCityArea from PQByCityAreaDTOV2
        /// </summary>//DetailedDealerQuotationEntity
        /// <param name="pqCityAea"></param>
        /// <returns></returns>
        internal static PQByCityAreaDTOV2 ConvertV2(PQByCityAreaEntity pqCityAea)
        {
          
            var versionPrices = Mapper.Map<PQByCityAreaEntity, PQByCityAreaDTOV2>(pqCityAea);
            versionPrices.VersionList = ConvertBikeVersionToVersionDetail(pqCityAea.VersionList);
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

        internal static Bikewale.DTO.PriceQuote.Version.v3.PQByCityAreaDTO ConvertV3(PQByCityAreaEntity pqCityAea)
        {
            var versionPrices = Mapper.Map<PQByCityAreaEntity, Bikewale.DTO.PriceQuote.Version.v3.PQByCityAreaDTO>(pqCityAea);
            versionPrices.VersionList = ConvertBikeVersionToVersionDetail(pqCityAea.VersionList);
            return versionPrices;
        }

        /// <summary>
        /// Author  : Kartik Raahtod on 20 jun 2018 pricequote changes
        /// </summary>
        /// <param name="pqCityAea"></param>
        /// <returns></returns>
        internal static Bikewale.DTO.PriceQuote.Version.v4.PQByCityAreaDTO ConvertV4(Bikewale.Entities.PriceQuote.v3.PQByCityAreaEntity pqCityAea)
        {
            var versionPrices = Mapper.Map<Bikewale.Entities.PriceQuote.v3.PQByCityAreaEntity, Bikewale.DTO.PriceQuote.Version.v4.PQByCityAreaDTO>(pqCityAea);
            versionPrices.VersionList = ConvertBikeVersionToVersionDetail(pqCityAea.VersionList);
            return versionPrices;
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 01 Jul 2016
        /// Description :   AutoMapper Entity to DTO for Popular Bikes
        /// </summary>
        /// <param name="objModelList"></param>
        /// <returns></returns>
        internal static IEnumerable<MostPopularBikes> Convert(IEnumerable<MostPopularBikesBase> objModelList)
        {
            return Mapper.Map<IEnumerable<MostPopularBikesBase>, IEnumerable<MostPopularBikes>>(objModelList);
        }

        /// <summary>
        /// Created by  :   Sangram Nandkhile on 09 Feb 2017
        /// Description :   AutoMapper Entity to DTO for mapping All model images
        /// </summary>
        /// <param name="imageList"></param>
        /// <returns></returns>
        internal static IEnumerable<ColorImageBaseDTO> Convert(IEnumerable<ColorImageBaseEntity> imageList)
        {
            return Mapper.Map<IEnumerable<ColorImageBaseEntity>, IEnumerable<ColorImageBaseDTO>>(imageList);
        }

        /// <summary>
        /// Created By : Lucky Rathore on 17 June 2016
        /// Descritpion : Mapping for V4 version of ModelpageEntity.
        /// Modified by : Ashutosh Sharma on 14 May 2018.
        /// Description : Calculating hashCode to cache campaign template before setting PageUrl of LeadCampaign.
        /// Modifier    : Kartik Rathod on 16 may 2018, Pageurl for capitalfirst dealer added dealername and sendLeadSMSCustomer
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <returns></returns>
        internal static DTO.Model.v5.ModelPage ConvertV5(IPriceQuoteCache objPqCache, BikeModelPageEntity objModelPage, PQByCityAreaEntity pqEntity, Entities.PriceQuote.v2.DetailedDealerQuotationEntity dealers, ushort platformId = 0)
        {

            bool isApp = platformId == 3;
            DTO.Model.v5.ModelPage objDTOModelPage = null;
            try
            {
                DateTime dt3 = DateTime.Now;
                var modelDetails = objModelPage.ModelDetails;
                objDTOModelPage = new DTO.Model.v5.ModelPage();
                objDTOModelPage.SmallDescription = objModelPage.ModelDesc.SmallDescription;
                objDTOModelPage.MakeId = modelDetails.MakeBase.MakeId;
                objDTOModelPage.MakeName = modelDetails.MakeBase.MakeName;
                objDTOModelPage.ModelId = modelDetails.ModelId;
                objDTOModelPage.ModelName = modelDetails.ModelName;
                objDTOModelPage.ReviewCount = modelDetails.ReviewCount;
                objDTOModelPage.ReviewRate = modelDetails.ReviewRate;
                objDTOModelPage.NewsCount = modelDetails.NewsCount;
                objDTOModelPage.IsUpcoming = modelDetails.Futuristic;
                objDTOModelPage.IsSpecsAvailable = (objModelPage.SpecsSummaryList != null && objModelPage.SpecsSummaryList.Any(spec => spec.Value != ""));

                objDTOModelPage.Review = new DTO.Model.v5.Review()
                {
                    ExpertReviewCount = modelDetails.ExpertReviewsCount,
                    RatingCount = (uint)modelDetails.RatingCount,
                    UserReviewCount = (uint)modelDetails.ReviewCount
                };

                if (!objDTOModelPage.IsUpcoming)
                {
                    objDTOModelPage.IsDiscontinued = !modelDetails.New;
                }

                if (objDTOModelPage.IsSpecsAvailable)
                {
                    string displayValue;
                    foreach (var spec in objModelPage.SpecsSummaryList)
                    {
                        displayValue = string.IsNullOrEmpty(spec.Value)? null : FormatMinSpecs.ShowAvailable(spec.Value, spec.UnitType, spec.DataType, spec.Id);
                        switch ((EnumSpecsFeaturesItems)spec.Id)
                        {
                            case EnumSpecsFeaturesItems.Displacement:
                                objDTOModelPage.Capacity = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.FuelEfficiencyOverall:
                                objDTOModelPage.Mileage = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.MaxPowerBhp:
                                objDTOModelPage.MaxPower = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.KerbWeight:
                                objDTOModelPage.Weight = displayValue;
                                break;
                        }
                    }
                }



                if (objModelPage.AllPhotos != null && objModelPage.AllPhotos.Any())
                {
                    objDTOModelPage.Gallery = new DTO.Model.v5.Gallery
                    {
                        ImageCount = (uint)objModelPage.AllPhotos.Count(),
                        ColorCount = objModelPage.colorPhotos != null && objModelPage.colorPhotos.Any(m => m.IsImageExists) ? (uint)objModelPage.colorPhotos.Count(m => m.IsImageExists) : 0,
                        VideoCount = (uint)modelDetails.VideosCount
                    };

                    var photos = new List<CMSModelImageBase>();

                    var addPhoto = new CMSModelImageBase()
                    {
                        HostUrl = objModelPage.AllPhotos.ElementAt(0).HostUrl,
                        OriginalImgPath = objModelPage.AllPhotos.ElementAt(0).OriginalImgPath
                    };
                    photos.Add(addPhoto);

                    objDTOModelPage.Photos = photos;
                }
                if (objModelPage.colorPhotos != null && objModelPage.colorPhotos.Any())
                {
                    objDTOModelPage.ModelColors = ModelMapper.Convert(objModelPage.colorPhotos).OrderByDescending(m => m.IsImageExists);
                }

                if (pqEntity != null)
                {
                    objDTOModelPage.IsCityExists = pqEntity.IsCityExists;
                    objDTOModelPage.IsAreaExists = pqEntity.IsAreaExists;
                    objDTOModelPage.IsExShowroomPrice = pqEntity.IsExShowroomPrice;
                    objDTOModelPage.ModelVersions = ConvertBikeVersionToVersionDetail(pqEntity.VersionList);
                    objDTOModelPage.DealerId = pqEntity.DealerId;
                    objDTOModelPage.PQId = pqEntity.PqId;
                }
                // Upcoming section
                if (modelDetails.Futuristic && objModelPage.UpcomingBike != null)
                {
                    var upcomingBike = objModelPage.UpcomingBike;
                    objDTOModelPage.ExpectedLaunchDate = upcomingBike.ExpectedLaunchDate;
                    objDTOModelPage.ExpectedMinPrice = upcomingBike.EstimatedPriceMin;
                    objDTOModelPage.ExpectedMaxPrice = upcomingBike.EstimatedPriceMax;
                }


                if (dealers != null)
                {

                    var campaignDTO = new CampaignBaseDto();
                    var detailsCampaignDTO = new DetailsDto();
                    var dealerCampaignBaseDTO = new DealerCampaignBase();

                    if (dealers.PrimaryDealer != null && dealers.PrimaryDealer.DealerDetails != null)
                    {
                        #region Dealer Offers DTO
                        var offerList = dealers.PrimaryDealer.OfferList;
                        if (offerList != null && offerList.Any())
                        {
                            var dealerOffer = new List<DPQOffer>();
                            foreach (var offer in offerList)
                            {
                                var addOffer = new DPQOffer()
                                {
                                    Id = (int)offer.OfferId,
                                    OfferCategoryId = (int)offer.OfferCategoryId,
                                    Text = offer.OfferText
                                };
                                dealerOffer.Add(addOffer);
                            }
                            dealerCampaignBaseDTO.Offers = dealerOffer;
                        }
                        #endregion

                        #region Dealer Details
                        var dealerDetailsEntity = dealers.PrimaryDealer.DealerDetails;
                        var dealerDetailsDTO = new DealerBase();

                        campaignDTO.CampaignType = CampaignType.DS;
                        dealerDetailsDTO.Name = dealerDetailsEntity.Organization;
                        dealerDetailsDTO.MaskingNumber = dealerDetailsEntity.MaskingNumber;
                        dealerDetailsDTO.Area = dealerDetailsEntity.objArea.AreaName;
                        dealerDetailsDTO.DealerId = dealerDetailsEntity.DealerId;
                        dealerDetailsDTO.DealerPkgType = (DTO.PriceQuote.DealerPackageType)dealerDetailsEntity.DealerPackageType;
                        dealerCampaignBaseDTO.IsPremium = dealers.PrimaryDealer.IsPremiumDealer;
                        dealerCampaignBaseDTO.CaptionText = String.Format("Authorized dealer in {0}", dealerDetailsEntity.objArea.AreaName);


                        dealerCampaignBaseDTO.PrimaryDealer = dealerDetailsDTO;
                        #endregion
                    }
                    dealerCampaignBaseDTO.SecondaryDealerCount = (ushort)dealers.SecondaryDealerCount;

                    detailsCampaignDTO.Dealer = dealerCampaignBaseDTO;

                    campaignDTO.DetailsCampaign = detailsCampaignDTO;

                    objDTOModelPage.Campaign = campaignDTO;


                }

                if
                    (pqEntity != null &&
                    (dealers == null || dealers.PrimaryDealer == null || dealers.PrimaryDealer.DealerDetails == null) &&
                    pqEntity.ManufacturerCampaign != null &&
                    pqEntity.ManufacturerCampaign.LeadCampaign != null)
                {
                    var leadCampaign = pqEntity.ManufacturerCampaign.LeadCampaign;
                    Entities.manufacturecampaign.v2.ManufactureCampaignLeadEntity LeadCampaign = new Entities.manufacturecampaign.v2.ManufactureCampaignLeadEntity()
                    {
                        CampaignId = leadCampaign.CampaignId,
                        DealerId = leadCampaign.DealerId,
                        DealerRequired = leadCampaign.DealerRequired,
                        EmailRequired = leadCampaign.EmailRequired,
                        LeadsButtonTextDesktop = leadCampaign.LeadsButtonTextDesktop,
                        LeadsButtonTextMobile = leadCampaign.LeadsButtonTextMobile,
                        LeadSourceId = (int)LeadSourceEnum.Model_Mobile,
                        PqSourceId = (int)PQSourceEnum.Mobile_ModelPage,
                        LeadsHtmlDesktop = leadCampaign.LeadsHtmlDesktop,
                        LeadsHtmlMobile = leadCampaign.LeadsHtmlMobile,
                        LeadsPropertyTextDesktop = leadCampaign.LeadsPropertyTextDesktop,
                        LeadsPropertyTextMobile = leadCampaign.LeadsPropertyTextMobile,
                        MakeName = modelDetails.MakeBase.MakeName,
                        Organization = leadCampaign.Organization,
                        MaskingNumber = leadCampaign.MaskingNumber,
                        PincodeRequired = leadCampaign.PincodeRequired,
                        PopupDescription = leadCampaign.PopupDescription,
                        PopupHeading = leadCampaign.PopupHeading,
                        PopupSuccessMessage = leadCampaign.PopupSuccessMessage,
                        ShowOnExshowroom = leadCampaign.ShowOnExshowroom,
                        VersionId = (objModelPage.ModelVersionMinSpecs != null ? (uint)objModelPage.ModelVersionMinSpecs.VersionId : 0),
                        PlatformId = platformId,
                        IsAmp = !isApp,
                        BikeName = string.Format("{0} {1}", modelDetails.MakeBase.MakeName, modelDetails.ModelName),
                        LoanAmount = (objModelPage.ModelVersionMinSpecs != null ? (uint)System.Convert.ToUInt32((pqEntity.VersionList.FirstOrDefault(m => m.VersionId == objModelPage.ModelVersionMinSpecs.VersionId).Price) * 0.8) : 0),
                        SendLeadSMSCustomer = leadCampaign.SendLeadSMSCustomer
                    };
                    #region Render the partial view
                    //This hash code is being used as memcache key. Do not assign pqid and LoadAmount in "LeadCampaign" before generating hash code.
                    var hashCode = Bikewale.PWA.Utils.PwaCmsHelper.GetSha256Hash(JsonConvert.SerializeObject(LeadCampaign));
                    LeadCampaign.LoanAmount = (uint)System.Convert.ToUInt32((pqEntity.VersionList.FirstOrDefault(m => m.VersionId == objModelPage.ModelVersionMinSpecs.VersionId).Price) * 0.8);
                    if (LeadCampaign.DealerId == BWConfiguration.Instance.CapitalFirstDealerId)
                    {
                        LeadCampaign.PageUrl = String.Format("{8}/m/finance/capitalfirst/?campaingid={0}&amp;dealerid={1}&amp;pqid={2}&amp;leadsourceid={3}&amp;versionid={4}&amp;url=&amp;platformid={5}&amp;bike={6}&amp;loanamount={7}&amp;dealerName={9}&amp;sendLeadSMSCustomer={10}&amp;cityid={11}",
                                               LeadCampaign.CampaignId, LeadCampaign.DealerId, pqEntity.PqId, LeadCampaign.LeadSourceId, pqEntity.VersionList.FirstOrDefault().VersionId, platformId, LeadCampaign.BikeName, LeadCampaign.LoanAmount, BWConfiguration.Instance.BwHostUrl, LeadCampaign.Organization, LeadCampaign.SendLeadSMSCustomer,
                                               (pqEntity.City!=null?pqEntity.City.CityId.ToString() : string.Empty));
                    }
                    else if (LeadCampaign.DealerId == BWConfiguration.Instance.BajajAutoFinanceDealerId)
                    {
                        LeadCampaign.PageUrl = string.Format("{0}/m/finance/bajaj/?dealerid={1}&amp;campaignid={2}&amp;pqid={3}&amp;leadsourceid={4}&amp;versionid={5}&amp;url=&amp;platformid={6}&amp;bike={7}&amp;dealerName={8}&amp;sendLeadSMSCustomer={9}&amp;cityid={10}", BWConfiguration.Instance.BwHostUrl, LeadCampaign.DealerId, LeadCampaign.CampaignId, pqEntity.PqId, LeadCampaign.LeadSourceId, pqEntity.VersionList.FirstOrDefault().VersionId, platformId, LeadCampaign.BikeName, LeadCampaign.Organization, LeadCampaign.SendLeadSMSCustomer, (pqEntity.City != null ? pqEntity.City.CityId.ToString() : string.Empty));
                    }
                    else
                    {
                        LeadCampaign.PageUrl = string.Format("{0}/m/popup/leadcapture/?q={1}&amp;platformid={2}", BWConfiguration.Instance.BwHostUrl, Bikewale.Utility.TripleDES.EncryptTripleDES(string.Format("modelid={0}&cityid={1}&areaid={2}&bikename={3}&location={4}&city={5}&area={6}&ismanufacturer={7}&dealerid={8}&dealername={9}&dealerarea={10}&versionid={11}&leadsourceid={12}&pqsourceid={13}&mfgcampid={14}&pqid={15}&pageurl={16}&clientip={17}&dealerheading={18}&dealermessage={19}&dealerdescription={20}&pincoderequired={21}&emailrequired={22}&dealersrequired={23}&sendLeadSMSCustomer={24}&organizationName={25}",
                                               modelDetails.ModelId, (pqEntity.City != null ? pqEntity.City.CityId.ToString() : string.Empty), string.Empty, string.Format(LeadCampaign.BikeName), string.Empty, string.Empty, string.Empty, true, LeadCampaign.DealerId, String.Format(LeadCampaign.LeadsPropertyTextMobile, LeadCampaign.Organization), LeadCampaign.Area, pqEntity.VersionList.FirstOrDefault().VersionId, LeadCampaign.LeadSourceId, LeadCampaign.PqSourceId, LeadCampaign.CampaignId, pqEntity.PqId, string.Empty, string.Empty, LeadCampaign.PopupHeading, String.Format(LeadCampaign.PopupSuccessMessage, 
                                               LeadCampaign.Organization), LeadCampaign.PopupDescription, leadCampaign.PincodeRequired, leadCampaign.EmailRequired, leadCampaign.DealerRequired,leadCampaign.SendLeadSMSCustomer,leadCampaign.Organization)), platformId);
                    }
                    string template = objPqCache.GetManufacturerCampaignMobileRenderedTemplateV2(hashCode, LeadCampaign);
                    #endregion

                                


                    var esCampaignBase = new CampaignBaseDto() { CampaignType = CampaignType.ES };
                    var detailsDto = new DetailsDto();

                    var esCampaignDTO = new ESCampaignBase();
                    esCampaignDTO.FloatingBtnText = LeadCampaign.LeadsButtonTextMobile;
                    esCampaignDTO.CaptionText = String.Format(LeadCampaign.LeadsPropertyTextMobile, LeadCampaign.Organization);
                    esCampaignDTO.LeadSourceId = (int)LeadSourceEnum.Model_Mobile;
                    esCampaignDTO.LinkUrl = HttpUtility.HtmlDecode(LeadCampaign.PageUrl);

                    
                    var esPreRenderCampaignDTO = new PreRenderCampaignBase();

                    //Check if it contains javascript:void(0), replace it with Android method.
                    if (isApp && !string.IsNullOrEmpty(template))
                    {
                        template = template.Replace("href=\"javascript:void(0)\"", "onclick=\"Android.openLeadCaptureForm();\"");
                    }
                    
                    esPreRenderCampaignDTO.TemplateHtml = template;
                    detailsDto.EsCamapign = esPreRenderCampaignDTO;

                    esCampaignBase.DetailsCampaign = detailsDto;
                    esCampaignBase.CampaignLeadSource = esCampaignDTO;
                    objDTOModelPage.Campaign = esCampaignBase;
                }
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Model.ModelController.ConvertV5({0})", objModelPage.ModelDetails.ModelId));
            }
            return objDTOModelPage;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 11 October 2018
        /// Description : new version for PQId related changes
        /// </summary>
        /// <param name="objPqCache"></param>
        /// <param name="objModelPage"></param>
        /// <param name="pqEntity"></param>
        /// <param name="dealers"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        internal static DTO.Model.v6.ModelPage ConvertV6(IPriceQuoteCache objPqCache, BikeModelPageEntity objModelPage, Bikewale.Entities.PriceQuote.v3.PQByCityAreaEntity pqEntity, Entities.PriceQuote.v2.DetailedDealerQuotationEntity dealers, ushort platformId = 0)
        {

            bool isApp = platformId == 3;
            DTO.Model.v6.ModelPage objDTOModelPage = null;
            try
            {
                DateTime dt3 = DateTime.Now;
                var modelDetails = objModelPage.ModelDetails;
                objDTOModelPage = new DTO.Model.v6.ModelPage();
                objDTOModelPage.SmallDescription = objModelPage.ModelDesc.SmallDescription;
                objDTOModelPage.MakeId = modelDetails.MakeBase.MakeId;
                objDTOModelPage.MakeName = modelDetails.MakeBase.MakeName;
                objDTOModelPage.ModelId = modelDetails.ModelId;
                objDTOModelPage.ModelName = modelDetails.ModelName;
                objDTOModelPage.ReviewCount = modelDetails.ReviewCount;
                objDTOModelPage.ReviewRate = modelDetails.ReviewRate;
                objDTOModelPage.NewsCount = modelDetails.NewsCount;
                objDTOModelPage.IsUpcoming = modelDetails.Futuristic;
                objDTOModelPage.IsSpecsAvailable = (objModelPage.SpecsSummaryList != null && objModelPage.SpecsSummaryList.Any(spec => spec.Value != ""));

                objDTOModelPage.Review = new DTO.Model.v5.Review()
                {
                    ExpertReviewCount = modelDetails.ExpertReviewsCount,
                    RatingCount = (uint)modelDetails.RatingCount,
                    UserReviewCount = (uint)modelDetails.ReviewCount
                };

                if (!objDTOModelPage.IsUpcoming)
                {
                    objDTOModelPage.IsDiscontinued = !modelDetails.New;
                }

                if (objDTOModelPage.IsSpecsAvailable)
                {
                    string displayValue;
                    foreach (var spec in objModelPage.SpecsSummaryList)
                    {
                        displayValue = string.IsNullOrEmpty(spec.Value) ? null : FormatMinSpecs.ShowAvailable(spec.Value, spec.UnitType, spec.DataType, spec.Id);
                        switch ((EnumSpecsFeaturesItems)spec.Id)
                        {
                            case EnumSpecsFeaturesItems.Displacement:
                                objDTOModelPage.Capacity = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.FuelEfficiencyOverall:
                                objDTOModelPage.Mileage = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.MaxPowerBhp:
                                objDTOModelPage.MaxPower = displayValue;
                                break;
                            case EnumSpecsFeaturesItems.KerbWeight:
                                objDTOModelPage.Weight = displayValue;
                                break;
                        }
                    }
                }



                if (objModelPage.AllPhotos != null && objModelPage.AllPhotos.Any())
                {
                    objDTOModelPage.Gallery = new DTO.Model.v5.Gallery
                    {
                        ImageCount = (uint)objModelPage.AllPhotos.Count(),
                        ColorCount = objModelPage.colorPhotos != null && objModelPage.colorPhotos.Any(m => m.IsImageExists) ? (uint)objModelPage.colorPhotos.Count(m => m.IsImageExists) : 0,
                        VideoCount = (uint)modelDetails.VideosCount
                    };

                    var photos = new List<CMSModelImageBase>();

                    var addPhoto = new CMSModelImageBase()
                    {
                        HostUrl = objModelPage.AllPhotos.ElementAt(0).HostUrl,
                        OriginalImgPath = objModelPage.AllPhotos.ElementAt(0).OriginalImgPath
                    };
                    photos.Add(addPhoto);

                    objDTOModelPage.Photos = photos;
                }
                if (objModelPage.colorPhotos != null && objModelPage.colorPhotos.Any())
                {
                    objDTOModelPage.ModelColors = ModelMapper.Convert(objModelPage.colorPhotos).OrderByDescending(m => m.IsImageExists);
                }

                if (pqEntity != null)
                {
                    objDTOModelPage.IsCityExists = pqEntity.IsCityExists;
                    objDTOModelPage.IsAreaExists = pqEntity.IsAreaExists;
                    objDTOModelPage.IsExShowroomPrice = pqEntity.IsExShowroomPrice;
                    objDTOModelPage.ModelVersions = ConvertBikeVersionToVersionDetail(pqEntity.VersionList);
                    objDTOModelPage.DealerId = pqEntity.DealerId;
                    objDTOModelPage.PQId = pqEntity.PqId;
                }
                // Upcoming section
                if (modelDetails.Futuristic && objModelPage.UpcomingBike != null)
                {
                    var upcomingBike = objModelPage.UpcomingBike;
                    objDTOModelPage.ExpectedLaunchDate = upcomingBike.ExpectedLaunchDate;
                    objDTOModelPage.ExpectedMinPrice = upcomingBike.EstimatedPriceMin;
                    objDTOModelPage.ExpectedMaxPrice = upcomingBike.EstimatedPriceMax;
                }


                if (dealers != null)
                {

                    var campaignDTO = new CampaignBaseDto();
                    var detailsCampaignDTO = new DetailsDto();
                    var dealerCampaignBaseDTO = new DealerCampaignBase();

                    if (dealers.PrimaryDealer != null && dealers.PrimaryDealer.DealerDetails != null)
                    {
                        #region Dealer Offers DTO
                        var offerList = dealers.PrimaryDealer.OfferList;
                        if (offerList != null && offerList.Any())
                        {
                            var dealerOffer = new List<DPQOffer>();
                            foreach (var offer in offerList)
                            {
                                var addOffer = new DPQOffer()
                                {
                                    Id = (int)offer.OfferId,
                                    OfferCategoryId = (int)offer.OfferCategoryId,
                                    Text = offer.OfferText
                                };
                                dealerOffer.Add(addOffer);
                            }
                            dealerCampaignBaseDTO.Offers = dealerOffer;
                        }
                        #endregion

                        #region Dealer Details
                        var dealerDetailsEntity = dealers.PrimaryDealer.DealerDetails;
                        var dealerDetailsDTO = new DealerBase();

                        campaignDTO.CampaignType = CampaignType.DS;
                        dealerDetailsDTO.Name = dealerDetailsEntity.Organization;
                        dealerDetailsDTO.MaskingNumber = dealerDetailsEntity.MaskingNumber;
                        dealerDetailsDTO.Area = dealerDetailsEntity.objArea.AreaName;
                        dealerDetailsDTO.DealerId = dealerDetailsEntity.DealerId;
                        dealerDetailsDTO.DealerPkgType = (DTO.PriceQuote.DealerPackageType)dealerDetailsEntity.DealerPackageType;
                        dealerCampaignBaseDTO.IsPremium = dealers.PrimaryDealer.IsPremiumDealer;
                        dealerCampaignBaseDTO.CaptionText = String.Format("Authorized dealer in {0}", dealerDetailsEntity.objArea.AreaName);


                        dealerCampaignBaseDTO.PrimaryDealer = dealerDetailsDTO;
                        #endregion
                    }
                    dealerCampaignBaseDTO.SecondaryDealerCount = (ushort)dealers.SecondaryDealerCount;

                    detailsCampaignDTO.Dealer = dealerCampaignBaseDTO;

                    campaignDTO.DetailsCampaign = detailsCampaignDTO;

                    objDTOModelPage.Campaign = campaignDTO;


                }

                if
                    (pqEntity != null &&
                    (dealers == null || dealers.PrimaryDealer == null || dealers.PrimaryDealer.DealerDetails == null) &&
                    pqEntity.ManufacturerCampaign != null &&
                    pqEntity.ManufacturerCampaign.LeadCampaign != null)
                {
                    var leadCampaign = pqEntity.ManufacturerCampaign.LeadCampaign;
                    Bikewale.Entities.manufacturecampaign.v2.ManufactureCampaignLeadEntity LeadCampaign = new Bikewale.Entities.manufacturecampaign.v2.ManufactureCampaignLeadEntity()
                    {
                        CampaignId = leadCampaign.CampaignId,
                        DealerId = leadCampaign.DealerId,
                        DealerRequired = leadCampaign.DealerRequired,
                        EmailRequired = leadCampaign.EmailRequired,
                        LeadsButtonTextDesktop = leadCampaign.LeadsButtonTextDesktop,
                        LeadsButtonTextMobile = leadCampaign.LeadsButtonTextMobile,
                        LeadSourceId = (int)LeadSourceEnum.Model_Mobile,
                        PqSourceId = (int)PQSourceEnum.Mobile_ModelPage,
                        LeadsHtmlDesktop = leadCampaign.LeadsHtmlDesktop,
                        LeadsHtmlMobile = leadCampaign.LeadsHtmlMobile,
                        LeadsPropertyTextDesktop = leadCampaign.LeadsPropertyTextDesktop,
                        LeadsPropertyTextMobile = leadCampaign.LeadsPropertyTextMobile,
                        MakeName = modelDetails.MakeBase.MakeName,
                        Organization = leadCampaign.Organization,
                        MaskingNumber = leadCampaign.MaskingNumber,
                        PincodeRequired = leadCampaign.PincodeRequired,
                        PopupDescription = leadCampaign.PopupDescription,
                        PopupHeading = leadCampaign.PopupHeading,
                        PopupSuccessMessage = leadCampaign.PopupSuccessMessage,
                        ShowOnExshowroom = leadCampaign.ShowOnExshowroom,
                        VersionId = (objModelPage.ModelVersionMinSpecs != null ? (uint)objModelPage.ModelVersionMinSpecs.VersionId : 0),
                        PlatformId = platformId,
                        IsAmp = !isApp,
                        BikeName = string.Format("{0} {1}", modelDetails.MakeBase.MakeName, modelDetails.ModelName),
                        LoanAmount = (objModelPage.ModelVersionMinSpecs != null ? (uint)System.Convert.ToUInt32((pqEntity.VersionList.FirstOrDefault(m => m.VersionId == objModelPage.ModelVersionMinSpecs.VersionId).Price) * 0.8) : 0),
                        SendLeadSMSCustomer = leadCampaign.SendLeadSMSCustomer
                    };
                    #region Render the partial view
                    //This hash code is being used as memcache key. Do not assign pqid and LoadAmount in "LeadCampaign" before generating hash code.
                    var hashCode = Bikewale.PWA.Utils.PwaCmsHelper.GetSha256Hash(JsonConvert.SerializeObject(LeadCampaign));
                    LeadCampaign.LoanAmount = (uint)System.Convert.ToUInt32((pqEntity.VersionList.FirstOrDefault(m => m.VersionId == objModelPage.ModelVersionMinSpecs.VersionId).Price) * 0.8);
                    LeadCampaign.PQId = pqEntity.PqId;
                    if (LeadCampaign.DealerId == BWConfiguration.Instance.CapitalFirstDealerId)
                    {
                        LeadCampaign.PageUrl = String.Format("{8}/m/finance/capitalfirst/?campaingid={0}&amp;dealerid={1}&amp;pqid={2}&amp;leadsourceid={3}&amp;versionid={4}&amp;url=&amp;platformid={5}&amp;bike={6}&amp;loanamount={7}&amp;dealerName={9}&amp;sendLeadSMSCustomer={10}&amp;cityid={11}",
                                               LeadCampaign.CampaignId, LeadCampaign.DealerId, LeadCampaign.PQId, LeadCampaign.LeadSourceId, pqEntity.VersionList.FirstOrDefault().VersionId, platformId, LeadCampaign.BikeName, LeadCampaign.LoanAmount, BWConfiguration.Instance.BwHostUrl, LeadCampaign.Organization, LeadCampaign.SendLeadSMSCustomer,
                                               (pqEntity.City != null ? pqEntity.City.CityId.ToString() : string.Empty));
                    }
                    else if (LeadCampaign.DealerId == BWConfiguration.Instance.BajajAutoFinanceDealerId)
                    {
                        LeadCampaign.PageUrl = string.Format("{0}/m/finance/bajaj/?dealerid={1}&amp;campaignid={2}&amp;pqid={3}&amp;leadsourceid={4}&amp;versionid={5}&amp;url=&amp;platformid={6}&amp;bike={7}&amp;dealerName={8}&amp;sendLeadSMSCustomer={9}&amp;cityid={10}", BWConfiguration.Instance.BwHostUrl, LeadCampaign.DealerId, LeadCampaign.CampaignId, LeadCampaign.PQId, LeadCampaign.LeadSourceId, pqEntity.VersionList.FirstOrDefault().VersionId, platformId, LeadCampaign.BikeName, LeadCampaign.Organization, LeadCampaign.SendLeadSMSCustomer, (pqEntity.City != null ? pqEntity.City.CityId.ToString() : string.Empty));
                    }
                    else
                    {
                        LeadCampaign.PageUrl = string.Format("{0}/m/popup/leadcapture/?q={1}&amp;platformid={2}", BWConfiguration.Instance.BwHostUrl, Bikewale.Utility.TripleDES.EncryptTripleDES(string.Format("modelid={0}&cityid={1}&areaid={2}&bikename={3}&location={4}&city={5}&area={6}&ismanufacturer={7}&dealerid={8}&dealername={9}&dealerarea={10}&versionid={11}&leadsourceid={12}&pqsourceid={13}&mfgcampid={14}&pqguid={15}&pageurl={16}&clientip={17}&dealerheading={18}&dealermessage={19}&dealerdescription={20}&pincoderequired={21}&emailrequired={22}&dealersrequired={23}&sendLeadSMSCustomer={24}&organizationName={25}",
                                               modelDetails.ModelId, (pqEntity.City != null ? pqEntity.City.CityId.ToString() : string.Empty), string.Empty, string.Format(LeadCampaign.BikeName), string.Empty, string.Empty, string.Empty, true, LeadCampaign.DealerId, String.Format(LeadCampaign.LeadsPropertyTextMobile, LeadCampaign.Organization), LeadCampaign.Area, pqEntity.VersionList.FirstOrDefault().VersionId, LeadCampaign.LeadSourceId, LeadCampaign.PqSourceId, LeadCampaign.CampaignId, LeadCampaign.PQId, string.Empty, string.Empty, LeadCampaign.PopupHeading, String.Format(LeadCampaign.PopupSuccessMessage,
                                               LeadCampaign.Organization), LeadCampaign.PopupDescription, leadCampaign.PincodeRequired, leadCampaign.EmailRequired, leadCampaign.DealerRequired, leadCampaign.SendLeadSMSCustomer, leadCampaign.Organization)), platformId);
                    }
                    string template = objPqCache.GetManufacturerCampaignMobileRenderedTemplateV2(hashCode, LeadCampaign);
                    #endregion




                    var esCampaignBase = new CampaignBaseDto() { CampaignType = CampaignType.ES };
                    var detailsDto = new DetailsDto();

                    var esCampaignDTO = new ESCampaignBase();
                    esCampaignDTO.FloatingBtnText = LeadCampaign.LeadsButtonTextMobile;
                    esCampaignDTO.CaptionText = String.Format(LeadCampaign.LeadsPropertyTextMobile, LeadCampaign.Organization);
                    esCampaignDTO.LeadSourceId = (int)LeadSourceEnum.Model_Mobile;
                    esCampaignDTO.LinkUrl = HttpUtility.HtmlDecode(LeadCampaign.PageUrl);


                    var esPreRenderCampaignDTO = new PreRenderCampaignBase();

                    //Check if it contains javascript:void(0), replace it with Android method.
                    if (isApp && !string.IsNullOrEmpty(template))
                    {
                        template = template.Replace("href=\"javascript:void(0)\"", "onclick=\"Android.openLeadCaptureForm();\"");
                    }

                    esPreRenderCampaignDTO.TemplateHtml = template;
                    detailsDto.EsCamapign = esPreRenderCampaignDTO;

                    esCampaignBase.DetailsCampaign = detailsDto;
                    esCampaignBase.CampaignLeadSource = esCampaignDTO;
                    objDTOModelPage.Campaign = esCampaignBase;
                }
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Model.ModelController.ConvertV5({0})", objModelPage.ModelDetails.ModelId));
            }
            return objDTOModelPage;
        }
        /// <summary>
        /// Created by  : Vivek Singh Tomar on 4th Oct 2017
        /// Summary : Map BikeModelPage to gallery component
        /// Modified by : Rajan Chauhan on 25 Jan 2018
        /// Description : Changed the categoryCount for photos to match AllPhotos count
        /// </summary>
        /// <param name="objModelPage"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        internal static ModelGallery ConvertToModelGallery(BikeModelPageEntity objModelPage, int modelId)
        {
            ModelGallery objModelGallery = new ModelGallery();
            int colorPhotoCount = 0;
            int allPhotosCount = 0;
            if (objModelPage != null)
            {
                ICollection<ModelGalleryComponent> objGalleryComponent = new List<ModelGalleryComponent>();
                if (objModelPage.ModelDetails != null)
                {

                    colorPhotoCount = objModelPage.colorPhotos.Any() ? objModelPage.colorPhotos.Count(m => m.IsImageExists) : colorPhotoCount;
                    allPhotosCount = objModelPage.AllPhotos.Any() ? objModelPage.AllPhotos.Count() : allPhotosCount;
                    if (allPhotosCount > 0)
                    {
                        objGalleryComponent.Add(
                                new ModelGalleryComponent
                                {
                                    CategoryId = 1,
                                    CategoryName = "Photos",
                                    CategoryCount = allPhotosCount,
                                    DataUrl = string.Format("api/model/{0}/photos/", modelId)
                                }
                            );
                    }

                    if (objModelPage.ModelDetails.VideosCount > 0)
                    {
                        objGalleryComponent.Add(
                                new ModelGalleryComponent
                                {
                                    CategoryId = 2,
                                    CategoryName = "Videos",
                                    CategoryCount = objModelPage.ModelDetails.VideosCount,
                                    DataUrl = string.Format("api/v2/videos/pn/1/ps/{0}/model/{1}/", objModelPage.ModelDetails.VideosCount, modelId)
                                }
                            );
                    }

                    if (colorPhotoCount > 0)
                    {

                        objGalleryComponent.Add(
                                new ModelGalleryComponent
                                {
                                    CategoryId = 3,
                                    CategoryName = "Colours",
                                    CategoryCount = colorPhotoCount,
                                    DataUrl = string.Format("api/model/{0}/colorphotos/", modelId)
                                }
                            );
                    }


                }

                var component = objGalleryComponent.FirstOrDefault();

                if (component != null)
                {
                    objModelGallery.SelectedCategoryId = component.CategoryId;
                    objModelGallery.GalleryComponents = objGalleryComponent;
                }
            }

            return objModelGallery;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 5th Oct 2017
        /// Summary : Map Model color Image entity to dto
        /// </summary>
        /// <param name="objAllPhotosEntity"></param>
        /// <returns></returns>
        internal static IEnumerable<ModelColorPhoto> Convert(IEnumerable<ModelColorImage> objAllPhotosEntity)
        {
            return Mapper.Map<IEnumerable<ModelColorImage>, IEnumerable<ModelColorPhoto>>(objAllPhotosEntity);
        }

        private static IEnumerable<VersionDetail> ConvertBikeVersionToVersionDetail(IEnumerable<BikeVersionMinSpecs> versionList)
        {
            try
            {
                if (versionList != null && versionList.Any())
                {
                    IList<VersionDetail> versionDetailList = new List<VersionDetail>();
                    VersionDetail objBikeVersionDetail;
                    foreach (BikeVersionMinSpecs bikeVersion in versionList)
                    {
                        objBikeVersionDetail = SpecsFeaturesMapper.ConvertToVersionDetail(bikeVersion);
                        versionDetailList.Add(objBikeVersionDetail);
                    }
                    return versionDetailList;
                }
            }
            catch (Exception) { }
            return null;
        }
        /// <summary>
        /// Method for supporting displayName field for specsCategory in api/model/bikespecs/ 
        /// on which icons setting condition is applied
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private static string GetCategoryDisplayName(int currentIndex)
        {
            if (currentIndex >= 0 && currentIndex < _categoryDisplayNameList.Count)
                return _categoryDisplayNameList[currentIndex];
            else
                return "";
        }

    }
}