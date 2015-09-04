using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public static class UrlFormatter
    {
        public static string BikePageUrl(string makeMaskingName,string modelMaskingName)
        {
            return String.Format("/{0}-bikes/{1}",makeMaskingName,modelMaskingName);
        }
    }
}
