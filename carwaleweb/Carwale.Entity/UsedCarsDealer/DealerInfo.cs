using Carwale.Entity.Stock;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.UsedCarsDealer
{
    [Validator(typeof(DealerInfoValidator))]
    public class DealerInfo
    {
        public int dealerId { get; set; }
    }

}
