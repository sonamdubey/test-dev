using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.Certification
{
    public class StockCertificationItemApp
    {
        public int CarItemId { get; set; }
        public string Name { get; set; }
        public string PreviewImagePath { get; set; }
        public string DetailImagePath { get; set; }
        public string Score { get; set; }
        public string Condition { get; set; }
        public string ScoreColor { get; set; }
        public List<StockCertificationSubItemApp> SubItems { get; set; }
    }
}
