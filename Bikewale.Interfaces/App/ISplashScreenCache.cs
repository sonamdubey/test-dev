using Bikewale.Entities;

namespace Bikewale.Interfaces.App
{
    /// <summary>
    /// Created By  :  Sangram Nandkhile on 05 May 2017
    /// Description : Interface for cache of app splashscreen
    /// </summary>
    public interface ISplashScreen
    {
        SplashScreenEntity GetAppSplashScreen();
    }
}
