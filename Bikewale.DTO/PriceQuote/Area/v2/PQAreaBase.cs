using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.Area.v2
{
    /// <summary>
    /// Price Quote Area Base new version
    /// Author  :   Sushil Kumar
    /// Date    :   12th Feb 2016
    /// </summary>
    public class PQAreaBase
    {
        [JsonProperty("id")]
        public UInt32 AreaId { get; set; }
        [JsonProperty("name")]
        public string AreaName { get; set; }
    }
}

