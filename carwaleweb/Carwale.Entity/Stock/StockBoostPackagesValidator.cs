using FluentValidation;

namespace Carwale.Entity.Stock
{
    public class StockBoostPackagesValidator : AbstractValidator<StockBoostPackages>
    {
        public StockBoostPackagesValidator()
        {
            RuleFor(x => x.BoostPackageId).GreaterThan(0);
        }
    }
}
