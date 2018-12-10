using FluentValidation;
using Carwale.Entity.Enum;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class ListingStatusValidator : AbstractValidator<ListingDetails>
    {
        public ListingStatusValidator(StatusValidationType validationType)
        {
            RuleFor(x => x.InquiryId).GreaterThan(0);
            switch (validationType)
            {
                case StatusValidationType.Update:
                    RuleFor(x => x.Status).IsInEnum();
                    break;
                case StatusValidationType.Delete:
                    RuleFor(x => x.Status).IsInEnum().NotEqual(ListingStatus.Active);
                    break;
            }
        }
    }
}
