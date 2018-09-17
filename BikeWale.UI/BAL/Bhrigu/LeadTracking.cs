using System;
using System.Collections.Specialized;
using Bikewale.Entities.Tracking;
using Bikewale.BAL.ApiGateway.Adapters.Bhrigu;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Microsoft.Practices.Unity;
using Bikewale.Notifications;
using System.Web.Hosting;

namespace Bikewale.BAL.Bhrigu
{
    /// <summary>
    /// Created By : Sanjay George on 17 August 2018
    /// </summary>
    public class LeadTracking
    {
        static IApiGatewayCaller _apiGatewayCaller;
        static readonly IUnityContainer _container;

        static LeadTracking()
        {
            _container = new UnityContainer();
            _container.RegisterType<IApiGatewayCaller, ApiGatewayCaller>();
            _apiGatewayCaller = _container.Resolve<IApiGatewayCaller>();
        }

        private static void CallBhriguTracker(NameValueCollection nvc)
        {
            BhriguTrackEntity objTrackingData = FormatTrackingData(nvc);
            AddTrackEventAdapter adapter = new AddTrackEventAdapter();
            adapter.AddApiGatewayCall(_apiGatewayCaller, objTrackingData);
            _apiGatewayCaller.Call();
        }

        /// <summary>
        /// Description : Function to push data to bhrigu through adapter
        /// Modified By : Rajan Chauhan on 29 August 2018
        /// Description : Added Call to background task
        /// </summary>
        /// <param name="nvc"></param>
        public static void PushDataToBhrigu(NameValueCollection nvc)
        {
            try
            {
                if (validateMessage(nvc))
                {
                    HostingEnvironment.QueueBackgroundWorkItem(f => CallBhriguTracker(nvc));
                }         
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.Bhrigu.LeadTracking PushDataToBhrigu");
            }
        }

        /// <summary>
        /// Description : Function to convert NameValueCollection to BhriguTrackEntity
        /// </summary>
        /// <param name="objTrack"></param>
        /// <returns>BhriguTrackEntity</returns>
        static BhriguTrackEntity FormatTrackingData(NameValueCollection nvc)
        {
            BhriguTrackEntity objTrackingData = new BhriguTrackEntity();

            objTrackingData.Category = nvc["category"];
            objTrackingData.Action = nvc["action"];
            objTrackingData.Label = String.Format("leadId={0}|leadSourceId={1}|platformId={2}|versionId={3}|dealerId={4}|appversion={5}|pageId={6}|campaignId={7}", 
                String.IsNullOrEmpty(nvc["leadId"]) ? "0" : nvc["leadId"],
                String.IsNullOrEmpty(nvc["leadSourceId"]) ? "0" : nvc["leadSourceId"], 
                String.IsNullOrEmpty(nvc["platformId"]) ? "0" : nvc["platformId"], 
                String.IsNullOrEmpty(nvc["versionId"]) ? "0" : nvc["versionId"],
                String.IsNullOrEmpty(nvc["dealerId"]) ? "0": nvc["dealerId"],
                String.IsNullOrEmpty(nvc["appVersion"]) ? "0" : nvc["appVersion"],
                String.IsNullOrEmpty(nvc["pageId"]) ? "0" : nvc["pageId"],
                String.IsNullOrEmpty(nvc["campaignId"]) ? "0" : nvc["campaignId"]);           
            objTrackingData.CookieId = nvc["cookieId"];
            objTrackingData.SessionId = nvc["sessionId"];
            objTrackingData.PageUrl = nvc["pageUrl"];
            objTrackingData.QueryString = nvc["queryString"];
            objTrackingData.Cookie = String.Format("name={0}; mobile={1}; email={2}; bwtest={3}; bwutmz={4}; cwv={5}; locationCity={6}; locationArea={7}",
                nvc["name"], nvc["mobile"], nvc["email"], nvc["bwtest"], nvc["bwutmz"], nvc["cwv"], nvc["locationCity"], nvc["locationArea"]);                 
            objTrackingData.ClientIP = nvc["clientIP"];
            objTrackingData.UserAgent = nvc["userAgent"];
            objTrackingData.Referrer = nvc["referrer"];

            return objTrackingData;
        }

        /// <summary>
        /// Description : Function to validate NVC
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        static bool validateMessage(NameValueCollection nvc)
        {
            try
            {
                return nvc != null && !String.IsNullOrEmpty(nvc["cookieId"]);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeWale.BhriguTracking validateMessage ");
            }
            return false;
        }
    }
}
