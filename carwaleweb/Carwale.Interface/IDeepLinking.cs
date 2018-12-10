using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.DeepLinking;
using System.Collections.Specialized;
using Carwale.DTO.DeepLinking;

namespace Carwale.Interfaces
{
    public interface IDeepLinking
    {
        DeepLinkingEntity GetLinkToApp(string siteUrl);
        DeepLinkingDTO GetLinkToAppV2(string queryString);
        
    }
}
