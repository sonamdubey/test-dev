using Bikewale.DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.BikeBooking
{
    public class BikeCancellationCustomer : CustomerBase
    {
        [JsonProperty("bookingDate")]
        public DateTime BookingDate { get; set; }

        [JsonProperty("bikeName")]
        public string BikeName { get; set; }
    }
}
