using Carwale.Utility;
using FluentValidation;

namespace Carwale.Entity.Blocking
{
    public class BlockIPInputsValidator : AbstractValidator<BlockIPInputs>
    {
        public BlockIPInputsValidator()
        {
            RuleFor(x => x.IpAddresses).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .ShouldNotContainMoreThan(20,"ip addresses")
                .NotContainEmptyOrNullElement()
                .SetCollectionValidator(new IPAddressValidator());
            RuleFor(x => x.Reason).Length(0, 200);
        }
    }
}
