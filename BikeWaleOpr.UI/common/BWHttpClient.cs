using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace BikeWaleOpr.Common
{
    public class BWHttpClient
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 7 Oct 2014
        /// Summary    : Method to get data from web api
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
                HttpContext.Current.Trace.Warn("GetApiResponse", ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("GetApiResponse", err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return objTask;
        }


        public static async Task<T> PostAsync<T>(string hostUrl, string requestType, string apiUrl, T postObject)
        {
            T objTask = postObject;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl); ;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));
                // HTTP POST

                HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, postObject);
                HttpContext.Current.Trace.Warn("post async :" + response.StatusCode.ToString());
                if (response.IsSuccessStatusCode)
                {

                    return objTask;
                    //// Get the URI of the created resource.
                    //Uri gizmoUrl = response.Headers.Location;

                }
            }
            return objTask;


        }


        /// <summary>
        /// Written By : Ashwini Todkar on 8 Nov 2014
        /// Method to call delete api synchronously
        /// </summary>
        /// <param name="hostUrl"></param>
        /// <param name="requestType"></param>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public static bool DeleteSync(string hostUrl, string requestType, string apiUrl)
        {
            bool isSuccess = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl);
                HttpResponseMessage response = client.DeleteAsync(apiUrl).Result;

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
        public static bool PostSync<T>(string hostUrl, string requestType, string apiUrl, T objToPost)
        {
            bool isSuccess = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                HttpResponseMessage response = client.PostAsJsonAsync(apiUrl, objToPost).Result;

                if (response.IsSuccessStatusCode)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        //Created By : Sadhana Upadhyay on 9 Nov 2014
        //Summary : To call get api synchronously
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
                            objTask = JsonConvert.DeserializeObject<T>(_response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                HttpContext.Current.Trace.Warn("GetApiResponse", ex.Message);
                //ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                //
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("GetApiResponse", err.Message);
                //ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                //
            }

            return objTask;
        }

    }
}