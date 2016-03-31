using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    /// <summary>
    /// Author      :   Sumit Kate on 30 Mar 2016
    /// Description :   Google Distance API Url Helper
    /// </summary>
    public sealed class GoogleDistanceAPIHelper
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
        internal string FormatCSVLatLon(GeoLocation location)
        {
            return string.Format("{0},{1}", location.Latitude, location.Longitude);
        }

        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   Returns Lat-Lon as CSV</summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        internal string FormatCSVLatLonArr(IEnumerable<GeoLocation> locations)
        {
            string strRetVal = String.Empty;
            StringBuilder retVal = new StringBuilder();
            int arrLen = locations != null ? locations.Count() : 0;
            for (int index = 0; index < arrLen; index++)
            {
                retVal.AppendFormat("{0}|", FormatCSVLatLon(locations.ElementAt(index)));
            }
            if (arrLen > 0)
            {
                strRetVal = retVal.ToString();
                strRetVal = strRetVal.Substring(0, strRetVal.Length - 1);
            }
            return strRetVal;
        }

        /// <summary>
        /// Author      :   Sumit Kate on 30 Mar 2016
        /// Description :   Returns the Google Location API which can be used to send request
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        internal string FormatRequestUrl(GeoLocation source, IEnumerable<GeoLocation> destinations)
        {
            string retURL = String.Empty;
            retURL = String.Format(GoogleUrl, FormatCSVLatLon(source), FormatCSVLatLonArr(destinations), "AIzaSyDjG8tpNdQI86DH__-woOokTaknrDQkMC8");
            return retURL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public IEnumerable<GeoLocationDestination> GetDistance(GeoLocation source, IEnumerable<GeoLocation> destinations,bool showShortedFirst = false)
        {
            JObject response = null;
            IList<GeoLocationDestination> lstGeoLocationDestination = null;
            string url = String.Empty;
            try
            {
                url = FormatRequestUrl(source, destinations);
                lstGeoLocationDestination = new List<GeoLocationDestination>();
                response = CallAPI(url);

                if (response != null)
                {
                    int index = 0;
                    var resp = JObject.FromObject(response);
                    var resultObjects = AllChildren(resp["rows"])
            .First(c => c.Type == JTokenType.Array && c.Path.Contains("elements"))
            .Children<JObject>();
                    var distance = default(IEnumerable<JProperty>);
                    foreach (JObject result in resultObjects)
                    {

                        distance = result.Properties().Where(m => m.Name == "distance");

                        foreach (var dis in distance)
                        {
                            lstGeoLocationDestination.Add(
                                new GeoLocationDestination()
                                {
                                    Source = source,
                                    strDistance = Convert.ToString(JObject.FromObject(dis.Value)["value"]),
                                    Latitude = destinations.ElementAt(index).Latitude,
                                    Longitude = destinations.ElementAt(index).Longitude
                                }
                            );
                        }
                        index++;                     
                    }                    
                }

            }
            catch (Exception)
            {

            }
            if (showShortedFirst)
            {
                return lstGeoLocationDestination.OrderBy(m => m.Distance);
            }
            else
            {
                return lstGeoLocationDestination;
            }
        }

        internal JObject CallAPI(string apiUrl)
        {
            HttpClient httpClient = null;
            JObject response = null;
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
            return response;
        }
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
    }

    public class GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class GeoLocationDestination : GeoLocation
    {
        public GeoLocation Source { get; set; }
        public string strDistance { get; set; }
        public double Distance { get { double d = 0; Double.TryParse(strDistance, out d); return d; } }
    }
}
