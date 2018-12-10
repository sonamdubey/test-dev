using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Advertizings.Apps;
using Carwale.DTOs.Advertisment;

namespace Carwale.Interfaces.Advertizings.App
{
    public interface IAppSplashScreenBL
    {
        SplashScreenBanner GetRandomSplashScreen(int platformId, int applicationId);
        CustomSplashDTO GetSplashScreenByPriority(int platformId, int applicationId);
    }
}
