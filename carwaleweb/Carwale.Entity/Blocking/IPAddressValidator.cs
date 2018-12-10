using Carwale.Utility;
using FluentValidation;

namespace Carwale.Entity.Blocking
{
    public class IPAddressValidator : AbstractValidator<string>
    {
        public IPAddressValidator()
        {
            RuleFor(x => x).Must(x => RegExValidations.IsValidIpAddress(x)).WithMessage("{PropertyValue} is not in correct ip-address format.");
        }
    }
}
