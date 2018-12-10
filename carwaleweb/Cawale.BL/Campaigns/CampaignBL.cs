using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.Dealer;
using Carwale.Entity.Campaigns;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.Dealers;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using Carwale.DTOs.Geolocation;
using Carwale.Interfaces.Geolocation;
using Carwale.Entity.Geolocation;
using Carwale.Entity.CarData;
using Carwale.BL.GeoLocation;
using Carwale.Notifications.Logs;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.Campaigns;
using ProtoBufClass.Campaigns;
using ProtoBufClass.Common;
using Carwale.Entity;
using Microsoft.Practices.Unity;
using Campaigns.DealerCampaignClient;
using System.Text.RegularExpressions;

namespace Carwale.BL.Campaigns
{
    public class CampaignBL : ICampaign
    {
        private readonly ICampaignRepository _campaignRepo;
        private readonly ICampaignPrediction _campaignPrediction;
        private readonly INewCarDealersCache _newCarDealersCache;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private readonly ITemplate _campaignTemplate;
        private readonly IDealerCache _dealersCache;
        private readonly IUnityContainer _unityContainer;

        private static readonly Array _removeMaskingNumCampaigns = (ConfigurationManager.AppSettings["RemoveMaskingNumCampaigns"] ?? string.Empty).ConvertStringToArray();
        private static string _testDriveCampaignIdsList = ConfigurationManager.AppSettings["TestDriveCampaignIds"] ?? string.Empty;
        public static List<int> _testDriveCampaignIds = string.IsNullOrEmpty(_testDriveCampaignIdsList) ? new List<int>() : _testDriveCampaignIdsList.Split(',').Select(x => int.Parse(x)).ToList();

        public CampaignBL(ICampaignRepository campaignRepo, ICampaignPrediction campaignPrediction, INewCarDealersCache newCarDealersCache,
            IDealerCache dealersCache, IGeoCitiesCacheRepository geoCitiesCacheRepo, ITemplate campaignTemplate, IUnityContainer unityContainer)
        {
            _campaignRepo = campaignRepo;
            _campaignPrediction = campaignPrediction;
            _newCarDealersCache = newCarDealersCache;
            _geoCitiesCacheRepo = geoCitiesCacheRepo;
            _campaignTemplate = campaignTemplate;
            _dealersCache = dealersCache;
            _unityContainer = unityContainer;
        }

        public Entity.Campaigns.Campaign GetCampaignWithScore(Entity.Campaigns.Campaign campaign, HttpRequest httpRequest, int modelId, int cityId, int zoneId, int platformId)
        {
            try
            {
                var predictionData = _campaignPrediction.GetPredictionModelResponse(httpRequest, modelId, cityId, zoneId, platformId);
                campaign.PredictionData = predictionData;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CampaignBL.GetCampaignWithScore()");
            }
            return campaign;
        }

        /// <summary>
        /// Author:Rakesh Yadav on 02 Dec 2015
        /// Desc:Will give all campaign for model,city and source
        /// </summary>
        public List<Entity.Campaigns.Campaign> GetAllCampaign(int modelId, Location locationObj, int platformId, bool usePriority = true)
        {
            try
            {
                var campaignList = GetAllRunningCampaignsOnCriteria(modelId, locationObj, platformId);

                if (campaignList.Count == 0)
                {
                    return null;
                }

                foreach (var campaign in campaignList)
                {
                    campaign.IsTestDriveCampaign = IsTestDriveCampaign(campaign.Id);
                }

                if (usePriority)
                {
                    var campaignMinPriorityList = FilterCampaignByPriority(campaignList);
                    return campaignMinPriorityList;
                }
                else
                {
                    return campaignList;
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, HttpContext.Current.Request.ServerVariables["URL"]);
                return null;
            }
        }

