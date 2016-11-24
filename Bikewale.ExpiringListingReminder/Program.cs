

using Consumer;
using System;
namespace Bikewale.ExpiringListingReminder
{
    class Program
    {
        static void Main(string[] args)
        {
            Logs.WriteInfoLog("Started the Expiring Listing Reminder Job");

            try
            {
                Logs.WriteInfoLog("Started the NotifySellerAboutListingExpiry function");
                (new NotifySellerListingExpiry()).NotifySellerAboutListingExpiry();
                Logs.WriteInfoLog("Ended the NotifySellerAboutListingExpiry function");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in Main : " + ex.Message);
            }

            Logs.WriteInfoLog("Ended the Expiring Listing Reminder Job");
        }
    }
}
