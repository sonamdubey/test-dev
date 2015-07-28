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
						sql = " Update Customers Set SourceId = @sourceId Where id = @id";
						break;
						
					case EnumTableType.CustomerSellInquiries :
						sql = " Update CustomerSellInquiries Set SourceId = @sourceId Where id = @id";
						break;
						
					case EnumTableType.PriceQuote :
						sql = " Update NewCarPurchaseInquiries Set SourceId = @sourceId Where id = @id";
						break;
						
					case EnumTableType.CustomerReviews :
						sql = " Update CustomerReviews Set SourceId = @sourceId Where id = @id";
						break;
						
					case EnumTableType.CustomerReviewsComments :
						sql = " Update CustomerReviewComments Set SourceId = @sourceId Where id = @id";
						break;
						
					default:
						break;
				}
				
				if(sql != "")
				{
					Database db = new Database();
					try
					{
						SqlParameter [] param ={new SqlParameter("@sourceId", sourceId), new SqlParameter("@id", id)};
						db.UpdateQry(sql, param);
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
