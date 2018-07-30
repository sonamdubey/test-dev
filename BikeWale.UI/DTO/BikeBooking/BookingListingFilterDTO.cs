using Newtonsoft.Json;

namespace Bikewale.DTO.BikeBooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Feb 2016
    /// Description :   Booking Listing Filter DTO
    /// </summary>
    public class BookingListingFilterDTO
    {
        [JsonProperty("makeIds")]
        public string MakeIds { get; set; }
        [JsonProperty("budget")]
        public string Budget { get; set; }
        [JsonProperty("rideStyle")]
        public string RideStyle { get; set; }
        [JsonProperty("pageNo")]
        public int PageNo { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("so")]
        public string so { get; set; }
        [JsonProperty("sc")]
        public string sc { get; set; }
    }
}
