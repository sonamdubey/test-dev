using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockImageStatusListValidator: AbstractValidator<StockImageStatusList>
    {
        public StockImageStatusListValidator()
        {
            RuleFor(s => s.SellerType).NotNull().GreaterThan(0);
            RuleFor(s => s.Status).NotNull().SetCollectionValidator(new StockImageStatusValidator());
        }
    }
}
