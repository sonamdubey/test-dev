using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Dealer;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Dealers;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using ProtoBufClass.Campaigns;
using Carwale.Utility;
using Carwale.Interfaces.Campaigns;
using Carwale.Entity.Enum;
using System.Linq;
using Carwale.Notifications.Logs;
using AEPLCore.Cache.Interfaces;
using System.Configuration;
using Campaigns.DealerCampaignClient;

namespace Carwale.BL.Dealers
{
    public class NewCarDealers : INewCarDealers
    {
        protected readonly INewCarDealersRepository _dealersRepo;
        protected readonly INewCarDealersCache _dealersCacheRepo;
        protected readonly ICacheManager _cacheProvider;
        protected readonly IUnityContainer _container;
        protected readonly IDealers _dealersBL;
        protected readonly ICarModels _carModelsBL;
        protected readonly ICampaign _campaignBL;
        private readonly ICarModelCacheRepository _carModelCache;
        private readonly IDealerCache _dealerCache;
        private readonly IDealerSponsoredAdRespository _dealerSponsorRepo;
        private readonly string _dealerLocatorPriorityDealerId =ConfigurationManager.AppSettings["DealerLocatorPriorityDealerId"] ?? "0";

        public NewCarDealers(IUnityContainer container, INewCarDealersRepository dealersRepo, ICacheManager cacheProvider, INewCarDealersCache dealersCacheRepo, IDealers dealersBL,
            ICampaign campaignBL, ICarModelCacheRepository carModelCache, ICarModels carModelsBL, IDealerCache dealerCache, IDealerSponsoredAdRespository dealerSponsorRepo) // modified by
        {
            _dealersRepo = dealersRepo;
            _dealersCacheRepo = dealersCacheRepo;
            _cacheProvider = cacheProvider;
            _container = container;
            _dealersBL = dealersBL;
            _carModelsBL = carModelsBL;
            _campaignBL = campaignBL;
            _carModelCache = carModelCache;
            _dealerCache = dealerCache;
            _dealerSponsorRepo = dealerSponsorRepo;
        }

        public NewCarDealerEntiy GetDealersList(int stateId, int cityId, int makeId, bool showDealerImage = true)
        {
            var dealersList = _cacheProvider.GetFromCache<NewCarDealerEntiy>(string.Format("NewCarDealers_v2_{0}_{1}_{2}", stateId, cityId, makeId));
            if (dealersList == null)
            {
                dealersList = GetDealersListFromDB(makeId, cityId, stateId, showDealerImage);
            }

            ReorderDealerList(dealersList);


            return dealersList;
        }

