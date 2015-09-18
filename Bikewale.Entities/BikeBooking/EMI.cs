using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Written by Ashwini Todkar on 29 Oct 2014
    /// </summary>
    
    public class EMI
    {
        [JsonProperty("id")]
        public UInt32 Id { get; set; }

        [JsonProperty("tenure")]
        public UInt16 Tenure { get; set; }

        [JsonProperty("loanToValue")]
        public UInt16 LoanToValue { get; set; }

        [JsonProperty("rateOfInterest")]
        public float RateOfInterest { get; set; }

        [JsonProperty("loanProvider")]
        public string LoanProvider { get; set; }
    }
}
