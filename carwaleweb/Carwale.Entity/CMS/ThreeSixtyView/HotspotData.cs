using System;
using System.Collections.Generic;

namespace Carwale.Entity.CMS
{
    [Serializable]
    public class HotspotData
    {
        public int TotalImages { get; set; }
        public string ImageVersion { get; set; }
        public Dictionary<int, Hotspot> Hotspots { get; set; }
        public Dictionary<int, List<Hotspot>> HotspotPositions { get; set; }
    }
}
