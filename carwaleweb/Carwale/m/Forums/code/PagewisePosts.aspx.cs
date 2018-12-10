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
	public class PagewisePosts : Page
	{
		protected PagePosts ucPagePosts;
	
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
				if(Request.QueryString["thread"] != null && Request.QueryString["thread"] != "" && CommonOpn.CheckId(Request.QueryString["thread"]) && Request.QueryString["page"] != null && CommonOpn.CheckId(Request.QueryString["page"]))
				{
					ucPagePosts.ThreadId = Convert.ToInt32(Request.QueryString["thread"].ToString());
					ucPagePosts.PageSize = 10;
					ucPagePosts.PageNo = Convert.ToInt32(Request.QueryString["page"].ToString());
					ucPagePosts.BindPage();
				}
			}
		}
	}
}			