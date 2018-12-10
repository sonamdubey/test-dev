using Carwale.Utility;
using FluentValidation;

namespace Carwale.Entity.Blocking
{
    public class BlockMobileInputsValidator : AbstractValidator<BlockMobileInputs>
    {
        public BlockMobileInputsValidator()
        {

            RuleFor(x => x.MobileNos).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                .ShouldNotContainMoreThan(100, "mobile nos")
                .NotContainEmptyOrNullElement()
                .SetCollectionValidator(new MobileValidator());
            RuleFor(x => x.Reason).Length(0, 200);
        }
    }
}
