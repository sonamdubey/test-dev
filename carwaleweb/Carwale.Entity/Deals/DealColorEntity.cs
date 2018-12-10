using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.CarData;

namespace Carwale.Entity.Deals
{
    public class DealColorEntity
    {
        public ModelColors ModelColorsEntity { get; set; }
        public CarImageBase CarImage { get; set; }
        public int CurrentYear { get; set; }
        public List<DealsStock> DealsStock { get; set; }
    }
}
