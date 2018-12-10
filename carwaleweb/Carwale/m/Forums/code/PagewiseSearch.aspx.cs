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
	public class PagewiseSearch : Page
	{
		protected Repeater rptThreads;
	
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
			if (Request.QueryString["searchId"] != null && Request.QueryString["searchId"].ToString() != "" && CommonOpn.CheckId(Request.QueryString["searchId"].ToString()) && Request.QueryString["page"] != null && Request.QueryString["page"].ToString() != "" && CommonOpn.CheckId(Request.QueryString["page"].ToString()))
			{
				Forum obj = new Forum();
				obj.GetRepeater = true;
				obj.Rpt = rptThreads;
				obj.GetPagewiseSearchThreads(Request.QueryString["searchId"].ToString(), "10", Request.QueryString["page"].ToString());		
			}
		}
	}
}			