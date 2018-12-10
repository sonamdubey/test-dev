using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    public class CarDataPresentation
    {
        public List<CarData> Specifications { get; set; }
        public List<CarData> Features { get; set; }
        public List<CategoryItem> Overview { get; set; }
    }
}
