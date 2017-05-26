using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by : Aditi Srivastava on 26 May 2017 
    /// Summary    : Entity for seller and bike details
    /// </summary>
    public class UsedBikeProfileDetails
    {
        public Customer.CustomerEntityBase SellerDetails { get; set; }
        public DateTime MakeYear { get; set; }
        public ushort Owner { get; set; }
        public string RideDistance { get; set; }
        public string City { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath {get;set;}
    }
}
