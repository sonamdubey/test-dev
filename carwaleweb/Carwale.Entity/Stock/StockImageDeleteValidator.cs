using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockImageDeleteValidator : AbstractValidator<StockImageDelete>
    {
        public StockImageDeleteValidator()
        {
            RuleFor(stockImageDelete => stockImageDelete.SellerType).NotNull().InclusiveBetween(1, 2); ;
            RuleFor(stockImageDelete => stockImageDelete.SourceId).NotNull().InclusiveBetween(1, 2);
            RuleFor(stockImageDelete => stockImageDelete.Ids).NotEmpty();
        }
    }
}
