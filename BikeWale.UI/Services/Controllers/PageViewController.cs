using Bikewale.BAL.Bhrigu;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Bikewale.Services.Controllers
{
    public class PageViewController : CompressionApiController//ApiController
    {
        private static HashSet<string> _ampCors = new HashSet<string> { "https://www-bikewale-com.cdn.ampproject.org", "https://www-bikewale-com.amp.cloudflare.com", "https://cdn.ampproject.org", "https://www.bikewale.com" };
        
        [HttpGet, Route("api/trackamppageview/"), EnableCors("https://www-bikewale-com.cdn.ampproject.org, https://www-bikewale-com.amp.cloudflare.com, https://cdn.ampproject.org,https://www.bikewale.com", "*", "GET")]
        public IHttpActionResult TrackAmpPageView()
        {
            string ampSourceOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
            string ampOrigin = Request.Headers.Contains("Origin") ? Request.Headers.GetValues("Origin").FirstOrDefault() : string.Empty;


            //if (!string.IsNullOrEmpty(ampSourceOrigin) && _ampCors.Contains(ampOrigin))
            //{
            BWCookies.AddAmpHeaders(ampSourceOrigin, ampOrigin, true);
            //}
            NameValueCollection objNVC = new NameValueCollection();
            var cookies = HttpContext.Current.Response.Cookies;

            objNVC.Add("category", "BWPageViews");
            objNVC.Add("action", string.Empty);
            objNVC.Add("label", "IsAmp=true");
            objNVC.Add("cookieId", cookies["BWC"] != null ? cookies["BWC"].Value : string.Empty);
            objNVC.Add("sessionId", cookies["_cwv"] != null ? cookies["_cwv"].Value : string.Empty);
            objNVC.Add("pageUrl", HttpContext.Current.Request.Url.AbsoluteUri);
            objNVC.Add("queryString", !String.IsNullOrEmpty(ampSourceOrigin) && ampSourceOrigin.Contains("?") ? ampSourceOrigin.Split('?')[1].Replace('&', '|') : String.Empty);
            objNVC.Add("clientIP", CurrentUser.GetClientIP());
            objNVC.Add("userAgent", HttpContext.Current.Request.UserAgent);
            objNVC.Add("referrer", HttpContext.Current.Request.UrlReferrer.ToString());
            objNVC.Add("_bwtest", cookies["_bwtest"] != null ? cookies["_bwtest"].Value : string.Empty);
            objNVC.Add("location", HttpContext.Current.Request.Cookies["location"] != null ? HttpContext.Current.Request.Cookies["location"].Value : string.Empty);
            objNVC.Add("_bwutmz", cookies["_bwutmz"] != null ? cookies["_bwutmz"].Value : string.Empty);

            AmpPageViewTracking.PushDataToBhrigu(objNVC);
            return Ok();
        }
    }
}