using Bikewale.Entities.App;

namespace Bikewale.Interfaces.App
{
    /// <summary>
    /// Created By  :  Sushil Kumar on 30th June 2016
    /// Description : Interface for cache of app version
    /// </summary>
    public interface IAppVersionCache
    {
        AppVersion CheckVersionStatus(uint appVersion, uint sourceId);
    }
}
