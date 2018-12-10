using Carwale.Utility.Classified;
using FluentValidation;

namespace Carwale.Entity.Classified.Leads
{
    public class LeadValidator : AbstractValidator<Lead>
    {
        public LeadValidator()
        {
            RuleFor(x => x.ProfileId).NotNull().Must(StockValidations.IsProfileIdValid).WithMessage("ProfileId is not valid.");
            RuleFor(x => x.Buyer).NotNull().SetValidator(new BuyerValidator());
            RuleFor(x => x.LeadTrackingParams).SetValidator(new LeadTrackingParamsValidator());
        }
    }
}
