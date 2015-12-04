using Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceQuoteLeadSMS
{
    class LeadSMS
    {
        internal void SendSMS()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendSMS Method : " + ex.Message);
            }
        }
    }
}
