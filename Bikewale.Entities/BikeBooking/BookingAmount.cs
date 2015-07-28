using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entity.BikeBooking
{
    /// <summary>
    /// Written By : Ashwini Todkar on 15 dec 2014
    /// </summary>
    /// 
    [Serializable]
    public class BookingAmount
    {
        [JsonProperty("amount")]
        public UInt32 Amount { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}
