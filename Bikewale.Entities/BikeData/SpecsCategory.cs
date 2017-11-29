using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class SpecsCategory
    {
        public string CategoryName { get; set; }
        public string DisplayName { get; set; }
        public List<Specs> Specs { get; set; }
    }
}
