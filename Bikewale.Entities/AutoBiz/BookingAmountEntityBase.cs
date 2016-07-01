using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Written By : Ashwini Todkar on 15 dec 2014
    /// </summary>
    /// 
    public class BookingAmountEntityBase
    {
        [JsonProperty("id")]
        public UInt32 Id { get; set; }

        [JsonProperty("amount")]
        public UInt32 Amount { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}
