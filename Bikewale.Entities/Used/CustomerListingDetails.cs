
using System;
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by : Sajal Gupta on 25-11-16
    /// Desc : Entity for customer listing details.
    /// </summary>
    public class CustomerListingDetails : BikeDetailsMin
    {
        public int InquiryId { get; set; }
        public UInt16 StatusId { get; set; }
        public string CityMaskingName { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public UInt16 TotalViews { get; set; }
        public string Color { get; set; }
        public int Owner { get; set; }
        public bool IsApproved { get; set; }
        public UInt16 DaysRemaining { get; set; }
        public UInt16 SellerType { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
