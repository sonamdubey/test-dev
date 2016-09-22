
using System;
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Classified Inquiry Details Min
    /// </summary>
    public class ClassifiedInquiryDetailsMin
    {
        public string BikeName { get; set; }
        public Customer.CustomerEntityBase Seller { get; set; }
        public uint Price { get; set; }
        public DateTime MakeYear { get; set; }
        public uint KmsDriven { get; set; }
        public string City { get; set; }
    }
}
