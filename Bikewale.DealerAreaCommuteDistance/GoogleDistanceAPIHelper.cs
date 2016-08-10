using BikewaleOpr.Entities;
using Consumer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Bikewale.DealerAreaCommuteDistance
{
    /// <summary>
    /// Author      :  Sumit Kate on 30 Mar 2016
    /// Description :  For handling and communicating Goolge Map API.
    /// </summary>
    internal class GoogleDistanceAPIHelper
    {
        private string GoogleUrl = "https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&mode=driving&origins={0}&destinations={1}&key={2}";
        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   Returns Lat-Lon as CSV
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        internal string FormatCSVLatLon(string latitude, string longitude)
        {
            return string.Format("{0},{1}", latitude, longitude);
        }
        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   Returns Lat-Lon as CSV
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        internal string FormatCSVLatLon(double latitude, double longitude)
        {
            return string.Format("{0},{1}", latitude, longitude);
        }
        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   Returns Lat-Lon as CSV
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        internal string FormatCSVLatLon(GeoLocationEntity location)
        {
            return string.Format("{0},{1}", location.Latitude, location.Longitude);
        }
        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   Returns Lat-Lon as CSV</summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        internal string FormatCSVLatLonArr(IEnumerable<GeoLocationEntity> locations)
        {
            StringBuilder retVal = new StringBuilder();
            int arrLen = locations != null ? locations.Count() : 0;
            for (int index = 0; index < arrLen; index++)
            {
                retVal.AppendFormat("{0}|", FormatCSVLatLon(locations.ElementAt(index)));
            }
            if (arrLen > 0)
                retVal.Length--;
            return retVal.ToString();
        }
        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   Returns the Google Location API which can be used to send request
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        internal string FormatRequestUrl(GeoLocationEntity source, IEnumerable<GeoLocationEntity> destinations)
        {
            return String.Format(GoogleUrl, FormatCSVLatLon(source), FormatCSVLatLonArr(destinations), ConfigurationManager.AppSettings["GoogleDistanceMatrixAPIKey"]);
        }

        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   To Get Distance Using Array Index.
        /// Modified By : Lucky Rathore 13 Apr. 2016
        /// Description : StrDistance changed from meter to km.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public IEnumerable<GeoLocationDestinationEntity> GetDistanceUsingArrayIndex(GeoLocationEntity source, IEnumerable<GeoLocationEntity> destinations, bool showShortedFirst = false)
        {
            JObject response = null;
            IList<GeoLocationDestinationEntity> lstLinqGeoLocationDestination = null;
            string url = String.Empty;
            try
            {
                url = FormatRequestUrl(source, destinations);
                response = CallAPI(url);
                if (response != null)
                {
                    int resultCount = 0;
                    var resp = JObject.FromObject(response);

                    if (resp != null && Convert.ToString(resp["status"]) == "OK")
                    {
                        resultCount = resp["rows"][0]["elements"].Count();
                        lstLinqGeoLocationDestination =
                            new List<GeoLocationDestinationEntity>();
                        for (int index = 0; index < resultCount; index++)
                        {
                            lstLinqGeoLocationDestination.Add(
                               new GeoLocationDestinationEntity()
                               {
                                   StrDistance = Convert.ToString(resp["rows"][0]["elements"][index]["distance"]["value"]),
                                   Source = source,
                                   Id = destinations.ElementAt(index).Id,
                                   Latitude = destinations.ElementAt(index).Latitude,
                                   Longitude = destinations.ElementAt(index).Longitude
                               });
                        }
                    }
                    else
                    {
                        Logs.WriteInfoLog("resp status in not OK in GetDistanceUsingArrayIndex Mathod, Response :  " + resp);
                    }

                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("GetDistanceUsingArrayIndex : " + ex.Message);
            }
            if (showShortedFirst)
            {
                return lstLinqGeoLocationDestination.OrderBy(m => m.DistanceInKm);
            }
            else
            {
                return lstLinqGeoLocationDestination;
            }
        }

        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   Returns the Google Location API which can be used to send request
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        internal JObject CallAPI(string apiUrl)
        {
            HttpClient httpClient = null;
            JObject response = null;
            try
            {
                httpClient = new HttpClient();
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
                                response = _response.Content.ReadAsAsync<JObject>().Result;
                                _response.Content.Dispose();
                                _response.Content = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("CallAPI : " + ex.Message);
            }
            return response;
        }

        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }

        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   format Api Responce
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        internal string FormatApiResp(IEnumerable<GeoLocationDestinationEntity> apiResp)
        {
            StringBuilder resp = new StringBuilder();
            foreach (GeoLocationDestinationEntity location in apiResp)
            {
                resp.Append(location.AreaDistance);
                resp.Append(",");
            }
            resp.Length--;
            return resp.ToString();
        }
    }
}
