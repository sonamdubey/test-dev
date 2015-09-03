using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Bikewale.Common;


namespace Bikewale.Common
{
    public class BWHttpClient
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 7 Oct 2014
        /// Summary    : PopulateWhere to get data from web api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hostUrl"></param>
        /// <param name="requestType">Json,text,xml or application</param>
        /// <param name="apiUrl"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public static async Task<T> GetApiResponse<T>(string hostUrl, string requestType, string apiUrl, T responseType)
        {
            T objTask = responseType;
            try
            {            
                using (var client = new HttpClient())
                {
                    // New code:       
                    //sets the base URI for HTTP requests
                    client.BaseAddress = new Uri(hostUrl);
                    client.DefaultRequestHeaders.Accept.Clear();

                    //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                    //HTTP GET
                    HttpResponseMessage _response = await client.GetAsync(apiUrl);

                    _response.EnsureSuccessStatusCode(); //Throw if not a success code.

                    if (_response.IsSuccessStatusCode)
                    {
                        if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status
                        objTask = await _response.Content.ReadAsAsync<T>();                
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objTask;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 11 Nov 2014
        /// Summary : PopulateWhere to get data from web api synchronously
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
            try
            {
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
            }
            catch (HttpRequestException ex)
            {
                HttpContext.Current.Trace.Warn("GetApiResponse", ex.Message);
                //ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                //objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("GetApiResponse", err.Message);
                //ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                //objErr.SendMail();
            }

            return objTask;
        }

        public static async Task<bool> PostAsync<T>(string hostUrl, string requestType, string apiUrl, T postObject)
        {
            bool isSuccess = false;

            using (var client = new HttpClient())
            {
                // TODO - Send HTTP requests
                client.BaseAddress = new Uri(hostUrl); ;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));
                // HTTP POST

                HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, "xyz");

                if (response.IsSuccessStatusCode)
                {
                    //// Get the URI of the created resource.
                    //Uri gizmoUrl = response.Headers.Location;
                    isSuccess = true;
                }
            }
            return isSuccess;
           
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 8 Nov 2014
        /// PopulateWhere to call delete api synchronously
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="requestType"></param>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public static bool DeleteSync(string hostUrl, string requestType, string apiUrl)
        {
            //Task<bool> t = default(Task<bool>);
            bool isSuccess = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl);

                var response = client.DeleteAsync(apiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                }                   
            }

            return isSuccess;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 8 Nov 2014
        /// </summary>
        /// <typeparam name="T">Generic object to Post</typeparam>
        /// <param name="hostUrl"></param>
        /// <param name="requestType"></param>
        /// <param name="apiUrl"></param>
        /// <param name="objResponse"></param>
        /// <returns></returns>
        public static bool PostSync<T>(string hostUrl, string requestType, string apiUrl, T objResponse)
        {
            bool isSuccess = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                var response = client.PostAsJsonAsync(apiUrl, objResponse).Result;

                if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }
    }
}