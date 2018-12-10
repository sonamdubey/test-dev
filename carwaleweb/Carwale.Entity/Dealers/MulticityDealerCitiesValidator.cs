using Carwale.Utility;
using FluentValidation;

namespace Carwale.Entity.Dealers
{
    public class MulticityDealerCitiesValidator : AbstractValidator<MulticityDealerCities>
    {
        public MulticityDealerCitiesValidator()
        {
            RuleFor(x => x.CityIds).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().ShouldNotContainMoreThan(50, "cityids");
        }
    }
}
