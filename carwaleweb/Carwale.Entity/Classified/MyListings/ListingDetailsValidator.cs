using Carwale.Entity.Classified.SellCarUsed;
using FluentValidation;
using System;

namespace Carwale.Entity.Classified.MyListings
{
    public class ListingDetailsValidator : AbstractValidator<SellCarInfo>
    {
        public ListingDetailsValidator()
        {
            RuleSet("common", () =>
            {
                RuleFor(x => x.VersionId).GreaterThan(0);
                When(x => !string.IsNullOrEmpty(x.AlternateFuel), () => RuleFor(x => x.AlternateFuel).Matches("(?i)(CNG|LPG)"));
                RuleFor(x => x.Color).NotEmpty();
                RuleFor(x => x.Owners).InclusiveBetween(1, 8);
                RuleFor(x => x.KmsDriven).InclusiveBetween(100, 900000);
                RuleFor(x => x.ExpectedPrice).InclusiveBetween(20000, 100000000);
                When(x => x.Insurance != null, () => RuleFor(x => x.Insurance).IsInEnum());
                When(x => x.InsuranceExpiryYear != null, () => RuleFor(x => x.InsuranceExpiryYear).InclusiveBetween(DateTime.Now.Year, DateTime.Now.Year + 5));
                When(x => x.InsuranceExpiryMonth != null, () => RuleFor(x => x.InsuranceExpiryMonth).InclusiveBetween(1, 12));
                When(x => x.RegistrationNumber != null, () => RuleFor(x => x.RegistrationNumber).Matches(@"^[A-Za-z]{2}([0-9A-Za-z])*$"));
            });
            RuleSet("carregistrationtype", () =>
            {
                RuleFor(x => x.RegType).IsInEnum();
            });

        }
    }
}
