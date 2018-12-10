using System;
using System.Collections.Generic;

namespace Carwale.Entity.CMS.ThreeSixtyView
{
    [Serializable]
    public class ThreeSixty
    {
        public string HostUrl { get; set; }
        public string PreviewImage { get; set; }
        public int StartIndex { get; set; }
        public ThreeSixtyCamera CameraAngle { get; set; }
        public List<string> ExteriorImages { get; set; }
        public Dictionary<string, ThreeSixtyViewImage> InteriorImages { get; set; }
    }
}
