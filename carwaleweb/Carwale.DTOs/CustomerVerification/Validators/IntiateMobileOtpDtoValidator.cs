using Carwale.Entity.Vernam;
using Carwale.Utility;
using FluentValidation;
namespace Carwale.DTOs.CustomerVerification.Validators
{
    public class InitiateMobileOtpDtoValidator : AbstractValidator<InitiateVerificationDto>
    {
        public InitiateMobileOtpDtoValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(dto => dto.Mobile)
                .Must(mob => RegExValidations.IsValidMobile(mob))
                .WithMessage("Invalid mobile number");
            RuleFor(dto => dto.SourceModule)
                .IsInEnum()
                .Must(sm => sm != SourceModule.Unknown)
                .WithMessage("Invalid source module");
        }
    }
}
