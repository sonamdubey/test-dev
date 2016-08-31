
using Consumer;
using System;
namespace Bikewale.DealerAreaCommuteDistance
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                Console.WriteLine("Processing...");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Logs.WriteInfoLog("Started at : " + DateTime.Now);
                CommuteDistanceBL bl = new CommuteDistanceBL();
                bl.UpdateCommuteDistances();
                watch.Stop();
                var elapsedMs = String.Format("Processing Time taken in ms : {0}", watch.Elapsed.Seconds);

                Console.WriteLine(elapsedMs);
                Logs.WriteInfoLog(elapsedMs);
                Console.WriteLine("Done!!!");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception " + ex.Message);
                Console.WriteLine("Exception " + ex.Message);
            }
            finally
            {
                Logs.WriteInfoLog("End at : " + DateTime.Now);
            }
        }
    }
}
