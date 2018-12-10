using System;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using MobileWeb.DataLayer;

namespace MobileWeb.Controls
{
	public class PageThreads : UserControl
	{
		private int _pageSize = 10, _pageNo = 1;
		protected Repeater rptThreads;
		private int _forumSubCategoryId;
		
		public int PageSize 
		{ 
			get {return _pageSize;} 
			set {_pageSize=value; } 
		}
		
		public int PageNo 
		{ 
			get {return _pageNo;} 
			set {_pageNo=value; } 
		}
		
		public int ForumSubCategoryId 
		{ 
			get {return _forumSubCategoryId;} 
			set {_forumSubCategoryId=value; } 
		}
	
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
				
			}
		}
		
		public void BindPage()
		{
			Forum obj = new Forum();
			obj.GetRepeater = true;
			obj.Rpt = rptThreads;
			obj.GetPagewiseThreads(ForumSubCategoryId.ToString(), PageSize.ToString(), PageNo.ToString());	
		}
	}
}		