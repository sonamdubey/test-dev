using Carwale.Entity.Classified.SellCarUsed;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.SellCar
{
    public class UpdateListingValidator: AbstractValidator<UpdateListing>
    {
        public UpdateListingValidator()
        {
            RuleFor(x => x.ManufactureYear).InclusiveBetween(1998, DateTime.Now.Year);
            RuleFor(x => x.ManufactureMonth).InclusiveBetween(1, 12);
            RuleFor(x => x.VersionId).GreaterThan(0);
            When(x => !string.IsNullOrEmpty(x.AlternateFuel), () => RuleFor(x => x.AlternateFuel).Matches("(?i)(CNG|LPG)"));
            RuleFor(x => x.Color).NotEmpty();
            RuleFor(x => x.Owners).InclusiveBetween(1, 8);
            RuleFor(x => x.KmsDriven).InclusiveBetween(100, 900000);
            RuleFor(x => x.ExpectedPrice).IsValidInteger().StrictlyBetween(20000, 100000000);
            When(x => x.Insurance != null, () => RuleFor(x => x.Insurance).IsInEnum());
            When(x => x.Insurance != null && x.Insurance == InsuranceType.Expired, () => RuleFor(x => x.InsuranceExpiryYear).Empty());
            When(x => x.Insurance != null && x.Insurance == InsuranceType.Expired, () => RuleFor(x => x.InsuranceExpiryMonth).Empty());
            When(x => x.Insurance != null && x.Insurance != InsuranceType.Expired, () => RuleFor(x => x.InsuranceExpiryYear).NotEmpty());
            When(x => x.Insurance != null && x.Insurance != InsuranceType.Expired, () => RuleFor(x => x.InsuranceExpiryMonth).NotEmpty());
            When(x => x.InsuranceExpiryYear != null, () => RuleFor(x => x.InsuranceExpiryYear).InclusiveBetween(DateTime.Now.Year, DateTime.Now.Year + 5));
            When(x => x.InsuranceExpiryMonth != null, () => RuleFor(x => x.InsuranceExpiryMonth).InclusiveBetween(1, 12));
            When(x => x.RegistrationNumber != null, () => RuleFor(x => x.RegistrationNumber).Matches(@"^[A-Za-z]{2}([0-9A-Za-z])*$"));
        }
    }
}
