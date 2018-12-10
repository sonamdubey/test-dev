using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carwale.UI.Common
{
    public class OutputCacheAttr : OutputCacheAttribute
    {
        private static int _sysduration = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OutputCacheDuration"] ?? "180");
        private static bool activeSysDuration = _sysduration > 0;
        public OutputCacheAttr(string VaryByParamInput, int _duration = 0)
        {
            bool activeParamDuration = _duration > 0;
            if (activeSysDuration)
            {
                VaryByParam = VaryByParamInput;
                Duration = activeParamDuration ? _duration : _sysduration;
            }
            else { Duration = 1; VaryByParam = "none"; Location = System.Web.UI.OutputCacheLocation.None; }
            //VaryByParam is none for no caching. comma seperated parameter names, for differentiating between cache objects
        }
    }
}