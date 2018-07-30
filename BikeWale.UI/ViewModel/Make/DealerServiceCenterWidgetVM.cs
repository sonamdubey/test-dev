using Bikewale.Entities.Dealer;
using Bikewale.Models.ServiceCenters;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <author>
    /// Create by: Sangram Nandkhile on 25-Mar-2017
    /// Summary:  View Model for  Widget to show service center and dealers
    /// </author>
    public class DealerServiceCenterWidgetVM
    {
        public string Title { get; set; }
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public ICollection<PopularCityDealerEntity> DealerDetails { get; set; }
        public ServiceCenterDetailsWidgetVM ServiceCenters { get; set; } 
        public uint TotalDealerCount { get; set; }
        public uint TotalServiceCenterCount { get; set; }
    }
}
