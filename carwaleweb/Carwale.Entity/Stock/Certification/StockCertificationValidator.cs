using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock.Certification
{
    public class StockCertificationValidator : AbstractValidator<StockCertification>
    {
        public StockCertificationValidator()
        {
            When(x => x.ReportUrl != null, () => RuleFor(x => x.ReportUrl).Length(5, 200).Matches(@"\.(?i)(pdf)$"));
            RuleFor(x => x.IsActive).NotNull();
            RuleFor(x => x.OverallScore).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
            RuleFor(x => x.OverallScoreColorId).NotNull().GreaterThan(0).LessThanOrEqualTo(3);
            RuleFor(x => x.OverallCondition).NotNull().Length(1, 50);
            RuleFor(x => x.CarExteriorImageUrl).NotEmpty().Matches(@"\.(?i)(jpg|png|gif|jpeg)(?:\?[^?]+)?$");
            RuleFor(x => x.Description).Must(HaveNonRepeatingCarItems).WithMessage("'Description' should not contain duplicate CarItemIds.");
        }

        private bool HaveNonRepeatingCarItems(StockCertification stockCert, List<StockCertificationItem> description)
        {
            if (description != null)
            {
                return !description.GroupBy(c => c.CarItemId).Any(g => g.Count() > 1);
            }
            return true;
        }
    }
}
