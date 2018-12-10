using Carwale.Entity.Classified.SellCarUsed;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.SellCar
{
    public class UpdateListing
    {
        public int? VersionId { get; set; }
        public InsuranceType? Insurance { get; set; }
        public int? InsuranceExpiryYear { get; set; }
        public int? InsuranceExpiryMonth { get; set; }
        public string RegistrationNumber { get; set; }
        public int? ManufactureYear { get; set; }
        public int? ManufactureMonth { get; set; }
        public string AlternateFuel { get; set; }
        public string Color { get; set; }
        public int? Owners { get; set; }
        public int? KmsDriven { get; set; }
        public string ExpectedPrice { get; set; }
        public bool? IsPremium { get; set; }
        public string MaskingNumber { get; set; }
        public CarRegistrationType? RegType { get; set; }
        public bool? CustomerEditable { get; set; }
    }
}
