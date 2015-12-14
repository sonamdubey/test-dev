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
using System.Configuration;

namespace Bikewale.Utility
{
    /// <summary>
    /// Modified By     :   Ashish
    /// </summary>
    public sealed class BWHttpClient : IDisposable
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
        public async Task<T> GetApiResponse<T>(string hostUrl, string requestType, string apiUrl, T responseType)
        {
            T objTask = default(T);
            HttpClient httpClient = null;
            #region commented old code
            //using (var client = CreateHttpClient())
            //{
            //        // New code:       
            //        //sets the base URI for HTTP requests
            //        client.BaseAddress = new Uri(hostUrl);
            //        client.DefaultRequestHeaders.Accept.Clear();

            //        //sets the Accept header to "application/json", which tells the server to send data in JSON format.
            //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

            //        //HTTP GET
            //        using (HttpResponseMessage _response = await client.GetAsync(apiUrl))
            //        {
            //          //_response.EnsureSuccessStatusCode(); //Throw if not a success code.

            //          if (_response.IsSuccessStatusCode)
            //          {
            //            if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status
            //              objTask = await _response.Content.ReadAsAsync<T>();
            //          }
            //        }
            //}  
            #endregion
            try
            {
                //SingletonBWHttpClient.Instance.BaseAddress = new Uri(hostUrl);
                //SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Clear();

                ////sets the Accept header to "application/json", which tells the server to send data in JSON format.
                //SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));
                httpClient = SingletonBWHttpClient.Instance;

                //HTTP GET
                using (HttpResponseMessage _response = await httpClient.GetAsync(apiUrl))
                {
                    //_response.EnsureSuccessStatusCode(); //Throw if not a success code.

                    if (_response.IsSuccessStatusCode)
                    {
                        if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status
                            objTask = await _response.Content.ReadAsAsync<T>();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponse<T>");
                objErr.SendMail();
            }
            return objTask;
        }

        public async Task<T> GetApiResponse<T>(APIHost apiHost, string requestType, string apiUrl, T responseType)
        {
            T objTask = default(T);
            HttpClient httpClient = null;
            try
            {
                switch (apiHost)
                {
                    case APIHost.BW:
                        httpClient = SingletonBWHttpClient.Instance;
                        break;
                    case APIHost.CW:
                        httpClient = SingletonCWHttpClient.Instance;
                        break;
                    case APIHost.AB:
                        httpClient = SingletonABHttpClient.Instance;
                        break;
                    case APIHost.None:
                        break;
                    default:
                        break;
                }

                if (httpClient != null)
                {
                    //HTTP GET
                    using (HttpResponseMessage _response = await httpClient.GetAsync(apiUrl))
                    {
                        //_response.EnsureSuccessStatusCode(); //Throw if not a success code.

                        if (_response.IsSuccessStatusCode)
                        {
                            if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status
                                objTask = await _response.Content.ReadAsAsync<T>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponse<T>");
                objErr.SendMail();
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
        public T GetApiResponseSync<T>(string hostUrl, string requestType, string apiUrl, T responseType)
        {
            T objTask = default(T);
            try
            {
                // New code:       
                //sets the base URI for HTTP requests
                SingletonBWHttpClient.Instance.BaseAddress = new Uri(hostUrl);
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Clear();

                //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                //HTTP GET
                using (HttpResponseMessage _response = SingletonBWHttpClient.Instance.GetAsync(apiUrl).Result)
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

        public T GetApiResponseSync<T>(APIHost apiHost, string requestType, string apiUrl, T responseType)
        {
            T objTask = default(T);
            HttpClient httpClient = null;
            try
            {
                switch (apiHost)
                {
                    case APIHost.BW:
                        httpClient = SingletonBWHttpClient.Instance;
                        break;
                    case APIHost.CW:
                        httpClient = SingletonCWHttpClient.Instance;
                        break;
                    case APIHost.AB:
                        httpClient = SingletonABHttpClient.Instance;
                        break;
                    case APIHost.None:
                        break;
                    default:
                        break;
                }

                if (httpClient != null)
                {
                    //HTTP GET
                    using (HttpResponseMessage _response = httpClient.GetAsync(apiUrl).Result)
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
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponseSync<T>({0},{1})", apiHost, apiUrl));
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
        public T GetApiResponseSync<T>(string hostUrl, string requestType, string apiUrl, T responseType, IDictionary<string, string> headerParameters)
        {
            T objTask = default(T);
            try
            {

                // New code:       
                //sets the base URI for HTTP requests
                SingletonBWHttpClient.Instance.BaseAddress = new Uri(hostUrl);
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Clear();

                //sets the Accept header to "application/json", which tells the server to send data in JSON format.
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                //Add parameter
                if (headerParameters != null && headerParameters.Count() > 0)
                {
                    foreach (var param in headerParameters)
                    {
                        SingletonBWHttpClient.Instance.DefaultRequestHeaders.Add(param.Key, param.Value);
                    }
                }

                //HTTP GET
                using (HttpResponseMessage _response = SingletonBWHttpClient.Instance.GetAsync(apiUrl).Result)
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponseSync<T>");
                objErr.SendMail();
            }

            return objTask;
        }

        public T GetApiResponseSync<T>(APIHost apiHost, string requestType, string apiUrl, T responseType, IDictionary<string, string> headerParameters)
        {
            T objTask = default(T);
            HttpClient httpClient = null;
            try
            {
                switch (apiHost)
                {
                    case APIHost.BW:
                        httpClient = SingletonBWHttpClient.Instance;
                        break;
                    case APIHost.CW:
                        httpClient = SingletonCWHttpClient.Instance;
                        break;
                    case APIHost.AB:
                        httpClient = SingletonABHttpClient.Instance;
                        break;
                    case APIHost.None:
                        break;
                    default:
                        break;
                }

                if (httpClient != null)
                {
                    //Add parameter
                    if (headerParameters != null && headerParameters.Count() > 0)
                    {
                        foreach (var param in headerParameters)
                        {
                            httpClient.DefaultRequestHeaders.Add(param.Key, param.Value);
                        }
                    }

                    //HTTP GET
                    using (HttpResponseMessage _response = httpClient.GetAsync(apiUrl).Result)
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponseSync<T>({0},{1})", apiHost, apiUrl));
                objErr.SendMail();
            }
            finally
            {
                if (httpClient != null && headerParameters != null && headerParameters.Count() > 0)
                {
                    foreach (var param in headerParameters)
                    {
                        httpClient.DefaultRequestHeaders.Remove(param.Key);
                    }
                }
            }
            return objTask;
        }

        public async Task<bool> PostAsync<T>(string hostUrl, string requestType, string apiUrl, T postObject)
        {
            bool isSuccess = false;
            try
            {
                // TODO - Send HTTP requests
                SingletonBWHttpClient.Instance.BaseAddress = new Uri(hostUrl);
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Clear();
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));
                // HTTP POST

                using (HttpResponseMessage response = await SingletonBWHttpClient.Instance.PostAsJsonAsync(apiUrl, postObject))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //// Get the URI of the created resource.
                        //Uri gizmoUrl = response.Headers.Location;
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostAsync<T>");
                objErr.SendMail();
            }
            return isSuccess;

        }

        public async Task<bool> PostAsync<T>(APIHost apiHost, string requestType, string apiUrl, T postObject)
        {
            bool isSuccess = false;
            HttpClient httpClient = null;
            try
            {
                switch (apiHost)
                {
                    case APIHost.BW:
                        httpClient = SingletonBWHttpClient.Instance;
                        break;
                    case APIHost.CW:
                        httpClient = SingletonCWHttpClient.Instance;
                        break;
                    case APIHost.AB:
                        httpClient = SingletonABHttpClient.Instance;
                        break;
                    case APIHost.None:
                        break;
                    default:
                        break;
                }

                if (httpClient != null)
                {
                    // HTTP POST

                    using (HttpResponseMessage response = await httpClient.PostAsJsonAsync(apiUrl, postObject))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            //// Get the URI of the created resource.
                            //Uri gizmoUrl = response.Headers.Location;
                            isSuccess = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostAsync<T>");
                objErr.SendMail();
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
        public bool DeleteSync(string hostUrl, string requestType, string apiUrl)
        {
            //Task<bool> t = default(Task<bool>);
            bool isSuccess = false;

            try
            {

                SingletonBWHttpClient.Instance.BaseAddress = new Uri(hostUrl);

                using (var response = SingletonBWHttpClient.Instance.DeleteAsync(apiUrl).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.DeleteSync");
                objErr.SendMail();
            }

            return isSuccess;
        }

        public bool DeleteSync(APIHost apiHost, string requestType, string apiUrl)
        {
            //Task<bool> t = default(Task<bool>);
            bool isSuccess = false;
            HttpClient httpClient = null;
            try
            {
                switch (apiHost)
                {
                    case APIHost.BW:
                        httpClient = SingletonBWHttpClient.Instance;
                        break;
                    case APIHost.CW:
                        httpClient = SingletonCWHttpClient.Instance;
                        break;
                    case APIHost.AB:
                        httpClient = SingletonABHttpClient.Instance;
                        break;
                    case APIHost.None:
                        break;
                    default:
                        break;
                }

                if (httpClient != null)
                {
                    using (var response = httpClient.DeleteAsync(apiUrl).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            isSuccess = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.DeleteSync");
                objErr.SendMail();
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
        public U PostSync<T, U>(string hostUrl, string requestType, string apiUrl, T objToPost)
        {
            U objResponse = default(U);

            try
            {
                SingletonBWHttpClient.Instance.BaseAddress = new Uri(hostUrl);
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Clear();
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                using (var response = SingletonBWHttpClient.Instance.PostAsJsonAsync(apiUrl, objToPost).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        objResponse = response.Content.ReadAsAsync<U>().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostSync<T, U>");
                objErr.SendMail();
            }
            return objResponse;
        }

        public U PostSync<T, U>(APIHost apiHost, string requestType, string apiUrl, T objToPost)
        {
            U objResponse = default(U);
            HttpClient httpClient = null;
            try
            {
                switch (apiHost)
                {
                    case APIHost.BW:
                        httpClient = SingletonBWHttpClient.Instance;
                        break;
                    case APIHost.CW:
                        httpClient = SingletonCWHttpClient.Instance;
                        break;
                    case APIHost.AB:
                        httpClient = SingletonABHttpClient.Instance;
                        break;
                    case APIHost.None:
                        break;
                    default:
                        break;
                }

                if (httpClient != null)
                {
                    using (var response = httpClient.PostAsJsonAsync(apiUrl, objToPost).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            objResponse = response.Content.ReadAsAsync<U>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostSync<T, U>");
                objErr.SendMail();
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
        public U PostSync<T, U>(string hostUrl, string requestType, string apiUrl, T objToPost, IDictionary<string, string> headerParameters)
        {
            U objResponse = default(U);

            try
            {
                SingletonBWHttpClient.Instance.BaseAddress = new Uri(hostUrl);
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Clear();
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                if (headerParameters != null && headerParameters.Count() > 0)
                {
                    foreach (var param in headerParameters)
                    {
                        SingletonBWHttpClient.Instance.DefaultRequestHeaders.Add(param.Key, param.Value);
                    }
                }


                using (var response = SingletonBWHttpClient.Instance.PostAsJsonAsync(apiUrl, objToPost).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        objResponse = response.Content.ReadAsAsync<U>().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostSync<T, U>");
                objErr.SendMail();
            }
            return objResponse;
        }

        public U PostSync<T, U>(APIHost apiHost, string requestType, string apiUrl, T objToPost, IDictionary<string, string> headerParameters)
        {
            U objResponse = default(U);
            HttpClient httpClient = null;
            try
            {
                switch (apiHost)
                {
                    case APIHost.BW:
                        httpClient = SingletonBWHttpClient.Instance;
                        break;
                    case APIHost.CW:
                        httpClient = SingletonCWHttpClient.Instance;
                        break;
                    case APIHost.AB:
                        httpClient = SingletonABHttpClient.Instance;
                        break;
                    case APIHost.None:
                        break;
                    default:
                        break;
                }

                if (httpClient != null)
                {
                    if (headerParameters != null && headerParameters.Count() > 0)
                    {
                        foreach (var param in headerParameters)
                        {
                            httpClient.DefaultRequestHeaders.Add(param.Key, param.Value);
                        }
                    }


                    using (var response = httpClient.PostAsJsonAsync(apiUrl, objToPost).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            objResponse = response.Content.ReadAsAsync<U>().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostSync<T, U>");
                objErr.SendMail();
            }
            finally
            {
                if (httpClient != null && headerParameters != null && headerParameters.Count() > 0)
                {
                    foreach (var param in headerParameters)
                    {
                        httpClient.DefaultRequestHeaders.Remove(param.Key);
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
        public U PutSync<T, U>(string hostUrl, string requestType, string apiUrl, T objToPost)
        {
            U objResponse = default(U);

            try
            {
                SingletonBWHttpClient.Instance.BaseAddress = new Uri(hostUrl);
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Clear();
                SingletonBWHttpClient.Instance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(requestType));

                var response = SingletonBWHttpClient.Instance.PutAsJsonAsync(apiUrl, objToPost).Result;

                if (response.IsSuccessStatusCode)
                {
                    objResponse = response.Content.ReadAsAsync<U>().Result;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PutSync<T, U>");
                objErr.SendMail();
            }

            return objResponse;
        }

        public U PutSync<T, U>(APIHost apiHost, string requestType, string apiUrl, T objToPost)
        {
            U objResponse = default(U);
            HttpClient httpClient = null;
            try
            {
                switch (apiHost)
                {
                    case APIHost.BW:
                        httpClient = SingletonBWHttpClient.Instance;
                        break;
                    case APIHost.CW:
                        httpClient = SingletonCWHttpClient.Instance;
                        break;
                    case APIHost.AB:
                        httpClient = SingletonABHttpClient.Instance;
                        break;
                    case APIHost.None:
                        break;
                    default:
                        break;
                }

                if (httpClient != null)
                {
                    var response = httpClient.PutAsJsonAsync(apiUrl, objToPost).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        objResponse = response.Content.ReadAsAsync<U>().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PutSync<T, U>");
                objErr.SendMail();
            }

            return objResponse;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }   // class
}   // namespace
