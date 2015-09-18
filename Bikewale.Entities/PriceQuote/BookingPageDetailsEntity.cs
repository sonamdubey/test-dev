using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Booking Page Details Entity
    /// Author  :   Sumit Kate
    /// Created :   10 Sept 2015
    /// </summary>
    public class BookingPageDetailsEntity
    {
        /// <summary>
        /// Dealer Details Entity
        /// </summary>
        public DealerDetails Dealer { get; set; }
        /// <summary>
        /// List of versions price details
        /// </summary>
        public IList<BikeDealerPriceDetail> Varients { get; set; }
        /// <summary>
        /// Disclaimers list
        /// </summary>
        public IList<string> Disclaimers { get; set; }
        /// <summary>
        /// Offers list
        /// </summary>
        public IList<DealerOfferEntity> Offers { get; set; }

        /// <summary>
        /// Bike Model Colors
        /// </summary>
        public IEnumerable<BikeModelColor> BikeModelColors { get; set; }
    }
}
