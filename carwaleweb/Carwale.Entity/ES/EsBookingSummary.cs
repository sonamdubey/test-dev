using System;

namespace Carwale.Entity.ES
{
    public class EsBookingSummary
    {
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime InquiryDate { get; set; }
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerMobile { get; set; }
        public string DealerAddress { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string CarVersion { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public string ModelMaskingName { get; set; }
        public string VersionMaskingName { get; set; }
        public int BookingAmount { get; set; }
        public string PaymentType { get; set; }
        public int PaymentMode { get; set; }
        public int ExteriorColorId { get; set; }
        public int InteriorColorId { get; set; }
        public string TransactionId { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsTransactionCompleted { get; set; }
        public string Transmission { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
    }
}
