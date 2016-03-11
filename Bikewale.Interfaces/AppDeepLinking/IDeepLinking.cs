using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.AppDeepLinking;

namespace Bikewale.Interfaces.AppDeepLinking
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 10 March 2016
    /// </summary>
    public interface IDeepLinking
    {
      DeepLinkingEntity GetParameters(string url);
    }
}
