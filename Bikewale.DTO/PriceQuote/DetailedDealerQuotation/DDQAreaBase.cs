using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.PriceQuote.DetailedDealerQuotation
{
    /// <summary>
    /// Detailed dealer quotation area entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class DDQAreaBase
    {
        [JsonProperty("areaId")]
        public UInt32 AreaId { get; set; }

        [JsonProperty("areaName")]
        public string AreaName { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }
    }
}
