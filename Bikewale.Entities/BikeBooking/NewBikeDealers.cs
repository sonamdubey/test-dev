using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Bikewale.Entities.Location;

namespace Bikewale.Entities.BikeBooking
{
    public class NewBikeDealers
    {
        [JsonProperty("dealerId")]
        public UInt32 DealerId { get; set; }

        [JsonProperty("dealerName")]
        public string Name { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("mobileNo")]
        public string MobileNo { get; set; }

        [JsonProperty("phoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty("organization")]
        public string Organization { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("workingTime")]
        public string WorkingTime { get; set; }


        [JsonProperty("address")]
        public string Address { get; set; }

        public StateEntityBase objState { get; set; }

        public CityEntityBase objCity { get; set; }

        public AreaEntityBase objArea { get; set; }
    }   //End of Class
}   //End of namespace
