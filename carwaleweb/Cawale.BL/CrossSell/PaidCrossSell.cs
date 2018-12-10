using AutoMapper;
using Campaigns.DealerCampaignClient;
using Carwale.Entity.CarData;
using Carwale.Entity.CrossSell;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CrossSell;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using ProtoBufClass.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.CrossSell
{
    public class PaidCrossSell : CrossSellOperations, IPaidCrossSell
    {
        private readonly IPrices _prices;
        private readonly ICarVersionCacheRepository _versionCachedRepo;
        private readonly ICampaign _campaign;
        public PaidCrossSell(IPrices prices, ICarVersionCacheRepository versionCachedRepo, ICampaign campaign)
        {
            _prices = prices;
            _versionCachedRepo = versionCachedRepo;
            _campaign = campaign;
        }

        public List<CrossSellDetail> GetPaidCrossSellList(int versionId, Location locationObj)
        {
            try
            {
                var crossSells = CreatePaidCrossSellListModel(versionId, locationObj);
                return PrioritizeCrossSellList(crossSells);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetPaidCrossSellList");
                objErr.LogException();
                return null;
            }
        }

        public CrossSellDetail GetPaidCrossSell(int versionId, Location locationObj)
        {
            try
            {
                var crossSells = CreatePaidCrossSellListModel(versionId, locationObj);
                crossSells = PrioritizeAndFilterCrossSellList(crossSells);
                return GetRandomCrossSell(crossSells);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetPaidCrossSell");
                objErr.LogException();
                return null;
            }
        }

        private List<CrossSellDetail> CreatePaidCrossSellListModel(int versionId, Location locationObj)
        {
            try
            {
                List<CrossSellDetail> crossSellDetails = new List<CrossSellDetail>();

                var versionPrice = _prices.FilterPrices(locationObj.CityId, versionId);

                if (versionPrice == null || versionPrice.PriceQuoteList.Count <= 0)
                {
                    return null;
                }

                IEnumerable<FeaturedVersion> featuredCampaigns = FetchPaidCrossSell(versionId, locationObj);
                if (featuredCampaigns == null || !featuredCampaigns.Any())
                {
                    return null;
                }

                CarVersionDetails currentVersionDetails;
                foreach (var campaign in featuredCampaigns)
                {
                    currentVersionDetails = _versionCachedRepo.GetVersionDetailsById(campaign.VersionId);
                    var currentVersionPrice = _prices.FilterPrices(locationObj.CityId, campaign.VersionId);

                    if (currentVersionDetails == null || currentVersionPrice == null || currentVersionPrice.PriceQuoteList.Count <= 0)
                    {
                        continue;
                    }

                    var campaignDetail = _campaign.GetCampaignDetails(campaign.CampaignId);

                    if (campaignDetail.DealerId > 0)
                    {
                        crossSellDetails.Add(new CrossSellDetail
                        {
                            CampaignDetail = campaignDetail,
                            CarVersionDetail = currentVersionDetails
                        });
                    }
                }
                return crossSellDetails;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CreatePaidCrossSellListModel");
                objErr.LogException();
                return null;
            }
        }

        private IEnumerable<FeaturedVersion> FetchPaidCrossSell(int versionId, Location locationObj)
        {
            try
            {
                var paidCrossSellCampaigns = new List<FeaturedVersion>();

                var campaigns = DealerCampaignClient.GetPaidCrossSellCampaign(new CrossSellCampaignInput
                {
                    CityId = locationObj.CityId,
                    ZoneId = locationObj.ZoneId,
                    TargetVersionId = versionId,
                    ApplicationId = 1
                });

                if (campaigns != null)
                {
                    foreach (var campaign in campaigns.CrossSellCampaign)
                    {
                        paidCrossSellCampaigns.Add(Mapper.Map<ProtoBufClass.Campaigns.CrossSellCampaign, FeaturedVersion>(campaign));
                    }
                    return paidCrossSellCampaigns;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "FetchPaidCrossSellCampaigns");
                objErr.LogException();
            }
            return null;
        }
    }
}
