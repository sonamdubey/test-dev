using System;
using System.IO;
using System.Linq;
using System.Web;

namespace Carwale.Utility
{
    public static class HttpContextUtils
    {
        public static string GetRequestContent()
        {
            string content = null;
            var reqStream = HttpContext.Current.Request.InputStream;
            if (reqStream != null && reqStream.CanSeek)
            {
                reqStream.Position = 0;
                using (var reader = new StreamReader(reqStream))
                {
                    content = reader.ReadToEnd();
                }
            }
            return content;
        }

        public static string GetCookie(string cookieName)
        {
            return HttpContext.Current.Request?.Cookies[cookieName]?.Value;
        }

        public static T GetHeader<T>(string headerName)
        {
            string value = HttpContext.Current.Request?.Headers.GetValues(headerName)?.FirstOrDefault();
            if (!string.IsNullOrEmpty(value))
            {
                return value.ToType<T>();
            }
            return default(T);
        }

        public static void AddAmpHeaders(string ampOrigin, bool isCredentials)
        {
            HttpContext.Current.Response.AppendHeader("Access-Control-Expose-Headers", "AMP-Access-Control-Allow-Source-Origin");
            HttpContext.Current.Response.AppendHeader("AMP-Access-Control-Allow-Source-Origin", ampOrigin);
            if (isCredentials)
            {
                HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
            }
        }


        /// <summary>
        /// Get base url from the request i.e from http://carwale.com/used/sell/ => baseurl=http://carwale.com/
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Uri GetBaseUrl(HttpRequest request)
        {
            string baseUrl = string.Empty;
            if (request != null)
            {
                baseUrl = string.Format("{0}://{1}{2}/", request.IsLocal ? request.Url.Scheme : "https", request.Url.Authority, request.ApplicationPath.TrimEnd('/'));
            }
            return new Uri(baseUrl);
        }
    }
}
