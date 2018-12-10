using Carwale.Utility;
using FluentValidation;

namespace Carwale.Entity.Blocking
{
    public class MobileValidator : AbstractValidator<string>
    {
        public MobileValidator()
        {
            RuleFor(x => x).Matches(RegExValidations.MobileRegex).WithMessage("{PropertyValue} is not in correct mobile-number format.");
        }
    }
}
