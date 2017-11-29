using Bikewale.DTO.Customer;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.BikeBooking
{
    public class BikeCancellationCustomer : CustomerBase
    {
        [JsonProperty("bookingDate")]
        public DateTime BookingDate { get; set; }

        [JsonProperty("bikeName")]
        public string BikeName { get; set; }
    }

    public class ValidBikeCancellationResponse
    {
        [JsonProperty("isVerified")]
        public bool IsVerified { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("responseFlag")]
        public int ResponseFlag { get; set; }
    }
}
