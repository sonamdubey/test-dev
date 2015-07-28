using Consumer;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Bikewale.BookingSMS
{
    class Program
    {
        static void Main(string[] args)
        {
            Logs.WriteInfoLog("Started the Booking Offer SMS Job");
            bool isOfferAvailable = false;

            CustomSMS obj = new CustomSMS();
            try
            {
                isOfferAvailable = Convert.ToBoolean(ConfigurationManager.AppSettings["isOfferAvailable"]);
                if (isOfferAvailable)
                    obj.SendSMS();
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in Main : " + ex.Message);
            }           

            Logs.WriteInfoLog("Ended the Booking Offer SMS Job");
        }        
    }
}
