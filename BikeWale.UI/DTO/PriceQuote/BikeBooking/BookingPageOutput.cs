using Bikewale.DTO.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.BikeBooking
{
    /// <summary>
    /// Bike booking input
    /// </summary>
    public class BookingPageOutput
    {
        /// <summary>
        /// List of versions price details
        /// </summary>
        [JsonProperty("varients")]
        public IList<BikeDealerPriceDetailDTO> Varients { get; set; }
        /// <summary>
        /// Disclaimers list
        /// </summary>
        [JsonProperty("disclaimers")]
        public IList<string> Disclaimers { get; set; }
        /// <summary>
        /// Offers list
        /// </summary>
        [JsonProperty("offers")]
        public IList<DealerOfferDTO> Offers { get; set; }

        /// <summary>
        /// Bike Model Colors
        /// </summary>
        [JsonProperty("modelColors")]
        public IEnumerable<ModelColor> BikeModelColors { get; set; }
    }
}
