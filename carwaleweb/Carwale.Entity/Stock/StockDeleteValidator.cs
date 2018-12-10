using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockDeleteValidator : AbstractValidator<StockDelete>
    {
        public StockDeleteValidator()
        {
            RuleFor(x => x.SellerId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.SellerType).NotEmpty().InclusiveBetween(1, 2);
            RuleFor(x => x.SourceId).NotEmpty().InclusiveBetween(1, 2);
            RuleFor(x => x.Ids).NotEmpty();
        }
    }
}
