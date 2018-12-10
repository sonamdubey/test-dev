using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CompareCars
{
    [Serializable]
    public class HotCarComparison
    {
        public List<ComparisonCarModel> HotCars { get; set; }
        public CarImageBase Image { get; set; }
        public bool IsSponsored { get; set; }
        public string CompareUrl { get; set; }
        public int WidgetPage { get; set;}
    }
}
