using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockImagesValidator : AbstractValidator<StockImageList>
    {
        public StockImagesValidator()
        {
            RuleFor(stockImages => stockImages.SellerType).NotNull().InclusiveBetween(1, 2);
            RuleFor(stockImages => stockImages.SourceId).NotNull().InclusiveBetween(1, 2);
            RuleFor(stockImages => stockImages.StockImages).NotEmpty();
        }
    }
}
