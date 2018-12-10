/*
	This class will use to bind controls like filling makes, states
	Written by: Satish Sharma On Jan 21, 2008 12:28 PM
*/

using Carwale.DAL.CoreDAL;
using Carwale.Entity.AutoComplete;
using Carwale.Entity.Enum;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.UI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Carwale.UI.Common
{
    /*
        This class will contain all the common functions used in carwale		
    */
    public class CWCommon
    {
        public bool IsSearchEngine()
        {
            bool ret = false;
            try
            {
                if (HttpContext.Current.Request.Browser.Crawler)
                    ret = true;
            }
            catch (Exception err)
            {
                Logger.LogException(err, "IsSearchEngine");                
            }
            return ret;
        }        
    }//class
}//namespace
