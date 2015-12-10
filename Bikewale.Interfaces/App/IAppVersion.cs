using Bikewale.Entities.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
