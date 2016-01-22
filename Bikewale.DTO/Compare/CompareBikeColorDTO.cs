﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Bike Color DTO
    /// </summary>
    public class CompareBikeColorDTO
    {
        [JsonProperty("bikeColors")]
        public List<BikeColorDTO> bikeColors { get; set; }
    }
}
