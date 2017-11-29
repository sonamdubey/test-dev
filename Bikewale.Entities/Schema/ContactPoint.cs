﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 23rd Aug 2017
    /// Description : ContactPoint schema for local bussiness and places 
    /// </summary>
    public class ContactPoint
    {
        [JsonProperty("@type")]
        public string Type { get { return "ContactPoint"; } }

        [JsonProperty("telephone", NullValueHandling = NullValueHandling.Ignore)]
        public string Telephone { get; set; }

        [JsonProperty("contactType", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactType { get; set; }

        [JsonProperty("contactOption", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactOption { get; set; }

        [JsonProperty("areaServed", NullValueHandling = NullValueHandling.Ignore)]
        public string AreaServed { get; set; }

        [JsonProperty("availableLanguage", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> AvailableLanguage { get; set; }
    }
}
