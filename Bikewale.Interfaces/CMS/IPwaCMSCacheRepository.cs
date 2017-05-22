using Bikewale.Entities.PWA.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Pwa.CMS
{
    public interface IPwaCMSCacheRepository
    {
        PwaNewsListContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId);
    }
}
