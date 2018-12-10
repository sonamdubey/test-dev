using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Security.Principal;
using System.Net.Mail;
using System.IO;
using System.Xml;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;

namespace MobileWeb.DataLayer 
{
	//this class is inheriting from Parent class
	public class Users : Parent
	{
		//this function function checks handle name existance
		public void CheckUserHandle(string userId)
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("GetExistingHandleDetails_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_userId", DbType.Int64, userId));
                LoadDataMySql(cmd,DbConnections.CarDataMySqlReadConnection);
            }
		}
		
		//this function function checks handle name availability
		public void HandleAvailable(string handleName)
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("CheckHandleAvailable_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_HandleName", DbType.String, handleName));
                LoadDataMySql(cmd, DbConnections.CarDataMySqlReadConnection);
            }
		}
	}
}		
		