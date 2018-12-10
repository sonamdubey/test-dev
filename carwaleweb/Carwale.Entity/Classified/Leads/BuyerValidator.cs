using Carwale.Utility;
using FluentValidation;

namespace Carwale.Entity.Classified.Leads
{
    public class BuyerValidator : AbstractValidator<Buyer>
    {
        public BuyerValidator()
        {
            RuleFor(x => x.Mobile).NotEmpty().Matches(RegExValidations.MobileRegex);
            When(x => !string.IsNullOrEmpty(x.Name), () => RuleFor(x => x.Name).Length(3, 100).Matches(RegExValidations.NameRegex));
            When(x => !string.IsNullOrEmpty(x.Email), () => RuleFor(x => x.Email).Length(5, 100).Matches(RegExValidations.EmailRegex));
        }
    }
}
