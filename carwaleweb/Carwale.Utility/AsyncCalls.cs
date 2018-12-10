using System;
using System.Net.Http;
using System.Web;

namespace Carwale.Utility
{
    public static class AsyncCalls
    {
        static HttpClientHandler handler = new HttpClientHandler { UseCookies = false };
        static HttpClient client = new HttpClient(handler);

        public static void CallToAsync(string requestUri)
        {
            CallToAsync(requestUri, "");
        }

        public static void CallToAsync(string requestUri, string cookies)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Absolute),
                Method = HttpMethod.Get,
            };

            try
            {
                request.Headers.TryAddWithoutValidation("Client-IP", UserTracker.GetUserIp());
            }
            catch (Exception)
            {
                //This operation could fail for multiple reasons. And it is ok to skip it, if it fails
            }

            try
            {
                request.Headers.TryAddWithoutValidation("User-Agent", HttpContext.Current.Request.UserAgent);
            }
            catch (Exception)
            {
                //This operation could fail for multiple reasons. And it is ok to skip it, if it fails
            }

            if (!string.IsNullOrEmpty(cookies))
            {
                request.Headers.Add("Cookie", cookies);
            }

            client.SendAsync(request);
        }
    }
}