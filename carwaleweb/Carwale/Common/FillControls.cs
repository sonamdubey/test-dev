/*
	This class will use to bind controls like filling makes, states
	Written by: Satish Sharma On Jan 21, 2008 12:28 PM
*/

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Carwale.UI.Common;
using Carwale.Notifications;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Common
{
    public class FillControls
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
     
        // Fill all the makes who's price quote available
        // Written by: Satish Sharma On Jan 21, 2008 12:28 PM
        public static void FillStates(DropDownList drpStates)
        {
            CommonOpn op = new CommonOpn();

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("cwmasterdb.GetAllStates_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    op.FillDropDownMysql(cmd, drpStates, "StateName", "StateId",DbConnections.CarDataMySqlReadConnection);
                }
                ListItem item = new ListItem("--Select State--", "0");
                drpStates.Items.Insert(0, item);
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }
        
    }//class
}//namespace
