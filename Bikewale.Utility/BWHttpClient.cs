using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class BWHttpClient
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 11 Nov 2014
        /// Summary : Method to get data from web api synchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hostUrl"></param>
        /// <param name="requestType"></param>
        /// <param name="apiUrl"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public static T GetApiResponseSync<T>(string hostUrl, string requestType, string apiUrl, T responseType)
        {
            T objTask = responseType;
            using (var client = new HttpClient())
            {
                // New code:       
                //sets the base URI for HTTP requests
                client.BaseAddress = new Uri(hostUrl);
                client.DefaultRequestHeaders.Accept.Clear();

                //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                //HTTP GET
                HttpResponseMessage _response = client.GetAsync(apiUrl).Result;

                _response.EnsureSuccessStatusCode(); //Throw if not a success code.                    

                if (_response.IsSuccessStatusCode)
                {
                    if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status        
                        objTask = _response.Content.ReadAsAsync<T>().Result;
                }
            }
            

            return objTask;
        }
    }
}
