using Carwale.Entity.Enum;
using System;

namespace Carwale.Entity.Stock
{
    public class SellInquiry
    {
        public int? DealerId { get; set; }
        public int? SellerType { get; set; }
        public int? CarVersionId { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? Price { get; set; }
        public DateTime? MakeYear { get; set; }
        public int? Kilometers { get; set; }
        public string Color { get; set; }
        public int OwnerType { get; set; }       
        public DateTime? LastUpdated { get; set; }
        public int? TC_StockId { get; set; }
        public string AdditionalFuelType { get; set; }
        public int? SourceId { get; set; }
        public string CarRegNo { get; set; } //SellInquiries     
        public string RegistrationPlace { get; set; }  //Details  
        public string InteriorColor { get; set; }  //Details       
        public string Comments { get; set; } //SellInquiries     
        public string OneTimeTax { get; set; } //Details        
        public string Insurance { get; set; } //Details       
        public DateTime? InsuranceExpiry { get; set; } //Details      
        public int? CityMileage { get; set; } //Details             
        public string CarDriven { get; set; } //Details       
        public Boolean Accidental { get; set; } //Details     
        public Boolean FloodAffected { get; set; } //Details
        public string Modifications { get; set; } //Details
        public string VideoUrl { get; set; } //Details
        public int? CertProgId { get; set; } //SellInquiries
        public int CtePackageId { get; set; } //SellInquiries
    }
}
