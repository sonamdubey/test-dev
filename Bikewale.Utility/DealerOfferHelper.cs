using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    public class DealerOfferHelper
    {
        public static bool HasFreeInsurance(string dealerId, string modelId, string categoryName, UInt32 categoryValue, ref UInt32 insuranceValue)
        {
            bool retVal = false;            
            string[] dealers = null;
            NameValueCollection nvc = null;
            try
            {
                nvc = ConfigurationManager.GetSection("dealerInsurance") as NameValueCollection;
                if (nvc != null && nvc.HasKeys())
                {
                    dealers = nvc.AllKeys;
                }
                if (dealers.Contains(dealerId))
                {                    
                    if (categoryName.ToUpper().Contains("INSURANCE"))
                    {
                        retVal = true;
                    }
                }
            }
            catch (Exception)
            {

            }
            if (retVal)
            {
                insuranceValue = categoryValue;
            }
            else
            {
                if (insuranceValue == 0)
                    insuranceValue = 0;
            }
            return retVal;
        }
    }
}
