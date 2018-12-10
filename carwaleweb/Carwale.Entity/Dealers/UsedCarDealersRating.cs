using FluentValidation.Attributes;

namespace Carwale.Entity.Dealers
{
    [Validator(typeof(UsedCarDealersRatingValidator))]
    public class UsedCarDealersRating
    {
        public string RatingText { get; set; }
    }
}
