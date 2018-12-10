using Carwale.DTOs.Autocomplete;
using Carwale.DTOs.CarData;
using Carwale.Entity;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Customer;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Carwale.BL.NewCars
{
    public class VersionPageAdapterAndroidV1 : IServiceAdapterV2
    {
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly INewCarDealers _newCarDealersBL;
        private readonly ICarPriceQuoteAdapter _carPrices;
        private readonly ICarDataLogic _carDataLogic;
        private readonly ICarVersions _carVersionsBl;
        private readonly ICustomerTracking _trackingBL;

        private static readonly string _defaultCityName = ConfigurationManager.AppSettings["DefaultCityName"] ?? string.Empty;
        private static readonly string _defaultCityId = ConfigurationManager.AppSettings["DefaultCityId"] ?? "10";
        public static readonly string _apiHostUrl = ConfigurationManager.AppSettings["WebApiHostUrl"] ?? string.Empty;

        public VersionPageAdapterAndroidV1(ICarVersionCacheRepository carVersionsCacheRepo,
            ICarModelCacheRepository carModelsCacheRepo, ICarDataLogic carDataLogic,
            ICarVersions carVersionsBl, INewCarDealers newCarDealersBL, ICarPriceQuoteAdapter carPrices,
            ICustomerTracking trackingBL)
        {
            _carVersionsCacheRepo = carVersionsCacheRepo;
            _carModelsCacheRepo = carModelsCacheRepo;
            _newCarDealersBL = newCarDealersBL;
            _carPrices = carPrices;
            _carDataLogic = carDataLogic;
            _carVersionsBl = carVersionsBl;
            _trackingBL = trackingBL;
        }
        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetVersionPageDtoForAndroid(input), typeof(T));
        }

        private VersionDetailsDTO_AndroidV1 GetVersionPageDtoForAndroid<U>(U input)
        {
            VersionDetailsDTO_AndroidV1 versionDetailsResponse = new VersionDetailsDTO_AndroidV1();
            //Returns the Car Version details
            try
            {
                VersionAndroidRequest inputParam = (VersionAndroidRequest)Convert.ChangeType(input, typeof(U));
                var versionDetail = _carVersionsCacheRepo.GetVersionDetailsById(inputParam.VersionId);
                if (versionDetail != null)
                {
                    var modelDetails = _carModelsCacheRepo.GetModelDetailsById(versionDetail.ModelId);
                    List<int> versionList = new List<int> { inputParam.VersionId };
                    CarDataPresentation versionData = _carDataLogic.GetCombinedCarData(versionList)?.First();
                    var versionColors = _carVersionsBl.GetVersionsColors(versionList)?.First();
                    var otherCarVersions = _carVersionsCacheRepo.GetOtherCarVersionsOfModel(inputParam.VersionId);
                    //Populates Version details
                    PopulateVersionDetails(versionDetail, modelDetails, versionDetailsResponse);


                    if (versionData != null && versionData.Overview.IsNotNullOrEmpty())
                    {
                        //Populates overview of version
                        PopulateOverview(versionData, versionDetailsResponse);
                    }
                    if (versionData != null && versionData.Specifications.IsNotNullOrEmpty())
                    {
                        //Populates specifications of version
                        PopulateSpecs(versionData, versionDetailsResponse);
                    }
                    if (versionData != null && versionData.Features.IsNotNullOrEmpty())
                    {
                        //Populates features of version
                        PopulateFeatures(versionData, versionDetailsResponse);
                    }
                    //Populates Colors of version
                    PopulateColors(versionColors, versionDetailsResponse);
                    //Populates other version of the CarModel
                    PopulateOtherVersions(otherCarVersions, versionDetailsResponse);
                    PopulateApiUrl(versionDetail, versionDetailsResponse);
                    PopulateTrackingData(inputParam, versionDetailsResponse);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "VersionPageAdapterAndroidV1.GetVersionPageDtoForAndroid()");
            }
            return versionDetailsResponse;
        }

        private void PopulateApiUrl(CarVersionDetails versionDetail, VersionDetailsDTO_AndroidV1 versionDetailsResponse)
        {
            try
            {
                if (versionDetail.ModelId > 0)
                {
                    versionDetailsResponse.OnRoadPriceVersionCityUrl = _apiHostUrl + "PqVersionsAndCities/?modelId=" + versionDetail.ModelId;
                    versionDetailsResponse.ReviewUrl = _apiHostUrl + "UserReviews/?modelId=" + versionDetail.ModelId
                        + "&versionId=" + versionDetail.VersionId + "&pageNo=1&pageSize=10&sortCriteria=1";
                    versionDetailsResponse.NewCarPhotoUrl = _apiHostUrl + "NewCarPhotos/?modelId=" + versionDetail.ModelId + "&categoryId=-1";
                    versionDetailsResponse.NewCarGalleryUrl = _apiHostUrl.Replace("/api/", "/webapi/") +
                        "CarModeldata/Gallery/?applicationid=1&modelid=" + versionDetail.ModelId +
                        "&categoryidlist=8,10&totalrecords=500";
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "PopulateApiUrl()");
            }
        }

        private void PopulateTrackingData(VersionAndroidRequest inputParam, VersionDetailsDTO_AndroidV1 versionDetailsResponse)
        {
            try
            {
                int platformId = CustomParser.parseIntObject(HttpContext.Current.Request.Headers["sourceid"]);
                if (platformId == (int)Platform.CarwaleAndroid || platformId == (int)Platform.CarwaleiOS)
                {
                    var carDataTrackingEntity = new CarDataTrackingEntity();
                    carDataTrackingEntity.ModelId = CustomParser.parseIntObject(versionDetailsResponse.ModelId);
                    carDataTrackingEntity.Location.CityId = 0;
                    carDataTrackingEntity.Platform = CustomParser.parseIntObject(HttpContext.Current.Request.Headers["sourceid"]);
                    carDataTrackingEntity.VersionId = inputParam.VersionId;
                    carDataTrackingEntity.Category = "VersionPage";
                    carDataTrackingEntity.Action = "VersionImpression";

                    _trackingBL.AppsTrackModelVersionImpression(carDataTrackingEntity, null);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "PopulateTrackingData()");
            }
        }

        private void PopulateVersionDetails(CarVersionDetails versionDetails, CarModelSummary modelDetails, VersionDetailsDTO_AndroidV1 versionDetailsResponse)
        {
            try
            {
                versionDetailsResponse.MakeId = versionDetails.MakeId.ToString();
                versionDetailsResponse.ModelId = versionDetails.ModelId.ToString();
                versionDetailsResponse.MakeName = versionDetails.MakeName;
                versionDetailsResponse.ModelName = versionDetails.ModelName;
                versionDetailsResponse.VersionName = versionDetails.VersionName;
                versionDetailsResponse.VersionId = versionDetails.VersionId.ToString();
                versionDetailsResponse.LargePicUrl = versionDetails.ModelImageLarge;
                versionDetailsResponse.SmallPicUrl = versionDetails.ModelImageSmall;
                versionDetailsResponse.ReviewRate = Format.GetAbsReviewRate(modelDetails?.ModelRating ?? 0);
                versionDetailsResponse.ReviewCount = modelDetails?.ReviewCount.ToString();
                versionDetailsResponse.ExShowroomCity = _defaultCityName;
                versionDetailsResponse.ExShowroomPrice = Format.GetPrice(versionDetails.MinPrice.ToString());
                versionDetailsResponse.ShareUrl = ManageCarUrl.CreateVersionUrl(versionDetails.MakeName, versionDetails.MaskingName, versionDetails.VersionMasking, isAbsoluteUrl: true);
                versionDetailsResponse.CallSlugNumber = _newCarDealersBL.CallSlugNumberByMakeId(versionDetails.MakeId);
                versionDetailsResponse.HostUrl = versionDetails.HostURL;
                versionDetailsResponse.OriginalImgPath = versionDetails.OriginalImgPath;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "PopulateVersionDetails");
            }
        }

        private void PopulateOverview(CarDataPresentation versionData, VersionDetailsDTO_AndroidV1 versionDetailsResponse)
        {
            try
            {
                VersionSpecsFeatures category = new VersionSpecsFeatures();
                category.Name = "Overview";
                category.CategoryData = new List<LabelValueDTO>();

                versionDetailsResponse.Overview = new VersionSpecsFeaturesTab();
                versionDetailsResponse.Overview.Categories = new List<VersionSpecsFeatures>();

                for (int i = 0; i < versionData.Overview.Count; i++)
                {
                    category.CategoryData.Add(new LabelValueDTO
                    {
                        Label = Regex.Replace(versionData.Overview[i].Name, "~.*?~", string.Empty),
                        Value = (versionData.Overview[i]?.Value.Replace("~", "")) + " " + versionData.Overview[i].UnitTypeName
                    });
                }

                versionDetailsResponse.Overview.Categories.Add(category);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "PopulateOverview");
            }
        }

        private void PopulateSpecs(CarDataPresentation versionData, VersionDetailsDTO_AndroidV1 versionDetailsResponse)
        {
            try
            {
                VersionSpecsFeatures specCategory;
                versionDetailsResponse.Specifications = new VersionSpecsFeaturesTab();
                versionDetailsResponse.Specifications.Categories = new List<VersionSpecsFeatures>();

                for (int i = 0; i < versionData.Specifications.Count; i++)
                {
                    specCategory = new VersionSpecsFeatures();
                    specCategory.CategoryData = new List<LabelValueDTO>();
                    specCategory.Name = versionData.Specifications[i].CategoryName;

                    for (int j = 0; j < versionData.Specifications[i].Items.Count; j++)
                    {
                        specCategory.CategoryData.Add(new LabelValueDTO
                        {
                            Label = Regex.Replace(versionData.Specifications[i].Items[j].Name, "~.*?~", string.Empty),
                            Value = string.Format("{0} {1}", versionData.Specifications[i].Items[j].Value.Replace("~", ""), versionData.Specifications[i].Items[j].UnitTypeName)
                        });
                    }
                    versionDetailsResponse.Specifications.Categories.Add(specCategory);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "PopulateSpecs");
            }
        }

        private void PopulateFeatures(CarDataPresentation versionData, VersionDetailsDTO_AndroidV1 versionDetailsResponse)
        {
            try
            {
                VersionSpecsFeatures featureCategory;
                versionDetailsResponse.Features = new VersionSpecsFeaturesTab();
                versionDetailsResponse.Features.Categories = new List<VersionSpecsFeatures>();

                for (int i = 0; i < versionData.Features.Count; i++)
                {
                    featureCategory = new VersionSpecsFeatures();
                    featureCategory.CategoryData = new List<LabelValueDTO>();


                    featureCategory.Name = versionData.Features[i].CategoryName;
                    for (int j = 0; j < versionData.Features[i].Items.Count; j++)
                    {
                        featureCategory.CategoryData.Add(new LabelValueDTO
                        {
                            Label = (Regex.Replace(versionData.Features[i].Items[j].Name, "~.*?~", String.Empty)),
                            Value = (versionData.Features[i].Items[j].Value.Replace("~", ""))
                        });
                    }
                    versionDetailsResponse.Features.Categories.Add(featureCategory);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "PopulateFeatures");
            }
        }

        ///// <summary>
        ///// Populates colors of the Version
        ///// </summary>
        private void PopulateColors(List<Carwale.Entity.CompareCars.Color> versionColor, VersionDetailsDTO_AndroidV1 versionDetailsResponse)
        {
            try
            {
                VersionSpecsFeatures categoryColors;

                categoryColors = new VersionSpecsFeatures();
                categoryColors.Name = "Colors";
                categoryColors.CategoryData = new List<LabelValueDTO>();

                versionDetailsResponse.Colors = new VersionSpecsFeaturesTab();
                versionDetailsResponse.Colors.Categories = new List<VersionSpecsFeatures>();

                if (versionColor != null)
                {
                    for (int i = 0; i < versionColor.Count; i++)
                    {
                        categoryColors.CategoryData.Add(new LabelValueDTO { Label = versionColor[i].Name, Value = versionColor[i].Value });
                    }
                }
                versionDetailsResponse.Colors.Categories.Add(categoryColors);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "PopulateColors");
            }
        }

        ///// <summary>
        ///// Populates Other Versions of the Model
        ///// </summary>
        private void PopulateOtherVersions(List<CarVersions> otherCarVersions, VersionDetailsDTO_AndroidV1 versionDetailsResponse)
        {
            try
            {
                if (otherCarVersions.IsNotNullOrEmpty())
                {
                    var otherVersionList = otherCarVersions.Select(c => c.Id).ToList();
                    var versionsPrice = _carPrices.GetVersionsPriceForSameModel(Convert.ToInt32(versionDetailsResponse.ModelId), otherVersionList, Convert.ToInt32(_defaultCityId));
                    otherCarVersions.ForEach(c =>
                    {
                        PriceOverview carversionprice;
                        versionsPrice.TryGetValue(c.Id, out carversionprice);
                        c.MinPrice = (carversionprice != null && carversionprice.PriceStatus == (int)PriceBucket.HaveUserCity ? carversionprice.Price : 0);
                    });
                    CarVersionDTO_V2 otherVersionDetails;
                    versionDetailsResponse.OtherVersions = new List<CarVersionDTO_V2>();

                    foreach (var otherversion in otherCarVersions)
                    {
                        otherVersionDetails = new CarVersionDTO_V2
                        {
                            Id = otherversion.Id.ToString(),
                            Name = otherversion.Version,
                            VersionUrl = _apiHostUrl + "versiondetails/?versionid=" + otherversion.Id.ToString(),
                            SpecsSummary = otherversion.SpecsSummary,
                            Price = ((otherversion.MinPrice <= 0) ? "N/A" : Format.GetPrice(otherversion.MinPrice.ToString()))
                        };
                        versionDetailsResponse.OtherVersions.Add(otherVersionDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "PopulateOtherVersions");
            }
        }

    }

}
