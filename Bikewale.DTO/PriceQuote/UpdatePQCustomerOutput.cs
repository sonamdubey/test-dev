
using Bikewale.DTO.PriceQuote.BikeBooking;
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
    /// Author  :   Sushil Kumar
    /// Date    :   12th December 2015
    /// </summary>
    public class UpdatePQCustomerOutput
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("noOfAttempts")]
        public sbyte NoOfAttempts { get; set; }

    }
}
