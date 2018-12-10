using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.Certification
{
    public class StockCertificationSubItemApp
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public string Condition { get; set; }
        public string ScoreColor { get; set; }
        public List<StockCertificationSubItemDetailApp> Details { get; set; }
    }
}
