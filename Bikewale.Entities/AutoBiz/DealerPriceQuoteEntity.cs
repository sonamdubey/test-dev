using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BikeWale.Entities.AutoBiz;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 26 Oct 2015
    /// Modified By : Sushil Kumar on 10th January 2016
    /// Description : Added provision to retrieve bike availability by color
    /// </summary>
    public class DealerPriceQuoteEntity
    {
        public IEnumerable<PQ_Price> PriceList { get; set; }
        public IEnumerable<OfferEntityBase> OfferList { get; set; }
        public IEnumerable<DealerQuotation> DealerDetails { get; set; }
        public IEnumerable<BikeColorAvailability>BikeAvailabilityByColor {get;set;}
    }
}