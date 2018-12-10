using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.ThreeSixtyView;
using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.Interfaces.CMS
{
    public interface IThreeSixtyCache
    {
        HotspotData GetHotspots(int modelId, ThreeSixtyViewCategory type);
        Dictionary<string, List<Hotspot>> GetHotspotDetails(int modelId, ThreeSixtyViewCategory category);
        Dictionary<string, Dictionary<string, List<Hotspot>>> MultiGetHotspots(int modelId);
        Dictionary<string, HotspotData> MultiGetXmlVersions(int modelId);
    }
}
