using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Web.Security;
using System.Security.Principal;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace Carwale.UI.Common
{
    public class CommonDealers
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;             

        //Function to check dealer is authorised to access this page or not
        //this function will retuen true if user authorised else false
        public static bool IsValidUsedDealer(string dealerId)
        {

            string sql = @" SELECT CWDealerId FROM cwmasterdb.ct_addonpackages AS Ad, cwmasterdb.dealers AS Ds WHERE Ad.CWDealerId = @DealerId AND Ad.IsActive = 1 AND Ad.CWDealerId = Ds.Id AND Ds.Status = 0 AND Ad.AddOnPackageId = 100";
            
            DbCommand cmd = DbFactory.GetDBCommand(sql);
            int intDealerId;
            Int32.TryParse(dealerId, out intDealerId);

            cmd.Parameters.Add(DbFactory.GetDbParam("@DealerId", DbType.Int32,intDealerId));
            try
            {
                using (var dr = MySqlDatabase.SelectQuery(cmd, DbConnections.ClassifiedMySqlReadConnection))
                {
                    if (dr.Read())
                    {
                        return (!string.IsNullOrEmpty(dr[0].ToString()));                            
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Mysql - IsValidUsedDealer()");
                objErr.SendMail();
            }

            return false;
        }

        public static string GetFormattedAddress(string address1, string address2, string state, string city, string area, string pin)
        {
            StringBuilder completeAddress = new StringBuilder();

            if (address1.Trim().Length > 0)
            {
                completeAddress.Append(address1);
                completeAddress.Append(", ");
            }

            if (address2.Trim().Length > 0)
            {
                completeAddress.Append(address2);
                completeAddress.Append(", ");
            }

            if (area.Trim().Length > 0)
            {
                completeAddress.Append(area);
                completeAddress.Append(", ");
            }

            if (city.Trim().Length > 0)
            {
                completeAddress.Append(city);
            }

            if (state.Trim().Length > 0)
            {
                completeAddress.Append(", ");
                completeAddress.Append(state);
                completeAddress.Append(" ");
            }

            if (pin.Trim().Length > 0)
            {
                completeAddress.Append("- ");
                completeAddress.Append(pin);
            }

            return completeAddress.ToString();
        } // FormattedAddress
    }
}