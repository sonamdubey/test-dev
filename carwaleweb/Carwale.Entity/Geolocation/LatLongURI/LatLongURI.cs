using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation.LatLongURI
{
    /// <summary>
    /// for parameters to be passed in querysting for citybylatlong api
    /// written by Natesh kumar on 5/11/14
    /// </summary>
    public class LatLongURI
    {
        [JsonProperty(PropertyName = "latitude")]
        [Keyword(Name = "lattitude")]	//Added By : Sadhana Upadhyay on 22 Apr 2015
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "modelId")]
        public string ModelId { get; set; }
    }
}
