using RabbitMqPublishing.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Utility
{
    /// <summary>
    /// Createb By : Lucky Rathore
    /// Created On : 28 Jan 2016
    /// Description : To provide utility funtions for Dealer call assitant.
    /// </summary>
    public class DealerAssistance
    {
        /// <summary>
        /// Createb By : Lucky Rathore
        /// Created On : 28 Jan 2016
        /// Description : Verfiy Dealer ID and Timing to show Telephone number correspods to Dealer.
        /// </summary>
        /// <param name="dealerId">Dealer Id as String</param>
        /// <returns>true if verify mention Description.</returns>
        public bool IsDealerAssistance(string dealerId){
            if (!String.IsNullOrEmpty(dealerId))
            {
                NameValueCollection nvc = null;
                string[] dealerIds = null;
                try
                {
                    nvc = ConfigurationManager.GetSection("dealerNumber") as NameValueCollection;
                    if (nvc != null && nvc.HasKeys())
                    {
                        dealerIds = nvc.AllKeys;
                        if (dealerIds.Contains(dealerId))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 0 && DateTime.Now.Hour >= 10 && DateTime.Now.Hour <= 18) return true;
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, String.Format("Exception : Bikewale.Utility.BWHttpClient.DealerAssistance.IsDealerAssistance)"));
                    objErr.SendMail();
                }
            }
            return false;
        }
       
    }
}
