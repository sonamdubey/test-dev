using AutoMapper;
using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.OffersV1;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.OffersV1;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Offers;
using Carwale.Interfaces.Validations;
using Carwale.Notifications.Logs;
using Carwale.UI.Common;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Carwale.Interfaces.NewCars;

namespace Carwale.UI.Controllers.NewCars.OfferAndDealerAd
{
    public class OfferAndDealerAdController : Controller
    {
        private readonly IOffersAdapter _offersAdapter;
        private readonly ICampaign _campaign;
        private readonly IDealerAdProvider _dealerAdProviderBl;
        private readonly INewCarDealers _newCarDealersBl;
        private readonly IGeoCitiesCacheRepository _geoCitiesCache;
        private readonly IValidateMmv _validateMmv;
        private readonly IValidateLocation _validateLocation;
        private readonly ICarVersionCacheRepository _carVersionsCacheRepo;

        public OfferAndDealerAdController(IOffersAdapter offersAdapter, ICampaign campaign, IDealerAdProvider dealerAdProviderBl,
            INewCarDealers newCarDealersBl, IGeoCitiesCacheRepository geoCitiesCache, IValidateMmv validateMmv,
            IValidateLocation validateLocation, ICarVersionCacheRepository carVersionsCacheRepo)
        {
            _offersAdapter = offersAdapter;
            _campaign = campaign;
            _dealerAdProviderBl = dealerAdProviderBl;
            _newCarDealersBl = newCarDealersBl;
            _geoCitiesCache = geoCitiesCache;
            _validateMmv = validateMmv;
            _validateLocation = validateLocation;
            _carVersionsCacheRepo = carVersionsCacheRepo;
        }

