using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.ThreeSixtyView;
using Carwale.Entity.Enum;

namespace Carwale.Interfaces.CMS
{
    public interface IThreeSixtyView
    {
        ThreeSixty GetExterior360Config(int modelId, ThreeSixtyViewCategory category, string qualityFactor, int imageCount);
        ThreeSixty GetInterior360Config(int modelId, string qualityFactor);
        string GetExterior360XML(int modelId, ThreeSixtyViewCategory type, bool getHotspots, bool isMsite, int imageCount = 72, int qualityFactor = 80);
        string GetInterior360XML(int modelId, bool getHotspots, bool isMsite, int qualityFactor = 80);
    }
}
