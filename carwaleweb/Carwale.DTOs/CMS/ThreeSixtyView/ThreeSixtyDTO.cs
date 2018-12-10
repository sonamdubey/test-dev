using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.DTOs.CMS.ThreeSixtyView
{
    public class ThreeSixtyDTO
    {
        public string ActiveState { get; set; }
        public CarModelDetails ModelDetails { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public List<ModelImage> ModelImagesList { get; set; }
        public List<Video> ModelVideos { get; set; }
        public List<CarMakeEntityBase> ThreeSixtyMakes { get; set; }
        public Dictionary<string, List<Hotspot>> ExteriorHotspots { get; set; }
        public Dictionary<string, List<Hotspot>> OpenHotspots { get; set; }
        public Dictionary<string, List<Hotspot>> InteriorHotspots { get; set; }
        public ThreeSixtyViewCategory Category { get; set;}
        public Dictionary<ThreeSixtyViewCategory, string> XmlVersion { get; set; }
    }
}