        public ActionResult Index(int? inputVersionId, int? inputPageId, int? inputCampaignId, int? inputCityId)
        {
            try
            {
                var versionId = CustomParser.parseIntObject(inputVersionId);
                var pageId = CustomParser.parseIntObject(inputPageId);
                var campaignId = CustomParser.parseIntObject(inputCampaignId);
                var cityId = CustomParser.parseIntObject(inputCityId);

                if (!_validateMmv.IsModelVersionValid(versionId) || pageId <= 0)
                {
                    return new EmptyResult();
                }

                var offerAndDealerAd = new OfferAndDealerAdDTO();
                Cities cityDetails = new Cities { CityId = -1, StateId = -1 };
                int areaId = 0;

                if (cityId > 0)
                {
                    //location details
                    cityDetails = _geoCitiesCache.GetCityDetailsById(cityId);
                    areaId = (cityDetails != null && cityId == CookiesCustomers.MasterCityId && cityDetails.IsAreaAvailable) ? CookiesCustomers.MasterAreaId : 0;
                }

                SetLocation(offerAndDealerAd, cityDetails);

                //car details
                var versionDetails = _carVersionsCacheRepo.GetVersionDetailsById(versionId);
                var modelId = versionDetails != null ? versionDetails.ModelId : 0;
                var makeId = versionDetails != null ? versionDetails.MakeId : 0;

                //offer
                var offerInput = new OfferInput
                {
                    ApplicationId = (int)Application.CarWale,
                    CityId = cityId,
                    StateId = cityDetails.StateId,
                    MakeId = makeId,
                    ModelId = modelId,
                    VersionId = versionId
                };

                offerAndDealerAd.Offer = _offersAdapter.GetOffers(offerInput);

                 //campaign
                    var campaignInput = new CampaignInputv2
                    {
                        ModelId = modelId,
                        MakeId = makeId,
                        PlatformId = (int)Platform.CarwaleMobile,
                        PageId = pageId,
                        ApplicationId = (int)Application.CarWale,
                        CityId = cityId,
                        AreaId = areaId,
                        CampaignId = campaignId
                    };

                    GetDealerAd(campaignInput, versionId, offerAndDealerAd);
                
                bool isCampaignAvailable = IsCampaignAvailable(offerAndDealerAd.DealerAd, campaignId);
                bool isOfferAvailable = IsOfferAvailable(offerAndDealerAd.Offer);
                
                if (isCampaignAvailable)
                {
                    SetLeadSource(offerAndDealerAd, pageId);
                }

                if (isCampaignAvailable || isOfferAvailable)
                {
                    SetCarDetails(offerAndDealerAd, versionDetails);

                    offerAndDealerAd.Page = pageId == (int)CwPages.PicMsite ? Pages.PIC : Pages.ModelPage;
                    offerAndDealerAd.Platform = "Msite";
                    return PartialView("~/Views/Shared/m/OfferAndDealerAd/_OfferAndDealerAd.cshtml", offerAndDealerAd);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return new EmptyResult();
        }

        private bool IsOfferAvailable(OfferDto offerDTO)
        {
            return offerDTO != null && offerDTO.OfferDetails != null && offerDTO.CategoryDetails != null && offerDTO.CategoryDetails.Count > 0;
        }

        private bool IsCampaignAvailable(DealerAdDTO dealerAdDTO, int campaignId)
        {
            return dealerAdDTO != null && dealerAdDTO.Campaign != null && dealerAdDTO.Campaign.Id == campaignId;
        }

        private void SetLocation(OfferAndDealerAdDTO offerAndDealerAd, Cities cityDetails)
        {
            offerAndDealerAd.Location = Mapper.Map<CityAreaDTO>(cityDetails);
        }

        private void SetCarDetails(OfferAndDealerAdDTO offerAndDealerAd, CarVersionDetails versionDetails)
        {
            offerAndDealerAd.CarDetails = Mapper.Map<CarDetailsDTO>(versionDetails);
        }

        private bool ShowCampaignSlug(int modelId, int cityId)
        {
            if (modelId.Equals(CWConfiguration.RenaultLeadFormModelId) && cityId > 0)
            {
                return false;
            }
            return true;
        }

        private void GetDealerAd(CampaignInputv2 input, int versionId, OfferAndDealerAdDTO offerAndDealerAd)
        {
            Location locationObj = Mapper.Map<CampaignInputv2, Location>(input);
            CarIdEntity carEntity = Mapper.Map<CampaignInputv2, CarIdEntity>(input);

            DealerAd dealerAd = _dealerAdProviderBl.GetDealerAd(carEntity, locationObj, input.PlatformId, (int)CampaignAdType.Pq,
                                    input.CampaignId, input.PageId);

            if (dealerAd != null && dealerAd.Campaign != null)
            {
                offerAndDealerAd.DealerAd = Mapper.Map<DealerAdDTO>(dealerAd);

                var dealerDetails = _newCarDealersBl.NCDDetails(dealerAd.Campaign.DealerId, dealerAd.Campaign.Id,
                    input.MakeId, input.CityId);

                if (dealerDetails.CampaignId > 0)
                {
                    offerAndDealerAd.DealerAd.Campaign.DealerShowroom = AutoMapper.Mapper.Map<Entity.Dealers.DealerDetails, DealersDTO>(dealerDetails);
                }
            }
        }

        private void SetLeadSource(OfferAndDealerAdDTO offerAndDealerAd, int pageId)
        {
            var leadSources = new List<LeadSource>();
            short leadSource = 0;

            switch (pageId)
            {
                case (int)CwPages.ModelMsite: 
                    leadSource = (short)LeadSources.Model;
                    break;
                
                case (int)CwPages.PicMsite:
                    leadSource = (short)LeadSources.PriceInCity;
                    break;
            }

            leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithOffer", leadSource, (short)Platform.CarwaleMobile));
            leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithOfferReco", leadSource, (short)Platform.CarwaleMobile));
            leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithoutOffer", leadSource, (short)Platform.CarwaleMobile));
            leadSources.Add(Carwale.BL.Campaigns.LeadSource.GetLeadSource("OfferCampaignSlugWithoutOfferReco", leadSource, (short)Platform.CarwaleMobile));

            offerAndDealerAd.LeadSource.AddRange(Mapper.Map<List<LeadSourceDTO>>(leadSources));
        }
    }
}
