using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class Features
    {
        public List<Specs> FeaturesList { get; set; }
        public string DisplayName { get; set; }
    }
}
