using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.CarDetails
{
    public class FinanceWeb
    {
        public bool IsEligibleForFinance { get; set; }
        public decimal? Emi { get; set; }
        public string EmiFormatted { get; set; }
        public string FinanceUrl { get; set; }
        public string FinanceUrlText { get; set; }
    }
}
