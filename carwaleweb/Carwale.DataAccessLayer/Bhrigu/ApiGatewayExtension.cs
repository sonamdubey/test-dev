using AEPLCore.Logging;
using Bhrigu;
using Carwale.DAL.ApiGateway;
using Carwale.Utility;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BhriguServiceCaller;

namespace Carwale.DAL.Bhrigu
{
    public static class ApiGatewayExtension
    {
        private static Logger _logger = LoggerFactory.GetLogger();
        private static readonly string _bhriguModule = ConfigurationManager.AppSettings["BhriguTrackerModule"] ?? string.Empty;

        public static void AddTrackEvent(this IApiGatewayCaller caller, string category, string action, string label)
        {
            try
            {
                if (caller != null)
                {
                    var request = HttpContext.Current.Request;
                    var cid = request.Headers["IMEI"];
                    
                    TrackingRequest message = new TrackingRequest
                    {
                        Category = category,
                        Action = action,
                        Label = label,
                        Pageurl = request.Url != null ? String.Format("{0}{1}{2}{3}", request.Url.Scheme, Uri.SchemeDelimiter, request.Url.Host, request.Url.AbsolutePath) : string.Empty,
                        Querystring = request.QueryString != null ? request.QueryString.ToString() : "",
                        Clientip = UserTracker.GetUserIp(),
                        Useragent = HttpContextUtils.GetHeader<string>("User-Agent") ?? ""
                    };
                    if (!string.IsNullOrEmpty(cid))
                    {
                        message.Cookieid = cid;
                        message.Sessionid = "NA"; // harcoded to NA, as no session is maintained, we can cahange it later 
                    }
                    else
                    {
                        message.Cookie = HttpContextUtils.GetHeader<string>("Cookie") ?? "";
                        message.Referrer = request.UrlReferrer != null ? request.UrlReferrer.ToString() : "";
                    }
                    caller.Add(_bhriguModule, "TrackEvent", message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }

        public static void GetUserProfile(this IApiGatewayCaller caller, Application application, string cwcCookieId)
        {
            if (!string.IsNullOrWhiteSpace(cwcCookieId) && caller != null)
            {
                try
                {
                        UserProfileRequestBuilder userProfileRequestBuilder = new UserProfileRequestBuilder(application, cwcCookieId);
                        userProfileRequestBuilder.AddValuePreferenceKeyQuery(CWUserProfilePreferenceKeys.CARPREFERENCE);
                        userProfileRequestBuilder.AddValueSubKeyQuery(CWUserProfileKeys.COUNTS, CWUserProfileSubKeys.LEADS);
                        userProfileRequestBuilder.AddTopKKeysQuery(CWUserProfileKeys.MODELS, 5);
                        userProfileRequestBuilder.AddTopKKeysQuery(CWUserProfileKeys.BODYSTYLE, 1);
                        userProfileRequestBuilder.AddTopKKeysQuery(CWUserProfileKeys.BUDGETSEGMENT, 2);
                        userProfileRequestBuilder.AddNumberSubKeysQuery(CWUserProfileKeys.MODELS);
                        caller.Add(_bhriguModule, "GetUserProfile", userProfileRequestBuilder.GetUserProfileRequest());
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex);
                }
            }
        }
        public static void GetModelScore(this IApiGatewayCaller caller, string cwcCookieId)
        {
            if (!string.IsNullOrWhiteSpace(cwcCookieId) && caller != null)
            {
                try
                {
                    ScoreRequest message = new ScoreRequest
                    {
                        Cookieid = cwcCookieId
                    };
                    caller.Add(_bhriguModule, "GetModelScore", message);
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex);
                }
            }
        }
    }
}
