using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
using Ajax;
using System.IO;

namespace BikeWaleOpr.EditCms
{
	public class Default : Page
	{	
		//server controls
		protected DropDownList ddlCategory, ddlPeriod;
		protected DataGrid dgArticles;
		protected Button btnGo;
		protected DateControl dtDate;
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnGo.Click += new EventHandler( btnGo_Click );
			dgArticles.PageIndexChanged += new DataGridPageChangedEventHandler(Page_Change);
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			if (!Page.IsPostBack)
			{
                //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
                //    Response.Redirect("../users/Login.aspx?ReturnUrl=../editcms/default.aspx");
				
                //if ( Request.Cookies["Customer"] == null )
                //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../editcms/default.aspx");
					
                //int pageId = 53;
                //CommonOpn op = new CommonOpn();
                //if ( !op.verifyPrivilege( pageId ) )
                //    Response.Redirect("../NotAuthorized.aspx");
			
				LoadCategory();
				LoadBasicData("","");
			}	
		}
		
		private void LoadCategory()
		{
			string sql = "SELECT Id, Name FROM Con_EditCms_Category WHERE IsActive = 1 ORDER BY NAME";
		
			CommonOpn op = new CommonOpn();
			try
			{
				op.FillDropDown(sql, ddlCategory, "Name", "ID");
			}
			catch(SqlException err)
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();				
			} // catch Exception
			
			ListItem item = new ListItem("--Category--", "");
			ddlCategory.Items.Insert(0, item);
		}
		
		void btnGo_Click( object Sender, EventArgs e )
		{
			//DateTime date = dtDate.Value;
			string period = ddlPeriod.SelectedItem.Value;
			string categoryId = ddlCategory.SelectedItem.Value;
			LoadBasicData(period, categoryId);
		}
		
		void Page_Change(object sender,DataGridPageChangedEventArgs e)
		{
			// Set CurrentPageIndex to the page the user clicked.
			//DateTime date = dtDate.Value;
			string period = ddlPeriod.SelectedItem.Value;
			string categoryId = ddlCategory.SelectedItem.Value;
			dgArticles.CurrentPageIndex = e.NewPageIndex;
			LoadBasicData(period, categoryId);
		}
		
		private void LoadBasicData(string period, string categoryId)
		{
			string sql = string.Empty;
			
			sql = " Select Id, DisplayDate, Title, CategoryId,"
				+ " (Select Name From Con_EditCms_Category Where Id = CategoryId ) As CatName, "
				+ " (Case IsPublished  When 0 Then 'No' Else 'Yes' End) As Published "
				+ " From Con_EditCms_Basic Where IsActive = 1 ";
			if( categoryId != string.Empty )
			{
				sql += " And CategoryId = " + categoryId;
			}	
			if( period != string.Empty )
			{
				switch( period )
				{
					case "TODAY": 
						//sql += " AND DisplayDate = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
						sql += " And Convert(VarChar, DisplayDate, 103) = Convert(VarChar, GetDate(), 103) ";
						break;
					case "YESTERDAY":
						//sql += " AND DisplayDate = '" + DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd") + "'";
						sql += " And Convert(VarChar, DisplayDate, 103) = Convert(VarChar, GetDate()-1, 103) ";
						break;
					case "L7D":
						//sql += " AND DisplayDate BETWEEN '" + DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + "' AND '" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
						//sql += " And Convert(VarChar, DisplayDate, 103) Between  Convert(VarChar, GetDate()-7, 103) And Convert(VarChar, GetDate(), 103) ";
						sql += " And Convert(DateTime, Convert(VarChar, DisplayDate, 103), 103) Between Convert(DateTime, Convert(VarChar, GetDate()-6, 103), 103) And Convert(DateTime, Convert(VarChar, GetDate(), 103), 103) ";
						break;
					case "L30D":
						//sql += " AND DisplayDate BETWEEN '" + DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd") + "' AND '" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
						//sql += " And Convert(VarChar, DisplayDate, 103) Between Convert(VarChar, GetDate()-30, 103) And Convert(VarChar, GetDate(), 103) ";
						sql += " And Convert(DateTime, Convert(VarChar, DisplayDate, 103), 103) Between Convert(DateTime, Convert(VarChar, GetDate()-29, 103), 103) And Convert(DateTime, Convert(VarChar, GetDate(), 103), 103) ";
						break;
				}
			}	
			
			sql += " Order By ID Desc";	
			
			CommonOpn op = new CommonOpn();
			
			try
			{
				op.BindGridSet(sql, dgArticles, 10);
			}
			catch(Exception err)
			{
				Trace.Warn("Error LoadBasicData(): " + err.ToString());
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();				
			}
		}
		
		protected string GetIsPublishedData(string id, string categoryId, string publishStatus, string categoryName)
		{
			if (publishStatus == "Yes")
                return "Yes&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"#\" onclick=\"Publish(" + id + "," + categoryId + ", this, '" + categoryName + "',1)\">Unpublish</a>";
			else
				return "No&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"#\" onclick=\"Publish("+ id +","+ categoryId +", this, '" + categoryName +"',0)\">Publish</a>";
		}
		
		protected string GetOnlineLink(string _categoryId, string _id)
		{
			if (_categoryId == "3")
			{
				if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 )
                    return "http://www.carwale.com/editorpanel/buyingused/view.aspx?id=" + _id;
				else
                    return "http://webserver/editorpanel/buyingused/view.aspx?id=" + _id;	
			}	
			return "#";	
		}
	}
}