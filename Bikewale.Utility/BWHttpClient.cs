using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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
            catch (Exception)
            {
                return objTask;
            }
            return objTask;
        }

        /// <summary>
        /// Modified By : Kartik rathod on  30 march 2018
        /// Desc    : added case for GoogleApi, CWOPR
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
                    case APIHost.GoogleApi:
                        httpClient = SingletonGoogleAPIHttpClient.Instance;
                        break;
                    case APIHost.CWOPR:
                        httpClient = SingletonCWOPRHttpClient.Instance;
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
                return objTask;
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
                return objTask;
            }
            finally
            {
                //if (httpClient != null && headerParameters != null && headerParameters.Any())
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
                return false;
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
            catch (Exception)
            {
                return false;
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
                    using (var response = httpClient.PostAsJsonAsync(apiUrl, objToPost).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            objResponse = response.Content.ReadAsAsync<U>().Result;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return objResponse;
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
                return objResponse;
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
                return objResponse;
            }

            return objResponse;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }   // class
}   // namespace
