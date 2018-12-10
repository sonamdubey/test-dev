using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock.Certification
{
    public class StockCertificationSubItemValidator : AbstractValidator<StockCertificationSubItem>
    {
        public StockCertificationSubItemValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 50);
            RuleFor(x => x.Condition).NotEmpty().Length(1, 50);
            RuleFor(x => x.LegendId).NotNull().GreaterThan(0).LessThanOrEqualTo(7);
        }
    }
}
