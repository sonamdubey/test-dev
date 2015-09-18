using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Dealer Details
    /// with address
    /// Author  :   Sumit Kate
    /// Created :   10 Sept 2015
    /// </summary>
    public class DealerDetails
    {
        public uint Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Organization { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Pincode { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string MobileNo { get; set; }
        public string WebsiteUrl { get; set; }
        public string ContactHours { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
    }
}
