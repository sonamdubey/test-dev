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
using System.Text.RegularExpressions;
using MobileWeb.Common;

namespace MobileWeb.Controls
{
	public class PagePosts : UserControl
	{
		private int _pageSize = 10, _pageNo = 1;
		protected Repeater rptPosts;
		private int _threadId;
		
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
		
		public int ThreadId 
		{ 
			get {return _threadId;} 
			set {_threadId=value; } 
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
			obj.Rpt = rptPosts;
			obj.GetPagewisePosts(ThreadId.ToString(), PageSize.ToString(), PageNo.ToString());	
		}
		
		public string GetDateTime(string value)
		{
			if(value != "")
				return Convert.ToDateTime(value).ToString("dd-MMM, yyyy hh:mm tt");
			else
				return "N/A";
		}
		
		public string GetUserTitle( string role, string posts, string bannedCust )
		{
			string title = "";

			if(bannedCust != "-1")
			{
				title = "[BANNED]";
			}
			else if ( role != "" )
			{
				title = "Moderator";
			}
			else
			{
				int noOfPosts = int.Parse( posts );
				
				if ( noOfPosts < 26 )
					title = "New Arrival";
				else if ( noOfPosts < 51 )
					title = "Driven";
				else if ( noOfPosts < 101 )
					title = "Road-tested";
				else if ( noOfPosts < 251 )
					title = "Long-termer";
				else if ( noOfPosts < 501 )
					title = "Beloved";
				else if ( noOfPosts < 1001 )
					title = "Best-seller";
				else title = "Legend";
			}	

			return title;
		}
		
		public string GetMessage(string value)
		{
			string post = value;
			string quoteStart = "<div class='quote'>Posted by <b>";
			string quotePostedByEnd = "</b><br>";
			string quoteEnd = "</div>";
			
			// Identify and replace quotes
			if ( post.ToLower().IndexOf( "[^^quote=" ) >= 0 )
			{
				Trace.Warn( "Quote Found" );
				post = post.Replace( "[^^quote=", quoteStart );
				post = post.Replace( "[^^/quote^^]", quoteEnd );
				post = post.Replace( "^^]", quotePostedByEnd );
			}
			
			if ( Request["h"] != null && Request["h"] != "" )
			{
				post = post.ToLower().Replace( Request["h"], "<span style='background:#FFFF00;color:#000000;'>" + Request["h"] + "</span>" );
			}
			
			post = post.Replace( "<p>&nbsp;</p>", ""); // remove empty paragraphs.
            Trace.Warn(post);
            post = CommonOpn.RemoveAnchorTag(post);
            Trace.Warn(post);
			return post;
		}
	}
}		