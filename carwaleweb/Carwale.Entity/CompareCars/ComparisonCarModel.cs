using Carwale.Entity.CarData;
using Carwale.Entity.PriceQuote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CompareCars
{
    [Serializable]
    public class ComparisonCarModel : CarEntity
    {
        public CarReviewBase Review { get; set; }
        public int Price { get; set; }
        public PriceOverview PriceOverview { get; set; }
    }

}
