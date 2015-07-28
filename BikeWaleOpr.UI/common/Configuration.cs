using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;

namespace BikeWaleOpr.Common
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
    }
}