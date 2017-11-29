
using Bikewale.Entities.Dealer;
namespace Bikewale.Models
{
    /// <author>
    /// Create by: Sangram Nandkhile on 31-Mar-2017
    /// Summary:  View Model for make page Dealers and service centers in India are shown
    /// Modified by Sajal Gupta on  23-11-2017
    /// Dewsc: Added IsIndiaCardNeeded
    /// </author>
    public class DealersServiceCentersIndiaWidgetVM
    {
        public PopularDealerServiceCenter DealerServiceCenters { get; set; }
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public bool IsServiceCenterPage { get; set; }
        public string CityCardLink { get; set; }

        public string CityCardTitle { get; set; }
        public bool IsIndiaCardNeeded { get; set; }
    }
}
