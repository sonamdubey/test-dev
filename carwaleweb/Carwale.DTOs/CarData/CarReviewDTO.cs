﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CarReviewDTO
    {
        [JsonProperty("overallRating")]
        public float OverallRating { get; set; }
        [JsonProperty("reviewCount")]
        public ushort ReviewCount { get; set; }
    }
}
