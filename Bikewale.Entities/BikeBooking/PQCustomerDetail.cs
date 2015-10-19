using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeBooking
{
    public class PQCustomerDetail
    {
        public CustomerEntity objCustomerBase { get; set; }
        public VersionColor objColor { get; set; }
        public bool IsTransactionCompleted {get;set;}
        public string AbInquiryId { get; set; }
    }
}
