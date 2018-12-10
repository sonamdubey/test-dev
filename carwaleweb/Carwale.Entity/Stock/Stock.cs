using Carwale.Entity.Enum;
using FluentValidation;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    [JsonObject, Serializable, Validator(typeof(StockValidator))]
    public class Stock
    {
        public int? Id { get; set; }

        public int? SellerId { get; set; }

        public int? SellerType { get; set; }

        public int? SourceId { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        public int? VersionId { get; set; }

        public int? Price { get; set; }

        public int? Kms { get; set; }

        public string AdditionalFuel { get; set; }

        public DateTime? MfgDate { get; set; }

        public string Color { get; set; }

        public string OwnerType { get; set; }

        public string InspectionLink { get; set; }

        public List<StockImage> Images { get; set; }

        public string CarRegNo { get; set; } //SellInquiries

        public string RegistrationPlace { get; set; }  //Details (should not be empty)

        public string InteriorColor { get; set; }  //Details

        public string Comments { get; set; } //SellInquiries

        public string OneTimeTax { get; set; } //Details 

        public string Insurance { get; set; } //Details 

        public DateTime? InsuranceExpiry { get; set; } //Details

        public int? CityMileage { get; set; } //Details (should not be zero)

        public string CarDriven { get; set; } //Details

        public Boolean Accidental { get; set; } //Details 

        public Boolean FloodAffected { get; set; } //Details

        public string Modifications { get; set; } //Details

        public string VideoUrl { get; set; } //Details

        public int? CertProgId { get; set; } //SellInquiries

        public int CtePackageId { get; set; }
    }
}
