﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeBooking.City
{
    /// <summary>
    /// Bikebooking City
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBCityBase
    {
        [JsonProperty("cityId")]
        public uint CityId { get; set; }
        [JsonProperty("cityName")]
        public string CityName { get; set; }
        [JsonProperty("cityMaskingName")]
        public string CityMaskingName { get; set; }
    }
}
