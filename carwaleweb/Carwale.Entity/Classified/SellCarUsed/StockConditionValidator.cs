using Carwale.Entity.Stock;
using Carwale.Utility;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class StockConditionValidator : AbstractValidator<StockCondition>
    {
        public StockConditionValidator()
        {
            RuleFor(x => x.AccidentDetails).Length(0, 200);
            RuleFor(x => x.PartReplacedText).Length(0, 50);
            RuleFor(x => x.MinorScratchText).Length(0, 50);
            RuleFor(x => x.DentedPartText).Length(0, 50);
            RuleFor(x => x.EngineIssueText).Length(0, 50);
            RuleFor(x => x.ElectricIssueText).Length(0, 50);
        }
    }
}
