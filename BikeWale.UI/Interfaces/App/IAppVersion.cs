using Bikewale.Entities.App;

namespace Bikewale.Interfaces.App
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Description :   APP Version Interface
    /// Created On  :   07 Dec 2015
    /// </summary>
    public interface IAppVersion
    {
        AppVersion CheckVersionStatus(uint appVersion, uint sourceId);
    }
}
