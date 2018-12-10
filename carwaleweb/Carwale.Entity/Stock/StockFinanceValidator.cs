using Carwale.Utility.Classified;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockFinanceValidator : AbstractValidator<StockFinance>
    {
        public StockFinanceValidator()
        {
            RuleFor(x => x.ProfileId).NotNull().Must(StockValidations.IsProfileIdValid).WithMessage("ProfileId is not valid.");
            When(x => x.EmiAmount != null, () => RuleFor(x => x.EmiAmount).Must((y, z) => BitConverter.GetBytes(Decimal.GetBits(z.Value)[3])[2] <= 2).WithMessage("'Emi Amount' must not contain more than 2 decimal places.").GreaterThanOrEqualTo(0));
            RuleFor(x => x.IsEligible).NotNull();
        }
    }
}
