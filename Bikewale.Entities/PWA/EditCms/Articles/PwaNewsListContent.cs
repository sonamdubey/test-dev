using Bikewale.Entities.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.Entities.PWA.Articles
{
    public class PwaNewsListContent
    {
        public CMSContent Articles { get; set; }
        public IHtmlString ServerRouterWrapper { get; set; }
        public IHtmlString WindowState { get; set; }
    }
}
