using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carwale.Entity.Classified.SellCar
{
    public class CustomerSellInquiry
    {
        public int Id { get; set; }
        public int LLInquiryId { get; set; }
        public string CarName { get; set; }
        public DateTime MakeYear { get; set; }
        public int Price { get; set; }
        public int Kilometers { get; set; }
        public DateTime EntryDate { get; set; }
        public string HostURL { get; set; }
        public string OriginalImgPath { get; set; }
        public int PhotoCount { get; set; }
        public bool IsPremium { get; set; }
        public DateTime ClassifiedExpiryDate { get; set; }
        public DateTime PackageExpiryDate { get; set; }
        public string Status { get; set; }
        public int CurrentStep { get; set; }
        public bool IsShowContact { get; set; }
        public bool IsListingCompleted { get; set; }
        public int PackageId { get; set; }
        public int TotalInq { get; set; }
        public int PaymentMode { get; set; }
        public int Free { get; set; }
        public int Paid { get; set; }
        public bool IsCustomerEditable { get; set; }
        public ClassifiedPackageType PackageType { get; set; }
        public int CityId { get; set; }
    }
}
