/*THIS CLASS HOLDS ALL TH EFUNCTION FOR BINDING GRID, FILLING DROPDOWN LIST AND OTHER SORTS OF
COMMON OPERATIONS.
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
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Mail;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Net;
using System.IO;
using System.Data.Common;
using MySql.CoreDAL;

namespace Bikewale.Common 
{
	public enum EnumTableType
	{
		Customers = 1,
		CustomerSellInquiries = 2,
		PriceQuote = 3,
		CustomerReviews = 4,
		CustomerReviewsComments = 5
	}
	
	//this class is for the forwarding of the leads
	public class SourceIdCommon
	{
        public static void UpdateSourceId(EnumTableType tbl, string id)
		{
			//get the soruce id from the web.config
			string sourceId = ConfigurationManager.AppSettings["SourceId"].ToString();
			
			if(sourceId != "1" && id != "")	//if source is not carwale then only proceed
			{
				//get the sql and then depending on the table update the sourceId
				string sql = "";
				
				switch(tbl)
				{
					case EnumTableType.Customers :
						sql = " update customers set sourceid = @sourceid where id = @id";
						break;
						
					case EnumTableType.CustomerSellInquiries :
						sql = " update customersellinquiries set sourceid = @sourceid where id = @id";
						break;
						
					case EnumTableType.PriceQuote :
						sql = " update newcarpurchaseinquiries set sourceid = @sourceid where id = @id";
						break;
						
					case EnumTableType.CustomerReviews :
						sql = " update customerreviews set sourceid = @sourceid where id = @id";
						break;
						
					case EnumTableType.CustomerReviewsComments :
						sql = " update customerreviewcomments set sourceid = @sourceid where id = @id";
						break;
						
					default:
						break;
				}
				
				if(sql != "")
				{
					try
					{
                        using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                        {
                            cmd.Parameters.Add(DbFactory.GetDbParam("@sourceid", DbType.Int32, sourceId));
                            cmd.Parameters.Add(DbFactory.GetDbParam("@id", DbType.Int32, id));

                            MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                        }
					}
					catch(Exception err)
					{
						HttpContext.Current.Trace.Warn("SourceIdCommon.UpdateSourceId : " + err.Message);
						ErrorClass objErr = new ErrorClass(err,"SourceIdCommon.UpdateSourceId");
						objErr.SendMail();
					}
				}
			}
		}
				
	}
	
}//namespace
