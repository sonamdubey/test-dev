using Carwale.DTOs.Monetization;
using Carwale.Interfaces.Deals;
using Carwale.Interfaces.Monetization;
using System;
using AutoMapper;
using Carwale.DTOs.Deals;
using Carwale.Entity.Dealers;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Enum;
using Carwale.DTOs.CarData;
using Carwale.BL.Advertizing;
using Carwale.Entity;
using Carwale.Interfaces.Campaigns;
using Carwale.Utility;
using Carwale.Interfaces.Customer;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Campaigns;
using Carwale.DTOs.Campaigns;
using Carwale.Notifications.Logs;

namespace Carwale.BL.Monetization
{
    public class MonetizationBL : IMonetization
    {
        private readonly IDeals _deals;
        private readonly ICampaign _campaignBL;
        private readonly SponsoredCampaignApp _sponsoredCampaignApp;
        private readonly ICustomerTracking _trackingBL;
        private readonly IDealerAdProvider _dealerAdProvider;

        public MonetizationBL(IDeals deals, ICampaign campaignBL, SponsoredCampaignApp sponsoredCampaignApp, ICustomerTracking trackingBL, IDealerAdProvider dealerAdProvider)
        {
            _deals = deals;
            _campaignBL = campaignBL;
            _sponsoredCampaignApp = sponsoredCampaignApp;
            _trackingBL = trackingBL;
            _dealerAdProvider = dealerAdProvider;
        }

        public MonetizationModelDTO ModelAddUnits(int modelId, int cityId, string zoneId, int platform, string screenType)
        {
            var monetizationModelDto = new MonetizationModelDTO();
            try
            {
                DiscountSummaryDTO_AndroidV1 advantageAd = AdvantageAd(modelId, cityId);
                if (advantageAd != null)
                {
                    monetizationModelDto.AdvantageAdUnit = new AdvantageMonetizationDTO
                    {
                        AdvantageDiscountSummary = AdvantageAd(modelId, cityId),
                        Priority = 2
                    };
                }

                SponsoredDealer pqDealerAdd = _campaignBL.GetSponsorDealerAd(modelId, platform, new Entity.Geolocation.Location { CityId = cityId, ZoneId = CustomParser.parseIntObject(zoneId) });
                if (pqDealerAdd.DealerId > 0 && !string.IsNullOrWhiteSpace(pqDealerAdd.DealerName))
                {
                    monetizationModelDto.PQAdUnit = new SponsoredDealerMonetizationDTO
                    {
                        PQDealerAd = Mapper.Map<SponsoredDealer, SponsoredDealerDTO>(pqDealerAdd),
                        Priority = 3
                    };
                }

                OnlyAppAdsDTO screenNativeAd = ScreenNativeAds(modelId, platform, screenType);
                if (screenNativeAd != null && screenNativeAd.NativeAds != null)
                {
                    monetizationModelDto.SponsoredAdUnit = new AppDTOV1
                    {
                        SponsoredAds = screenNativeAd,
                        Priority = 1
                    };
                }

                if (platform == (int)Platform.CarwaleAndroid || platform == (int)Platform.CarwaleiOS)
                {
                    var carDataTrackingEntity = new CarDataTrackingEntity
                    {
                        ModelId = modelId,
                        Platform = platform,
                        VersionId = 0,
                        Category = "ModelPage",
                        Action = "ModelImpression"
                    };
                    carDataTrackingEntity.Location.CityId = cityId;

                    _trackingBL.AppsTrackModelVersionImpression(carDataTrackingEntity, pqDealerAdd);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, "MonetizationBL.ModelAddUnits");
            }
            return monetizationModelDto;
        }
        public MonetizationModelDTOV1 ModelAddUnitsV1(int modelId, Location location, int platform, string screenType, int campaignId)
        {
            var monetizationModelDto = new MonetizationModelDTOV1();
            try
            {
                var advantageAd = AdvantageAd(modelId, location.CityId);
                if (advantageAd != null)
                {
                    monetizationModelDto.AdvantageAdUnit = new AdvantageMonetizationDTO
                    {
                        AdvantageDiscountSummary = AdvantageAd(modelId, location.CityId),
                        Priority = 2
                    };
                }

                DealerAd pqDealerAdd = _dealerAdProvider.GetDealerAd(new CarIdEntity { ModelId = modelId }, location, platform, (int)Entity.Enum.CampaignAdType.Pq, campaignId, 0);
                var dealerCampaign = Mapper.Map<DealerAd, DealerAdDTO>(pqDealerAdd);
                if (dealerCampaign != null && dealerCampaign.Campaign != null && dealerCampaign.Campaign.Id > 0)
                {
                    monetizationModelDto.dealerAdUnit = new DealerAdMonetizationDTO
                    {
                        dealerAd = dealerCampaign,
                        Priority = 3
                    };
                }

                OnlyAppAdsDTO screenNativeAd = ScreenNativeAds(modelId, platform, screenType);
                if (screenNativeAd != null && screenNativeAd.NativeAds != null)
                {
                    monetizationModelDto.SponsoredAdUnit = new AppDTOV1
                    {
                        SponsoredAds = screenNativeAd,
                        Priority = 1
                    };
                }

                if (platform == (int)Platform.CarwaleAndroid || platform == (int)Platform.CarwaleiOS)
                {
                    var carDataTrackingEntity = new CarDataTrackingEntity
                    {
                        ModelId = modelId,
                        Platform = platform,
                        VersionId = 0,
                        Category = "ModelPage",
                        Action = "ModelImpression"
                    };
                    carDataTrackingEntity.Location.CityId = location.CityId;
                    if (pqDealerAdd != null)
                    {
                        _trackingBL.AppsTrackModelVersionImpressionV1(carDataTrackingEntity, pqDealerAdd.Campaign);
                    }
                    else
                    {
                        _trackingBL.AppsTrackModelVersionImpressionV1(carDataTrackingEntity, null);
                    }
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, "MonetizationBL.ModelAddUnitsV1");
            }
            return monetizationModelDto;
        }

        private DiscountSummaryDTO_AndroidV1 AdvantageAd(int modelId, int cityId)
        {
            try
            {
                var dealSummary = _deals.GetDealsSummaryByModelandCity_Android(modelId, cityId);
                if (dealSummary != null)
                {
                    return Mapper.Map<DiscountSummaryDTO_Android, DiscountSummaryDTO_AndroidV1>(dealSummary);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, "MonetizationBL.AdvantageAd");
            }
            return null;
        }

        private OnlyAppAdsDTO ScreenNativeAds(int modelId, int platform, string screenType)
        {
            try
            {
                MobilePlatformScreenType screen;

                if (Enum.TryParse<MobilePlatformScreenType>(screenType.ToLower(), out screen))
                {
                    var sponsoredCampaignResponse = _sponsoredCampaignApp.Response(platform, (int)screen, modelId.ToString()) as OnlyAppAdsDTO;

                    if (sponsoredCampaignResponse != null && sponsoredCampaignResponse.NativeAds != null)
                    {
                        return sponsoredCampaignResponse;
                    }
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err, "MonetizationBL.ScreenNativeAds");
            }
            return null;
        }
    }
}
