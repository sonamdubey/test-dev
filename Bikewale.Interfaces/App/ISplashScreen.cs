
using Bikewale.Entities;
using System.Collections.Generic;
namespace Bikewale.Interfaces.App
{
    /// <summary>
    /// Created By  :  Sangram Nandkhile on 05 May 2017
    /// Description : Interface for app splashscreen
    /// </summary>
    public interface ISplashScreenCacheRepository
    {
        IEnumerable<SplashScreenEntity> GetAppSplashScreen();
    }

    /// <summary>
    /// Created By  :  Sangram Nandkhile on 05 May 2017
    /// Description : Interface for DAL app splashscreen
    /// </summary>
    public interface ISplashScreenRepository
    {
        IEnumerable<SplashScreenEntity> GetAppSplashScreen();
    }
}
