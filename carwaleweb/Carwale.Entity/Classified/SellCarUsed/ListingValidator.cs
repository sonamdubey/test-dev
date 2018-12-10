using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Carwale.Entity.Enum;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class ListingValidator : AbstractValidator<Listing>
    {
        public ListingValidator(StatusValidationType validationType)
        {
            RuleFor(x => x.SellerType).NotNull().IsInEnum().NotEqual(Carwale.Entity.Enum.SellerType.Dealer).WithMessage("Invalid seller type");
            RuleFor(x => x.StockSource).NotNull().IsInEnum();
            RuleFor(x => x.ListingDetails).NotNull();
            When(x => x.ListingDetails != null, () => RuleFor(x => x.ListingDetails).Must((x, d) => { return d.Count <= 1000; }));
            When(x => x.ListingDetails != null && x.ListingDetails.Count <= 1000, () =>
                RuleForEach(x => x.ListingDetails).SetValidator(new ListingStatusValidator(validationType)));
        }
    }
}
