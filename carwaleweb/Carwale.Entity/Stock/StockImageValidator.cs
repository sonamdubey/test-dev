using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockImageValidator : AbstractValidator<StockImage>
    {
        public StockImageValidator()
        {
            RuleFor(stockImage => stockImage.Id).NotNull().GreaterThan(0);
            RuleFor(stockImage => stockImage.Url).NotEmpty().Matches(@"\.(?i)(jpg|png|gif|jpeg)(?:\?[^?]+)?$");
            RuleFor(stockImage => stockImage.DefaultImg).NotNull();
            RuleFor(stockImage => stockImage.Title).Length(0, 100);
            RuleFor(stockImage => stockImage.AltText).Length(0, 200);
        }
    }
}
