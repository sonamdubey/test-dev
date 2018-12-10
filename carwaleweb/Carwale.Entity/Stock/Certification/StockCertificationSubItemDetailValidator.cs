using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock.Certification
{
    public class StockCertificationSubItemDetailValidator : AbstractValidator<StockCertificationSubItemDetail>
    {
        public StockCertificationSubItemDetailValidator()
        {
            RuleFor(x => x.Text).NotEmpty().Length(1,200);
            RuleFor(x => x.LegendId).NotNull().GreaterThan(0).LessThanOrEqualTo(7);
        }
    }
}
