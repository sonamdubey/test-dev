using Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceQuoteLeadSMS
{
    class Program
    {
        static void Main(string[] args)
        {
            Logs.WriteInfoLog("Started the Price quote Lead SMS Job");

            try
            {
                LeadSMS obj = new LeadSMS();

                obj.SendSMS();
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in Main : " + ex.Message);
            }

            Logs.WriteInfoLog("Ended the Price quote Lead SMS Job");
        }
    }
}
