using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using AutoMapper;
using Carwale.BL.CMS;
using Carwale.BL.Experiments;
using Carwale.DTOs.CarData;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.VehicleData;
using Carwale.Entity.ViewModels.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.PriceQuote;
using Carwale.Interfaces.SponsoredCar;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Carwale.BL.CarData
{
    /// <summary>
    /// Created By : Shalini
    /// </summary>
    public class CarModelsBL : ICarModels
    {

        private readonly ICarModelCacheRepository _modelsCacheRepo;
        private readonly ICarPriceQuoteAdapter _prices;
        protected readonly ICarVersions _carVersionsBL;
        protected readonly IPhotos _carPhotosBL;
        private readonly IUnityContainer _container;
        private readonly ISponsoredCarBL _sponsoredBL;
        private readonly ICarDataLogic _carDataLogicBL;
        private readonly ICarRecommendationLogic _carRecommendationLogic;
        private static string _imgHostUrl = CWConfiguration._imgHostUrl;
        private readonly int similarModelsCount = Int32.Parse(ConfigurationManager.AppSettings["SimilarModelsDefaultCount"]);
        private readonly int similarModelsForUpcomingCount = Int32.Parse(ConfigurationManager.AppSettings["SimilarModelsForUpcomingDefaultCount"]);

        public CarModelsBL(IUnityContainer container, ICarModelCacheRepository modelsCacheRepo, ICarVersions carVersions,
            IPhotos carPhotosBL, ISponsoredCarBL sponsoredBL, ICarDataLogic carDataLogicBL, ICarPriceQuoteAdapter carPrices, ICarRecommendationLogic carRecommendationLogic)
        {
            _modelsCacheRepo = modelsCacheRepo;
            _prices = carPrices;
            _container = container;
            _carVersionsBL = carVersions;
            _carPhotosBL = carPhotosBL;
            _sponsoredBL = sponsoredBL;
            _carDataLogicBL = carDataLogicBL;
            _carRecommendationLogic = carRecommendationLogic;
        }
        /// <summary>
        /// Returns the List of New Cars 
        /// Written By : Shalini Nair on 19/09/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>List of New CarModels </returns>
        public List<CarModelSummary> GetNewModelsByMake(int makeId, int dealerId = 0)
        {
            var newModelsList = new List<CarModelSummary>();
            try
            {
                newModelsList = _modelsCacheRepo.GetModelsByMake(makeId, dealerId).FindAll(x => x.New) ?? new List<CarModelSummary>();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return newModelsList;
        }

        /// <summary>
        /// Returns the List of Discontinued Cars
        /// Written By : Shalini Nair on 19/09/14
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>List of Discontinued CarModels</returns>
        public List<CarModelSummary> GetDiscontinuedModelsByMake(int makeId)
        {
            var discontinuedModelsList = new List<CarModelSummary>();
            try
            {
                discontinuedModelsList = _modelsCacheRepo.GetModelsByMake(makeId).FindAll(x => !x.New);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return discontinuedModelsList;
        }

        /// <summary>
        /// Sets IsFeatured=1 for Sponsored Car
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>List of SimilarCar models</returns>
        public List<SimilarCarModels> GetSimilarCarsByModel(int modelId, string userIdentifier)
        {
            List<SimilarCarModels> similarCars = new List<SimilarCarModels>();
            try
            {
                var similarCarRequest = new SimilarCarRequest
                {
                    CarId = modelId,
                    Count = similarModelsCount,
                    UserIdentifier = userIdentifier,
                    IsBoost = true
                };

                var similarCarList = _carRecommendationLogic.GetSimilarCarsByModel(similarCarRequest);
                if (similarCarList != null)
                {
                    foreach (var x in similarCarList)
                    {
                        x.IsFeatured = x.FeaturedModelId == x.ModelId ? 1 : 0; // Check if its a featured car
                        if (x.New)
                        {
                            similarCars.Add(x);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return similarCars ?? new List<SimilarCarModels>();
        }

        /// <summary>
        /// To fetch similars along with prices 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="isCityPage"></param>
        /// <returns>List of SimilarCar models with prices</returns>
        public SimilarCarsDTO GetSimilarCarVm(SimilarCarVmRequest similarCarVmRequest)
        {
            SimilarCarsDTO similarCars = new SimilarCarsDTO
            {
                SourceModelName = similarCarVmRequest.ModelName,
                SourceModelId = similarCarVmRequest.ModelId,
                WidgetPageSource = (int)similarCarVmRequest.WidgetSource,
                PageName = similarCarVmRequest.PageName
            };
            try
            {
                List<SimilarCarModels> similarCarsModels = null;
                similarCarsModels = GetSimilarCarsByModel(similarCarVmRequest.ModelId, similarCarVmRequest.CwcCookie);
                //to get price of similar cars
                var modellist = similarCarsModels != null ? similarCarsModels.Select(x => x.ModelId).ToList() : new List<int>();
                //to get price of similar cars                    
                IDictionary<int, PriceOverview> similarModelPrices = _prices.GetModelsCarPriceOverview(modellist, similarCarVmRequest.CityId, true);
                Tuple<int, string> modelTuple = new Tuple<int, string>(similarCarVmRequest.ModelId, (Format.FormatSpecial(similarCarVmRequest.MakeName) + '-' + similarCarVmRequest.MaskingName));
                similarCarsModels?.ForEach(x =>
                {
                    var priceOverview = similarModelPrices[x.ModelId];
                    x.PricesOverview = (priceOverview ?? new PriceOverview());
                    if (!similarCarVmRequest.IsFuturistic)
                    {
                        List<Tuple<int, string>> compareCarsTupleList = new List<Tuple<int, string>> {
                        modelTuple,
                        new Tuple<int, string>(x.ModelId, (Format.FormatSpecial(x.MakeName)+'-'+ x.MaskingName))
                        };
                        string formatCompareUrl = Format.GetCompareUrl(compareCarsTupleList);
                        x.CompareCarUrl = string.Format("{0}/comparecars/{1}",
                            (similarCarVmRequest.IsMobile ? "/m" : string.Empty), !string.IsNullOrWhiteSpace(formatCompareUrl) ? formatCompareUrl + "/" : string.Empty);
                    }
                });
                similarCars.SimilarCarModels = Mapper.Map<List<SimilarCarModels>, List<SimilarCarModelsDTOV2>>(similarCarsModels);
                similarCars.SimilarUpcomingCar = _modelsCacheRepo.GetSimilarUpcomingCarModel(similarCarVmRequest.ModelId) ?? new UpcomingCarModel();
                return similarCars;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            similarCars.SimilarCarModels = new List<SimilarCarModelsDTOV2>();
            return similarCars;
        }

        /// <summary>
        /// Returns car model specs 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<CarModelSpecs> GetProcessedCarModelSpecs(IEnumerable<int> versionIds, int modelId)
        {
            var carSpecsList = new List<CarModelSpecs>();
            try
            {
                carSpecsList = _carDataLogicBL.GetCarModelSpecs(versionIds, modelId);
                if (carSpecsList != null)
                {
                    foreach (var x in carSpecsList)
                    {
                        x.ItemValue = x.ItemValue.Replace("~", "");  // Formatting the data 
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return carSpecsList;
        }

        public ModelDataSummary GetProcessedCarModelDataSummary(List<int> versionIds, int modelId)
        {
            var modelData = new ModelDataSummary();
            try
            {
                modelData = _carDataLogicBL.GetCarModelDataSummary(versionIds, modelId);
                if (modelData != null)
                {
                    if (modelData.ModelFeature == null || modelData.ModelFeature.Count <= 0)
                    {
                        foreach (var x in modelData.SpecsSummary)
                        {
                            x.ItemValue = x.ItemValue.Replace("~", "");  // Formatting the data 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelData;
        }

        public string Heading(string headingText, string makeName, string modelName)
        {
            return (!string.IsNullOrEmpty(headingText) ? headingText : $"{makeName} {modelName}");
        }


        public string Summary(string summaryText, string makeName, string modelName, int[] processVersionData,
            List<DTOs.NewCars.NewCarVersionsDTO> versions, bool isCityPage, string cityName)
        {
            StringBuilder summaryResult = new StringBuilder();
            List<DTOs.NewCars.NewCarVersionsDTO> consolidatedList;
            string cityText = (isCityPage ? " in " + cityName : "");
            try
            {
                if (!string.IsNullOrEmpty(summaryText))
                {
                    return summaryText;
                }
                if (versions.IsNotNullOrEmpty())
                {
                    var priorityBucketVersions = versions.Where(x => x.CarPriceOverview != null && x.CarPriceOverview.Price > 0).ToList();
                    int priceStatus = versions.First().CarPriceOverview != null ? versions.First().CarPriceOverview.PriceStatus : (int)PriceBucket.NoUserCity;
                    priorityBucketVersions = priorityBucketVersions.Where(x => x.CarPriceOverview.PriceStatus == priceStatus).ToList();
                    if (priorityBucketVersions.IsNotNullOrEmpty())
                    {
                        consolidatedList = versions.GroupBy(x => x.CarFuelType).Select(group => group.First()).ToList();
                        summaryResult.Append(string.Format("{0} {1} price{2} starts at ₹ {3}{4}. ", makeName, modelName,
                            cityText, Format.GetFormattedPriceV2(priorityBucketVersions.First().CarPriceOverview.Price.ToString()),
                            (priorityBucketVersions.Count > 1 ? " and goes upto ₹ " + Format.GetFormattedPriceV2(priorityBucketVersions.Last().CarPriceOverview.Price.ToString()) : "")));

                        foreach (var version in consolidatedList)
                        {
                            summaryResult.Append(string.Format("{0} {1} price{2} starts at ₹ {3}. ", version.CarFuelType,
                                modelName, cityText, Format.GetFormattedPriceV2(version.CarPriceOverview.Price.ToString())));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarModelsBL.Summary");
            }
            return summaryResult.ToString();
        }

        /// <summary>
        /// Returns the Title for Model Details page 
        /// </summary>
        /// <param name="title">Title from Page Meta Tags</param>
        /// <param name="makeName"></param>
        /// <param name="modelName"></param>
        /// <param name="futuristic"></param        /// <returns></returns>
        public string Title(string titleText, string makeName, string modelName)
        {
            string processedTitle = string.Empty;
            // title from Page meta tags 
            processedTitle = (!string.IsNullOrEmpty(titleText) ? titleText : $"{makeName} {modelName} Price (GST Rates), Images, Mileage, Colours");
            return Regex.Replace(processedTitle, "maruti suzuki", "Maruti", RegexOptions.IgnoreCase);
        }

        public string GetDescription(string makeName, string modelName, string modelPrice)
        {
            return string.Format("{0} {1} Price (GST Rates) in India starts at ₹ {2}. Check out {0} {1} Colours, Review, Images and {1} Variants On Road Price at Carwale.com.",
                                    makeName, modelName, modelPrice);
        }


        #region FRQ - Frequently Requested Queries
        /// <summary>
        /// Written By : Ashwini Todkar on 24 July 2015
        /// Method to get pagewise top selling car models
        /// </summary>
        /// <param name="page">pageno = current page,pagesize = number of records for that page</param>
        /// <returns></returns>
        public List<TopSellingCarModel> GetTopSellingCarModels(Pagination page, int cityId, bool orp = false)
        {
            List<TopSellingCarModel> topSell = null;
            try
            {
                var allCar = _modelsCacheRepo.GetTopSellingModels(50);

                ushort _startIndex;
                ushort _endIndex;
                Utility.Calculation.GetStartEndIndex(page.PageNo, page.PageSize, out _startIndex, out _endIndex);
                topSell = allCar.Where((x, i) => i >= _startIndex - 1 && i < _endIndex).ToList<TopSellingCarModel>();
                if (topSell != null)
                {
                    var modelPrices = _prices.GetModelsCarPriceOverview(topSell.Select(x => x.Model.ModelId).Distinct().ToList(), cityId, orp);
                    if (modelPrices != null)
                    {
                        topSell.ForEach(cc =>
                        {
                            PriceOverview versionPriceOverview;
                            modelPrices.TryGetValue(cc.Model.ModelId, out versionPriceOverview);
                            cc.PriceOverview = (versionPriceOverview ??
                            new PriceOverview());
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return topSell;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 24 July 2015
        /// Method to get pagewise top newly launched car models
        /// </summary>
        /// <param name="page">pageno = current page,pagesize = number of records for that page</param>
        /// <returns></returns>
        public List<LaunchedCarModel> GetLaunchedCarModels(Pagination page, int cityId, bool orp = false)
        {
            List<LaunchedCarModel> topLaunches = null;
            try
            {
                var allCar = _modelsCacheRepo.GetLaunchedCarModels(50);

                ushort _startIndex;
                ushort _endIndex;
                Utility.Calculation.GetStartEndIndex(page.PageNo, page.PageSize, out _startIndex, out _endIndex);

                topLaunches = allCar.Where((x, i) => i >= _startIndex - 1 && i < _endIndex).ToList<LaunchedCarModel>();

                if (topLaunches != null)
                {

                    var modelPrices = _prices.GetModelsCarPriceOverview(topLaunches.Select(x => x.Model.ModelId).Distinct().ToList(), cityId, orp);
                    if (modelPrices != null && modelPrices.Count > 0)
                    {
                        topLaunches.ForEach(cc =>
                        {
                            PriceOverview modelPriceOverview;
                            modelPrices.TryGetValue(cc.Model.ModelId, out modelPriceOverview);
                            cc.PriceOverview = (modelPriceOverview ?? new PriceOverview());
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return topLaunches;
        }



        public List<LaunchedCarModel> GetLaunchedCarModelsV1(Pagination page, int cityId, bool orp = false)
        {
            List<LaunchedCarModel> topLauches = null;
            try
            {
                var allCar = _modelsCacheRepo.GetLaunchedCarModelsV1();

                ushort _startIndex;
                ushort _endIndex;
                Utility.Calculation.GetStartEndIndex(page.PageNo, page.PageSize, out _startIndex, out _endIndex);

                topLauches = allCar.Where((x, i) => i >= _startIndex - 1 && i < _endIndex).ToList<LaunchedCarModel>();

                if (topLauches != null)
                {

                    var modelPrices = _prices.GetModelsCarPriceOverview(
                                        topLauches.Where(x => (x.Version == null || x.Version.VersionId <= 0)).Select(x => x.Model.ModelId).Distinct().ToList(),
                                        cityId, orp);
                    var versionPrices = _prices.GetVersionsPriceForDifferentModel(topLauches.Where(x => x.Version != null
                    && x.Version.VersionId > 0).Select(x => x.Version.VersionId).Distinct().ToList()
                                        , cityId, orp);

                    if (modelPrices != null && modelPrices.Count > 0)
                    {
                        topLauches.ForEach(cc =>
                        {
                            PriceOverview modelPriceOverview;
                            modelPrices.TryGetValue(cc.Model.ModelId, out modelPriceOverview);
                            cc.PriceOverview = (modelPriceOverview != null && (cc.Version == null || cc.Version.VersionId <= 0) ? modelPriceOverview : new PriceOverview());
                        });
                    }

                    if (versionPrices != null && versionPrices.Count > 0)
                    {
                        topLauches.ForEach(cc =>
                        {
                            if (cc.Version != null && cc.Version.VersionId > 0)
                            {
                                PriceOverview versionPriceOverview;
                                versionPrices.TryGetValue(cc.Version.VersionId, out versionPriceOverview);
                                cc.PriceOverview = (versionPriceOverview ?? new PriceOverview());
                            }
                        });
                    }


                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return topLauches;
        }




        /// Written By : Ashwini Todkar on 24 July 2015
        /// Method to get pagewise top upcoming car models
        public List<UpcomingCarModel> GetUpcomingCarModels(Pagination page, int makeId = 0, int sort = 0)
        {
            List<UpcomingCarModel> _topUpcoming = null;

            try
            {
                var _allCar = _modelsCacheRepo.GetUpcomingCarModelsByMake(makeId, 100);

                if (sort > 0)
                {
                    _allCar = CarSortedList(_allCar, sort);
                }
                ushort _startIndex;
                ushort _endIndex;
                Utility.Calculation.GetStartEndIndex(page.PageNo, page.PageSize, out _startIndex, out _endIndex);

                _topUpcoming = _allCar.Where((x, i) => i >= _startIndex - 1 && i < _endIndex).ToList<UpcomingCarModel>();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return _topUpcoming;
        }
        #endregion

        private static List<UpcomingCarModel> CarSortedList(List<UpcomingCarModel> result, int sort)
        {
            if (sort > 0)
            {
                switch (sort)
                {
                    case 2:
                        result = result.OrderByDescending(c => c.Price.MinPrice).ToList();
                        break;
                    case 3:
                        result = result.OrderBy(c => c.LaunchDate).ToList();
                        break;
                    case 4:
                        result = result.OrderByDescending(c => c.LaunchDate).ToList();
                        break;
                    default:
                        result = result.OrderBy(c => c.Price.MinPrice).ToList();
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// Created By : Chetan T. 
        /// Returns the list of similar models based on city and no. of Recommendation
        /// </summary>
        /// <returns></returns>

        public List<SimilarModelRecommendation> GetModelRecommendations(int modelId, int cityId, int noOfRecommendation)
        {
            List<SimilarModelRecommendation> campaignRecommendations = new List<SimilarModelRecommendation>();

            Mapper.CreateMap<CarModelDetails, CarModelsDTO>();
            Mapper.CreateMap<CarModelDetails, CarImageBaseDTO>()
                .ForMember(x => x.OriginalImgPath, o => o.MapFrom(s => s.OriginalImage));
            Mapper.CreateMap<CarModelDetails, CarMakesDTO>();
            Mapper.CreateMap<ModelPriceDTO, CarPricesDTO>();
            Mapper.CreateMap<CustLocation, CustLocationDTO>();

            try
            {
                List<CarModelDetails> similarModels = GetSimilarModelsOnBodyStyle(modelId);

                var prices = _container.Resolve<IPrices>();
                var geoCitiesCacheRepository = _container.Resolve<IGeoCitiesCacheRepository>();

                if (similarModels != null)
                {
                    for (int modelIndex = 0; modelIndex < similarModels.Count(); modelIndex++)
                    {
                        SimilarModelRecommendation campaignRecommendation = new SimilarModelRecommendation();
                        var ModelVersionPrices = prices.GetOnRoadPrice(similarModels[modelIndex].ModelId, cityId);
                        campaignRecommendation.CarModel = Mapper.Map<CarModelDetails, CarModelsDTO>(_modelsCacheRepo.GetModelDetailsById(similarModels[modelIndex].ModelId)); // To get Model Detail
                        campaignRecommendation.CarImageBase = Mapper.Map<CarModelDetails, CarImageBaseDTO>(
                        _modelsCacheRepo.GetModelDetailsById(similarModels[modelIndex].ModelId)); // To get Model Image
                        campaignRecommendation.CarMake = Mapper.Map<CarModelDetails, CarMakesDTO>(_modelsCacheRepo.GetModelDetailsById(similarModels[modelIndex].ModelId)); // To get Make Name
                        campaignRecommendation.CarPrices = Mapper.Map<ModelPriceDTO, CarPricesDTO>(ModelVersionPrices); // To get On Road price
                        if (ModelVersionPrices != null)
                            campaignRecommendation.CarPrices.BaseVersionOnRoadPrice = prices.GetMinOnRoadPrice(ModelVersionPrices.Versions);//To get Min Price of Version List
                        campaignRecommendation.CustLocation = Mapper.Map<CustLocation, CustLocationDTO>(geoCitiesCacheRepository.GetCustLocation(cityId, "")); // To get City Name

                        if (campaignRecommendation.CarModel != null && campaignRecommendation.CarImageBase != null && campaignRecommendation.CarMake != null &&
                            campaignRecommendation.CarPrices != null && campaignRecommendation.CustLocation != null && campaignRecommendation.CarPrices.MinPrice > 0)
                        {
                            campaignRecommendations.Add(campaignRecommendation);
                        }
                        if (campaignRecommendations.Count == noOfRecommendation)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelsBL.GetModelRecommendations()" + modelId + "CityId" + cityId);
                objErr.LogException();
            }
            return campaignRecommendations;
        }

        /// <summary>
        /// Returns the list of similar models based on bodystyle and subsegment
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="subsegmentId"></param>
        /// <returns></returns>

        private List<CarModelDetails> GetSimilarModelsOnBodyStyle(int modelId)
        {
            List<CarModelDetails> similarModelsBySameBodystyle = new List<CarModelDetails>();
            var _campaignRecommendationsBL = _container.Resolve<ICampaignRecommendationsBL>();

            try
            {
                var modelDetail = _modelsCacheRepo.GetModelDetailsById(modelId);

                var modelDetailList = _modelsCacheRepo.GetModelsByBodyStyle(modelDetail.BodyStyleId, true) ?? new List<CarModelDetails>();

                modelDetailList.RemoveAll(item => item.ModelId == modelId);

                _campaignRecommendationsBL.FilterBySubsegment(ref modelDetailList, modelDetail.SubSegmentId, 0); // "0" for similar car same subsegment

                similarModelsBySameBodystyle = modelDetailList.OrderByDescending(p => p.ModelPopularity).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return similarModelsBySameBodystyle;
        }

        public List<SimilarCarModelsDTO> GetSimilarCarsForApp(int modelId, string userIdentifier)
        {
            var similarCars = new List<SimilarCarModelsDTO>();
            try
            {
                similarCars = Mapper.Map<List<SimilarCarModels>, List<SimilarCarModelsDTO>>(GetSimilarCarsByModel(modelId, userIdentifier));

                similarCars = similarCars.Take(similarModelsCount).ToList();

                similarCars.ForEach(x => x.CarModelUrl = ConfigurationManager.AppSettings["WebApiHostUrl"]
                + "/modeldetails/budget=-1&fuelTypes=-1&bodyTypes=-1&transmission=-1&seatingCapacity=-1&enginePower=-1&importantFeatures=-1&modelId=" + x.ModelId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return similarCars;
        }

        public SubNavigationDTO GetModelQuickMenu(CarModelDetails modelDetailsObj, ArticlePageDetails articlePageDetails, bool isModelPage, bool isExpertReviewAvial, string category, string label)
        {
            SubNavigationDTO subNavigation = new SubNavigationDTO();
            try
            {
                subNavigation.ModelName = modelDetailsObj.ModelName;
                subNavigation.MaskingName = modelDetailsObj.MaskingName;
                subNavigation.MakeName = modelDetailsObj.MakeName;
                subNavigation.MakeId = modelDetailsObj.MakeId;
                subNavigation.ModelId = modelDetailsObj.ModelId;
                subNavigation.IsVersionDetailPage = false;
                subNavigation.IsUsedCarAvail = modelDetailsObj.Used > 0;
                subNavigation.Is360Avail = CMSCommon.IsThreeSixtyViewAvailable(modelDetailsObj);
                subNavigation.subNavOnCarCompare = articlePageDetails != null ? (articlePageDetails.CategoryId == 2 ? true : (!modelDetailsObj.New || modelDetailsObj.Futuristic)) : false;
                subNavigation.IsExpertReviewAvial = isExpertReviewAvial;
                subNavigation.VideoCount = modelDetailsObj.VideoCount;
                subNavigation.ImageCount = modelDetailsObj.PhotoCount;
                subNavigation.IsMileageAvail = modelDetailsObj.New && !modelDetailsObj.Futuristic;
                subNavigation.IsReviewsAvial = modelDetailsObj.ReviewCount > 0 || isExpertReviewAvial;
                subNavigation.IsUserReviewsAvailable = modelDetailsObj.ReviewCount > 0;
                subNavigation.ShowImagePopup = true;
                subNavigation.IsNew = modelDetailsObj.New;
                subNavigation.IsFuturistic = modelDetailsObj.Futuristic;
                subNavigation.IsModelPage = isModelPage;
                subNavigation.Category = category;
                subNavigation.Label = label;
                subNavigation.PageDetails = articlePageDetails;
                subNavigation.Default360Category = CMSCommon.Get360DefaultCategory(Mapper.Map<ThreeSixtyAvailabilityDTO>(modelDetailsObj));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return subNavigation;
        }

        /// <summary>
        /// Get All modelIds including Sponsored models at its respective position to be shown in History section in global car search
        /// Written By : Piyush Sahu on 08/03/2017
        /// </summary>
        /// <param name="modelIds"></param>

        public List<CarMakeModelAdEntityBase> GetHistoryModelDetails(string modelIds, int platformId)
        {
            try
            {
                var modelDetailsCallback = new Dictionary<string, MultiGetCallback<CarModelDetails>>();
                List<CarMakeModelAdEntityBase> modelDetails = new List<CarMakeModelAdEntityBase>();
                ISponsoredCarBL sponsoredCarBL = _container.Resolve<ISponsoredCarBL>();
                List<int> modelIdList = modelIds.Split(',').Select(i => int.Parse(i)).Distinct().ToList();

                var historyAdModel = sponsoredCarBL.GetAllSponsoredHistoryModels(modelIds, platformId);
                if (historyAdModel != null)
                {
                    bool ifModelExists = modelIdList.Any(item => item == historyAdModel.FeaturedModelId);
                    if (ifModelExists)
                    {
                        modelIdList.RemoveAll(item => item == historyAdModel.FeaturedModelId);
                    }

                    int sponsoredModelIndex = historyAdModel.AdPosition > modelIdList.Count ? modelIdList.Count : historyAdModel.AdPosition - 1;
                    modelIdList.Insert(sponsoredModelIndex, historyAdModel.FeaturedModelId);
                }
                int showHistoryModelsCount = modelIdList.Count - 3;
                if (showHistoryModelsCount > 0)
                {
                    modelIdList.RemoveRange(3, showHistoryModelsCount);
                }

                var multiGetModelDetails = _modelsCacheRepo.MultiGetModelDetails(modelIdList);

                foreach (var model in multiGetModelDetails)
                {
                    var item = new CarMakeModelAdEntityBase();
                    item.ModelId = model.ModelId;
                    item.MakeId = model.MakeId.ToString();
                    item.ModelName = model.ModelName;
                    item.MakeName = model.MakeName;
                    item.MaskingName = model.MaskingName;
                    item.IsSponsored = historyAdModel != null && item.ModelId == historyAdModel.FeaturedModelId;
                    modelDetails.Add(item);
                }
                return modelDetails;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        public ModelMaskingValidationEntity FetchModelIdFromMaskingName(string modelMaskingName, string modelId, string makeMaskingName = null, bool isMsite = false)
        {
            var modelValidation = new ModelMaskingValidationEntity
            {
                ModelId = -1,
                ModelMaskingName = string.Empty,
                IsRedirect = false,
                RedirectUrl = string.Empty,
                IsValid = true,
                Status = CarStatus.None
            };

            try
            {
                if (!string.IsNullOrEmpty(modelId) && RegExValidations.IsPositiveNumber(modelId))
                {
                    modelValidation.ModelId = CustomParser.parseIntObject(modelId);
                }
                else
                {
                    CarModelMaskingResponse cmr = new CarModelMaskingResponse();
                    if (!string.IsNullOrEmpty(modelMaskingName))
                    {
                        cmr = _modelsCacheRepo.GetModelByMaskingName(modelMaskingName);

                    }
                    if (cmr == null || cmr.MakeId <= 0)
                    {
                        if (!string.IsNullOrEmpty(makeMaskingName))
                        {
                            modelValidation.IsRedirect = true;
                            modelValidation.RedirectUrl = ManageCarUrl.CreateMakeUrl(makeMaskingName, isMsite);
                        }
                        else
                        {
                            modelValidation.IsValid = false;
                        }
                    }
                    else
                    {
                        if (cmr.Redirect)
                        {
                            modelValidation.RedirectUrl = ManageCarUrl.CreateModelUrl(cmr.MakeName, cmr.MaskingName);
                            modelValidation.IsRedirect = cmr.Redirect;
                        }
                        modelValidation.ModelId = cmr.ModelId;
                        modelValidation.ModelMaskingName = cmr.MaskingName;
                        modelValidation.Status = cmr.Status;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelValidation;
        }

        public TrendingCarDTO GetTrendingModelDetails(int count, int platfromId, bool isAmp)
        {
            TrendingCarDTO trendingCarData = new TrendingCarDTO();
            try
            {
                var modelData = _modelsCacheRepo.GetTrendingModels(count);

                trendingCarData.TrendingModels = Mapper.Map<List<CarMakeModelAdEntityBase>, List<CarMakeModelDTO>>(modelData);
                foreach (var data in trendingCarData.TrendingModels)
                {
                    //if the request is from amp page then set the url otherwise url is null
                    data.Url = (isAmp) ? ManageCarUrl.CreateModelUrl(data.MakeName, data.MaskingName, true) : null;
                }

                var sponsoredTrendingModel = _sponsoredBL.GetSponsoredTrendingCar(platfromId);
                if (sponsoredTrendingModel != null)
                {

                    trendingCarData.SponsoredModel = Mapper.Map<GlobalSearchSponsoredModelEntity, CarMakeModelAdDTO>(sponsoredTrendingModel);


                    bool ifModelExists = trendingCarData.TrendingModels.Any(cus => cus.ModelId == trendingCarData.SponsoredModel.ModelId);
                    if (ifModelExists)
                    {
                        trendingCarData.TrendingModels.RemoveAll((x) => x.ModelId == trendingCarData.SponsoredModel.ModelId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return trendingCarData;
        }

        public List<CarModelSummaryDTO> GetModelDetails(int makeId, int cityId)
        {
            List<CarModelSummaryDTO> modelDetails = new List<CarModelSummaryDTO>();
            var newModelsList = _modelsCacheRepo.GetModelsByMake(makeId, 0).FindAll(x => x.New);
            try
            {
                if (newModelsList != null && newModelsList.Count > 0)
                {
                    List<int> modelIdList = newModelsList.Select(x => x.ModelId).Distinct().ToList();
                    foreach (int modelId in modelIdList)
                    {
                        CarModelSummaryDTO carmodel = new CarModelSummaryDTO();
                        var modelBasicDetails = _modelsCacheRepo.GetModelDetailsById(modelId);

                        if (modelBasicDetails != null)
                        {
                            var pricesObj = _container.Resolve<IPrices>();
                            var priceDetails = _prices.GetAvailablePriceForModel(modelId, cityId, null);
                            if (priceDetails != null)
                            {
                                carmodel.CarPrices = new CarPricesDTO { Price = priceDetails.Price, PriceLabel = priceDetails.PriceLabel };
                                var ModelVersionPrices = pricesObj.GetOnRoadPrice(modelId, cityId);
                                if (ModelVersionPrices != null)
                                {
                                    carmodel.CarPrices.BaseVersionOnRoadPrice = pricesObj.GetMinOnRoadPrice(ModelVersionPrices.Versions);//To get Min Price of Version List
                                }
                            }
                            carmodel.CarModel = new CarModelsDTO { ModelName = modelBasicDetails.ModelName, ModelId = modelId };
                            carmodel.CarImageBase = new CarImageBaseDTO { HostUrl = _imgHostUrl, OriginalImgPath = modelBasicDetails.OriginalImage };
                            modelDetails.Add(carmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return modelDetails;
        }

        public List<ModelPriceInOtherCities> GetPricesInOtherCities(int modelId, int cityId)
        {
            List<ModelPriceInOtherCities> otherCities = new List<ModelPriceInOtherCities>();
            try
            {
                otherCities = _modelsCacheRepo.GetPricesForNearByCities(modelId, cityId);
                if (otherCities == null)
                {
                    otherCities = _modelsCacheRepo.GetPricesInOtherCities(modelId, cityId);
                    var modelIds = new List<int> { modelId };
                    if (otherCities != null && otherCities.Count > 0)
                    {
                        foreach (var city in otherCities)
                        {
                            var price = _prices.GetModelsCarPriceOverview(modelIds, city.CityId, true);
                            city.MinPrice = price != null && price.ContainsKey(modelId) ? price[modelId].Price : 0;
                        }
                    }
                    _modelsCacheRepo.SetPricesForNearByCities(modelId, cityId, otherCities);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return otherCities;
        }
        public List<UpcomingCarModel> GetSimilarUpcomingCars(int modelId, DateTime launchDate, double expectedPrice, int count = 5)
        {
            List<UpcomingCarModel> similarUpcomingCars = null;
            try
            {
                var price = expectedPrice * 100000;
                double lowerLimit = price * 0.8;
                double upperLimit = price * 1.2;
                similarUpcomingCars = _modelsCacheRepo.GetUpcomingCarModelsByMake(0);
                DateTime dateRange = launchDate != DateTime.MinValue ? launchDate.AddDays(120.0) : DateTime.Now.AddDays(120.0);
                if (similarUpcomingCars != null && similarUpcomingCars.Count > 0)
                {
                    similarUpcomingCars = similarUpcomingCars.Where(x => x.ModelId != modelId &&
                    (x.LaunchDate.Date < dateRange.Date) && x.Price.MinPrice >= lowerLimit && x.Price.MinPrice <= upperLimit).OrderBy(y => y.LaunchDate).Take(5).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return similarUpcomingCars;
        }
        public SimilarCarModels GetReplacedModelDetails(short replacedModelId, CarModelDetails activeModelDetails, bool isMobile)
        {
            try
            {
                var replacingModelDetails = _modelsCacheRepo.GetModelDetailsById(replacedModelId);
                if (replacingModelDetails != null && replacingModelDetails.MaskingName != null && replacingModelDetails.MakeName != null && replacingModelDetails.ModelName != null)
                {
                    List<Tuple<int, string>> compareCarsTupleList = new List<Tuple<int, string>>();
                    compareCarsTupleList.Add(new Tuple<int, string>(activeModelDetails.ModelId, (Format.FormatSpecial(activeModelDetails.MakeName) + '-' + activeModelDetails.MaskingName)));
                    compareCarsTupleList.Add(new Tuple<int, string>(replacingModelDetails.ModelId, (Format.FormatSpecial(replacingModelDetails.MakeName) + '-' + replacingModelDetails.MaskingName)));
                    return new SimilarCarModels { ModelName = replacingModelDetails.ModelName, CompareCarUrl = ManageCarUrl.CreateCompareCarUrl(compareCarsTupleList, false, isMobile) };
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        public List<SimilarCarModels> GetSimilarCarsWithPriceOverview(int modelId, int cityId, string userIdentifier)
        {
            List<SimilarCarModels> similarCars = GetSimilarCarsByModel(modelId, userIdentifier);
            try
            {
                if (similarCars == null || similarCars.Count == 0)
                {
                    return similarCars;
                }
                List<int> modelIds = similarCars.Select(c => c.ModelId).ToList();
                IDictionary<int, PriceOverview> modelPriceList = _prices.GetModelsCarPriceOverview(modelIds, cityId, true);
                for (int model = 0; model < similarCars.Count; model++)
                {
                    PriceOverview modelprice = null;
                    modelPriceList.TryGetValue(similarCars[model].ModelId, out modelprice);
                    if (modelprice != null && modelprice.Price > 0)
                    {
                        similarCars[model].PricesOverview = modelprice;
                    }
                }
                similarCars.RemoveAll(x => x.PricesOverview == null || x.PricesOverview.Price < 1);
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
            return similarCars;
        }

        public List<SimilarCarModels> GetSimilarCarsWithORP(int modelId, int cityId, string cwcCookie, int reqCount, string excludingModels = "")
        {
            List<SimilarCarModels> similarCars = new List<SimilarCarModels>();
            try
            {
                similarCars = GetSimilarCarsWithPriceOverview(modelId, cityId, cwcCookie);
                similarCars = similarCars.Where(x => x.PricesOverview.PriceStatus == 1).ToList();

                if (!string.IsNullOrEmpty(excludingModels))
                {
                    List<int> modelList = excludingModels.Split(',').Select(int.Parse).ToList();
                    similarCars = similarCars.Where(x => !modelList.Contains(x.ModelId)).ToList();
                }

                int similarCarsCount = similarCars.Count;
                int requiredSimilarCarsCount = reqCount <= similarCarsCount ? reqCount : similarCarsCount;
                return similarCars.Take(requiredSimilarCarsCount).ToList();
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                return similarCars;
            }
        }
        private static bool IsSimilarBodyStyle(CarBodyStyle sourceBodyStyle, CarBodyStyle destinationBodyStyle)
        {
            if (sourceBodyStyle == CarBodyStyle.SUV || sourceBodyStyle == CarBodyStyle.MUV)
            {
                return destinationBodyStyle == CarBodyStyle.SUV || destinationBodyStyle == CarBodyStyle.MUV;
            }
            else if (sourceBodyStyle == CarBodyStyle.Sedan || sourceBodyStyle == CarBodyStyle.CompactSedan)
            {
                return destinationBodyStyle == CarBodyStyle.Sedan || destinationBodyStyle == CarBodyStyle.CompactSedan;
            }
            else
            {
                return sourceBodyStyle == destinationBodyStyle;
            }
        }

        public JObject CreateJsonLdJObject(ModelSchema modelSchema, List<SimilarCarModelsDTOV2> similarCarModels, bool isMileagePage = false)
        {
            JObject jsonObj = new JObject
            (
                new JProperty("@context", "https://schema.org/"),
                new JProperty("@type", "Car"),
                new JProperty("name", String.Format("{0} {1}", modelSchema.ModelDetails.MakeName, modelSchema.ModelDetails.ModelName)),
                new JProperty("description", modelSchema.PageMetaTags.Description),
                new JProperty("url", modelSchema.PageMetaTags.Canonical),
                new JProperty("model", modelSchema.ModelDetails.ModelName)
            );

            if (modelSchema.ModelDetails.OriginalImage.IsNotNullOrEmpty())
            {
                jsonObj.Add
                (
                    new JProperty("image", ImageSizes.CreateImageUrl(modelSchema.ModelDetails.HostUrl, ImageSizes._310X174, modelSchema.ModelDetails.OriginalImage))
                );
            }
            if (modelSchema.ModelColors.IsNotNullOrEmpty())
            {
                string coloursOfModel = string.Join(",", modelSchema.ModelColors.Select(c => c.Color));
                jsonObj.Add(new JProperty("color", coloursOfModel));
            }
            jsonObj.Add(new JProperty("brand", modelSchema.ModelDetails.MakeName));
            jsonObj.Add(new JProperty("bodyType", (((CarBodyStyle)modelSchema.ModelDetails.BodyStyleId).ToString())));

            IEnumerable<string> displacementList = modelSchema.MileageData?.Select(x => x.Displacement)?.Distinct()?.Where(x => !string.IsNullOrWhiteSpace(x));
            if (displacementList.IsNotNullOrEmpty())
            {
                List<JObject> engineSpecs = new List<JObject>();
                foreach (var engine in displacementList)
                {
                    JObject engineSp = new JObject
                    (
                        new JProperty("@type", "EngineSpecification"),
                        new JProperty("name", engine)
                    );
                    engineSpecs.Add(engineSp);
                }
                jsonObj.Add(new JProperty("vehicleEngine", engineSpecs));
            }
            IEnumerable<string> seatingCapacityList = modelSchema.SeatingCapacity?.Distinct()?.Where(x => !string.IsNullOrWhiteSpace(x))
                ?.Select(x => string.Format("{0} seater", x));
            if (seatingCapacityList.IsNotNullOrEmpty())
            {
                List<JObject> seatingCapacity = new List<JObject>();
                foreach (var seating in seatingCapacityList)
                {
                    JObject seatingCap = new JObject
                    (
                        new JProperty("@type", "QuantitativeValue"),
                        new JProperty("name", seating)
                    );
                    seatingCapacity.Add(seatingCap);
                }
                jsonObj.Add(new JProperty("vehicleSeatingCapacity", seatingCapacity));
            }
            JObject manufacturer = new JObject
            (
                new JProperty("@type", "organization"),
                new JProperty("name", modelSchema.ModelDetails.MakeName)
            );
            jsonObj.Add(new JProperty("manufacturer", manufacturer));

            JObject offers = new JObject
            (
                new JProperty("@type", "AggregateOffer"),
                new JProperty("priceCurrency", "INR"),
                new JProperty("lowPrice", modelSchema.ModelDetails.MinPrice),
                new JProperty("highPrice", modelSchema.ModelDetails.MaxPrice)
            );
            jsonObj.Add(new JProperty("offers", offers));
            IEnumerable<string> wheelConfigList = modelSchema.Drivetrain?.Distinct()?.Where(x => !string.IsNullOrWhiteSpace(x));
            if (wheelConfigList.IsNotNullOrEmpty())
            {
                List<JObject> wheelConfiguration = new List<JObject>();
                foreach (var wheelconfig in wheelConfigList)
                {
                    JObject wheelCon = new JObject
                    (
                        new JProperty("@type", "DriveWheelConfigurationValue"),
                        new JProperty("name", wheelconfig)
                    );
                    wheelConfiguration.Add(wheelCon);
                }
                jsonObj.Add(new JProperty("driveWheelConfiguration", wheelConfiguration));
            }

            if (modelSchema.ModelDetails.ReviewCount > 0 && modelSchema.ModelDetails.ModelRating > 0)
            {
                JObject aggregateRating = new JObject(
                    new JProperty("@type", "AggregateRating"),
                    new JProperty("reviewCount", modelSchema.ModelDetails.ReviewCount),
                    new JProperty("ratingValue", modelSchema.ModelDetails.ModelRating),
                     new JProperty("worstRating", 1),
                      new JProperty("bestRating", 5)
                    );
                jsonObj.Add(new JProperty("aggregateRating", aggregateRating));
            }
            similarCarModels = similarCarModels?.Where(x => x.ModelId > 0)?.ToList();

            if (similarCarModels.IsNotNullOrEmpty())
            {
                List<JObject> similarCarList = new List<JObject>();
                foreach (var similarCars in similarCarModels)
                {
                    JObject similarCarsObj = new JObject
                    (
                        new JProperty("@type", "Car"),
                        new JProperty("name", $"{similarCars.MakeName} {similarCars.ModelName}"),
                        new JProperty("url", ManageCarUrl.CreateModelUrl(similarCars.MakeName, similarCars.MaskingName, true)),
                        new JProperty("image", ImageSizes.CreateImageUrl(similarCars.HostUrl, ImageSizes._310X174, similarCars.ModelImageOriginal))
                    );
                    similarCarList.Add(similarCarsObj);
                }
                jsonObj.Add(new JProperty("isSimilarTo", similarCarList));
            }
            IEnumerable<string> araiList = modelSchema.MileageData?.Distinct()?.Where(x => x.Arai > 0)
                ?.Select(x => string.Format("{0} {1}", x.Arai, x.MileageUnit ?? string.Empty));
            if (isMileagePage && araiList.IsNotNullOrEmpty())
            {
                List<JObject> fuelEfficiency = new List<JObject>();
                foreach (var mileage in araiList)
                {
                    fuelEfficiency.Add(new JObject(
                        new JProperty("@type", "QuantitativeValue"),
                        new JProperty("name", mileage)
                        ));
                }
                jsonObj.Add(new JProperty("fuelEfficiency", fuelEfficiency));

            }
            IEnumerable<string> fuelTypeList = modelSchema.MileageData?.Select(x => x.FuelType)?.Distinct()?.Where(x => !string.IsNullOrWhiteSpace(x));
            if (fuelTypeList.IsNotNullOrEmpty())
            {
                List<JObject> fuelTypes = new List<JObject>();
                foreach (var fuelType in fuelTypeList)
                {
                    JObject fuelTypeObj = new JObject
                    (
                        new JProperty("@type", "QualitativeValue"),
                        new JProperty("name", fuelType)
                    );
                    fuelTypes.Add(fuelTypeObj);
                }
                jsonObj.Add(new JProperty("fuelType", fuelTypes));
            }
            IEnumerable<string> transmissionTypeList = modelSchema.MileageData?.Select(x => x.Transmission)?.Distinct()?.Where(x => !string.IsNullOrWhiteSpace(x));
            if (transmissionTypeList.IsNotNullOrEmpty())
            {
                List<JObject> transmissionType = new List<JObject>();
                foreach (var transmission in transmissionTypeList)
                {
                    JObject transType = new JObject
                    (
                        new JProperty("@type", "QualitativeValue"),
                        new JProperty("name", transmission)
                    );
                    transmissionType.Add(transType);
                }
                jsonObj.Add(new JProperty("vehicleTransmission", transmissionType));
            }

            return jsonObj;
        }
    }
}
