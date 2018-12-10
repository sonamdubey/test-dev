using Carwale.Entity.Enum;
using System;

namespace Carwale.Entity.Classified.ListingPayment
{
    public class Receipt
    {
        public int InquiryId { get; set; }
        public string Path { get; set; }
        public int PgTransactionId { get; set; }
        public string UniqueTransactionId { get; set; }
        public DateTime EntryDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public int Amount { get; set; }
        public ClassifiedPackageType PackageType { get; set; }
        public int PackageId { get; set; }
        public int PackageValidity { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string CarVersion { get; set; }
        public Platform Platform { get; set; }
    }
}
