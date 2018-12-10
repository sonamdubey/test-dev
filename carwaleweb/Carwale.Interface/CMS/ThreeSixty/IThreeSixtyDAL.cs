using Carwale.Entity.CMS;
using Carwale.Entity.Enum;
using System.Collections.Generic;

namespace Carwale.Interfaces.CMS
{
    public interface IThreeSixtyDal
    {
        HotspotData GetHotspots(int modelId, ThreeSixtyViewCategory type);
        Dictionary<string, List<Hotspot>> GetHotspotDetails(int modelId, ThreeSixtyViewCategory category);
    }
}
