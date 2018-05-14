using Bikewale.Entities.PWA.Articles;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Models.Shared
{
    public class PwaBaseVM : ModelBase
    {
        public PwaReduxStore ReduxStore { get; set; }
        public IHtmlString ServerRouterWrapper { get; set; }
        public string WindowState { get; set; }
    }
}
