using AEPLCore.Cache.Interfaces;
using AutoMapper;
using Carwale.BL.GeoLocation;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carwale.BL.Campaigns
{
    public class CampaignRecommendationsBL : ICampaignRecommendationsBL
    {
        private readonly ICarModelRepository _carModelRepository;
        private readonly ICarModelCacheRepository _carModelCache;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;
        private readonly ICampaign _campaign;
        private readonly IPrices _prices;
        private readonly IDealers _dealers;
        private readonly ICacheManager _cacheCore;
        private readonly ICarModels _carModels;
        private readonly ICarRecommendationLogic _carRecommendationLogic;
        private readonly ICarPriceQuoteAdapter _carPriceQuote;
        private readonly static string[] _blockedRecommendationCites = (ConfigurationManager.AppSettings["BlockedRecommendationCities"]).Split(',');
        private readonly static int _similarRecommendationCountforCampaign = CustomParser.parseIntObject(ConfigurationManager.AppSettings["SimilarRecommendationCountforCampaign"] ?? "15");

        public CampaignRecommendationsBL(ICarModelRepository carModelRepository, ICarModelCacheRepository carModelCacheRepository,
            IGeoCitiesCacheRepository geoCitiesCacheRepository, ICampaign campaign,
            IPrices prices, IDealers dealers, ICacheManager cacheCore, ICarModels carModels, ICarPriceQuoteAdapter carPriceQuote, ICarRecommendationLogic carRecommendationLogic)
        {
            _carModelRepository = carModelRepository;
            _carModelCache = carModelCacheRepository;
            _geoCitiesCacheRepository = geoCitiesCacheRepository;
            _campaign = campaign;
            _prices = prices;
            _dealers = dealers;
            _cacheCore = cacheCore;
            _carModels = carModels;
            _carPriceQuote = carPriceQuote;
            _carRecommendationLogic = carRecommendationLogic;
        }

        /// <summary>
        /// For getting alternate Cars with active campaigns on submitting a lead
        /// Created: Vicky Lund, 01/12/2015
        /// </summary>
        /// <returns></returns>
        private List<CampaignRecommendationEntity> GetCampaignRecommendations(string historyModelList, List<CampaignInput> userRecentLeads,
            CampaignInput campaignInput, int noOfRecommendations, int campaignRecoType)
        {
            List<CampaignRecommendationEntity> campaignRecommendations = null;

            try
            {
                List<int> userHistoryModels = new List<int>();
                CarModelDetails currentLeadModelDetails = new CarModelDetails();
                var modelDetailList = new List<CarModelDetails>();
                var similarModelDetailList = new List<CarModelDetails>();
                List<int> filteredHistoryModels = new List<int>();

                GetModelIds(historyModelList, ref userHistoryModels);

                currentLeadModelDetails = _carModelCache.GetModelDetailsById(campaignInput.ModelId);

                // Operations for User history Search 
                modelDetailList = OperateOnUserHistory(currentLeadModelDetails, userHistoryModels, modelDetailList);

                if (modelDetailList != null)
                {
                    filteredHistoryModels = modelDetailList.Select(x => x.ModelId).ToList();
                }

                // Operations with same Subsegment and same bodyStyle
                similarModelDetailList = _carModelCache.GetModelsByBodyStyle(currentLeadModelDetails.BodyStyleId, true);

                if (similarModelDetailList != null)
                {
                    modelDetailList.AddRange(GetModelsBySubSegment(similarModelDetailList, currentLeadModelDetails.SubSegmentId, 0));
                }

                modelDetailList = ExcludeBaseModel(modelDetailList, campaignInput.ModelId);

                modelDetailList = ApplyFilters(modelDetailList, userRecentLeads.Select(x => x.ModelId).ToList());

                if (campaignRecoType != (int)CampaignRecommendationType.ModelSuggestion)
                {
                    ExcludeMake(ref modelDetailList, currentLeadModelDetails.MakeId);
                }

                modelDetailList = modelDetailList.Where(x => x.New).ToList();

                if (modelDetailList == null || modelDetailList.Count == 0)
                {
                    return null;
                }

                campaignRecommendations = GetCampaignsOnModels(modelDetailList.Select(x => x.ModelId).ToList(), noOfRecommendations, campaignInput, campaignRecoType);

                foreach (var campaign in campaignRecommendations)
                {
                    if (filteredHistoryModels.Contains(campaign.CarModel.ModelId))
                    {
                        campaign.Source = (int)CampaignRecommendationSource.UserHistory;
                    }
                    else
                    {
                        campaign.Source = (int)CampaignRecommendationSource.Similar;
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.GetCampaignRecommendations()");
                objErr.LogException();
            }
            return campaignRecommendations;
        }

        private List<CarModelDetails> ExcludeBaseModel(List<CarModelDetails> modelDetailList, int baseModelId)
        {
            return modelDetailList.Where(x => x.ModelId != baseModelId).ToList();
        }

        /// <summary>
        /// For getting Similar cars recommendation
        /// Created By: Sanjay Soni, 16/03/2016
        /// </summary>
        /// <returns></returns>
        public List<CampaignRecommendationEntity> SimilarCampaignRecommend(int modelId, Location locationObj, int subsegmentRange, int recommendationCount, bool isSameBodystyle)
        {
            List<CampaignRecommendationEntity> campaignRecommendations = null;

            try
            {
                var modelDetail = _carModelCache.GetModelDetailsById(modelId);

                var modelDetailList = _carModelCache.GetModelsByBodyStyle(modelDetail.BodyStyleId, isSameBodystyle).OrderByDescending(x => x.ModelPopularity).ToList();

                if (modelDetailList == null)
                {
                    return null;
                }

                ExcludeMake(ref modelDetailList, modelDetail.MakeId);

                if (subsegmentRange != -1)
                {
                    FilterBySubsegment(ref modelDetailList, modelDetail.SubSegmentId, subsegmentRange);
                }

                campaignRecommendations = filterByCampaign(modelDetailList, locationObj, Platform.CarwaleDesktop.GetHashCode(), recommendationCount);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.SimilarCampaignRecommend()");
                objErr.LogException();
            }
            return campaignRecommendations;
        }

        public List<MakeModelEntity> GetPQRecommendations(string cwcCookie, string modelList, int referenceModel, int noOfRecommendations, Location locationObj, int platformId)
        {
            var carData = new MakeModelEntity();
            List<int> userHistoryModels = new List<int>();
            List<int> userPQTakenModels = new List<int>();
            CarModelDetails currentLeadModelDetails = new CarModelDetails();
            var modelDetailList = new List<CarModelDetails>();
            var recommendedCampaigns = new List<CampaignRecommendationEntity>();
            var similarModelDetailList = new List<CarModelDetails>();

            try
            {
                userPQTakenModels = _cacheCore.GetFromCache<List<int>>(string.Format("cwc-cookie-{0}", cwcCookie));

                GetModelIds(modelList, ref userHistoryModels);

                currentLeadModelDetails = _carModelCache.GetModelDetailsById(referenceModel);

                modelDetailList = OperateOnUserHistory(currentLeadModelDetails, userHistoryModels, modelDetailList); // gives model ids filtered by subseg 

                similarModelDetailList = _carModelCache.GetModelsByBodyStyle(currentLeadModelDetails.BodyStyleId, true);

                if (similarModelDetailList != null)
                {
                    modelDetailList.AddRange(GetModelsBySubSegment(similarModelDetailList, currentLeadModelDetails.SubSegmentId, 0));
                }

                if (modelDetailList != null && modelDetailList.Count > 0)
                {
                    modelDetailList = ApplyFilters(modelDetailList, userPQTakenModels);

                    ExcludeMake(ref modelDetailList, currentLeadModelDetails.MakeId);

                    modelDetailList = modelDetailList.Where(x => x.New).ToList();

                    recommendedCampaigns = GetRecommendedCampaigns(modelDetailList.Select(x => x.ModelId).ToList(),
                        noOfRecommendations, locationObj, platformId, (int)CampaignRecommendationType.PQSuggestion).Take(noOfRecommendations).ToList();
                }

                var PQRecoModels = Mapper.Map<List<CampaignRecommendationEntity>, List<MakeModelEntity>>(recommendedCampaigns);

                if (PQRecoModels.Count >= 3)
                {
                    return PQRecoModels;
                }
                else
                {
                    var similarCars = _carModels.GetModelRecommendations(referenceModel, locationObj.CityId, noOfRecommendations - recommendedCampaigns.Count);
                    var similarCarsWithoutCampaign = Mapper.Map<List<SimilarModelRecommendation>, List<MakeModelEntity>>(similarCars);

                    if (similarCarsWithoutCampaign != null && similarCarsWithoutCampaign.Count > 0)
                    {
                        if (userPQTakenModels != null && userPQTakenModels.Count > 0)
                            FilterModelsListOnPQTaken(ref similarCarsWithoutCampaign, userPQTakenModels);

                        PQRecoModels.AddRange(similarCarsWithoutCampaign);

                        return PQRecoModels.GroupBy(x => x.ModelId).Select(y => y.First()).ToList();
                    }
                    return PQRecoModels;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.GetPQRecommendations()");
                objErr.LogException();
                return null;
            }
        }

        public List<CampaignRecommendationEntity> GetCampaignRecommendationsByLead(string historyModelList, string mobileNumber, int noOfRecommendations)
        {
            try
            {
                List<CampaignInput> userRecentLeads = _carModelRepository.GetUserRecentLeadModels(mobileNumber);
                if (userRecentLeads != null && userRecentLeads.Count > 0)
                {
                    var latestLeadInputs = userRecentLeads[0];

                    if (_blockedRecommendationCites.Contains(latestLeadInputs.CityId.ToString()))
                    {
                        return null;
                    }
                    var recommendations = GetCampaignRecommendations(historyModelList, userRecentLeads, latestLeadInputs, noOfRecommendations, (int)CampaignRecommendationType.Lead);
                    if (recommendations == null)
                    {
                        return null;
                    }
                    return recommendations.Take(noOfRecommendations).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.GetCampaignRecommendationsByLead()");
                objErr.LogException();
            }

            return null;
        }

        private void GetModelIds(string modelList, ref List<int> userHistoryModels)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(modelList))
                {
                    userHistoryModels.AddRange(modelList.Split(',').Select(CustomParser.parseIntObject).Distinct().Reverse());
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.GetModelIds()");
                objErr.LogException();
            }
        }

        private List<CarModelDetails> OperateOnUserHistory(CarModelDetails modelDetails, List<int> userHistoryModels, List<CarModelDetails> modelDetailList)
        {
            try
            {
                GetUserHistoryModelDetails(userHistoryModels, ref modelDetailList);

                FilterBySubsegment(ref modelDetailList, modelDetails.SubSegmentId, 2);

                return modelDetailList;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.OperateOnUserHistory()");
                objErr.LogException();
                return null;
            }
        }

        private List<CarModelDetails> GetModelsBySubSegment(List<CarModelDetails> similarModelDetailList, int subSegmentId, int range)
        {
            try
            {
                FilterBySubsegment(ref similarModelDetailList, subSegmentId, range);

                similarModelDetailList = similarModelDetailList.OrderByDescending(x => x.ModelPopularity).ToList();

                return similarModelDetailList;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.GetModelsBySubSegment()");
                objErr.LogException();
                return null;
            }
        }

        private List<CarModelDetails> ApplyFilters(List<CarModelDetails> modelDetailList, List<int> userSearchedModels)
        {
            try
            {
                if (userSearchedModels != null)
                {
                    FilterModelsListOnLeads(ref modelDetailList, userSearchedModels);
                }

                modelDetailList = modelDetailList.GroupBy(x => x.ModelId, (key, g) => g.First()).ToList();

                return modelDetailList;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.ApplyFiltersAndExludeMake()");
                objErr.LogException();
                return null;
            }
        }

        /// <summary>
        /// Filters user history models on the basis of recent leads submitted by user
        /// Created: Vicky Lund, 02/12/2015
        /// </summary>
        private void FilterModelsListOnLeads(ref List<CarModelDetails> recommendedModels, List<int> userRecentLeads)
        {
            try
            {
                bool skipIteration = false;
                for (int historyCounter = 0; historyCounter < recommendedModels.Count; historyCounter++)
                {
                    skipIteration = false;
                    for (int recentLeadCounter = 0; recentLeadCounter < userRecentLeads.Count; recentLeadCounter++)
                    {
                        if (recommendedModels[historyCounter].ModelId == userRecentLeads[recentLeadCounter])
                        {
                            skipIteration = true;
                            break;
                        }
                    }
                    if (skipIteration)
                    {
                        recommendedModels.RemoveAt(historyCounter);
                        historyCounter--;
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.FilterModelsListOnLeads()");
                objErr.LogException();
            }
        }

        private void FilterModelsListOnPQTaken(ref List<MakeModelEntity> recommendedModels, List<int> userRecentLeads)
        {
            try
            {
                bool skipIteration = false;
                for (int historyCounter = 0; historyCounter < recommendedModels.Count; historyCounter++)
                {
                    skipIteration = false;
                    for (int recentLeadCounter = 0; recentLeadCounter < userRecentLeads.Count; recentLeadCounter++)
                    {
                        if (recommendedModels[historyCounter].ModelId == userRecentLeads[recentLeadCounter])
                        {
                            skipIteration = true;
                            break;
                        }
                    }
                    if (skipIteration)
                    {
                        recommendedModels.RemoveAt(historyCounter);
                        historyCounter--;
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.FilterModelsListOnPQTaken()");
                objErr.LogException();
            }
        }

        /// <summary>
        /// Filters user history models on the basis of sub segment id
        /// Created: Vicky Lund, 02/12/2015
        /// </summary>
        private void GetUserHistoryModelDetails(List<int> userHistoryModels, ref List<CarModelDetails> modelDetailList)
        {
            try
            {
                foreach (int model in userHistoryModels)
                {
                    var currentModelDetails = _carModelCache.GetModelDetailsById(model);

                    if (currentModelDetails != null)
                    {
                        modelDetailList.Add(currentModelDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.GetUserHistoryModelDetails()");
                objErr.LogException();
            }
        }

        /// <summary>
        /// For getting Cars to be bind on UI of PriceQuote page
        /// Created By: Chetan Thambad, 01/12/2015
        /// </summary>
        /// <returns></returns>
        private List<CampaignRecommendationEntity> GetCampaignsOnModels(List<int> modelList, int noOfRecommendations, CampaignInput campaignInput, int campaignRecoType)
        {
            Location loc = new Location { CityId = campaignInput.CityId, ZoneId = campaignInput.ZoneId };
            return GetRecommendedCampaigns(modelList, noOfRecommendations, loc, campaignInput.PlatformId, campaignRecoType);
        }

        private List<CampaignRecommendationEntity> GetRecommendedCampaigns(List<int> modelList, int noOfRecommendations, Location loc, int platformId, int campaignRecoType)
        {

            Campaign campaignData = new Campaign();
            List<CampaignRecommendationEntity> campaignRecommendations = new List<CampaignRecommendationEntity>();

            try
            {
                foreach (int modelId in modelList)
                {
                    if (campaignRecoType == (int)CampaignRecommendationType.ModelSuggestion)
                    {
                        campaignData = _campaign.GetCampaignByCarLocation(modelId, loc, platformId, false);
                    }
                    else
                    {
                        campaignData = _campaign.GetCampaignByCarLocation(modelId, loc, platformId, true);
                    }

                    if (CampaignValidation.IsCampaignValid(campaignData))
                    {
                        CampaignRecommendationEntity campaignRecommendation;
                        campaignRecommendation = CreateCampaignRecommendationEntity(campaignData, modelId, loc.CityId, CustomParser.parseIntObject(loc.ZoneId));
                        if (campaignRecommendation != null)
                        {
                            campaignRecommendations.Add(campaignRecommendation);
                        }
                    }
                }
                return campaignRecommendations;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.GetRecommendedCampaigns()");
                objErr.LogException();
                return null;
            }
        }


        private CampaignRecommendationEntity CreateCampaignRecommendationEntity(Campaign campaignData, int modelId, int cityId, int zoneId)
        {
            var campaignRecommendation = new CampaignRecommendationEntity();

            try
            {
                campaignRecommendation.DealerSummary = Mapper.Map<DealerDetails, DealerSummaryDTO>(_dealers.GetDealerDetailsOnDealerId(campaignData.DealerId));
                if (campaignRecommendation.DealerSummary != null && campaignRecommendation.DealerSummary.DealerId == 0)
                {
                    return null;
                }

                var modelVersionPrices = _prices.GetOnRoadPrice(modelId, cityId);
                var modelDetail = _carModelCache.GetModelDetailsById(modelId);
                campaignRecommendation.CarModel = Mapper.Map<CarModelDetails, CarModelsDTO>(modelDetail);
                campaignRecommendation.CarImageBase = Mapper.Map<CarModelDetails, CarImageBaseDTO>(modelDetail);
                campaignRecommendation.CarMake = Mapper.Map<CarModelDetails, CarMakesDTO>(modelDetail);
                campaignRecommendation.CarPrices = Mapper.Map<ModelPriceDTO, CarPricesDTO>(modelVersionPrices);

                if (modelVersionPrices != null)
                {
                    campaignRecommendation.CarPrices.BaseVersionOnRoadPrice = _prices.GetMinOnRoadPrice(modelVersionPrices.Versions);//To get Min Price of Version List
                    campaignRecommendation.CarPrices.MinPrice = campaignRecommendation.CarPrices.BaseVersionOnRoadPrice;
                    campaignRecommendation.CarPrices.MaxPrice = campaignRecommendation.CarPrices.BaseVersionOnRoadPrice;
                }

                campaignRecommendation.CustLocation = Mapper.Map<CustLocation, CustLocationDTO>(_geoCitiesCacheRepository.GetCustLocation(cityId, zoneId.ToString()));
                campaignRecommendation.CampaignId = campaignData.Id;

                if (campaignRecommendation.CarModel != null && campaignRecommendation.CarImageBase != null && campaignRecommendation.CarMake != null &&
                    campaignRecommendation.CarPrices != null && campaignRecommendation.CustLocation != null && campaignRecommendation.DealerSummary != null
                    && campaignRecommendation.CarPrices.BaseVersionOnRoadPrice > 0)
                {
                    return campaignRecommendation;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ModelId : " + modelId + "City : " + cityId + "campaignID : " + campaignData.Id + "zoneId : " + zoneId);
                objErr.LogException();
            }
            return null;
        }

        private void ExcludeMake(ref List<CarModelDetails> modelDetailList, int makeId)
        {
            try
            {
                modelDetailList = modelDetailList.Where(s => s.MakeId != makeId).ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.ExcludeMake()");
                objErr.LogException();
            }
        }

        public void FilterBySubsegment(ref List<CarModelDetails> modelDetailList, int segment, int range)
        {
            try
            {
                modelDetailList = modelDetailList.Where(item => Math.Abs(item.SubSegmentId - segment) <= range).ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.filterBySubsegment()");
                objErr.LogException();
            }
        }

        private List<CampaignRecommendationEntity> filterByCampaign(List<CarModelDetails> modelList, Location locationObj, int platformId, int recommendationCount)
        {
            var campaignRecommendations = new List<CampaignRecommendationEntity>();

            foreach (var model in modelList)
            {
                var campaignData = _campaign.GetCampaignByCarLocation(model.ModelId, locationObj, platformId, false);
                if (CampaignValidation.IsCampaignValid(campaignData))
                {
                    var campaignRecommendation = CreateCampaignRecommendationEntity(campaignData, model.ModelId, locationObj.CityId, CustomParser.parseIntObject(locationObj.ZoneId));
                    if (campaignRecommendation != null)
                    {
                        campaignRecommendations.Add(campaignRecommendation);
                    }
                }
                if (recommendationCount > 0 && campaignRecommendations.Count >= recommendationCount)
                {
                    break;
                }
            }
            return campaignRecommendations;
        }

        private List<int> GetSimilarCars(int carId, int recommendationCount, string cwcCookie, bool boost)
        {
            List<int> carIds = null;
            try
            {
                if (carId > 0)
                {
                    SimilarCarRequest request = new SimilarCarRequest
                    {
                        CarId = carId,
                        IsVersion = false,
                        Count = recommendationCount,
                        UserIdentifier = cwcCookie,
                        IsBoost = boost
                    };
                    carIds = _carRecommendationLogic.GetSimilarCars(request);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignRecommendationsBL.GetsimilarCars()");
                objErr.LogException();
            }
            return carIds;
        }

        public List<CampaignRecommendation> GetCampaignRecommendation(string mobile, int recommendationCount, CampaignInput campaignInput, string cwcCookie, bool boost, bool isCheckRecommendation = true, bool isSameMakeFilter = true)
        {
            List<CampaignRecommendation> recommendedCampaign = new List<CampaignRecommendation>();
            try
            {
                if (campaignInput != null)
                {
                    List<int> modelList = GetSimilarCars(campaignInput.ModelId, _similarRecommendationCountforCampaign, cwcCookie, boost);
                    List<int> similarCarIdsNoLead = FilterCarsByRecentLeads(modelList, mobile);
                    CarModelDetails currentLeadModelDetails = _carModelCache.GetModelDetailsById(campaignInput.ModelId);

                    List<int> similarCarIdsNoLeadDiffMake = isSameMakeFilter ? FilterCarsByMake(similarCarIdsNoLead, currentLeadModelDetails.MakeId) : similarCarIdsNoLead;

                    if (similarCarIdsNoLeadDiffMake.Count > 0)
                    {
                        Location loc = new Location
                        {
                            CityId = campaignInput.CityId,
                            AreaId = campaignInput.AreaId,
                            ZoneId = campaignInput.ZoneId
                        };
                        loc = campaignInput.AreaId > 0 ? Mapper.Map<Area, Location>(new ElasticLocation().GetLocation(campaignInput.AreaId)) : loc;
                        List<Tuple<int, Campaign>> campaigns = _campaign.GetMultipleCampaign(similarCarIdsNoLeadDiffMake, loc, campaignInput.PlatformId, recommendationCount, (int)Application.CarWale, isCheckRecommendation);
                        recommendedCampaign = MapCampaignsToRecommendations(campaigns, loc);
                        return recommendedCampaign;
                    }
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, HttpContext.Current.Request.ServerVariables["URL"]);
            }
            return recommendedCampaign;
        }

        /// <summary>
        /// Filters out Cars for the specified make
        /// </summary>
        /// <param name="carIds"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        private List<int> FilterCarsByMake(List<int> carIds, int makeId)
        {
            List<int> outputCarIds = new List<int>();
            if (carIds.Count > 0)
            {
                var modelSummary = _carModelCache.GetModelsByMake(makeId);
                List<int> modelIds = modelSummary.Select(x => x.ModelId).ToList();
                outputCarIds = carIds.Where(x => !(modelIds.Contains(x))).ToList();
            }
            return outputCarIds;
        }

        /// <summary>
        /// Filters out Cars for which leads have been submitted by user
        /// </summary>
        /// <param name="carIds"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        private List<int> FilterCarsByRecentLeads(List<int> carIds, string mobile)
        {
            List<int> outputCarIds = new List<int>();
            if (carIds != null && carIds.Count > 0)
            {
                List<CampaignInput> userRecentLeads = _carModelRepository.GetUserRecentLeadModels(mobile);
                var userRecentLeadModels = userRecentLeads.Select(x => x.ModelId).ToList();
                outputCarIds = carIds.Where(x => !(userRecentLeadModels.Contains(x))).ToList();
            }
            return outputCarIds;
        }

        /// <summary>
        /// Returns list of campaign Recommendations for list of campaigns
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="campaignInput"></param>
        /// <param name="recommendationCount"></param>
        /// <returns></returns>
        private List<CampaignRecommendation> MapCampaignsToRecommendations(List<Tuple<int, Campaign>> campaigns, Location location)
        {
            List<CampaignRecommendation> campaignRecommendations = new List<CampaignRecommendation>();
            try
            {
                for (int index = 0; index < campaigns.Count; index++)
                {
                    CampaignRecommendation recommendedCampaign = new CampaignRecommendation();
                    recommendedCampaign.Campaign = campaigns[index].Item2;
                    recommendedCampaign.Campaign.Type = (campaigns[index].Item2.LeadPanel == (short)LeadPanel.Autobiz) ? (short)0 : (short)1;
                    if (recommendedCampaign.Campaign != null)
                    {
                        var modelDetail = _carModelCache.GetModelDetailsById(campaigns[index].Item1);
                        recommendedCampaign.CarData = modelDetail;
                        recommendedCampaign.PricesOverview = _carPriceQuote.GetAvailablePriceForModel(campaigns[index].Item1, location.CityId, null, true);
                        campaignRecommendations.Add(recommendedCampaign);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                return null;
            }
            return campaignRecommendations;
        }
    }
}
