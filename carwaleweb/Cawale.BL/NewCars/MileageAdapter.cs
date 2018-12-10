using Carwale.BL.CMS;
using Carwale.BL.Experiments;
using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.Entity;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Carwale.BL.NewCars
{

    public class MileageAdapter : IServiceAdapterV2
    {
        private readonly ICarModelCacheRepository _carModelsCacheRepo;
        private readonly ICarModels _carModelsBL;
        private readonly ICMSContent _cmsContentCacheRepo;
        private readonly ICarMileage _carMileage;
        private readonly ICarVersions _carVersionBL;
        private readonly ICarPriceQuoteAdapter _carPrices;
        private readonly static ushort _expertReviewsDesktopCount = Convert.ToUInt16(ConfigurationManager.AppSettings["ExpertReviewsDesktop_Count"] ?? "1");

        public MileageAdapter(ICarVersions carVersionBL, ICarModels carModelsBL, ICarModelCacheRepository carModelsCacheRepo, ICMSContent cmsContentCacheRepo, ICarMileage carMileage, ICarPriceQuoteAdapter carPrices)
        {
            _carVersionBL = carVersionBL;
            _carModelsBL = carModelsBL;
            _carModelsCacheRepo = carModelsCacheRepo;
            _cmsContentCacheRepo = cmsContentCacheRepo;
            _carMileage = carMileage;
            _carPrices = carPrices;
        }

        public T Get<T, U>(U input)
        {
            return (T)Convert.ChangeType(GetMileageDto(input), typeof(T));
        }

        private MileagePageDTO GetMileageDto<U>(U input)
        {
            try
            {
                CarDataAdapterInputs inputParam = (CarDataAdapterInputs)Convert.ChangeType(input, typeof(U));

                var modelDetails = _carModelsCacheRepo.GetModelDetailsById(inputParam.ModelDetails.ModelId);
                var versionList = _carVersionBL.GetCarVersions(inputParam.ModelDetails.ModelId, Status.New);
                List<int> versionIds = versionList.Select(x => x.Id).ToList();
                var similarCarVmRequest = new SimilarCarVmRequest
                {
                    ModelId = modelDetails.ModelId,
                    MakeName = modelDetails.MakeName,
                    ModelName = modelDetails.ModelName,
                    MaskingName = modelDetails.MaskingName,
                    CityId = inputParam.CustLocation.CityId,
                    WidgetSource = WidgetSource.MileagePageAlternativesDesktop,
                    PageName = "MileagePage",
                    IsMobile = inputParam.IsMobile,
                    CwcCookie = inputParam.CwcCookie
                };

                MileagePageDTO dto = new MileagePageDTO
                {
                    ModelDetails = modelDetails,
                    MileageData = _carMileage.GetMileageData(versionList),
                    BreadcrumbEntitylist = BindBreadCrumb(modelDetails, inputParam.IsMobile),
                    SimilarCars = _carModelsBL.GetSimilarCarVm(similarCarVmRequest)
                };
                dto.PageMetaTags = GetPageMetaTags(inputParam.ModelDetails.ModelId, modelDetails.MakeName, modelDetails.ModelName, modelDetails.MaskingName, dto.MileageData);
                if (!inputParam.IsMobile)
                {
                    dto.SubNavigation = BindModelQuickMenu(dto, inputParam);
                }

                dto.SimilarCars.PQPageId = 50;
                var versionPriceDictionary = _carPrices.GetVersionsPriceForSameModel(dto.ModelDetails.ModelId, versionIds, inputParam.CustLocation.CityId, true);
                if (versionPriceDictionary.IsNotNullOrEmpty())
                {
                    int maxPrice = Int32.MinValue;
                    int minPrice = Int32.MaxValue;
                    foreach (var version in versionPriceDictionary)
                    {
						if (version.Value != null)
						{
							minPrice = version.Value.Price > 0 && version.Value.Price < minPrice ? version.Value.Price : minPrice;
							maxPrice = version.Value.Price > maxPrice ? version.Value.Price : maxPrice;
						}
                    }
                    dto.ModelDetails.MinPrice = minPrice != Int32.MaxValue ? minPrice : dto.ModelDetails.MinPrice;
                    dto.ModelDetails.MaxPrice = maxPrice != Int32.MinValue ? maxPrice : dto.ModelDetails.MaxPrice;
                }
                dto.Schema = CreateJsonLdJObject(dto, versionList); 
                return dto;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "MileageAdapter");
            }

            return null;
        }

        private PageMetaTags GetPageMetaTags(int modelId, string makeName, string modelName, string maskingName, List<MileageDataEntity> mileageData)
        {
            PageMetaTags pageMetaTags = null;
            try
            {
                pageMetaTags = _carModelsCacheRepo.GetModelPageMetaTags(modelId, Convert.ToInt16(Pages.ModelPageId));
                pageMetaTags.Title = string.Format("{0} {1} Mileage - {1} Mileage in India", makeName, modelName);
                if (mileageData.IsNotNullOrEmpty())
                {
                    MileageDataEntity minMileage = mileageData.Min();
                    MileageDataEntity maxMileage = mileageData.Max();
                    pageMetaTags.Description = GetDescription(makeName, modelName, minMileage,maxMileage);
                    pageMetaTags.Summary = GetMileagePageSummary(makeName, modelName, minMileage, maxMileage, mileageData);
                }
                else
                {
                    pageMetaTags.Description = string.Format("Check out mileage of {0} {1} on CarWale.",
                                                makeName, modelName);
                }
                pageMetaTags.Heading = string.Format("{0} {1} Mileage", makeName, modelName);
                pageMetaTags.Canonical = ManageCarUrl.CreateModelUrl(makeName, maskingName, true) + "mileage/";
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
            }
            return pageMetaTags;
        }

        private static string GetDescription(string makeName, string modelName,MileageDataEntity minMileage,MileageDataEntity maxMileage)
        {
            string descriptionFormat = string.Empty;
            if (minMileage.Arai == maxMileage.Arai)
            {
                descriptionFormat = "{0} {1} mileage is {2} {3}. Check out mileage of {0} {1} for petrol and diesel variants on CarWale.";
            }
            else
            {
                descriptionFormat = "{0} {1} mileage is {2} {3} to {4} {5}. Check out mileage of {0} {1} for petrol and diesel variants on CarWale.";
            }
            return string.Format(descriptionFormat, makeName, modelName, minMileage.Arai, minMileage.MileageUnit, maxMileage.Arai, maxMileage.MileageUnit); 

        }
        private static string GetMileagePageSummary(string makeName, string modelName,MileageDataEntity minMileage,MileageDataEntity maxMileage,List<MileageDataEntity> mileageData)
        {
            StringBuilder summary = new StringBuilder();
            if (minMileage.Arai == maxMileage.Arai)
            {
                summary.Append(string.Format("As per ARAI, the {0} {1} mileage is {2} {3}.", makeName, modelName, minMileage.Arai, minMileage.MileageUnit));
            }
            else
            {
                summary.Append(string.Format("As per ARAI, the {0} {1} mileage is {2} {3} to {4} {5}.",
                                            makeName, modelName, minMileage.Arai, minMileage.MileageUnit, maxMileage.Arai, maxMileage.MileageUnit));
            }
            string fuelTansimitionSeoText = " The {0} {1} variant {2} has a mileage of {3} {4}.";
            foreach (var mileage in mileageData)
            {
                summary.Append(string.Format(fuelTansimitionSeoText, mileage.Transmission, mileage.FuelType, !string.IsNullOrEmpty(mileage.Displacement) ? string.Format("for {0} engine", mileage.Displacement) : string.Empty, mileage.Arai, mileage.MileageUnit));
            }
            return summary.ToString();
        }
        private List<BreadcrumbEntity> BindBreadCrumb(CarModelDetails modelDetails, bool isMobile)
        {
            try
            {
                string makeName = Format.FormatSpecial(modelDetails.MakeName);
                List<BreadcrumbEntity> _BreadcrumbEntitylist = new List<BreadcrumbEntity>();
                _BreadcrumbEntitylist.Add(new BreadcrumbEntity
                {
                    Title = string.Format("{0} Cars",
                    modelDetails.MakeName),
                    Link = string.Format("{0}/{1}-cars/", isMobile ? "/m" : string.Empty, makeName),
                    Text = modelDetails.MakeName
                });

                _BreadcrumbEntitylist.Add(new BreadcrumbEntity
                {
                    Title = string.Format("{0}",
                    modelDetails.ModelName),
                    Link = string.Format("/{0}-cars/{1}/", makeName, modelDetails.MaskingName),
                    Text = modelDetails.ModelName
                });

                _BreadcrumbEntitylist.Add(new BreadcrumbEntity { Text = "Mileage" });

                return _BreadcrumbEntitylist;
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        private SubNavigationDTO BindModelQuickMenu(MileagePageDTO dto, CarDataAdapterInputs inputParam)
        {
            SubNavigationDTO subNavDto = null;
            try
            {
                string label = "make:" + dto.ModelDetails.MakeName + "|model:" +
                    dto.ModelDetails.ModelName + "|city:" + inputParam.CustLocation.ZoneId;
                var ExpertReviewsByModel = _cmsContentCacheRepo.GetExpertReviewByModel(dto.ModelDetails.ModelId, _expertReviewsDesktopCount);
                subNavDto = _carModelsBL.GetModelQuickMenu(dto.ModelDetails, null,
                    false, (ExpertReviewsByModel != null && ExpertReviewsByModel.Count > 0), "MileagePage", label);
                subNavDto.IsModelPage = false;
                subNavDto.PQPageId = 48;
                subNavDto.Page = Pages.Mileage;
                subNavDto.ShowColorsLink = CMSCommon.IsModelColorPhotosPresent(dto.ModelDetails.ModelColors);
                subNavDto.IsMileagePage = true;

            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
            }
            return subNavDto;
        }

        private JObject CreateJsonLdJObject(MileagePageDTO mileagePageDTO, List<CarVersions> versionData)
        {
            try
            {
                if(!mileagePageDTO.MileageData.IsNotNullOrEmpty())
                {
                    return null;
                }
                    ModelSchema modelSchema = new ModelSchema
                    {
                        ModelDetails = mileagePageDTO.ModelDetails,
                        ModelColors = _carModelsCacheRepo.GetModelColorsByModel(mileagePageDTO.ModelDetails.ModelId) ?? new List<ModelColors>(),
                        PageMetaTags = mileagePageDTO.PageMetaTags,
                        MileageData = mileagePageDTO.MileageData,
                        SeatingCapacity = versionData.Select(x => x.SeatingCapacity).ToList(),
                        Drivetrain = versionData.Select(x => x.Drivetrain).ToList(),
                     };
                    return _carModelsBL.CreateJsonLdJObject(modelSchema, mileagePageDTO.SimilarCars?.SimilarCarModels, true);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }
    }
}
