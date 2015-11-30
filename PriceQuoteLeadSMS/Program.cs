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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Logs.WriteInfoLog("Started the Price quote Lead SMS Job");

            try
            {
                //After each 5 mins notify customer and dealer via mail and sms
                System.Timers.Timer timer = new System.Timers.Timer();

                timer.Start();
                timer.Interval = 500000;

                timer.Elapsed += SendLeadsToCustDealer;
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in Main : " + ex.Message);
            }

            Logs.WriteInfoLog("Ended the Price quote Lead SMS Job");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SendLeadsToCustDealer(object sender, System.Timers.ElapsedEventArgs e)
        {
            LeadSMS obj = new LeadSMS();

            obj.SendLeadsToCustDealer();
        }
    }
}
