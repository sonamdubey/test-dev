using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeBooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 05 Feb 2016
    /// Description :   Bike Booking Listing Output
    /// </summary>
    public class BikeBookingListingOutput
    {
        [JsonProperty("bikes")]
        public IEnumerable<BikeBookingListingDTO> Bikes { get; set; }
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
        [JsonProperty("fetchedCount")]
        public int FetchedCount { get; set; }
        [JsonProperty("pageUrl")]
        public PagingUrl PageUrl { get; set; }
        [JsonProperty("curPageNo")]
        public int CurPageNo { get; set; }
    }
}
