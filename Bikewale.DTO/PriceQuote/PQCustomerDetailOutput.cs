﻿using Bikewale.DTO.PriceQuote.BikeBooking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote
{
    /// <summary>
    /// Price quote customer details output entity
    /// Author  :   Sumit Kate
    /// Date    :   21 Aug 2015
    /// </summary>
    public class PQCustomerDetailOutput
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("dealer")]
        public DealerDetailsDTO Dealer { get; set; }

        [JsonProperty("noOfAttempts")]
        public sbyte NoOfAttempts { get; set; }

    }
}
