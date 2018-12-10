using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Advertizings.Apps;

namespace Carwale.Interfaces.Advertizings.App
{
    public interface IAppSplashRepository
    {
        List<SplashScreenBanner> GetSpalshSreenBanner(int platformId,int applicationId);
    }
}
