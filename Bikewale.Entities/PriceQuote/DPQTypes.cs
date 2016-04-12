using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Modified By : Lucky Rathore on 17 March 2016.
    /// Description : Subscription Model Added.
    /// </summary>
    public enum DPQTypes
    {
        NoOfferNoBooking = 1,
        NoOfferOnlineBooking = 2,
        OfferNoBooking = 3,
        OfferAndBooking = 4,
        AndroidAppOfferNoBooking = 5,
        AndroidAppNoOfferNoBooking = 6,
        SubscriptionModel = 7
    }
}
