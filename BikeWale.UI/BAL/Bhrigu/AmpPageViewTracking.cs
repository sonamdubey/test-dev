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
    /// Created By : Prabhu Puredla on 06 nov 2018
    /// </summary>
    public static class AmpPageViewTracking
    {
        static IApiGatewayCaller _apiGatewayCaller;
        static readonly IUnityContainer _container;

        static AmpPageViewTracking()
        {
            _container = new UnityContainer();
            _container.RegisterType<IApiGatewayCaller, ApiGatewayCaller>();
            _apiGatewayCaller = _container.Resolve<IApiGatewayCaller>();
        }
        /// <summary>
        /// Created By : Prabhu Puredla on 06 nov 2018
        /// Description : Function to call api gateway
        /// </summary>
        /// <param name="nvc"></param>
        private static void CallBhriguTracker(NameValueCollection nvc)
        {
            BhriguTrackEntity objTrackingData = FormatTrackingData(nvc);
            AddTrackEventAdapter adapter = new AddTrackEventAdapter();
            adapter.AddApiGatewayCall(_apiGatewayCaller, objTrackingData);
            _apiGatewayCaller.Call();
        }

        /// <summary>
        /// Description : Function to push data to bhrigu through adapter
        /// </summary>
        /// <param name="nvc"></param>
        public static void PushDataToBhrigu(NameValueCollection nvc)
        {
            try
            {
                if (nvc != null && !String.IsNullOrEmpty(nvc["cookieId"]))
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
        /// <param name="nvc"></param>
        /// <returns>BhriguTrackEntity</returns>
        private static BhriguTrackEntity FormatTrackingData(NameValueCollection nvc)
        {
            BhriguTrackEntity objTrackingData = new BhriguTrackEntity();

            objTrackingData.Category = nvc["category"];
            objTrackingData.Action = nvc["action"];
            objTrackingData.Label = nvc["label"];
            objTrackingData.CookieId = nvc["cookieId"];
            objTrackingData.SessionId = nvc["sessionId"];
            objTrackingData.PageUrl = nvc["pageUrl"];
            objTrackingData.QueryString = nvc["queryString"];
            objTrackingData.Cookie = String.Format("BWC={0}; _cwv={1} _bwtest={2}; location={3};_bwutmz={4};", 
                nvc["cookieId"], nvc["sessionId"], nvc["_bwtest"], nvc["location"], nvc["_bwutmz"]);                 
            objTrackingData.ClientIP = nvc["clientIP"];
            objTrackingData.UserAgent = nvc["userAgent"];
            objTrackingData.Referrer = nvc["referrer"];

            return objTrackingData;
        }
    }
}
