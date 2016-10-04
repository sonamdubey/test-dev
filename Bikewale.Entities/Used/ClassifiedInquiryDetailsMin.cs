
using System;
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created by  :   Sumit Kate on 01 Sep 2016
    /// Description :   Classified Inquiry Details Min
    /// Modified by :   Sumit Kate on 29 Sep 2016
    /// Description :   Added citymaskingname, makemaskingname and modelmaskingname property
    /// </summary>
    public class ClassifiedInquiryDetailsMin
    {
        public string BikeName { get; set; }
        public Customer.CustomerEntityBase Seller { get; set; }
        public uint Price { get; set; }
        public DateTime MakeYear { get; set; }
        public uint KmsDriven { get; set; }
        public string City { get; set; }
        public string CityMaskingName { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
    }
}
