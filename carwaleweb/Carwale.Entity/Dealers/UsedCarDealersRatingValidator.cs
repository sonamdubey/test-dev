using Carwale.Utility;
using FluentValidation;

namespace Carwale.Entity.Dealers
{
    public class UsedCarDealersRatingValidator : AbstractValidator<UsedCarDealersRating>
    {
        public UsedCarDealersRatingValidator()
        {
            RuleFor(x => x.RatingText).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Must(RatingText => RatingText.Length <= 20).WithMessage("Length of rating text must be less than or equal to 20");
        }
    }
}
