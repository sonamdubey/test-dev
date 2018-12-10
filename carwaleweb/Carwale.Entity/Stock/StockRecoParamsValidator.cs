using Carwale.Utility.Classified;
using FluentValidation;

namespace Carwale.Entity.Stock
{
    public class StockRecoParamsValidator : AbstractValidator<StockRecoParams>
    {
        public StockRecoParamsValidator()
        {
            When(s => !string.IsNullOrEmpty(s.ProfileId), () => RuleFor(s => s.ProfileId).Must(StockValidations.IsProfileIdValid).WithMessage("ProfileId is not valid"));
            RuleFor(s => s.Price).GreaterThan(0);
            RuleFor(s => s.RootId).GreaterThan(0);
            RuleFor(s => s.CityId).GreaterThan(0);
            RuleFor(s => s.RecommendationsCount).GreaterThan(0);
        }
    }
}
