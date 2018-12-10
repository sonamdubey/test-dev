using AutoMapper;
using Carwale.BL.Tracking;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Price;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Customer;
using Carwale.Interfaces.PriceQuote;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Carwale.BL.Customers
{
    public class CustomerTracking : ICustomerTracking
    {
        private readonly ICarPriceQuoteAdapter _prices;
        private readonly IPriceQuoteBL _priceQuoteBL;
        private readonly BhriguTracker _bhrigutrack;

        public CustomerTracking(ICarPriceQuoteAdapter prices, IPriceQuoteBL priceQuoteBL, BhriguTracker bhrigutrack)
        {
            _prices = prices;
            _priceQuoteBL = priceQuoteBL;
            _bhrigutrack = bhrigutrack;
        }

        public void AppsTrackModelVersionImpression(CarDataTrackingEntity carDataTrackingEntity, SponsoredDealer pqDealerAdd)
        {
            try
            {
                string cat = carDataTrackingEntity.Category;
                string act = carDataTrackingEntity.Action;
                string label = TrackingLabel(carDataTrackingEntity, pqDealerAdd);

                _bhrigutrack.Track(cat, act, label);
            }
            catch (Exception err)
            {
                Logger.LogException(err, string.Join(string.Empty, "CustomerTracking.CallToBriguTrackingAPIForModelVersion() : Sponsored Campaign ad unit platformId: ",
                                carDataTrackingEntity.Platform, " city: ", carDataTrackingEntity.Location.CityId, " modelId: ", carDataTrackingEntity.ModelId));
            }
        }

        public void AppsTrackModelVersionImpressionV1(CarDataTrackingEntity carDataTrackingEntity, Campaign campaign)
        {
            try
            {
                string cat = carDataTrackingEntity.Category;
                string act = carDataTrackingEntity.Action;
                string label = TrackingLabel(carDataTrackingEntity, campaign);

                _bhrigutrack.Track(cat, act, label);
            }
            catch (Exception err)
            {
                Logger.LogException(err, string.Join(string.Empty, "CustomerTracking.CallToBriguTrackingAPIForModelVersion() : Sponsored Campaign ad unit platformId: ",
                                carDataTrackingEntity.Platform, " city: ", carDataTrackingEntity.Location.CityId, " modelId: ", carDataTrackingEntity.ModelId));
            }
        }

        private string TrackingLabel(CarDataTrackingEntity carDataTrackingEntity, SponsoredDealer pqDealerAdd)
        {
            string label = "";

            var price = _prices.GetAvailablePriceForModel(carDataTrackingEntity.ModelId, carDataTrackingEntity.Location.CityId);

            if (carDataTrackingEntity.ModelId > 0)
            {
                label = label + "modelid=" + carDataTrackingEntity.ModelId;
            }

            if (carDataTrackingEntity.VersionId > 0)
            {
                label = label + "|versionid=" + carDataTrackingEntity.VersionId;
            }

            label = label + GetLocationLabel(carDataTrackingEntity.Location);

            if (carDataTrackingEntity.Platform > 0)
            {
                label = label + "|source=" + carDataTrackingEntity.Platform;
            }

            if (pqDealerAdd != null && pqDealerAdd.DealerId > 0)
            {
                label = label + "|dealerid=" + pqDealerAdd.ActualDealerId + "|campaignid=" + pqDealerAdd.DealerId + "|iscampaignshown=1" + "|campaignpanel="
                        + pqDealerAdd.LeadPanel + "|campaigntype=1";
            }

            if (price != null && price.PriceLabel == ConfigurationManager.AppSettings["CityPriceText"])
            {
                label = label + "|isorpshown=1";
            }
            return label;
        }

        private string TrackingLabel(CarDataTrackingEntity carDataTrackingEntity, Campaign pqDealerAdd)
        {
            string label = "";

            var price = _prices.GetAvailablePriceForModel(carDataTrackingEntity.ModelId, carDataTrackingEntity.Location.CityId);

            if (carDataTrackingEntity.ModelId > 0)
            {
                label = label + "modelid=" + carDataTrackingEntity.ModelId;
            }

            if (carDataTrackingEntity.VersionId > 0)
            {
                label = label + "|versionid=" + carDataTrackingEntity.VersionId;
            }

            label = label + GetLocationLabel(carDataTrackingEntity.Location);

            if (carDataTrackingEntity.Platform > 0)
            {
                label = label + "|source=" + carDataTrackingEntity.Platform;
            }

            if (pqDealerAdd != null && pqDealerAdd.Id > 0)
            {
                label = label + "|dealerid=" + pqDealerAdd.DealerId + "|campaignid=" + pqDealerAdd.Id + "|iscampaignshown=1" + "|campaignpanel="
                        + pqDealerAdd.LeadPanel + "|campaigntype=1";
            }

            if (price != null && price.PriceLabel == ConfigurationManager.AppSettings["CityPriceText"])
            {
                label = label + "|isorpshown=1";
            }
            return label;
        }

        public void TrackPqImpression(CarDataTrackingEntity carDataTrackingEntity, SponsoredDealer pqDealerAdd, List<PQItem> priceQuoteList)
        {
            if (carDataTrackingEntity == null)
            {
                return;
            }

            try
            {
                GetOnRoadPrice(ref carDataTrackingEntity, priceQuoteList);

                string cat = "QuotationPage";
                string act = "PQImpression";
                string label = GetPqImpressionLabel(carDataTrackingEntity, pqDealerAdd, priceQuoteList);

                _bhrigutrack.Track(cat, act, label);
            }
            catch (Exception err)
            {
                Logger.LogException(err, "CustomerTracking.TrackPqImpression() - platformId: " + carDataTrackingEntity.Platform
                                    + " city: " + carDataTrackingEntity.Location.CityId + " modelId: " + carDataTrackingEntity.ModelId);
            }
        }

        private void GetOnRoadPrice(ref CarDataTrackingEntity carDataTrackingEntity, List<PQItem> priceQuoteList)
        {
            carDataTrackingEntity.OnRoadPrice = _priceQuoteBL.CalculateOnRoadPrice(priceQuoteList);
        }

        private static string GetPqImpressionLabel(CarDataTrackingEntity carDataTrackingEntity, SponsoredDealer pqDealerAdd, List<PQItem> priceQuoteList)
        {
            string label = "modelid=" + carDataTrackingEntity.ModelId + "|source=" + carDataTrackingEntity.Platform + "|pqsource=" + carDataTrackingEntity.PageId;

            if (carDataTrackingEntity.VersionId > 0)
            {
                label = label + "|versionid=" + carDataTrackingEntity.VersionId;
            }

            label = label + GetLocationLabel(carDataTrackingEntity.Location);

            if (pqDealerAdd != null && pqDealerAdd.DealerId > 0)
            {
                label = label + "|dealerid=" + pqDealerAdd.ActualDealerId + "|campaignid=" + pqDealerAdd.DealerId + "|iscampaignshown=1" + "|campaignpanel="
                        + pqDealerAdd.LeadPanel + "|campaigntype=" + carDataTrackingEntity.CampaignType;
            }

            label = label + GetPriceLabel(carDataTrackingEntity.OnRoadPrice);

            return label;
        }


        public void TrackPqImpression(CarDataTrackingEntity carDataTrackingEntity, SponsoredDealerDTO pqDealerAdd, List<PQItemDTO> priceQuoteList)
        {
            try
            {
                TrackPqImpression(carDataTrackingEntity, Mapper.Map<SponsoredDealer>(pqDealerAdd), Mapper.Map<List<PQItem>>(priceQuoteList));
            }
            catch (Exception err)
            {
                Logger.LogException(err, "CustomerTracking.TrackPqImpression() - platformId: " + carDataTrackingEntity.Platform
                                     + " city: " + carDataTrackingEntity.Location.CityId + " modelId: " + carDataTrackingEntity.ModelId);
            }
        }

        public void TrackPriceQuoteImpression(CarDataTrackingEntity carData, DealerAd dealerAd)
        {
            if (carData == null)
            {
                return;
            }

            try
            {
                string cat = "QuotationPage";
                string act = "PQImpression";
                string label = GetPqImpressionLabel(carData, dealerAd);

                _bhrigutrack.Track(cat, act, label);
            }
            catch (Exception err)
            {
                Logger.LogException(err, "CustomerTracking.AppsTrackPqImpression() : Sponsored Campaign ad unit platformId: "
                                    + carData.Platform + " city: " + carData.Location.CityId + " modelId: " + carData.ModelId);
            }
        }

        private static string GetPqImpressionLabel(CarDataTrackingEntity carDataTrackingEntity, DealerAd dealerAd)
        {
            string label = string.Format("modelid={0}|source={1}|pqsource={2}", carDataTrackingEntity.ModelId,
                           carDataTrackingEntity.Platform, carDataTrackingEntity.PageId);

            label = string.Format("{0}{1}", label, GetLocationLabel(carDataTrackingEntity.Location));

            if (carDataTrackingEntity.VersionId > 0)
            {
                label = string.Format("{0}|versionid={1}", label, carDataTrackingEntity.VersionId);
            }

            if (dealerAd != null && dealerAd.Campaign.Id > 0)
            {
                label = string.Format("{0}|dealerid={1}|campaignid={2}|iscampaignshown=1|campaignpanel={3}|campaigntype={4}", label,
                    dealerAd.DealerDetails.DealerId, dealerAd.Campaign.Id, dealerAd.Campaign.LeadPanel, carDataTrackingEntity.CampaignType);
            }

            label = label + GetPriceLabel(carDataTrackingEntity.OnRoadPrice);

            return label;
        }

        private static string GetCookies(CarDataTrackingEntity carData)
        {
            string cookies = "";

            if (carData.Platform == (int)Platform.CarwaleMobile || carData.Platform == (int)Platform.CarwaleDesktop)
            {
                string cwvCookie = HttpContextUtils.GetCookie("_cwv");
                if (string.IsNullOrEmpty(cwvCookie))
                {
                    cwvCookie = ConfigurationManager.AppSettings["DummycwvCookie"];
                }

                cookies = string.Format("_abtest={0};_cwv={1};__utma={2};__utmz={3};_cwutmz={4}", HttpContextUtils.GetCookie("_abtest"),
                    cwvCookie, HttpContextUtils.GetCookie("__utma"), HttpContextUtils.GetCookie("__utmz"),
                    HttpContextUtils.GetCookie("_cwutmz"));
            }

            return cookies;
        }

        private static string GetPriceLabel(long onRoadPrice)
        {
            if (onRoadPrice > 0)
            {
                return "|isorpshown=1|orp=" + onRoadPrice;
            }

            return "";
        }

        private static string GetLocationLabel(CustLocation custLocation)
        {
            string label = "";

            if (custLocation != null)
            {
                int cityId = custLocation.CityId;
                int zoneId = CustomParser.parseIntObject(custLocation.ZoneId);
                int areaId = custLocation.AreaId;

                string cityLabel = cityId > 0 ? "|cityid=" + cityId : "";
                string zoneLabel = zoneId > 0 ? "|zoneid=" + zoneId : "";
                string areaLabel = areaId > 0 ? "|areaid=" + areaId : "";

                label = string.Format("{0}{1}{2}", cityLabel, zoneLabel, areaLabel);
            }

            return label;
        }

        public void TrackNewsTags(List<VehicleTag> vehicleTagsList)
        {
            string lbl;
            string allMake = "";
            string allVersion = "";
            string allModel = "";
            foreach (var item in vehicleTagsList)
            {
            allMake = item.MakeBase.MakeId > 0 ? string.Format("{0}{1},", allMake, item.MakeBase.MakeId) : "";
            allModel = item.ModelBase.ModelId > 0 ? string.Format("{0}{1},", allModel, item.ModelBase.ModelId) : "";
            allVersion = item.VersionBase.ID > 0 ? string.Format("{0}{1},", allVersion, item.VersionBase.ID) : "";
            }
            int makeLen = allMake.Length;
            int modelLen = allModel.Length;
            int versionLen = allVersion.Length;

            allMake = makeLen > 0 ? string.Format("makeid={0}", allMake.Remove(makeLen - 1)) : "";
            allModel = modelLen > 0 ? string.Format("modelid={0}", allModel.Remove(modelLen - 1)) : "";
            allVersion = versionLen > 0 ? string.Format("versionid={0}", allVersion.Remove(versionLen - 1)) : "";
            //bind all make , model and version
            lbl = (makeLen > 0 ? allMake + "|" : "");
            lbl = string.Format("{0}{1}", lbl, modelLen > 0 ? allModel + "|" : "");
            lbl = string.Format("{0}{1}", lbl, versionLen > 0 ? allVersion + "|" : "");
            //remove last pipe
            if (lbl.Length > 0)
            {
                lbl = lbl.Remove(lbl.Length - 1);
                _bhrigutrack.Track("NewsDetails", "NewsRead", lbl);
            }
        }
    }
}
