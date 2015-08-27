﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.CustomerDetails
{
    /// <summary>
    /// Price Quote Bike Color Entity
    /// Author  :   Sumit Kate
    /// Date    :   19 Aug 2015
    /// </summary>
    public class PQColor
    {
        [JsonProperty("colorId")]
        public uint ColorId { get; set; }
        [JsonProperty("colorName")]
        public string ColorName { get; set; }
        [JsonProperty("colorCode")]
        public string ColorCode { get; set; }
        [JsonProperty("companyCode")]
        public string CompanyCode { get; set; }
    }
}
