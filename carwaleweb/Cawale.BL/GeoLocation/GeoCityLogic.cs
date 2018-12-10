using AEPLCore.Entity.Geolocation;
using AEPLCore.Utils.GeoMaps;
using AutoMapper;
using Carwale.Interfaces.AutoComplete;
using Carwale.Interfaces.Geolocation;
using Carwale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Carwale.BL.GeoLocation
{
    public class GeoCityLogic : IGeoCityLogic
    {
        private readonly IAutoComplete_v1 _autoComplete_v1;

        public GeoCityLogic(IAutoComplete_v1 autoComplete_v1)
        {
            _autoComplete_v1 = autoComplete_v1;
        }

        public DTOs.Suggest GetCityFromLatLong(string latitude, string longitude)
        {
            City cityObjFromAPI = Mapper.Map<City>(LocationFinder.GetAddressByLatLong(latitude, longitude));
            if (cityObjFromAPI != null && (!string.IsNullOrEmpty(cityObjFromAPI.CityName) || !string.IsNullOrEmpty(cityObjFromAPI.StateName)))
            {
                NameValueCollection nvc = new NameValueCollection { { "term", cityObjFromAPI.CityName } };
                var citySuggestion = _autoComplete_v1.GetCitySuggestion(nvc);
                if (citySuggestion.IsNotNullOrEmpty())
                {
                    return citySuggestion
                        .FirstOrDefault(cityObj => cityObj.Result.Split(',').Last().Trim().Equals(cityObjFromAPI.StateName, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    Carwale.Notifications.Logs.Logger.LogInfo($@"Null response from ElasticSearch against Response from GoogleMapsAPI: 
                                                                { JsonConvert.SerializeObject(cityObjFromAPI) } for lat,long : { latitude },{ longitude }");
                }
            }
            return null;
        }
    }
}
