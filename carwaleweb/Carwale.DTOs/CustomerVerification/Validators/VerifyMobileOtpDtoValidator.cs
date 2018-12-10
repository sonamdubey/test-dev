using Carwale.Entity.Vernam;
using Carwale.Utility;
using FluentValidation;

namespace Carwale.DTOs.CustomerVerification.Validators
{
    public class VerifyMobileOtpDtoValidator : AbstractValidator<VerifyMobileOtpDto>
    {
        public VerifyMobileOtpDtoValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(dto => dto.MobileNumber)
                .Must(mn => RegExValidations.IsValidMobile(mn))
                .WithMessage("Invalid mobile number");
            RuleFor(dto => dto.OtpCode)
                .NotEmpty();
            RuleFor(dto => dto.SourceModule)
                .IsInEnum()
                .Must(sm => sm != SourceModule.Unknown)
                .WithMessage("Invalid source module");
        }
    }
}
