using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Advertizings.Apps;

namespace Carwale.Interfaces.Advertizings.App
{
    public interface IAppSplashCache
    {
        List<SplashScreenBanner> GetSplashSreenBanner(int platformId, int applicationId);
        void DeactivaSplashBanner(int platformId, int applicationId);
    }
}
