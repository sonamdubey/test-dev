using Bikewale.Entities;
using Bikewale.Interfaces.App;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.App
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 05-May-2017
    ///  BAL for splash screen
    /// </summary>
    /// <seealso cref="Bikewale.Interfaces.App.ISplashScreen" />
    public class SplashScreen : ISplashScreen
    {
        private readonly ISplashScreenCacheRepository _splashCache;
        public SplashScreen(ISplashScreenCacheRepository splashScreen)
        {
            _splashCache = splashScreen;
        }
        public SplashScreenEntity GetAppSplashScreen()
        {
            SplashScreenEntity result = null;
            IEnumerable<SplashScreenEntity> objChachedData = null;
            try
            {
                objChachedData = _splashCache.GetAppSplashScreen();
                if (objChachedData != null)
                {
                    int count = objChachedData.Count();
                    if (count == 1)
                    {
                        result = objChachedData.First();
                    }
                    else if (count > 1)
                    {
                        result = objChachedData.Skip(new Random().Next(0, count)).First();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.App.SplashScreen.GetAppSplashScreen()");
            }
            return result;
        }
    }
}
