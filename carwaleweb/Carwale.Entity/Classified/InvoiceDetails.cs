using System;

namespace Carwale.Entity.Classified
{
    public class InvoiceDetails
    {
        public string CprID { get; set; }
        public string ConsumerName { get; set; }
        public string ConsumerEmail { get; set; }
        public string ConsumerContactNo { get; set; }
        public string ConsumerAddress { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentModeDetails { get; set; }
        public string Amount { get; set; }
        public string PackageName { get; set; }
        public string PackageDetails { get; set; }
        public string EntryDateTime { get; set; }
    }
}

