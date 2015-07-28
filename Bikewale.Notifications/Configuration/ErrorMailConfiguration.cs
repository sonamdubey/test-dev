using System;
using System.Configuration;

namespace Bikewale.Notifications.Configuration
{
    /// <summary>
    /// Create By : Ashish G. Kamble on 3 Mar 2014
    /// Summary : class to return configuration for error mails.
    /// </summary>
    class ErrorMailConfiguration
    {
        public static string ERRORMAILTO { get { return ConfigurationManager.AppSettings["errorMailTo"]; } }
        public static string APPLICATIONNAME { get { return ConfigurationManager.AppSettings["ApplicationName"]; } }
    }
}