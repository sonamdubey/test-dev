using System;
using System.Configuration;

namespace Bikewale.Notifications.Configuration
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 3 Mar 2014
    /// Summary : Class to return the properties to be required for configuring the mail.
    /// </summary>
    class MailConfiguration
    {
        public static string SMTPSERVER { get { return ConfigurationManager.AppSettings["SMTPSERVER"]; } }
        public static string LOCALMAIL { get { return ConfigurationManager.AppSettings["localMail"]; } }
        public static string MAILFROM { get { return ConfigurationManager.AppSettings["MailFrom"]; } }
        public static string REPLYTO { get { return ConfigurationManager.AppSettings["ReplyTo"]; } }
    }
}
