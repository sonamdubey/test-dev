using AEPLCore.Cache;
using Carwale.Cache.CarData;
using Carwale.Cache.Geolocation;
using Carwale.DAL.CarData;
using Carwale.DAL.Geolocation;
using Carwale.Entity.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications;
using Carwale.Utility;
using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Carwale.BL.PresentationLogic
{
    public class NewCar
    {
        protected static ICarModelCacheRepository _modelsCache;
        protected static IGeoCitiesCacheRepository _cityCache;
        private static string _predictionModelName = ConfigurationManager.AppSettings["LeadQualityPredictionModelName"];
        private static string _showLink = ConfigurationManager.AppSettings["CitiesForSellCarOnPQ"];

        static NewCar()
        {
            _modelsCache = new CarModelsCacheRepository(new CarModelsRepository(), new CacheManager());
            _cityCache = new GeoCitiesCacheRepository(new GeoCitiesRepository(), new CacheManager());
        }

        public static PredictionModelRequest GetPredictionModelRequest(HttpRequest httpRequest, int modelId, int cityId, int zoneId, int platformId, int campaignId)
        {
            PredictionModelRequest predictionModelRequest = new PredictionModelRequest();
            try
            {
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

                string globalCity = (globalCityId > 0 && cookies["_CustCityMaster"] != null) ? cookies["_CustCityMaster"].Value.ToString() : "";
                string globalZone = (globalZoneId > 0 && cookies["_CustZoneMaster"] != null) ? cookies["_CustZoneMaster"].Value.ToString() : "";
                string referrerUrl = httpRequest.ServerVariables["HTTP_REFERER"];
                var modelDetails = _modelsCache.GetModelDetailsById(modelId);
                var cityDetails = _cityCache.GetCustLocation(cityId, zoneId.ToString());
                bool isUserLoggedIn = (cookies[".ASPXAUTH"] != null);

                predictionModelRequest = new PredictionModelRequest()
                {
                    Name = _predictionModelName,
                    CookieId = UserTracker.GetSessionCookie(),
                    PageUrl = httpRequest.RawUrl,
                    Platform = (Enum.GetName(typeof(Carwale.Entity.Enum.Platform), platformId)),
                    CarModelName = modelDetails.ModelName,
                    ReferrerUrl = referrerUrl != null ? referrerUrl : "",
                    Global = new PredictionModelRequestLocation() { CityName = globalCity, ZoneName = globalZone },
                    User = new PredictionModelRequestLocation() { CityName = cityDetails.CityName, ZoneName = cityDetails.ZoneName },
                    IsUserLoggedIn = isUserLoggedIn,
                    CampaignId = campaignId
                };
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CampaignBL.PresentationLogic.NewCar GetPredictionModelRequest ");
                objErr.LogException();
            }
            return predictionModelRequest;
        }

        public static bool ShowSellCarLinkOnPq(int cityId = 0)
        {
            string currentRequestCity = cityId <= 0 && HttpContext.Current.Request.Cookies["_CustCityIdMaster"] != null ?
                HttpContext.Current.Request.Cookies["_CustCityIdMaster"].Value :
                cityId.ToString();
            if (_showLink.Split(',').Contains(currentRequestCity))
            {
               return true;
            }
            return false;
        }
    }
}