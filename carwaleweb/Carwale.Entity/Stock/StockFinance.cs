using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Stock
{
    [Validator(typeof(StockFinanceValidator))]
    public class StockFinance
    {
        public string ProfileId { get; set; }
        public decimal? EmiAmount { get; set; }
        public bool? IsEligible { get; set; }
    }
}
