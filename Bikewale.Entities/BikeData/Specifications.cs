using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class Specifications
    {
        public List<SpecsCategory> SpecsCategory { get; set; }
        public string DisplayName { get; set; }
    }
}
