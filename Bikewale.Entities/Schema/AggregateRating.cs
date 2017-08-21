﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 15th August 2017
    /// Description : To add aggregrate ratings to thing or a product type element
    /// </summary>
    public class AggregateRating
    {
        [JsonProperty("@type")]
        public string Type { get { return "AggregateRating"; } }

        [JsonProperty("ratingValue", NullValueHandling = NullValueHandling.Ignore)]
        public double? RatingValue { get; set; }

        [JsonProperty("ratingCount", NullValueHandling = NullValueHandling.Ignore)]
        public uint? RatingCount { get; set; }

        [JsonProperty("reviewCount", NullValueHandling = NullValueHandling.Ignore)]
        public uint? ReviewCount { get; set; }
    }
}
