using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData
{
    [Serializable]
    public class Overview
    {
        public List<Specs> OverviewList { get; set; }
        public string DisplayName { get; set; }
    }
}
