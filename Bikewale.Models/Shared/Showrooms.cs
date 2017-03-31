using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;

namespace Bikewale.Models.Shared
{
    /// <summary>
    /// Created by : Aditi Srivastava on 14 Mar 2017
    /// Summary    : model for dealer card
    /// </summary>
    public class ShowroomsVM
    {
        public string Title { get; set; }
        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }
        public uint CityId { get; set; }
        public string CityName { get; set; }
        public string CityMaskingName { get; set; }
        public DealersEntity dealers { get; set; }
        public PopularDealerServiceCenter dealerServiceCenter { get; set; }

        public bool IsDataAvailable { get; set; }
    }
}
