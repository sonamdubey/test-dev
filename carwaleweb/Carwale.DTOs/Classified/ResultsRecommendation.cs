using Carwale.Entity.Classified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified
{
    public class ResultsRecommendation
    {
        public IList<StockBaseEntity> ResultsData { get; set; }
    }
}
