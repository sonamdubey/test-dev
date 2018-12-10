﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class CityAreaDTO
    {
        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("areaId")]
        public int AreaId { get; set; }

        [JsonProperty("areaName")]
        public string AreaName { get; set; }
    }
}
