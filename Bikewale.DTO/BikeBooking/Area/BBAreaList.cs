﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeBooking.Area
{
    /// <summary>
    /// Bikebooking Area List
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBAreaList
    {
        [JsonProperty("areas")]
        public IEnumerable<BBAreaBase> Areas { get; set; }
    }
}
