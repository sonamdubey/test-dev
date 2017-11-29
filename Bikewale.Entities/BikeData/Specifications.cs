using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class Specifications
    {
        public List<SpecsCategory> SpecsCategory { get; set; }
        public string DisplayName { get; set; }
    }
}
