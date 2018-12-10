using AutoMapper;
using Carwale.Entity.Campaigns;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using log4net;
using Microsoft.Practices.Unity;
using Predictive;
using System;
using System.Configuration;
using System.Web;
namespace Carwale.BL.Campaigns
{
    public class CampaignPredictionBL : ICampaignPrediction
    {
        public readonly ICarModelCacheRepository _modelsCache;
        private readonly IGeoCitiesCacheRepository _cityCache;

        private string _predictionModelName = ConfigurationManager.AppSettings["PredictionModelName"];

        public CampaignPredictionBL(IUnityContainer container, ICarModelCacheRepository modelsCache, IGeoCitiesCacheRepository cityCache)
        {
            _modelsCache = modelsCache;
            _cityCache = cityCache;
        }

        public PredictionCampaignRequest GetPredictionModelRequest(HttpRequest httpRequest, int modelId, int cityId, int zoneId, int platformId)
        {
            //TODO:Bangalore Zone Refactoring
            zoneId = ((cityId == 1 || cityId == 2 || cityId == 10) ? zoneId : 0);

            var cookies = httpRequest.Cookies;

            int globalCityId = 0;
            int globalZoneId = 0;

            if (cookies["_CustCityIdMaster"] != null)
            {
                int.TryParse(cookies["_CustCityIdMaster"].Value, out globalCityId);
            }

            if (cookies["_CustZoneIdMaster"] != null)
            {
                int.TryParse(cookies["_CustZoneIdMaster"].Value, out globalZoneId);
            }

            string globalCity = (globalCityId > 0 && cookies["_CustCityMaster"] != null && cookies["_CustCityMaster"].Value != null) ? cookies["_CustCityMaster"].Value.ToString() : "";
            string globalZone = (globalZoneId > 0 && cookies["_CustZoneMaster"] != null && cookies["_CustZoneMaster"].Value != null) ? cookies["_CustZoneMaster"].Value.ToString() : "";
            string referrerUrl = httpRequest.ServerVariables["HTTP_REFERER"];
            var cityDetails = _cityCache.GetCustLocation(cityId, zoneId.ToString());
            bool isUserLoggedIn = (cookies[".ASPXAUTH"] != null);
            string source = (cookies["_cwutmz"] != null && cookies["_cwutmz"].Value != null) ? cookies["_cwutmz"].Value.ToString() : "";

            PredictionCampaignRequest predictionCampaignRequest = new PredictionCampaignRequest()
            {
                Name = _predictionModelName,
                CookieId = UserTracker.GetSessionCookie(),
                PageUrl = httpRequest.RawUrl,
                Platform = (Enum.GetName(typeof(Carwale.Entity.Enum.Platform), platformId)),
                ModelId = modelId,
                ReferrerUrl = referrerUrl != null ? referrerUrl : "",
                Global = new PredictionModelRequestLocation() { CityName = globalCity, ZoneName = globalZone },
                User = new PredictionModelRequestLocation() { CityName = cityDetails.CityName, ZoneName = cityDetails.ZoneName },
                IsUserLoggedIn = isUserLoggedIn,
                Source = source
            };
            return predictionCampaignRequest;
        }

        public PredictionModelResponse GetPredictionModelResponse(HttpRequest httpRequest, int modelId, int cityId, int zoneId, int platformId)
        {
            var predictionData = new PredictionModelResponse();
            try
            {
                if (httpRequest.Cookies == null)
                {
                    Carwale.Notifications.Logs.Logger.LogInfo("Cookie is null!!");
                }
                else if (!string.IsNullOrEmpty(UserTracker.GetSessionCookie()))
                {
                    var predictionClient = new PredictiveScore.PredictiveScoreClient(true);
                    var campaignRequest = Mapper.Map<CampaignRequest>(GetPredictionModelRequest(httpRequest, modelId, cityId, zoneId, platformId));
                    predictionData = Mapper.Map<PredictionModelResponse>(predictionClient.GetCampaignScore(campaignRequest));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, String.Format("GetPredictionModelResponse parameters {0}-{1}-{2}-{3}", modelId, cityId, zoneId, platformId));
            }
            return predictionData;
        }
    }
}
