using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carwale.UI.Common
{
    public class DealerSponsoredCommon
    {
        public static bool OEMDealerIds(int Id)
        {
            string oemDealerIds = System.Configuration.ConfigurationManager.AppSettings["OEMDealerId"];

            List<int> oemDealers = Carwale.Utility.ExtensionMethods.ConvertStringToList<int>(oemDealerIds, ',');

            bool found = oemDealers.Where(p => p == Id).Count() > 0;

            return found;
        }
    }
}