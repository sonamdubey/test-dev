using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MobileWeb.Common;
using MobileWeb.DataLayer;
using MobileWeb.Controls;

namespace MobileWeb.Forums
{
	public class PagewiseThreads : Page
	{
		protected PageThreads ucPageThreads;
	
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			if (!Page.IsPostBack)
			{
				if(Request.QueryString["forum"] != null && Request.QueryString["forum"] != "" && CommonOpn.CheckId(Request.QueryString["forum"]) && Request.QueryString["page"] != null && CommonOpn.CheckId(Request.QueryString["page"]))
				{
					ucPageThreads.ForumSubCategoryId = Convert.ToInt32(Request.QueryString["forum"].ToString());
					ucPageThreads.PageSize = 10;
					ucPageThreads.PageNo = Convert.ToInt32(Request.QueryString["page"].ToString());
					ucPageThreads.BindPage();
				}
			}
		}
	}
}			