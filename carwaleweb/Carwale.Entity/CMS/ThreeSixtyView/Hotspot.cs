using System;

namespace Carwale.Entity.CMS
{
    [Serializable]
    public class Hotspot
    {
        public int HotspotXmlId { get; set; }
        public int CategoryId { get; set; }
        public int ImageId { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Pan { get; set; }
        public double Tilt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}
