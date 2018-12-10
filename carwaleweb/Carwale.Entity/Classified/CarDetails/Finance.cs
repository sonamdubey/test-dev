using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified.CarDetails
{
    [Serializable]
    public class Finance
    {
        public bool IsEligibleForFinance { get; set; }
        public decimal? Emi { get; set; }
        public string EmiFormatted { get; set; }
        public string FinanceUrl { get; set; }
        public string FinanceUrlText { get; set; }
    }
}
