using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    public class StockValidator : AbstractValidator<Stock>
    {
        public StockValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.EntryDate).GreaterThanOrEqualTo(new DateTime(1900, 1, 1)).NotEmpty();
            RuleFor(x => x.LastUpdated).GreaterThanOrEqualTo(new DateTime(1900, 1, 1)).NotEmpty();
            RuleFor(x => x.VersionId).NotNull().GreaterThan(0);
            RuleFor(x => x.Price).NotNull().GreaterThanOrEqualTo(10000);
            RuleFor(x => x.Kms).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.MfgDate).GreaterThanOrEqualTo(new DateTime(1900, 1, 1)).NotEmpty();
            RuleFor(x => x.OwnerType).NotEmpty();
            RuleFor(x => x.CarRegNo).Matches(@"^$|^[0-9a-zA-Z\s-]+$");
            RuleFor(x => x.Comments).Length(0, 2000);   
            RuleFor(x => x.InsuranceExpiry).GreaterThanOrEqualTo(new DateTime(1900, 1, 1));
            RuleFor(x => x.OwnerType).Matches(@"^(First Owner|Second Owner|Third Owner|Fourth Owner|More than 4 owners|UnRegistered Car)");
            RuleFor(x => x.Modifications).Length(0, 500);
            RuleFor(x => x.CertProgId).GreaterThan(0);
            RuleFor(x => x.VideoUrl).Length(0, 200);
        }
    }
}
