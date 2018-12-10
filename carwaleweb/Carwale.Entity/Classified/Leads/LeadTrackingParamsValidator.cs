using FluentValidation;

namespace Carwale.Entity.Classified.Leads
{
    public class LeadTrackingParamsValidator : AbstractValidator<LeadTrackingParams>
    {
        public LeadTrackingParamsValidator()
        {
            RuleFor(ltp => ltp.LeadType)
                .IsInEnum();
        }
    }
}
