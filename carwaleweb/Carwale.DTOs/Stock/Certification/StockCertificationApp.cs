using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Stock.Certification
{
    public class StockCertificationApp
    {
        public string OverallScore { get; set; }
        public string OverallCondition { get; set; }
        public string OverallScoreColor { get; set; }
        public int MaxScore { get; set; }
        public string ImageHostUrl { get; set; }
        public string ExteriorOriginalImgPath { get; set; }
        public List<StockCertificationItemApp> Description { get; set; }
    }
}