        private void ReorderDealerList(NewCarDealerEntiy dealersList)
        {
            if (dealersList != null && dealersList.NewCarDealers.Count > 0)
            {
                for(int i=1; i< dealersList.NewCarDealers.Count ; i++)
                {
                    if (_dealerLocatorPriorityDealerId.Contains(dealersList.NewCarDealers[i].DealerId.ToString()))
                    {
                        var item = dealersList.NewCarDealers[i];
                        dealersList.NewCarDealers.RemoveAt(i);
                        if (item.IsPremium)
                        {
                            dealersList.NewCarDealers.Insert(0, item);
                        }
                        else
                        {
                            int indexToInsert = dealersList.NewCarDealers.FindIndex(x => x.IsPremium == false);
                            if (indexToInsert >= 0)
                            {
                                dealersList.NewCarDealers.Insert(indexToInsert, item);
                            }
                            else
                            {
                                dealersList.NewCarDealers.Add(item);
                            }
                        }
                    }
                }
            }
        }
        private NewCarDealerEntiy GetDealersListFromDB(int makeId, int cityId, int stateId, bool showDealerImage)
        {
            var newCarDealers = new NewCarDealerEntiy();
            var newCarDealersList = new List<NewCarDealer>();
            try
            {
                var dealers = DealerCampaignClient.GetDealersOnMakeCity(new MakeCity
                {
                    Make = new ProtoBufClass.Common.Item { Id = makeId },
                    City = new ProtoBufClass.Common.Item { Id = cityId },
                    State = new ProtoBufClass.Common.Item { Id = stateId }
                });

                if (dealers != null)
                {
                    foreach (var dealer in dealers.DealerDetails.Where(d => d.IsLocatorActive == true))
                    {
                        var dealerDetails = Mapper.Map<ProtoBufClass.Campaigns.Dealer, Entity.Dealers.NewCarDealer>(dealer);

                        if (dealer.DealerLocatorConfigurationId > 0 && dealer.CampaignId.Id > 0)
                        {
                            var campaignDetails = _campaignBL.GetCampaignDetails(dealer.CampaignId.Id);

                            if (campaignDetails.DealerId > 0)
                            {
                                dealerDetails.MobileNo = campaignDetails.ContactNumber;
                                dealerDetails.IsPremium = true;
                                dealerDetails.BusinessType = campaignDetails.Type;
                                dealerDetails.CampaignId = campaignDetails.Id;
                                dealerDetails.ServerTime = DateTime.Now.ToUnixTime();
                                dealerDetails.ShowEmail = campaignDetails.IsEmailRequired;
                            }
                        }
                        if (dealer.StartTime != null && dealer.EndTime != null)
                        {
                            dealerDetails.ShowroomStartTime = dealer.StartTime;
                            dealerDetails.ShowroomEndTime = dealer.EndTime;
                            dealerDetails.WorkingTime = _dealersBL.GetDealerWorkingTime(dealer.StartTime, dealer.EndTime);
                        }
                        dealerDetails.DealerContent = "";

                        if (showDealerImage)
                        {
                            var micrositeImages = _dealersBL.GetDealerImages(dealer.Dealer_.Id).Where(x => x.IsMainBanner).FirstOrDefault();
                            if (micrositeImages != null)
                            {
                                dealerDetails.ProfilePhotoHostUrl = micrositeImages.HostUrl;
                                dealerDetails.ProfilePhotoUrl = null;
                                dealerDetails.ShowroomImage = (!string.IsNullOrEmpty(micrositeImages.IsMainBanner.ToString())) ? micrositeImages.HostUrl + ImageSizes._940X300 + micrositeImages.OriginalImgPath : "";
                            }
                        }
                        dealerDetails.ShowroomImage = dealerDetails.ShowroomImage == null ? "" : dealerDetails.ShowroomImage;
                        newCarDealersList.Add(dealerDetails);
                        newCarDealers.ShareUrl = "https://www.carwale.com/new/" + Format.RemoveSpecialCharacters(dealer.Make.Name) + "-dealers/" + cityId + "-" + Format.RemoveSpecialCharacters(dealer.City.Name) + ".html";
                        newCarDealers.MakeName = dealer.Make.Name;
                        newCarDealers.CityName = dealer.City.Name;
                    }

                    newCarDealers.NewCarDealers = newCarDealersList.OrderByDescending(x => x.IsPremium).ToList();
                    _dealersCacheRepo.StoreDealersList(string.Format("NewCarDealers_v2_{0}_{1}_{2}", stateId, cityId, makeId), newCarDealers);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealers.GetDealersListFromDB()");
                objErr.LogException();
            }
            return newCarDealers;
        }

        public List<CarMakeEntityBase> GetMakesByCity(int cityId)
        {
            return _dealersCacheRepo.GetMakesByCity(cityId);
        }

        /// <summary>
        /// Returns the list of Cities and popular cities based on makeid passed.
        /// Written By : Supriya on 8/9/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Tuple<List<DealerStateEntity>, List<PopularCitiesEntity>> GetCitiesByMake(int makeId)
        {
            var listStateCity = new List<DealerStateEntity>();
            var listPopularCity = new List<PopularCitiesEntity>();

            listStateCity = GetStatesAndCitiesByMake(makeId);
            listPopularCity = _dealersCacheRepo.GetPopularCitiesByMake(makeId);

            return new Tuple<List<DealerStateEntity>, List<PopularCitiesEntity>>(listStateCity, listPopularCity);
        }

        public List<NewCarDealerCountByMake> GetMakesAndCount(string type)
        {
            return _dealersCacheRepo.NewCarDealerCountMake(type);
        }

        public DealerShowroomDetails GetDealerDetails(int dealerId, int? pqCampaignId, int? pqCityId, int pqMakeId = 0)
        {
            DealerShowroomDetails resp = new DealerShowroomDetails();
            try
            {
                var dealerDetails = new DealerDetails();
                dealerDetails = _cacheProvider.GetFromCache<DealerDetails>(string.Format("DealerDetails_{0}_camp{1}_city{2}_make{3}", dealerId, pqCampaignId, pqCityId, pqMakeId));

                if (dealerDetails == null)
                {
                    dealerDetails = NCDDetails(dealerId, CustomParser.parseIntObject(pqCampaignId), CustomParser.parseIntObject(pqMakeId), CustomParser.parseIntObject(pqCityId));
                }

                resp.objImageList = new List<AboutUsImageEntity>();
                if (dealerDetails.DealerId > 0)
                {
                    _dealersCacheRepo.StoreDealerDetails(string.Format("DealerDetails_{0}_camp{1}_city{2}_make{3}", dealerId, pqCampaignId, pqCityId, pqMakeId), dealerDetails);
                    resp.objImageList = _dealersBL.GetDealerImages(dealerDetails.DealerId);
                }

                resp.objDealerDetails = dealerDetails;

                resp.objModelDetails = _dealerCache.GetDealerModelsOnMake((pqMakeId != 0 ? pqMakeId : Convert.ToInt32(dealerDetails.MakeId)), dealerDetails.DealerId);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealers.GetDealerDetails()");
                objErr.LogException();
            }
            return resp;
        }

        /// <summary>
        /// for corrensponding make of input model,
        /// Searchs makeid in webconfig "TheNumberMakes" key and returns "TheNumber" key value if found
        /// otherwise returns empty string if not found
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public string CallSlugNumberByModelId(int modelId)
        {

            ICarModelCacheRepository cache = _container.Resolve<ICarModelCacheRepository>();

            return CallSlugNumberByMakeId(cache.GetModelDetailsById(modelId).MakeId);
        }

        /// <summary>
        /// Searchs makeid in webconfig "CallSlugMakes" key and returns "CallSlugNumber" key value if found
        /// otherwise returns empty string if not found
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public string CallSlugNumberByMakeId(int makeId)
        {
            List<int> makes = CallSlugMakes();
            bool makeFound = makes.Contains(makeId);
            if (makeFound)
            {
                string number = System.Configuration.ConfigurationManager.AppSettings["CallSlugNumber"];
                return !string.IsNullOrEmpty(number) ? number : "";
            }
            return "";
        }

        /// <summary>
        /// returns the list of makeids for "CallSlugMakes" key value
        /// </summary>
        /// <returns></returns>
        public List<int> CallSlugMakes()
        {
            List<int> makeList = new List<int>();

            string makeIds = System.Configuration.ConfigurationManager.AppSettings["CallSlugMakes"];

            var makearray = !string.IsNullOrWhiteSpace(makeIds) ? makeIds.Split(',') : new string[] { "" };

            if (!string.IsNullOrWhiteSpace(makearray[0]))
            {
                foreach (string makeid in makearray)
                {
                    makeList.Add(Convert.ToInt16(makeid));
                }
            }
            return makeList;
        }

        /// <summary>
        /// Returns the list of states and cities of particular make in alphabetical order 
        /// Written By : Shalini Nair on 02/12/2015
        /// Modified By : Shalini Nair on 14/12/2015
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<DealerStateEntity> GetStatesAndCitiesByMake(int makeId)
        {
            List<DealerStateEntity> statesAndCities = new List<DealerStateEntity>();
            try
            {
                statesAndCities = _dealersCacheRepo.GetCitiesByMake(makeId);
                var count = 0;
                List<DealerStateEntity> states = null;
                if (statesAndCities.IsNotNullOrEmpty())
                {
                    count = statesAndCities.Select(x => x.TotalCount).ToList().Sum();
                    states = statesAndCities.Select(x => new DealerStateEntity { StateName = x.StateName, StateId = x.StateId }).GroupBy(x => x.StateId)
                        .Select(i => i.First()).OrderBy(x => x.StateName).ToList();

                    foreach (var city in states) //adding cities to state list
                    {
                        List<DealerCityEntity> currentCities = statesAndCities.Where(y => y.StateId == city.StateId)
                            .Select(x => new DealerCityEntity { CityId = x.CityId, CityName = x.CityName, CityMaskingName = x.CityMaskingName, TotalCount = x.TotalCount })
                            .OrderBy(x => x.CityName).ToList();

                        city.cities.AddRange(currentCities);
                    }
                    statesAndCities = states;
                    statesAndCities[0].TotalCount = count;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "NewCarDealers.GetStatesAndCitiesByMake()");
            }
            return statesAndCities ?? new List<DealerStateEntity>();
        }

        /// <summary>
        /// Returns the list of MakeId and Models with respect to perticular Dealer 
        /// Written By : Chetan Thambad on 03/02/2016
        /// </summary> 
        public List<DealerModelListDTO> DealerModelListBl(int dealerId)
        {
            var dealerModelList = new List<DealerModelListDTO>();
            try
            {
                List<MakeModelEntity> makeModelEntityList = _dealersCacheRepo.GetDealerModels(dealerId);
                List<int> makeIdList = makeModelEntityList.Select(s => s.MakeId).Distinct().ToList();
                Mapper.CreateMap<MakeModelEntity, CarModelsDTO>();
                foreach (var makeId in makeIdList)
                {
                    dealerModelList.Add(new DealerModelListDTO()
                    {
                        MakeId = makeId,
                        ModelDetails = Mapper.Map<List<MakeModelEntity>, List<CarModelsDTO>>(makeModelEntityList.Where(x => x.MakeId == makeId).ToList())
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealers.DealerModelListBl()");
                objErr.LogException();
            }

            return dealerModelList;
        }

        /// <summary>
        /// This function returns the list of new car dealers considering the exclusion models
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public NewCarDealerEntiy GetNCDealersList(int makeId, int modelId, int cityId)
        {
            try
            {
                var dealersList = GetDealersList(-1, cityId, makeId, false);
                var _dealersCache = _container.Resolve<IDealerCache>();

                foreach (var dealer in dealersList.NewCarDealers.ToList())
                {
                    var dealerModels = _dealersCache.GetDealerModelsOnMake(makeId, dealer.DealerId);

                    if (dealer.IsPremium && !dealerModels.Exists(x => x.ModelId == modelId))
                    {
                        dealersList.NewCarDealers.Find(a => a.DealerId == dealer.DealerId).IsPremium = false;
                    }
                }
                return dealersList;
            }
            catch (Exception ex)
            {
                var exObj = new ExceptionHandler(ex, "NewCarDealers.GetNCDealersList()");
                exObj.LogException();
                return null;
            }
        }

        public DealerDetails GetPremiumDealerDetails(int dealerId)
        {
            try
            {
                if (dealerId > 0)
                {
                    var dealerDetails = DealerCampaignClient.GetPremiumDealerDetails(new ProtoBufClass.Campaigns.DealerId
                    {
                        Id = new ProtoBufClass.Common.Item { Id = dealerId },
                        ApplicationId = (int)Application.CarWale
                    });
                    var premiumDealerDetails = Mapper.Map<ProtoBufClass.Campaigns.Dealer, DealerDetails>(dealerDetails);

                    if (premiumDealerDetails.CampaignId > 0)
                    {
                        if (premiumDealerDetails.IsLocatorActive)
                        {
                            premiumDealerDetails.ContactNo = premiumDealerDetails.Mobile;
                            premiumDealerDetails.IsPremium = 1;
                            //premiumDealerDetails.Mobile = premiumDealerDetails.ContactNo;
                        }
                        return premiumDealerDetails;
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealers.GetPremiumDealerDetails() dealerId-" + dealerId);
                objErr.LogException();
            }
            return new DealerDetails();
        }

        public DealerDetails GetPremiumDealerDetails(int campaignId, int makeId, int cityId)
        {
            try
            {
                var dealersList = DealerCampaignClient.GetDealerDetailsByCampaignId(new CampaignId
                {
                    Id = campaignId,
                    ApplicationId = (int)Application.CarWale
                });
                var dealersOnMakeCity = dealersList.DealerDetails.Where(dealer => dealer.City.Id == cityId && dealer.Make.Id == makeId && dealer.IsLocatorActive).FirstOrDefault();

                var dealerDetails = Mapper.Map<ProtoBufClass.Campaigns.Dealer, DealerDetails>(dealersOnMakeCity);

                if (dealerDetails != null && dealerDetails.CampaignId > 0)
                {
                    //dealerDetails.Mobile = dealerDetails.ContactNo;
                    return dealerDetails;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealers.GetPremiumDealerDetails() campaignId-" + campaignId + "makeId-" + makeId + "cityId-" + cityId);
                objErr.LogException();
            }

            return new DealerDetails();
        }

        public DealerDetails NCDDetails(int dealerId, int campaignId, int makeId, int cityId)
        {
            if (campaignId > 0)
                return GetPremiumDealerDetails(campaignId, makeId, cityId);
            else
                return GetPremiumDealerDetails(dealerId);
        }

        public DealerDetails GetCampaignDealerDetails(int campaignType, int modelId, int dealerId, int campaignId, int cityId)
        {
            var dealerDetails = new DealerDetails();
            try
            {
                if (campaignType == DealerShowroomBusinessTypeEnum.LeadBusinessType.GetHashCode())
                {
                    int makeId = _carModelCache.GetModelDetailsById(modelId).MakeId;
                    dealerDetails = NCDDetails(dealerId, campaignId, makeId, cityId);

                    if (string.IsNullOrEmpty(dealerDetails.Name))
                        return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealers.GetCampaignDealerDetails");
                objErr.LogException();
            }
            return dealerDetails;
        }

        public List<DealerLocatorDTO> GetDealersByMakeModel(int makeId, int modelId, int cityId)
        {
            List<DealerLocatorDTO> dealerLocatorList = new List<DealerLocatorDTO>();
            try
            {
                NewCarDealerEntiy dealersListbyModel = new NewCarDealerEntiy();
                NewCarDealerEntiy dealersList = GetDealersList(-1, cityId, makeId, false);
                if (modelId > 0)
                {
                    foreach (var dealer in dealersList.NewCarDealers)
                    {
                        var dealerModels = _dealerCache.GetDealerModelsOnMake(makeId, dealer.DealerId);
                        if (dealerModels != null && dealerModels.Exists(x => x.ModelId == modelId))
                        {
                            dealersListbyModel.NewCarDealers.Add(dealer);
                        }
                    }
                }
                dealerLocatorList = Mapper.Map<List<NewCarDealer>, List<DealerLocatorDTO>>(modelId > 0 ? dealersListbyModel.NewCarDealers : dealersList.NewCarDealers);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealers.GetDealersByMakeModel");
                objErr.LogException();
            }
            return dealerLocatorList;
        }

        public SponsoredDealer GetCampaignDealerDetailsById(DealerInquiryDetails t)
        {
            var campaignDetails = new SponsoredDealer();
            try
            {
                if (t.PlatformSourceId == (int)Platform.CarwaleiOS)
                {
                    var campaignList = DealerCampaignClient.GetCampaignsOnModelCityPlatform(new PQRule()
                    {
                        Model = new ProtoBufClass.Common.Item { Id = t.ModelId },
                        City = new ProtoBufClass.Common.Item { Id = t.CityId },
                        Zone = new ProtoBufClass.Common.Item { Id = CustomParser.parseIntObject(t.ZoneId) },
                        Platform = new ProtoBufClass.Common.Item { Id = t.PlatformSourceId },
                        ApplicationId = t.ApplicationId
                    });

                    if (campaignList != null && campaignList.Campaigns != null && campaignList.Campaigns.Count > 0)
                    {
                        var campaignById = campaignList.Campaigns.FirstOrDefault(x => (x.Id.Id == t.DealerId));
                        var campaignByDealerId = campaignList.Campaigns.FirstOrDefault(x => (x.DealerId == t.DealerId));

                        if ((campaignById != null && campaignById.Id.Id > 0) && (campaignByDealerId == null || campaignByDealerId.Id.Id <= 0))
                        {
                            campaignDetails = Mapper.Map<SponsoredDealer>(campaignById);
                        }
                        else if ((campaignById == null || campaignById.Id.Id <= 0) && (campaignByDealerId != null && campaignByDealerId.Id.Id > 0))
                        {
                            t.DealerId = campaignByDealerId.Id.Id;
                            campaignDetails = Mapper.Map<SponsoredDealer>(campaignByDealerId);
                        }
                        else
                        {
                            return campaignDetails;
                        }
                    }
                }
                else
                {
                    var campaignById = DealerCampaignClient.GetCampaignDetailsById(new CampaignId() { Id = t.DealerId });
                    if (campaignById != null && campaignById.Id.Id > 0)
                    {
                        campaignDetails = Mapper.Map<SponsoredDealer>(campaignById);
                    }
                }

                var dealerDetails = _dealerSponsorRepo.GetDealerDetailsByDealerId(campaignDetails.DealerId);

                campaignDetails.DealerActualMobile = dealerDetails.DealerMobile;
                campaignDetails.DealerLeadBusinessType = (campaignDetails.LeadPanel == (short)LeadPanel.Autobiz) ? (short)0 : (short)1;
                campaignDetails.DealerAddress = dealerDetails.DealerAddress;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignBL.GetCampaignDealerDetailsById()");
                objErr.LogException();
            }
            return campaignDetails;
        }


        public IEnumerable<NewCarDealersList> GetNcsDealers(int modelId, int cityId, int campaignId)
        {
            try
            {
                var dealerList = _dealersCacheRepo.GetNCSDealers(modelId, cityId);
                var clientCampaignMappings = _dealersCacheRepo.GetClientCampaignMapping();
                var mapping = clientCampaignMappings.Where(camp => camp.CampaignId == campaignId).FirstOrDefault();
                if (dealerList != null && mapping != null)
                {
                    return dealerList.Where(x => x.ClientId == mapping.ClientId);
                }
                return dealerList;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public NewCarDealerEntiy NewCarDealerListByCityMake(int makeId, int modelId, int cityId, bool withCampaign)
        {
            try
            {
                NewCarDealerEntiy newCarDealers = new NewCarDealerEntiy();
                var dealersList = _dealersCacheRepo.GetDealerListByCityMake(makeId, cityId);

                if (dealersList != null && dealersList.Any())
                {
                    if (withCampaign)
                    {
                        var campaignsList = GetCampaignList(modelId, cityId);

                        BindDealerListWithCampaign(dealersList, campaignsList);
                    }
                    newCarDealers.NewCarDealers = dealersList.OrderByDescending(x => x.IsPremium).ToList();
                }

                return newCarDealers;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        private void BindDealerListWithCampaign(IEnumerable<NewCarDealer> dealersList, List<Entity.Campaigns.Campaign> campaignsList)
        {
            if (dealersList != null && dealersList.Any() && campaignsList != null && campaignsList.Count > 0)
            {
                var dealersCampaigns = (from dealer in dealersList
                                        join campaign in campaignsList
                                        on dealer.DealerId equals campaign.DealerId
                                        select new { dealer, campaign });
                foreach (var dealerCampaign in dealersCampaigns)
                {
                    dealerCampaign.dealer.MobileNo = dealerCampaign.campaign.ContactNumber;
                    dealerCampaign.dealer.IsPremium = true;
                    dealerCampaign.dealer.BusinessType = dealerCampaign.campaign.Type;
                    dealerCampaign.dealer.CampaignId = dealerCampaign.campaign.Id;
                    dealerCampaign.dealer.ServerTime = DateTime.Now.ToUnixTime();
                    dealerCampaign.dealer.ShowEmail = dealerCampaign.campaign.IsEmailRequired;
                }
            }
        }

        private List<Entity.Campaigns.Campaign> GetCampaignList(int modelId, int cityId)
        {
            try
            {
                var campaigns = DealerCampaignClient.GetCampaignsOnModelCityPlatform(new PQRule
                {
                    Model = new ProtoBufClass.Common.Item { Id = modelId },
                    City = new ProtoBufClass.Common.Item { Id = cityId },
                    Platform = new ProtoBufClass.Common.Item { Id = (int)PlatformSource.CarwaleDesktop },
                    ApplicationId = (int)Application.CarWale,
                    NoZoneFilter = true,
                    DealerLocator = true
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
                Logger.LogException(ex);
            }
            return null;
        }
    }
}
