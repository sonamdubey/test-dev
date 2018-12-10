using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class AdvantageSearchFilterDTO
    {
        public string Manufacturers { get; set; }
        public string Fuels { get; set; }
        public string Transmissions { get; set; }
        public string BodyTypes { get; set; }
        public string BudgetRange { get; set; }
        public int SO { get; set; }
        public int SC { get; set; }
    }
}
