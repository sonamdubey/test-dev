using Campaigns.DealerCampaignClient;
using Carwale.Entity.CarData;
using Carwale.Entity.CrossSell;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CrossSell;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications;
using Carwale.Utility;
using ProtoBufClass.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.CrossSell
{
    public class HouseCrossSell : CrossSellOperations,IHouseCrossSell
    {
        private readonly IPrices _prices;
        private readonly ICarVersionCacheRepository _versionCachedRepo;
        private readonly ICampaign _campaign;
        public HouseCrossSell(IPrices prices, ICarVersionCacheRepository versionCachedRepo, ICampaign campaign)
        {
            _prices = prices;
            _versionCachedRepo = versionCachedRepo;
            _campaign = campaign;
        }

        public List<CrossSellDetail> GetHouseCrossSellList(int versionId, Location locationObj, int platformId)
        {
            try
            {
                var crossSells = CreateHouseCrossSellListModel(versionId, locationObj, platformId);
                return PrioritizeCrossSellList(crossSells);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetHouseCrossSellList");
                objErr.LogException();
                return null;
            }
        }

        public CrossSellDetail GetHouseCrossSell(int versionId, Location locationObj, int platformId)
        {
            try
            {
                var crossSells = CreateHouseCrossSellListModel(versionId, locationObj, platformId);
                crossSells = PrioritizeAndFilterCrossSellList(crossSells);
                return GetRandomCrossSell(crossSells);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetHouseCrossSell");
                objErr.LogException();
                return null;
            }
        }

        private List<CrossSellDetail> CreateHouseCrossSellListModel(int versionId, Location locationObj, int platformId)
        {
            try
            {
                var versionPrice = _prices.FilterPrices(locationObj.CityId, versionId);

                if (versionPrice == null || versionPrice.PriceQuoteList.Count <= 0)
                {
                    return null;
                }

                var versions = FetchHouseCrossSell(versionId, locationObj);
                if (versions == null)
                {
                    return null;
                }
                
                List<int> versionIds = new List<int>();

                foreach (var version in versions.Versions)
                {
                    versionIds.Add(version.Id);
                }

                if (versionIds == null || versionIds.Count == 0)
                {
                    return null;
                }

                List<CrossSellDetail> crossSellDetails = new List<CrossSellDetail>();
                CarVersionDetails currentVersionDetails;
                foreach (var currentVersion in versionIds)
                {
                    currentVersionDetails = _versionCachedRepo.GetVersionDetailsById(currentVersion);
                    var currentVersionPrice = _prices.FilterPrices(locationObj.CityId, currentVersion);

                    if (currentVersionDetails == null || currentVersionPrice == null || currentVersionPrice.PriceQuoteList.Count <= 0)
                    {
                        continue;
                    }

                    var campaigns = _campaign.GetAllCampaign(currentVersionDetails.ModelId, locationObj, platformId);

                    if (campaigns != null && campaigns.Count > 0)
                    {
                        campaigns = campaigns.Where(x => x.IsFeaturedEnabled).ToList(); //Get only those campaigns which are enabled for house cross sell
                    }

                    if (campaigns != null && campaigns.Count > 0)
                    {
                        for (int campaignIndex = 0; campaignIndex < campaigns.Count; campaignIndex++)
                        {
                            if ((platformId == (int)Platform.CarwaleAndroid || platformId == (int)Platform.CarwaleiOS) && String.IsNullOrWhiteSpace(campaigns[campaignIndex].ContactNumber))
                            {
                                campaigns[campaignIndex].ContactNumber = CWConfiguration.tollFreeNumber;
                            }
                            crossSellDetails.Add(new CrossSellDetail
                            {
                                CampaignDetail = campaigns[campaignIndex],
                                CarVersionDetail = currentVersionDetails
                            });
                        }
                    }
                }
                return crossSellDetails;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GetHouseCrossSellAllCampaigns()");
                objErr.LogException();
                return null;
            }
        }

        private FeaturedVersionsList FetchHouseCrossSell(int versionId, Location locationObj)
        {
            try
            {
                var versionsList = DealerCampaignClient.GetHouseCrossSellVersions(new CrossSellCampaignInput
                {
                    CityId = locationObj.CityId,
                    ZoneId = locationObj.ZoneId,
                    TargetVersionId = versionId
                });

                if (versionsList != null && versionsList.Versions.Count > 0)
                {
                    return versionsList;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "FetchHouseCrossSell");
                objErr.LogException();
            }
            return null;
        }
    }
}
