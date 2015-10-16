using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class Features
    {
        public List<Specs> FeaturesList { get; set; }
        public string DisplayName { get; set; }
    }
}
