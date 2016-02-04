using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeBooking
{
    public class BookingListingFilterDTO
    {
        [JsonProperty("makeIds")]
        public string MakeIds { get; set; }
        [JsonProperty("displacement")]
        public string Displacement { get; set; }
        [JsonProperty("budget")]
        public string Budget { get; set; }
        [JsonProperty("mileage")]
        public string Mileage { get; set; }
        [JsonProperty("rideStyle")]
        public string RideStyle { get; set; }
        [JsonProperty("antiBreakingSystem")]
        public string AntiBreakingSystem { get; set; }
        [JsonProperty("brakeType")]
        public string BrakeType { get; set; }
        [JsonProperty("alloyWheel")]
        public string AlloyWheel { get; set; }
        [JsonProperty("startType")]
        public string StartType { get; set; }
        [JsonProperty("pageNo")]
        public string PageNo { get; set; }
        [JsonProperty("pageSize")]
        public string PageSize { get; set; }
        [JsonProperty("so")]
        public string so { get; set; }
        [JsonProperty("sc")]
        public string sc { get; set; }
    }
}
