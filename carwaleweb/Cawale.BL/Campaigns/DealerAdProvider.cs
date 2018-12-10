using AutoMapper;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CrossSell;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PageProperty;
using Carwale.Entity.CarData;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CrossSell;
using Carwale.Interfaces.Dealers;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.BL.Campaigns
{
    public class DealerAdProvider : IDealerAdProvider
    {
        private readonly ICampaign _campaign;
        private readonly IPaidCrossSell _paidCrossSell;
        private readonly IHouseCrossSell _houseCrossSell;
        private readonly ITemplate _campaignTemplate;
        private readonly IDealers _dealer;
        private static readonly IEnumerable<int> _testDriveCampaignIds = ConfigurationManager.AppSettings["TestDriveCampaignIds"]?.Split(',').Select(s => int.Parse(s));
        public DealerAdProvider(ICampaign campaign, IPaidCrossSell paidCrossSell, IHouseCrossSell houseCrossSell, ITemplate campaignTemplate, IDealers dealer)
        {
            _campaign = campaign;
            _paidCrossSell = paidCrossSell;
            _houseCrossSell = houseCrossSell;
            _campaignTemplate = campaignTemplate;
            _dealer = dealer;
        }
        public DealerAd GetDealerAd(CarIdEntity carEntity, Location locationObj, int platformId)
        {
            try
            {
                DealerAd dealerAd = GetPQCampaign(carEntity, locationObj, platformId);

                if (dealerAd == null)
                {
                    dealerAd = GetPaidCrossSell(carEntity, locationObj, platformId, 1).FirstOrDefault();

                    if (dealerAd == null)
                    {
                        dealerAd = GetHouseCrossSell(carEntity, locationObj, platformId, 1).FirstOrDefault();
                    }
                }

                return dealerAd;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public DealerAd GetDealerAd(CarIdEntity carEntity, Location locationObj, int platformId, int adType, int campaignId, int PageId)
        {
            try
            {
                DealerAd dealerAd = null;
                switch (adType)
                {
                    case (int)Entity.Enum.CampaignAdType.Pq:
                        {
                            dealerAd = GetPQCampaign(carEntity, locationObj, platformId, campaignId, PageId);
                            break;
                        }
                    case (int)Entity.Enum.CampaignAdType.PaidCrossSell:
                        {
                            dealerAd = GetPaidCrossSell(carEntity, locationObj, platformId, 1).FirstOrDefault();
                            break;
                        }
                    case (int)Entity.Enum.CampaignAdType.HouseCrossSell:
                        {
                            dealerAd = GetHouseCrossSell(carEntity, locationObj, platformId, 1).FirstOrDefault();
                            break;
                        }
                    default:
                        {
                            dealerAd = GetCampaign(carEntity, locationObj, platformId, campaignId, PageId, 1).FirstOrDefault();
                            break;
                        }
                }
                if (dealerAd != null)
                {
                    dealerAd.Campaign.IsTestDriveCampaign = IsTestDriveCampaign(dealerAd.Campaign.Id);
                }
                return dealerAd;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public List<DealerAd> GetDealerAdList(CarIdEntity carEntity, Location locationObj, int platformId, int adType, int campaignId, int PageId, int count = 0)
        {
            try
            {
                List<DealerAd> dealerAd = new List<DealerAd>();
                switch(adType)
                {
                    case (int)Entity.Enum.CampaignAdType.Pq:
                        {
                        DealerAd campaignDetail = GetPQCampaign(carEntity, locationObj, platformId, campaignId, PageId);
                        if (campaignDetail != null)
                            dealerAd.Add(campaignDetail);
                        break ;
                    }
                    case (int)Entity.Enum.CampaignAdType.PaidCrossSell:
                        {
                            dealerAd = GetPaidCrossSell(carEntity, locationObj, platformId, count);
                            break;
                        }
                    case (int)Entity.Enum.CampaignAdType.HouseCrossSell:
                        {
                            dealerAd = GetHouseCrossSell(carEntity, locationObj, platformId, count);
                            break;
                        }
                    default:
                        { 
                            dealerAd = GetCampaign(carEntity, locationObj, platformId, campaignId, PageId, count);
                            break;
                        }
                }
                    
                    dealerAd.Each(x => x.Campaign.IsTestDriveCampaign = IsTestDriveCampaign(x.Campaign.Id));
                return dealerAd;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }
        private List<DealerAd> GetCampaign(CarIdEntity carEntity, Location locationObj, int platformId, int campaignId, int PageId, int count = 0)
        {
            List<DealerAd> dealerAd = new List<DealerAd>();
            DealerAd campaignDetail = GetPQCampaign(carEntity, locationObj, platformId, campaignId, PageId);

            if (campaignDetail != null)
                dealerAd.Add(campaignDetail);

            if (dealerAd.Count < 1)
            {
                dealerAd = GetPaidCrossSell(carEntity, locationObj, platformId, count);

                if (dealerAd.Count < 1)
                {
                    dealerAd = GetHouseCrossSell(carEntity, locationObj, platformId, count);
                }
            }
            return dealerAd;
        }

        private DealerAd GetPQCampaign(CarIdEntity carEntity, Location locationObj, int platformId, int campaignId = 0, int pageId = 0)
        {
            DealerAd dealerAd = new DealerAd();
            if (campaignId <= 0 && (platformId == (int)Platform.CarwaleDesktop || platformId == (int)Platform.CarwaleMobile))
            {
                campaignId = _campaign.GetPersistedCampaign(carEntity.ModelId, locationObj);
            }

            Campaign campaignDetail = _campaign.GetDealerCampaign(carEntity.ModelId, locationObj, platformId, campaignId);

            if (campaignDetail == null || campaignDetail.DealerId < 1)
            {
                return null;
            }
            else
            {
                campaignDetail.IsTurboMla = IsTurboMla(locationObj.CityId, carEntity.MakeId);
                
                dealerAd.Campaign = campaignDetail;
                dealerAd.DealerDetails = _dealer.GetDealerDetailsOnDealerId(campaignDetail.DealerId);
                dealerAd.FeaturedCarData = Mapper.Map<CarVersionDetails>(carEntity);
                dealerAd.PageProperty = _campaignTemplate.GetPageProperties(platformId, pageId, campaignDetail);
                dealerAd.CampaignType = Entity.Enum.CampaignAdType.Pq;
            }

            if (platformId == (int)Platform.CarwaleDesktop || platformId == (int)Platform.CarwaleMobile)
            {
                _campaign.SetPersistedCampaign(carEntity.ModelId, locationObj, dealerAd.Campaign.Id);
            }
            return dealerAd;
        }

        private List<DealerAd> GetPaidCrossSell(CarIdEntity carEntity, Location locationObj, int platformId, int count)
        {
            List<DealerAd> dealerAd = new List<DealerAd>();
            List<CrossSellDetail> paidCrossSellCampaign = _paidCrossSell.GetPaidCrossSellList(carEntity.VersionId, locationObj);
            if (paidCrossSellCampaign != null)
            {
                for (int i = 0; i < paidCrossSellCampaign.Count && (count == 0 || i < count); i++)
                {
                    if (paidCrossSellCampaign[i].CampaignDetail != null && paidCrossSellCampaign[i].CampaignDetail.DealerId > 0)
                    {
                        if ((platformId == (int)Platform.CarwaleAndroid || platformId == (int)Platform.CarwaleiOS) && String.IsNullOrWhiteSpace(paidCrossSellCampaign[i].CampaignDetail.ContactNumber))
                        {
                            paidCrossSellCampaign[i].CampaignDetail.ContactNumber = CWConfiguration.tollFreeNumber;
                        }
                        dealerAd.Add(MapCampaignDetails(paidCrossSellCampaign[i], Entity.Enum.CampaignAdType.PaidCrossSell));
                    }
                }
                if (count == 1 && dealerAd.Count == 1)
                {
                    dealerAd[0].PageProperty = new List<PageProperty>();
                    dealerAd[0].PageProperty.Add(new PageProperty { Id = 1, Template = _campaignTemplate.GetCampaignTemplates(dealerAd[0].Campaign, SponsoredCarPageId.PaidCrossSell.GetHashCode().ToString(), platformId) });
                }
            }
            return dealerAd;
        }

        private List<DealerAd> GetHouseCrossSell(CarIdEntity carEntity, Location locationObj, int platformId, int count)
        {
            List<DealerAd> dealerAd = new List<DealerAd>();
            List<CrossSellDetail> houseCrossSellCampaign = _houseCrossSell.GetHouseCrossSellList(carEntity.VersionId, locationObj, platformId);
            if (houseCrossSellCampaign != null)
            {
                for (int i = 0; i < houseCrossSellCampaign.Count && (count == 0 || i < count); i++)
                {
                    if (houseCrossSellCampaign[i].CampaignDetail != null && houseCrossSellCampaign[i].CampaignDetail.DealerId > 0)
                    {
                        dealerAd.Add(MapCampaignDetails(houseCrossSellCampaign[i], Entity.Enum.CampaignAdType.HouseCrossSell));
                    }
                }
                if (count == 1 && dealerAd.Count == 1)
                {
                    dealerAd[0].PageProperty = new List<PageProperty>();
                    dealerAd[0].PageProperty.Add(new PageProperty { Id = 1, Template = _campaignTemplate.GetCampaignTemplates(dealerAd[0].Campaign, SponsoredCarPageId.HouseCrossSell.GetHashCode().ToString(), platformId) });
                }
            }
            return dealerAd;
        }

        private DealerAd MapCampaignDetails(CrossSellDetail crossSellCampaign, CampaignAdType type)
        {
            DealerAd campaignDetail = new DealerAd();
            campaignDetail.Campaign = crossSellCampaign.CampaignDetail;
            campaignDetail.DealerDetails = _dealer.GetDealerDetailsOnDealerId(crossSellCampaign.CampaignDetail.DealerId);
            campaignDetail.FeaturedCarData = Mapper.Map<CarVersionDetails>(crossSellCampaign.CarVersionDetail);
            campaignDetail.CampaignType = type;
            return campaignDetail;
        }

        private bool IsTestDriveCampaign(int campaignId)
        {
            if(_testDriveCampaignIds != null)
            {
                return _testDriveCampaignIds.Contains(campaignId);
            }
            return false;
           
        }

        private bool IsTurboMla(int cityId, int makeId)
        {
            return Array.IndexOf(CWConfiguration.TurboMLACityIds, cityId) > -1 
                && Array.IndexOf(CWConfiguration.TurboMLAMakeIds,makeId) > -1 
                && CustomerCookie.AbTest >= CWConfiguration.TurboMLAMinABTests;
        }
    }
}