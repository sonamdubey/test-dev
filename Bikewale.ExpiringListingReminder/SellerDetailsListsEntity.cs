using System.Collections.Generic;

namespace Bikewale.ExpiringListingReminder
{
    /// <summary>
    /// Created By Sajal Gupta on 23-11-2016.
    /// Desc : Have Lists to hold seller data for expiry listing.
    /// </summary>
    public class SellerDetailsListEntity
    {
        public IEnumerable<SellerDetailsEntity> sellerDetailsSevenDaysRemaining { get; set; }
        public IEnumerable<SellerDetailsEntity> sellerDetailsOneDayRemaining { get; set; }
    }
}
