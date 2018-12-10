using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StocksValidator : AbstractValidator<StockList>
    {
        public StocksValidator()
        {
            RuleFor(x => x.SellerId).NotNull().GreaterThan(0);
            RuleFor(x => x.SellerType).NotNull().InclusiveBetween(1, 2);
            RuleFor(x => x.SourceId).NotNull().InclusiveBetween(1, 2);
            RuleFor(x => x.Stocks).NotEmpty();
        }
    }

   
}
