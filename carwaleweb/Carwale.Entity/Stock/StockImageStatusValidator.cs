using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockImageStatusValidator: AbstractValidator<StockImageStatus>
    {
        public StockImageStatusValidator()
        {
            RuleFor(s => s.Id).NotNull().GreaterThan(0);
            RuleFor(s => s.Action).IsInEnum();
        }
    }
}
