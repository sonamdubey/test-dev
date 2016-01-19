using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.City
{
    /// <summary>
    /// Price Quote City base
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// Modified By :   Sumit Kate on 12 Jan 2016
    /// Summary     :   Added new property HasAreas
    /// </summary>
    public class PQCityBase
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }

        [JsonProperty("isPopular")]
        public bool IsPopular { get; set; }

        [JsonProperty("hasAreas")]
        public bool HasAreas { get; set; }
    }
}
