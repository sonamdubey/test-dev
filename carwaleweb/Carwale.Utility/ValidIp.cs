using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Carwale.Utility
{
   public class ValidIp
    {
        public static bool IsIPValid()
        {
            bool isIPValid = false;

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];	//changed for the new setup
                isIPValid = ConfigurationManager.AppSettings["CarWaleIP"].Contains(ipAddress);
            }
            return isIPValid;
        }
    }
}
