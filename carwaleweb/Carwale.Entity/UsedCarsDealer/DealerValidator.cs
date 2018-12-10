using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.UsedCarsDealer
{

    public class DealerInfoValidator : AbstractValidator<DealerInfo>
    {
        public DealerInfoValidator()
        {
            RuleFor(x => x.dealerId).GreaterThan(0);

        }
    }

  
}
