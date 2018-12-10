using Carwale.Entity.Enum;
using FluentValidation;
using FluentValidation.Results;
using System;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class SellCarInfoValidator : AbstractValidator<SellCarInfo>
    {
        public override ValidationResult Validate(SellCarInfo sellCarInfo)
        {
            return sellCarInfo == null
                ? new ValidationResult(new[] { new ValidationFailure("cardetails", "cardetails can not be null") })
                : base.Validate(sellCarInfo);
        }
        public SellCarInfoValidator()
        {
            RuleSet("address", () =>
            {
                RuleFor(x => x.PinCode).InclusiveBetween(100000, 999999);
            });

            RuleSet("carmake", () => {
                RuleFor(x => x.MakeId).GreaterThan(0);
            });
            
            RuleSet("carmodel", () => {
                RuleFor(x => x.ModelId).GreaterThan(0);
            });

            RuleSet("carversion", () =>
            {
                RuleFor(x => x.VersionId).GreaterThan(0);
            });

            RuleSet("source", () => {
                RuleFor(x => x.SourceId).Equal(15).WithMessage("Source is not valid"); // used specifically for the apis shared to cartrade(sourceid = 15).
            });

            RuleSet("cardetails", () => 
            {
                RuleFor(x => x.ManufactureYear).InclusiveBetween(1998, DateTime.Now.Year);
                RuleFor(x => x.ManufactureMonth).InclusiveBetween(1, 12);
                When(x => !string.IsNullOrEmpty(x.AlternateFuel), () => RuleFor(x => x.AlternateFuel).Matches("(?i)(CNG|LPG)"));
                RuleFor(x => x.Color).NotEmpty();
                RuleFor(x => x.Owners).InclusiveBetween(1, 8);
                RuleFor(x => x.KmsDriven).InclusiveBetween(100, 900000);
                RuleFor(x => x.ExpectedPrice).InclusiveBetween(20000, 100000000);
            });

            RuleSet("carregistration", () =>
            {
                When(x => x.RegistrationNumber != null, () => RuleFor(x => x.RegistrationNumber).Matches(@"^[A-Za-z]{2}([0-9A-Za-z])*$"));
            });

            RuleSet("carregistrationtype", () =>
            {
                RuleFor(x => x.RegType).IsInEnum().WithMessage("Should be either Individual/Corporate/Taxi");
            });

            RuleSet("carinsurance", () =>
            {
                When(x => x.Insurance != null, () => RuleFor(x => x.Insurance).IsInEnum());
                When(x => x.Insurance != null && x.Insurance == InsuranceType.Expired, () => RuleFor(x => x.InsuranceExpiryYear).Empty());
                When(x => x.Insurance != null && x.Insurance == InsuranceType.Expired, () => RuleFor(x => x.InsuranceExpiryMonth).Empty());
                When(x => x.Insurance != null && x.Insurance != InsuranceType.Expired, () => RuleFor(x => x.InsuranceExpiryYear).NotEmpty());
                When(x => x.Insurance != null && x.Insurance != InsuranceType.Expired, () => RuleFor(x => x.InsuranceExpiryMonth).NotEmpty());
                When(x => x.InsuranceExpiryYear != null, () => RuleFor(x => x.InsuranceExpiryYear).InclusiveBetween(DateTime.Now.Year, DateTime.Now.Year + 5));
                When(x => x.InsuranceExpiryMonth != null, () => RuleFor(x => x.InsuranceExpiryMonth).InclusiveBetween(1, 12));
            });
        }
    }
}
