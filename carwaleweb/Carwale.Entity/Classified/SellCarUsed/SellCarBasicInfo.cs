using System;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class SellCarBasicInfo
    {
        public int InquiryId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerMobile { get; set; }
        public int CityId { get; set; }
        public int CarVersionId { get; set; }
        public DateTime MakeYear { get; set; }
        public int Kms { get; set; }
        public int Price { get; set; }
        public int SourceId { get; set; }
        public int? PinCode { get; set; }
        public int PackageId { get; set; }
        public string AreaName { get; set; }
        public bool ShowContactDetails { get; set; }
        public string Referrer { get; set; }
        public string IPAddress { get; set; }
        public bool? ShareToCT { get; set; }
        public decimal  Owners { get; set; }
        public int PackageType { get; set; }
    }
}