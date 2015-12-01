using Newtonsoft.Json;
using RabbitMqPublishing.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            using (var client = new HttpClient())
            {
                    // New code:       
                    //sets the base URI for HTTP requests
                    client.BaseAddress = new Uri(hostUrl);
                    client.DefaultRequestHeaders.Accept.Clear();

                    //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                    //HTTP GET
                    using (HttpResponseMessage _response = await client.GetAsync(apiUrl))
                    {
                      //_response.EnsureSuccessStatusCode(); //Throw if not a success code.

                      if (_response.IsSuccessStatusCode)
                      {
                        if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status
                          objTask = await _response.Content.ReadAsAsync<T>();
                      }
                    }
            }           

            return objTask;
        }
        
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
                    using (HttpResponseMessage _response = client.GetAsync(apiUrl).Result)
                    {
                      //_response.EnsureSuccessStatusCode(); //Throw if not a success code.                    
                      if (_response.IsSuccessStatusCode)
                      {
                        if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status        
                        {
                          objTask = _response.Content.ReadAsAsync<T>().Result;
                          _response.Content.Dispose();
                            _response.Content=null;
                        }
                      }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponseSync<T>");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponseSync<T>");
                objErr.SendMail();
            }

            return objTask;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 18 Nov 2015
        /// Summary : Method to get data from web api synchronously and add Parameter in request Header.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hostUrl"></param>
        /// <param name="requestType"></param>
        /// <param name="apiUrl"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public static T GetApiResponseSync<T>(string hostUrl, string requestType, string apiUrl, T responseType, Dictionary<string,string> headerParameters)
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
                    
                    //Add parameter
                    if (headerParameters != null && headerParameters.Count() > 0)
                    {
                        foreach (var param in headerParameters)
                        {
                            client.DefaultRequestHeaders.Add(param.Key, param.Value);
                        }
                    }
                    
                    //HTTP GET
                    using (HttpResponseMessage _response = client.GetAsync(apiUrl).Result)
                    {
                        //_response.EnsureSuccessStatusCode(); //Throw if not a success code.                    
                        if (_response.IsSuccessStatusCode)
                        {
                            if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status        
                            {
                                objTask = _response.Content.ReadAsAsync<T>().Result;
                                _response.Content.Dispose();
                                _response.Content = null;
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponseSync<T>");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponseSync<T>");
                objErr.SendMail();
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

                using (HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, "xyz"))
                {
                  if (response.IsSuccessStatusCode)
                  {
                    //// Get the URI of the created resource.
                    //Uri gizmoUrl = response.Headers.Location;
                    isSuccess = true;
                  }
                }
            }
            return isSuccess;

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
            //Task<bool> t = default(Task<bool>);
            bool isSuccess = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl);

                using (var response = client.DeleteAsync(apiUrl).Result)
                {
                  if (response.IsSuccessStatusCode)
                  {
                    isSuccess = true;
                  }
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

                using (var response = client.PostAsJsonAsync(apiUrl, objResponse).Result)
                {
                  if (response.IsSuccessStatusCode)
                  {
                    isSuccess = true;
                  }
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
        public static T PostDataSync<T>(string hostUrl, string requestType, string apiUrl, T objResponse)
        {
            T objTask = objResponse;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                using (var response = client.PostAsJsonAsync(apiUrl, objResponse).Result)
                {
                  if (response.IsSuccessStatusCode)
                  {
                    objTask = response.Content.ReadAsAsync<T>().Result;
                  }
                }
            }
            return objTask;
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
        public static U PostSync<T, U>(string hostUrl, string requestType, string apiUrl, T objToPost)
        {            
            U objResponse = default(U);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                using (var response = client.PostAsJsonAsync(apiUrl, objToPost).Result)
                {
                  if (response.IsSuccessStatusCode)
                  {
                    objResponse = response.Content.ReadAsAsync<U>().Result;
                  }
                }
            }
            return objResponse;
        }

        /// <summary>
        /// Created By : Lucky Rathore
        /// Description : Implementiaon of HTTP Post Method with adding Headed parameter in HTTP request Header.
        /// </summary>
        /// <typeparam name="T">Data to Send.</typeparam>
        /// <typeparam name="U">return Type of Function.</typeparam>
        /// <param name="hostUrl"></param>
        /// <param name="requestType"></param>
        /// <param name="apiUrl"></param>
        /// <param name="objToPost"></param>
        /// <param name="headerParameter"></param>
        /// <returns></returns>
        public static U PostSync<T, U>(string hostUrl, string requestType, string apiUrl, T objToPost, Dictionary<string, string> headerParameters)
        {
            U objResponse = default(U);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                if (headerParameters != null && headerParameters.Count() > 0)
                {
                    foreach (var param in headerParameters)
                    {
                        client.DefaultRequestHeaders.Add(param.Key, param.Value);
                    }
                }


                using (var response = client.PostAsJsonAsync(apiUrl, objToPost).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        objResponse = response.Content.ReadAsAsync<U>().Result;
                    }
                }
            }
            return objResponse;
        }

        /// <summary>
        /// Written By : Sangram Nandkhile on 8 October 2015
        /// Put Request and fetch response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hostUrl"></param>
        /// <param name="requestType"></param>
        /// <param name="apiUrl"></param>
        /// <param name="objResponse"></param>
        /// <returns></returns>
        public static U PutSync<T, U>(string hostUrl, string requestType, string apiUrl, T objToPost)
        {
            U objResponse = default(U);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(hostUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                var response = client.PutAsJsonAsync(apiUrl, objToPost).Result;

                if (response.IsSuccessStatusCode)
                {
                    objResponse = response.Content.ReadAsAsync<U>().Result;
                }                
            }

            return objResponse;
        }

    }   // class
}   // namespace
