using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.URIs.CarData
{
    public class CarVersionsURI
    {
        public string type { get; set; }
        public int modelId { get; set; }
        public int cityId { get; set; }
        public UInt16? year { get; set; }
    }
}
