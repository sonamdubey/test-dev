using Carwale.Entity.Blocking;
using Carwale.Utility;
using FluentValidation;

namespace Carwale.Entity.CustomerVerification
{
    public class MobileUnverificationInputsValidator : AbstractValidator<MobileUnverificationInputs>
    {
        public MobileUnverificationInputsValidator()
        {
            RuleFor(x => x.MobileNos).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .ShouldNotContainMoreThan(100, "mobile nos")
                .NotContainEmptyOrNullElement()
                .SetCollectionValidator(new MobileValidator());
        }
    }
}
