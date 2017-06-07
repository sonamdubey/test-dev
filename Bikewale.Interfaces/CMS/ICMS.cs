using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.CMS
{
    public interface ICMS
    {
        string GetArticleDetailsPage(uint basicId);
        string GetArticleDetailsPages(uint basicId);
    }
}
