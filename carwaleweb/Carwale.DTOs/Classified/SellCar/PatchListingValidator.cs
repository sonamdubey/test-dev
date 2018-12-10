using FluentValidation;
using Carwale.Entity.Classified.SellCarUsed;
using System;
using Carwale.Utility;

namespace Carwale.DTOs.Classified.SellCar
{
    public class PatchListingValidator : AbstractValidator<UpdateListing>
    {
        public PatchListingValidator()
        {
            RuleSet("cardetails", () =>
            {
                RuleFor(car => car.ManufactureYear).NotNull().InclusiveBetween(1998, DateTime.Now.Year).When(car => car.VersionId != null);
                RuleFor(car => car.ManufactureMonth).NotNull().InclusiveBetween(1, 12).When(car => car.ManufactureYear != null);
                RuleFor(car => car.VersionId).NotNull().GreaterThan(0).When(car => car.ManufactureYear != null);
                RuleFor(car => car.RegistrationNumber).Matches(@"^[A-Za-z]{2}([0-9A-Za-z])*$").When(car => !string.IsNullOrEmpty(car.RegistrationNumber));
                RuleFor(car => car.Owners).InclusiveBetween(1, 8).When(car => car.Owners != null);
                RuleFor(car => car.KmsDriven).InclusiveBetween(100, 900000).When(car => car.KmsDriven != null);
                RuleFor(car => car.Color).Matches(@"[a-zA-Z]$").When(car => !string.IsNullOrEmpty(car.Color) && !string.IsNullOrEmpty(car.Color.Trim()));
                RuleFor(car => car.AlternateFuel).Matches(@"[a-zA-Z]$").When(car => !string.IsNullOrEmpty(car.AlternateFuel) && !string.IsNullOrEmpty(car.AlternateFuel.Trim()));
                When(car => car.ExpectedPrice != null, () => RuleFor(car => car.ExpectedPrice).IsValidInteger().StrictlyBetween(20000, 100000000));
            });

            RuleSet("premium", () =>
            {
                RuleFor(car => car.IsPremium).NotNull().WithMessage("IsPremium must not be empty").When(car => car.MaskingNumber != null);
            });

            RuleSet("maskingnumber", () =>
            {
                RuleFor(car => car.MaskingNumber).NotEmpty().Matches(RegExValidations.MobileRegex).WithMessage("{PropertyValue} is not a correct mobile number").
                    When(car => car.IsPremium != null && car.IsPremium.Value.Equals(true));
                RuleFor(car => car.MaskingNumber).Empty().When(car => car.IsPremium != null && car.IsPremium.Value.Equals(false));
            });

            RuleSet("carinsurance",() =>
            {
                RuleFor(car => car.Insurance).NotNull().IsInEnum().When(car => car.InsuranceExpiryYear != null && car.InsuranceExpiryMonth != null);
                RuleFor(car => car.InsuranceExpiryYear).Empty().When(car => car.Insurance != null && car.Insurance == InsuranceType.Expired);
                RuleFor(car => car.InsuranceExpiryMonth).Empty().When(car => car.Insurance != null && car.Insurance == InsuranceType.Expired);
                RuleFor(car => car.InsuranceExpiryYear).NotEmpty().When(car => car.Insurance != null && car.Insurance != InsuranceType.Expired);
                RuleFor(car => car.InsuranceExpiryMonth).NotEmpty().When(car => car.Insurance != null && car.Insurance != InsuranceType.Expired);
                RuleFor(car => car.InsuranceExpiryYear).InclusiveBetween(DateTime.Now.Year, DateTime.Now.Year + 5).When(car => car.InsuranceExpiryYear != null);
                RuleFor(car => car.InsuranceExpiryMonth).InclusiveBetween(1, 12).When(car => car.InsuranceExpiryMonth != null);

            });
            RuleSet("carregistrationtype", () =>
            {
                RuleFor(car => car.RegType).IsInEnum().When(car => car.RegType != null);
            });
        }
    }
}
