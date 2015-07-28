using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

namespace Bikewale.Common
{
    /// <summary>
    /// For all the configuration of BikeWale
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Get name of the default city name.
        /// </summary>
        public static string GetDefaultCityName
        {
            get { return ConfigurationManager.AppSettings["defaultName"]; }            
        }

        /// <summary>
        /// Get id of the default city,
        /// </summary>
        public static string GetDefaultCityId
        {
            get { return ConfigurationManager.AppSettings["defaultCity"]; }
        }

        /// <summary>
        /// Get id of the default city,
        /// </summary>
        public static string GetImgHostURL
        {
            get { return ConfigurationManager.AppSettings["imgHostURL"]; }
        }

        public static bool IsLocal
        {
            get
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("bikewale.com") >= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// This property used to get obile source id
        /// </summary>
        public static string MobileSourceId
        {
            get
            {
                return (string)ConfigurationManager.AppSettings["mobileSourceId"];
            }
        }

        /// <summary>
        /// This property used to get source id
        /// </summary>
        public static string SourceId
        {
            get
            {
                return (string)ConfigurationManager.AppSettings["sourceId"];
            }
        }

    }   // class
}   // namespace