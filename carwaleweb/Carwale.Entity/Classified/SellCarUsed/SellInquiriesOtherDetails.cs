using Carwale.Entity.Enum;
using System;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class SellInquiriesOtherDetails
    {
        public int Id { get; set; }
        public string CarName { get; set; }
        public DateTime MakeYear { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public int? Owners { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public string RegistrationNumber { get; set; }
        public string RegistrationPlace { get; set; }
        public string Insurance { get; set; }
        public string ReasonForSelling { get; set; }
        public string Comments { get; set; }
        public string OneTimeTax { get; set; }
        public DateTime InsuranceExpiry { get; set; }
        public CarRegistrationType RegType { get; set; }
        public ClassifiedPackageType PackageType { get; set; }
    }
}
