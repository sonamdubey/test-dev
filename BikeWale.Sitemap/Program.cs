using Consumer;
using System;
using System.Configuration;
using System.Diagnostics;

namespace BikeWale.Sitemap
{
    class Program
    {
        static void Main(string[] args)
        {
            UsedBikeSiteMap siteMap = new UsedBikeSiteMap();
            try
            {
                if (siteMap.GenerateSiteMap())
                {
                    CopyToServers();
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception " + ex.Message);
            }
        }
        static void CopyToServers()
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;
            Logs.WriteInfoLog("Started : CopyToServers");
            try
            {
                // Get the location of batch file which uploads sitemaps to the servers
                var copyToServerUtilLocation = ConfigurationManager.AppSettings["copyToServerUtilLocation"];
                var sitemapType = ConfigurationManager.AppSettings["SitemapType"];
                if (!String.IsNullOrEmpty(copyToServerUtilLocation) && !String.IsNullOrEmpty(sitemapType))
                {
                    processInfo = new ProcessStartInfo(copyToServerUtilLocation, sitemapType);

                    processInfo.CreateNoWindow = true;
                    processInfo.UseShellExecute = false;
                    // *** Redirect the output ***
                    processInfo.RedirectStandardError = true;
                    processInfo.RedirectStandardOutput = true;

                    process = Process.Start(processInfo);
                    process.WaitForExit();

                    // *** Read the streams ***
                    // Warning: This approach can lead to deadlocks, see Edit #2
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    exitCode = process.ExitCode;

                    Logs.WriteInfoLog(String.Format("output>> {0}", (String.IsNullOrEmpty(output) ? "(none)" : output)));
                    Logs.WriteInfoLog(String.Format("error>> {0}", (String.IsNullOrEmpty(error) ? "(none)" : error)));
                    Logs.WriteInfoLog(String.Format("ExitCode: {0}", exitCode.ToString()));
                    process.Close();
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(ex.Message);
            }
            finally
            {
                Logs.WriteInfoLog("Finished : CopyToServers");
            }
        }
    }
}