        public List<Entity.Campaigns.Campaign> FilterCampaignByPriority(List<Entity.Campaigns.Campaign> campaignList)
        {
            try
            {
                if (campaignList == null || campaignList.Count == 0)
                {
                    return null;
                }
                else
                {
                    var minPriority = campaignList.Min(x => x.Priority);
                    List<Entity.Campaigns.Campaign> campaignMinPriorityList = campaignList.Where(x => x.Priority == minPriority).ToList();
                    return campaignMinPriorityList;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                return null;
            }
        }

        public List<Entity.Campaigns.Campaign> GetAllAvailableCampaign(int modelId, Location locationObj, int platformId, int applicationId, int count = 0)
        {
            try
            {
                var campaignList = GetAllRunningCampaignsOnCriteria(modelId, locationObj, platformId, applicationId, count);

                if (campaignList == null || campaignList.Count == 0)
                {
                    return null;
                }

                return campaignList;
            }
            catch (Exception err)
            {
                Logger.LogException(err, HttpContext.Current.Request.ServerVariables["URL"]);
                return null;
            }
        }

        public IEnumerable<CampaignDTOv2> GetAllCampaignV2(int modelId, Location locationObj, int platformId, int applicationId, int count = 0)
        {
            try
            {
                var campaignList = GetAllRunningCampaignsOnCriteria(modelId, locationObj, platformId, applicationId, count);

                if (campaignList == null || campaignList.Count == 0)
                {
                    return null;
                }

                foreach (var campaign in campaignList)
                {
                    campaign.Type = (campaign.Type == 0) ? (short)CampaignType.Dealer : (short)CampaignType.Enterprise;
                }

                var campaignDtoList = new List<CampaignDTOv2>();


                foreach (var campaign in campaignList)
                {
                    var currentCampaign = Mapper.Map<Entity.Campaigns.Campaign, CampaignDTOv2>(campaign);

                    currentCampaign.TemplateHtml = _campaignTemplate.GetCampaignTemplate(campaign, SponsoredCarPageId.PQCampaign.GetHashCode().ToString(), platformId);

                    if (campaign.Type == (int)CampaignType.Enterprise && locationObj.CityId > 0)
                    {
                        currentCampaign.Dealers = Mapper.Map<IEnumerable<CTDealerDTO>>(_newCarDealersCache.GetNCSDealers(modelId, locationObj.CityId));
                    }

                    campaignDtoList.Add(currentCampaign);
                }

                return campaignDtoList;
            }
            catch (Exception err)
            {
                Logger.LogException(err, HttpContext.Current.Request.ServerVariables["URL"]);
                return null;
            }
        }

        public CampaignLeadDTO GetCampaignLeadInfo(int leadId)
        {
            try
            {
                return Mapper.Map<DealerInquiryDetails, CampaignLeadDTO>(_campaignRepo.GetCampaignLeadInfo(leadId));
            }
            catch (Exception err)
            {
                Logger.LogException(err, HttpContext.Current.Request.ServerVariables["URL"]);
                return null;
            }
        }

        public SponsoredDealer GetSponsorDealerAd(int modelId, int platformId, Location location)
        {
            try
            {
                int campaignId = GetPersistedCampaign(modelId, location);
                var campaign = GetCampaignByCarLocation(modelId, location, platformId, false, (int)Application.CarWale, campaignId);

                if (CampaignValidation.IsCampaignValid(campaign))
                {
                    SponsoredDealer sponsoredDealer = Mapper.Map<SponsoredDealer>(campaign);

                    if ((platformId == (int)Platform.CarwaleAndroid || platformId == (int)Platform.CarwaleiOS) && String.IsNullOrWhiteSpace(sponsoredDealer.DealerMobile))
                    {
                        sponsoredDealer.DealerMobile = CWConfiguration.tollFreeNumber;
                    }

                    sponsoredDealer.PredictionData = _campaignPrediction.GetPredictionModelResponse(HttpContext.Current.Request, modelId,
                        location.CityId, location.ZoneId, platformId);

                    sponsoredDealer.MaskingNumberEnabled = Array.IndexOf(_removeMaskingNumCampaigns, sponsoredDealer.DealerId) < 0;
                    return sponsoredDealer;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e, "CampaignBL.GetSponsorDealerAd()");
            }
            return (new SponsoredDealer());
        }

        public int GetPersistedCampaign(int modelId, Location locationObj)
        {
            int campaignId = 0;
            string dealerCookie = "";
            try
            {
                if (HttpContext.Current.Request.Cookies["_dealerCityModel"] != null && HttpContext.Current.Request.Cookies["_dealerCityModel"].Values.ToString() != "")
                {
                    dealerCookie = WebUtility.UrlDecode(HttpContext.Current.Request.Cookies["_dealerCityModel"].Values.ToString());
                }

                if (dealerCookie != string.Empty)
                {
                    campaignId = FetchDealerOnCookie(modelId, locationObj, dealerCookie);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, "CampaignBL.fetchDealerOnCookie();dealerCookie=" + (string.IsNullOrWhiteSpace(dealerCookie) ? "empty" : dealerCookie));
            }
            return campaignId;
        }

        public bool SetPersistedCampaign(int modelId, Location location, int campaignId)
        {
            string key = "_dealerCityModel";
            string dealerCookie = "";
            int zoneId = 0; //setting zoneId to 0 for all future references of this cookie
            string persistCampaignData = string.Format("{0}~{1}~{2}~{3}~{4}!", campaignId, location.CityId, modelId, zoneId ,location.AreaId);
            if (HttpContext.Current.Request.Cookies[key] != null && HttpContext.Current.Request.Cookies[key].Values.ToString() != "")
            {
                dealerCookie = WebUtility.UrlDecode(HttpContext.Current.Request.Cookies[key].Values.ToString());
            }

            if (dealerCookie != string.Empty)
            {
                string pattern = String.Format("!*\\d+~{0}~{1}~({2}|-1|0)~({3}|-1|0)!", location.CityId, modelId, location.ZoneId, location.AreaId);
                Match matches = Regex.Match(dealerCookie, pattern);
                if (matches.Length > 0)
                {
                    string partCookie = matches.Index > 0 ? string.Format("!{0}",persistCampaignData) : persistCampaignData;
                    dealerCookie = dealerCookie.Remove(matches.Index, matches.Length).Insert(matches.Index, partCookie);                    
                }
                else
                {
                    dealerCookie += persistCampaignData;
                }
                CookieManager.SetCookieByValue(key, dealerCookie, 1);
            }
            else
            {
                CookieManager.SetCookieByValue(key, persistCampaignData, 1);
            }
            return true;
        }

        private int FetchDealerOnCookie(int modelId, Location locationObj, string dealerCookie)
        {
            try
            {
                int currModelId = modelId;
                if (locationObj != null)
                {
                    int currCityId = locationObj.CityId;
                    int currZoneId = locationObj.ZoneId > 0 ? locationObj.ZoneId : 0;
                    int currAreaId = locationObj.AreaId > 0 ? locationObj.AreaId : 0;
                    string temp = dealerCookie;
                    var arrOfDealerInfo = temp.Split('!');
                    var len = arrOfDealerInfo.Length;
                    for (int i = 0; i < len - 1; i++)
                    {
                        var arrofCampaignInfo = arrOfDealerInfo[i].Split('~');
                        if (arrofCampaignInfo.Length > 3)
                        {
                            var cookieModel = CustomParser.parseIntObject(arrofCampaignInfo[2]);
                            var cookieCity = CustomParser.parseIntObject(arrofCampaignInfo[1]);
                            var cookieZone = CustomParser.parseIntObject(arrofCampaignInfo[3]) > 0 ? CustomParser.parseIntObject(arrofCampaignInfo[3]) : 0;
                            var cookieArea = 0;
                            if (arrofCampaignInfo.Length == 5)
                            {
                                cookieArea = CustomParser.parseIntObject(arrofCampaignInfo[4]) > 0 ? CustomParser.parseIntObject(arrofCampaignInfo[4]) : 0;
                            }
                            if (currCityId == cookieCity && currModelId == cookieModel)
                            {
                                if (arrofCampaignInfo.Length == 5 && cookieArea == currAreaId)
                                {
                                    return CustomParser.parseIntObject(arrofCampaignInfo[0]);
                                }
                                return CustomParser.parseIntObject(arrofCampaignInfo[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CampaignBL.FetchDealerOnCookie()");
            }
            return 0;
        }

        /// <summary>
        /// This function updates the campaign running status (IsRunning column)
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool ChangeCampaignRunningStatus(int campaignId, Entity.Campaigns.CampaignStatus status)
        {
            try
            {
                BoolMessage output = DealerCampaignClient.UpdateCampaignRunningStatus(new ProtoBufClass.Campaigns.CampaignStatus
                {
                    Id = campaignId,
                    RunningStatus = status.Status
                });

                if (output == null)
                {
                    return false;
                }

                Logger.LogInfo("1> ChangeCampaignRunningStatus = " + output.Status + " for campaign = " + campaignId + " Status = " + status.Status);
                
                return output.Status;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CampaignBL.ChangeCampaignRunningStatus()");
                return false;
            }
        }

        /// <summary>
        /// This function returns all running campaigns on model, city, zone, platform & applicationId
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="zoneId"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        /// to be deprecated with GetCampaignByCarLocation
        public List<Entity.Campaigns.Campaign> GetAllRunningCampaignsOnCriteria(int modelId, Location locationObj, int platformId, int applicationId = (int)Application.CarWale, int count = 0)
        {
            try
            {
                locationObj = new ElasticLocation().FormCompleteLocation(locationObj);

                var campaigns = DealerCampaignClient.GetCampaignsOnModelCityPlatform(new PQRule
                {
                    Model = new Item { Id = modelId },
                    City = new Item { Id = locationObj.CityId },
                    Zone = new Item { Id = locationObj.ZoneId },
                    Platform = new Item { Id = platformId },
                    ApplicationId = applicationId,
                    Count = count
                });

                var campaignsList = new List<Entity.Campaigns.Campaign>();
                if (campaigns != null && campaigns.Campaigns != null)
                {
                    foreach (var campaign in campaigns.Campaigns)
                    {
                        var campaignDetails = Mapper.Map<ProtoBufClass.Campaigns.Campaign, Entity.Campaigns.Campaign>(campaign);
                        if (campaignDetails != null)
                        {
                            campaignsList.Add(campaignDetails);
                        }
                    }
                }
                return campaignsList;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CampaignBL.GetAllRunningCampaignsOnCriteria()");
            }
            return new List<Entity.Campaigns.Campaign>();
        }



        /// <summary>
        /// This function returns the final running campaign on model, city, zone, platform & applicationId.It encompasses nearest
        /// dealer logic,random logic, persistance logic and filtering campaigns by priority
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="zoneId"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public Carwale.Entity.Campaigns.Campaign GetCampaignByCarLocation(int modelId, Location locationObj, int platformId, bool recommendation, int applicationId = (int)Application.CarWale, int campaignId = 0)
        {
            try
            {
                locationObj = new ElasticLocation().FormCompleteLocation(locationObj);

                var campaign = DealerCampaignClient.GetCampaign(new CampaignRequest
                {
                    Model = modelId,
                    Platform = platformId,
                    ApplicationId = applicationId,
                    ShowRecommendation = recommendation,
                    UserLocation = new ProtoBufClass.Campaigns.GeoLocation
                    {
                        CityId = locationObj.CityId,
                        ZoneId = locationObj.ZoneId,
                        AreaId = locationObj.AreaId
                    },
                    CampaignId = campaignId
                });

                var campaignDetails = Mapper.Map<ProtoBufClass.Campaigns.Campaign, Entity.Campaigns.Campaign>(campaign);
                return campaignDetails;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CampaignBL.GetCampaignByCarLocation()");
            }
            return null;
        }

        /// <summary>
        /// This function returns campaign details
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public Entity.Campaigns.Campaign GetCampaignDetails(int campaignId)
        {
            try
            {
                var campaignDetails = DealerCampaignClient.GetRunningCampaignDetailsById(new CampaignId() { Id = campaignId, UseCache = new BoolMessage() { Status = true } });
                if (campaignDetails != null)
                {
                    var campaign = Mapper.Map<ProtoBufClass.Campaigns.Campaign, Entity.Campaigns.Campaign>(campaignDetails);
                    return campaign;
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e, "CampaignBL.GetCampaignDetails()");
            }

            return new Entity.Campaigns.Campaign();
        }

        public CvlDetailsDTO GetCampaignCvlDetails(int campaignId)
        {
            Entity.Campaigns.Campaign campaign = GetCampaignDetails(campaignId);
            CvlDetails cvlDetails = new CvlDetails();

            if (campaign != null && campaign.Id > 0)
            {
                cvlDetails = campaign.CvlDetails;
            }
            else
            {
                try
                {
                    cvlDetails.IsCvl = DealerCampaignClient.GetCampaignDetailsById(new CampaignId { Id = campaignId }).IsCVL;
                }
                catch (Exception e)
                {
                    cvlDetails.IsCvl = true;
                    Logger.LogException(e, "CampaignBL.GetCampaignCvlDetails()");
                }
            }

            var cvlDetailsDto = Mapper.Map<CvlDetailsDTO>(cvlDetails);

            return cvlDetailsDto;
        }

        public List<CityDTO> GetCampaignCities(int campaignId, int modelId)
        {
            List<CityDTO> cities = new List<CityDTO>();
            try
            {
                ProtoBufClass.Campaigns.CampaignCities campaignCities = DealerCampaignClient.GetCampaignCities(new CampaignCitiesInput
                {
                    CampaignId = campaignId,
                    ModelId = modelId,
                    UseCache = new BoolMessage
                    {
                        Status = true
                    }
                });

                if (campaignCities != null)
                {
                    if (campaignCities.Type == (int)Entity.Enum.CampaignCities.CampaignRulesAvailable)
                    {
                        cities = Mapper.Map<List<CityDTO>>(campaignCities.Cities);
                    }
                    else if (campaignCities.Type == (int)Entity.Enum.CampaignCities.ModelPriceAvailable)
                    {
                        cities = Mapper.Map<List<CityDTO>>(_geoCitiesCacheRepo.GetPQCitiesByModelId(modelId));
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e, "CampaignBL.GetCampaignCities()");
            }

            return cities;
        }

        public bool IsCityCampaignExist(int modelId, Location locationObj, int platformId, int applicationId)
        {
            try
            {
                int isAreaCity = Array.IndexOf(CWConfiguration.AskingAreaCityIds, locationObj.CityId);

                if (isAreaCity > -1 && (locationObj.AreaId < 1 && locationObj.ZoneId < 1))
                {
                    return DealerCampaignClient.IsCampaignAvailable(new PQRule
                    {
                        Model = new ProtoBufClass.Common.Item { Id = modelId },
                        City = new ProtoBufClass.Common.Item { Id = locationObj.CityId },
                        Platform = new ProtoBufClass.Common.Item { Id = platformId },
                        ApplicationId = applicationId
                    }).Status;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }

        public Entity.Campaigns.Campaign GetDealerCampaign(int modelId, Location location, int platformId, int campaignId)
        {
            try
            {
                Entity.Campaigns.Campaign campaign = GetCampaignByCarLocation(modelId, location, platformId, false, (int)Application.CarWale, campaignId);

                if (CampaignValidation.IsCampaignValid(campaign))
                {
                    campaign.PredictionData = _campaignPrediction.GetPredictionModelResponse(HttpContext.Current.Request, modelId,
                        location.CityId, location.ZoneId, platformId);
                }

                if ((platformId == (int)Platform.CarwaleAndroid || platformId == (int)Platform.CarwaleiOS) && String.IsNullOrWhiteSpace(campaign.ContactNumber))
                {
                    campaign.ContactNumber = CWConfiguration.tollFreeNumber;
                }

                campaign.MaskingNumberEnabled = Array.IndexOf(_removeMaskingNumCampaigns, campaign.Id) < 0;

                

                return campaign;
            }
            catch (Exception e)
            {
                Logger.LogException(e, "CampaignBL.GetCampaign()");
                return null;
            }
        }

        public int FetchCampaignIdByDealerId(int dealerId)
        {
            int campaignId = 0;
            try
            {
                var campaigns = DealerCampaignClient.GetCampaignDetailsByCriteria(new CampaignCriteria
                {
                    DealerId = dealerId
                });
                var campaignList = campaigns.Campaigns.Where(x => (x.IsRunning == 1 || x.IsRunning == 2) && x.IsActive);
                campaignId = campaignList.Count() > 0 ? campaignList.First().Id.Id : 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return campaignId;
        }

        public List<Tuple<int, Entity.Campaigns.Campaign>> GetMultipleCampaign(List<int> modelIds,
            Location locationObj, int platformId, int count, int applicationId = (int)Application.CarWale, bool isCheckRecommendation = true)
        {
            List<Tuple<int, Entity.Campaigns.Campaign>> multipleCampaign = new List<Tuple<int, Carwale.Entity.Campaigns.Campaign>>();
            try
            {
                IApiGatewayCaller apiGatewayCaller = _unityContainer.Resolve<IApiGatewayCaller>();
                FetchCampaign(apiGatewayCaller, modelIds, locationObj, platformId, isCheckRecommendation, applicationId);
                int campaignCount = 0;
                for (int index = 0; modelIds != null && index < modelIds.Count && campaignCount < count; index++)
                {
                    try
                    {
                        Entity.Campaigns.Campaign campaign = Mapper.Map<ProtoBufClass.Campaigns.Campaign, Entity.Campaigns.Campaign>(apiGatewayCaller.GetResponse<ProtoBufClass.Campaigns.Campaign>(index));
                        if (CampaignValidation.IsCampaignValid(campaign))
                        {
                            Tuple<int, Entity.Campaigns.Campaign> campaignTuple = new Tuple<int, Entity.Campaigns.Campaign>(modelIds[index], campaign);
                            multipleCampaign.Add(campaignTuple);
                            campaignCount++;
                        }
                    }
                    catch (GateWayException e)
                    {
                        Logger.LogError("Call for campaign through API gateway failed with exception " + e);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e, "CampaignBL.FetchCampaign()");
            }
            return multipleCampaign;
        }

        private void FetchCampaign(IApiGatewayCaller apiGatewayCaller, List<int> modelIds, Location locationObj, int platformId, bool recommendation, int applicationId)
        {
            try
            {
                foreach (var model in modelIds)
                {
                    if (model > 0)
                    {
                        apiGatewayCaller.AddGetCampaign(model, locationObj, platformId, recommendation, applicationId);
                    }
                }
                apiGatewayCaller.Call();
            }
            catch (GateWayException e)
            {
                Logger.LogException(e);
            }
        }

        public Dictionary<int, int> FetchCampaignByDealers(List<int> dealerIds)
        {
            Dictionary<int, int> dealerCampaign = new Dictionary<int, int>();
            try
            {
                IApiGatewayCaller apiGatewayCaller = _unityContainer.Resolve<IApiGatewayCaller>();
                foreach (var dealerId in dealerIds)
                {
                    if (dealerId > 0)
                    {
                        apiGatewayCaller.AddGetCampaignDetailsByCriteria(dealerId);
                    }
                }
                apiGatewayCaller.Call();

                for (int index = 0; dealerIds != null && index < dealerIds.Count; index++)
                {
                    try
                    {
                        var campaigns = apiGatewayCaller.GetResponse<ProtoBufClass.Campaigns.CampaignList>(index);
                        if (campaigns != null)
                        {
                            var filteredCampaign = campaigns.Campaigns.Where(x => (x.IsRunning == 1 || x.IsRunning == 2) && x.IsActive);
                            int campaignId = filteredCampaign.Count() > 0 ? filteredCampaign.First().Id.Id : 0;
                            dealerCampaign.Add(dealerIds[index], campaignId);
                        }
                    }
                    catch (GateWayException e)
                    {
                        Logger.LogError("Call for campaign through API gateway failed with exception " + e);
                    }
                }
            }
            catch (GateWayException e)
            {
                Logger.LogException(e, "CampaignBL.FetchCampaignByDealers()");
            }
            return dealerCampaign;
        }

        /// <summary>
        /// This function returns all running campaigns on model, location, platform & applicationId
        /// It also sets the distance of dealer from userlocation and dealerarea.
        /// By default campaigns are sorted by distance and priority respectively
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="locationObj"></param>
        /// <param name="platformId"></param>
        /// <param name="applicationId"></param>
        /// <returns>list of running campaigns /returns>
        public List<DealerAd> GetAllRunningCampaigns(int modelId, Location locationObj, int platformId, int applicationId, bool isDealerLocator, bool dealerAdminFilter)
        {
            List<DealerAd> dealerAdList = new List<DealerAd>();

            try
            {
                locationObj = new ElasticLocation().FormCompleteLocation(locationObj);

                var allCampaigns = DealerCampaignClient.GetAllRunningCampaigns(new CampaignRequest
                {
                    Model = modelId,
                    Platform = platformId,
                    ApplicationId = applicationId,
                    UserLocation = Mapper.Map<Entity.Geolocation.Location, ProtoBufClass.Campaigns.GeoLocation>(locationObj),
                    DealerLocator = isDealerLocator,
                    DealerAdminFilter = dealerAdminFilter
                });

                if (allCampaigns != null && allCampaigns.Campaigns != null)
                {
                    HashSet<int> distinctDealerIds = new HashSet<int>();
                    foreach (var campaign in allCampaigns.Campaigns)
                    {
                        var dealerAd = new DealerAd();
                        var dealerDeatails = new DealerDetails();
                        var campaignDetails = Mapper.Map<ProtoBufClass.Campaigns.Campaign, Entity.Campaigns.Campaign>(campaign);
                        if (campaignDetails != null && !distinctDealerIds.Contains(campaign.DealerId))
                        {
                            distinctDealerIds.Add(campaign.DealerId);
                            dealerDeatails.DealerId = campaign.DealerId;
                            dealerDeatails.Distance = campaign.Distance != 0 ? campaign.Distance : 1;
                            dealerAd.DealerDetails = dealerDeatails;
                            dealerAd.Campaign = campaignDetails;
                            dealerAd.CampaignType = CampaignAdType.Pq;
                            dealerAd.FeaturedCarData = new CarVersionDetails { ModelId = modelId };
                            dealerAdList.Add(dealerAd);
                        }
                    }

                    var dealerIds = dealerAdList.Select(x => x.DealerDetails.DealerId).ToList();

                    Dictionary<int, DealerDetails> dealerDetails = _dealersCache.MultiGetDealerDetails(dealerIds);

                    foreach (DealerAd dealerAd in dealerAdList)
                    {
                        if (dealerDetails.ContainsKey(dealerAd.DealerDetails.DealerId) && dealerDetails[dealerAd.DealerDetails.DealerId] != null)
                        {
                            dealerAd.DealerDetails.DealerArea = dealerDetails[dealerAd.DealerDetails.DealerId].DealerArea;
                            dealerAd.DealerDetails.Name = dealerDetails[dealerAd.DealerDetails.DealerId].Name;
                        }
                    }

                    dealerAdList = dealerAdList.OrderBy(x => x.DealerDetails.Distance).ThenBy(x => x.Campaign.Priority).ToList();

                    dealerAdList.ForEach(dealerAd =>
                        dealerAd.DealerDetails.Distance = dealerAd.DealerDetails.Distance == Int16.MaxValue ? -1 : dealerAd.DealerDetails.Distance
                        );
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CampaignBL.GetAllRunningCampaigns()");
            }
            return dealerAdList;
        }

        public bool ValidateLocationOnArea(Location locationObj)
        {
            if (locationObj != null && locationObj.AreaId > 0)
            {
                Location locationElastic = Mapper.Map<Area, Location>(new ElasticLocation().GetLocation(locationObj.AreaId));
                if (locationElastic != null)
                {
                    return locationElastic.CityId == locationObj.CityId;
                }
            }
            return false;
        }

        public bool IsTestDriveCampaign(int campaignId)
        {
            return _testDriveCampaignIds.IndexOf(campaignId) > -1;
        }
    }
}
