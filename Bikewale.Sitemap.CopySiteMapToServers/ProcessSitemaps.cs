using Consumer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Bikewale.Sitemap.CopySiteMapToServers
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : Class have logic to copy sitemaps to different servers and submit sitemaps to google for indexing
    /// </summary>
    public class ProcessSitemaps
    {
        private static string SitemapeWebsiteDest { get; set; }
        private static string SitemapSourceFileSource { get; set; }

        /// <summary>
        /// Written By : Ashish G. Kamble on 31 Oct 2017
        /// Summary : Tuple<SitemapType, string, string> -> Item1- sitemaptype, Item2 - src_content_path, Item3 - dest_content_path
        /// </summary>
        private static ICollection<Tuple<SitemapType, string, string, string>> SitemapLocation;

        /// <summary>
        /// Written By : Ashish G. Kamble on 31 Oct 2017
        /// Enum to identify the type of sitemap need to process.
        /// </summary>
        public enum SitemapType
        {
            General = 1,
            PriceInCity = 2,
            UsedBikes = 3,
            DealerLocator = 4,
            ServiceCenter = 5
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 31 Oct 2017
        /// Summary : Constructor to initialize the variables
        /// </summary>
        static ProcessSitemaps()
        {
            try
            {
                var config = ConfigurationManager.GetSection("sitemapLocations") as SitemapConfigSection;

                if (config != null && config.Instances != null)
                {
                    SitemapLocation = new List<Tuple<SitemapType, string, string, string>>();
                    foreach (var e in config.Instances)
                    {
                        var ele = (e as SitemapLocationConfigElement);
                        SitemapLocation.Add(new Tuple<SitemapType, string, string, string>(ele.SitemapType, ele.SourcePath, ele.DestinationPath, ele.WebSitemapFolder));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }   // End of ProcessSitemaps method


        #region Code to copy sitemaps to all servers
        /// <summary>
        /// Written By : Ashish G. Kamble on 31 Oct 2017
        /// Summary : Function to copy the sitemaps to the specified servers
        /// </summary>
        public static void CopyToServers(SitemapType type)
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            try
            {

                // Get the location of batch file which uploads sitemaps to the servers
                var batchFileLoc = System.Configuration.ConfigurationManager.AppSettings["sitemap_batch_file_location"];
                Logs.WriteInfoLog(String.Format("Get the location of batch file which uploads sitemaps to the servers : {0}", batchFileLoc));
                // Get the source and destination of the sitemap to be copied
                var sitemapLocation = SitemapLocation.FirstOrDefault(m => m.Item1 == type);

                // Set Sitemap source destination
                SitemapSourceFileSource = sitemapLocation.Item2;
                SitemapeWebsiteDest = sitemapLocation.Item4;

                Logs.WriteInfoLog("SitemapSourceFileSource : " + SitemapSourceFileSource);
                Logs.WriteInfoLog("SitemapeWebsiteDest : " + SitemapeWebsiteDest);

                processInfo = new ProcessStartInfo(batchFileLoc, sitemapLocation.Item2 + " " + sitemapLocation.Item3);

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

                Logs.WriteInfoLog("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
                Logs.WriteInfoLog("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
                Logs.WriteInfoLog("ExitCode: " + exitCode.ToString());
                process.Close();
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(ex.Message);
            }
        }   // End of CopyToServers method 
        #endregion


        #region Code to submit sitemap to google
        /// <summary>
        /// Written By : Ashish G. Kamble on 31 Oct 2017
        /// Summary : Function to submit sitemap to the google for indexing
        /// </summary>
        public static void SubmitSitemapToGoogle()
        {
            try
            {
                string googleAPIKey = System.Configuration.ConfigurationManager.AppSettings["GoogleAPIConsoleApiKey"];

                string feedPath = string.Empty;
                ICollection<string> googleAPIURLs = new List<string>();

                //Get list of all URLs of sitemaps in the given sitemap folder which is currently being processed
                var filesList = Directory.GetFiles(SitemapSourceFileSource, "*.xml");

                // Process srouce files list and get file names and form google api url
                if (filesList != null)
                {
                    foreach (var fileName in filesList)
                    {
                        feedPath = string.Format(SitemapeWebsiteDest + Path.GetFileName(fileName));

                        if (!String.IsNullOrEmpty(feedPath))
                        {
                            googleAPIURLs.Add(String.Format("https://www.googleapis.com/webmasters/v3/sites/{0}/sitemaps/{1}/?key={2}", HttpUtility.UrlEncode("https://www.bikewale.com"), HttpUtility.UrlEncode(feedPath), googleAPIKey));
                        }
                    }
                }

                // Iterate through each url and submit sitemaps to google for indexing
                if (googleAPIURLs != null)
                {
                    var init = new Google.Apis.Services.BaseClientService.Initializer() { ApiKey = "AIzaSyBTZhbAbKgE_VA_RXq-__tzZkgrOa2HcDI" };


                    //var service = new SitemapsResource.SubmitRequest(, "", "");                    

                    //var service = new Google.Apis.SearchConsole.v1.SearchConsoleService( new Google.Apis.Services.BaseClientService.Initializer {
                    //    ApiKey = "AIzaSyBTZhbAbKgE_VA_RXq-__tzZkgrOa2HcDI",
                    //    ApplicationName = "bw-search-console-api-key"
                    //} );

                    //var service = new Google.Apis.web

                    //var result = service.UrlTestingTools.
                    //using (HttpClient httpClient = new HttpClient())
                    //{
                    //    if (httpClient != null)
                    //    {
                    //        foreach (var apiUrl in googleAPIURLs)
                    //        {
                    //            var response = httpClient.PutAsync(apiUrl, null).Result;

                    //            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    //            {
                    //                // success submitting the sitemap to google
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
        #endregion


    #region Section to read the sitemap locations from config file
    /// <summary>
    /// Summary : Class to define the custom section for sitemap
    /// </summary>
    public class SitemapConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public SitemapConfigInstanceCollection Instances
        {
            get { return (SitemapConfigInstanceCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    /// <summary>
    /// Summary : Create collection of sitemap location config element
    /// </summary>
    public class SitemapConfigInstanceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SitemapLocationConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //set to whatever Element Property you want to use for a key
            return ((SitemapLocationConfigElement)element).SitemapType;
        }
    }

    /// <summary>
    /// Summary : Class to define custom elements in the app.config
    /// </summary>
    public class SitemapLocationConfigElement : ConfigurationElement
    {
        //Make sure to set IsKey=true for property exposed as the GetElementKey above
        [ConfigurationProperty("sitemapType", IsKey = true, IsRequired = true)]
        public ProcessSitemaps.SitemapType SitemapType
        {
            get { return (ProcessSitemaps.SitemapType)base["sitemapType"]; }
            set { base["sitemapType"] = value; }
        }

        [ConfigurationProperty("source", IsRequired = true)]
        public string SourcePath
        {
            get { return (string)base["source"]; }
            set { base["source"] = value; }
        }

        [ConfigurationProperty("destination", IsRequired = true)]
        public string DestinationPath
        {
            get { return (string)base["destination"]; }
            set { base["destination"] = value; }
        }

        [ConfigurationProperty("webSitemapFolder", IsRequired = true)]
        public string WebSitemapFolder
        {
            get { return (string)base["webSitemapFolder"]; }
            set { base["webSitemapFolder"] = value; }
        }
    }
    #endregion
}
