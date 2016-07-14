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
    /// Modified By     :   Sumit Kate on 23 Dec 2015
    /// Description     :   Removed the Old methods
    /// </summary>
    public sealed class BWHttpClient : IDisposable
    {
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
                    using (HttpResponseMessage _response = await httpClient.GetAsync(apiUrl).ConfigureAwait(false))
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponse<T>({0},{1})", apiHost, apiUrl));
                objErr.SendMail();
            }
            return objTask;
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiHost"></param>
        /// <param name="requestType"></param>
        /// <param name="apiUrl"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
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
                    case APIHost.CWS:
                        httpClient = SingletonCWSHttpClient.Instance;
                        break;
                    case APIHost.None:
                        break;
                    default:
                        break;
                }

                if (httpClient != null)
                {
                    //HTTP GET
                    if (Utility.BWConfiguration.Instance.ApiMaxWaitTime <= 0)
                    {
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
                    else
                    {
                        var task = httpClient.GetAsync(apiUrl);

                        if (task.Wait(Utility.BWConfiguration.Instance.ApiMaxWaitTime))
                        {
                            using (HttpResponseMessage _response = task.Result)
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
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponseSync<T>({0},{1})", apiHost, apiUrl));
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
                    //if (headerParameters != null && headerParameters.Count() > 0)
                    //{
                    //    foreach (var param in headerParameters)
                    //    {
                    //        httpClient.DefaultRequestHeaders.Add(param.Key, param.Value);
                    //    }
                    //}

                    if (Utility.BWConfiguration.Instance.ApiMaxWaitTime <= 0)
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
                    else
                    {
                        var task = httpClient.GetAsync(apiUrl);

                        if (task.Wait(Utility.BWConfiguration.Instance.ApiMaxWaitTime))
                        {
                            using (HttpResponseMessage _response = task.Result)
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
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.GetApiResponseSync<T>({0},{1})", apiHost, apiUrl));
                objErr.SendMail();
            }
            finally
            {
                //if (httpClient != null && headerParameters != null && headerParameters.Count() > 0)
                //{
                //    foreach (var param in headerParameters)
                //    {
                //        httpClient.DefaultRequestHeaders.Remove(param.Key);
                //    }
                //}
            }
            return objTask;
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostAsync<T>({0},{1})", apiHost, apiUrl));                
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostSync<T, U>({0},{1})", apiHost, apiUrl));
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

                    //if (headerParameters != null && headerParameters.Count() > 0)
                    //{
                    //    foreach (var param in headerParameters)
                    //    {
                    //        httpClient.DefaultRequestHeaders.Add(param.Key, param.Value);
                    //    }
                    //}

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
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PostSync<T, U>({0},{1})", apiHost, apiUrl));
                objErr.SendMail();
            }
            finally
            {
                //if (httpClient != null && headerParameters != null && headerParameters.Count() > 0)
                //{
                //    foreach (var param in headerParameters)
                //    {
                //        httpClient.DefaultRequestHeaders.Remove(param.Key);
                //    }
                //}
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Utility.BWHttpClient.BWHttpClient.PutSync<T, U>({0},{1})", apiHost, apiUrl));
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
