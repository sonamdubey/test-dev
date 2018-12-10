using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carwale.Entity.Stock.Certification
{
    public class StockCertificationItemValidator : AbstractValidator<StockCertificationItem>
    {
        public StockCertificationItemValidator()
        {
            RuleFor(x => x.CarItemId).NotNull().GreaterThan(0);
            When(x => x.Score != null, () => RuleFor(x => x.Score).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5));
            When(x => x.Score != null, () => RuleFor(x => x.ScoreColorId).NotNull().GreaterThan(0).LessThanOrEqualTo(3));
            When(x => x.Condition != null, () => RuleFor(x => x.Condition).Length(1, 50));
        }
    }
}
