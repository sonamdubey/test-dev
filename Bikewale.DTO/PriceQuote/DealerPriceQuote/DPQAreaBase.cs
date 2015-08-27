﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.DealerPriceQuote
{
    /// <summary>
    /// Dealer Price Quote area base
    /// </summary>
    public class DPQAreaBase
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
