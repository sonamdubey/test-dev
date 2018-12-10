using Carwale.Entity.Blocking.Enums;
using Carwale.Utility;
using FluentValidation;

namespace Carwale.DTOs.Blocking
{
    public class BlockedCommunicationDtoValidator : AbstractValidator<BlockedCommunicationDto>
    {
        public BlockedCommunicationDtoValidator()
        {
            RuleSet("Common", () => {
                RuleFor(x => x.Module).IsInEnum();
                RuleFor(x => x.Type).IsInEnum();
                RuleFor(x => x.Value).NotNull().NotEmpty();
                When(x => x.Type == CommunicationType.Mobile,
                    () => RuleFor(y => y.Value).Matches(@"^\d{10}$")
                    .WithMessage("Value {PropertyValue} is invalid for Mobile type"));
                When(x => x.Type == CommunicationType.Email,
                    () => RuleFor(y => y.Value).Matches(RegExValidations.EmailRegex)
                    .WithMessage("Value {PropertyValue} is invalid for Email type"));
                When(x => x.Type == CommunicationType.Ip,
                    () => RuleFor(y => y.Value).Must(RegExValidations.IsValidIpAddress)
                    .WithMessage("Value {PropertyValue} is invalid for Ip type"));
            });
            RuleSet("Reason", () => {
                RuleFor(x => x.Reason).NotNull().NotEmpty().Length(1, 500);
            });
            RuleSet("ActionBy", () => {
                RuleFor(x => x.ActionBy).NotNull().NotEmpty().Length(1, 50);
            });
        }
    }
}
