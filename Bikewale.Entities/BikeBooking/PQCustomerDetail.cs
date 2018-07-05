using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;

namespace Bikewale.Entities.BikeBooking
{
    public class PQCustomerDetail
    {
        public CustomerEntity objCustomerBase { get; set; }
        public VersionColor objColor { get; set; }
        public bool IsTransactionCompleted {get;set;}
        public string AbInquiryId { get; set; }
        public uint SelectedVersionId { get; set; }
        public uint DealerId { get; set; }
        public uint LeadId { get; set; }
    }
}
