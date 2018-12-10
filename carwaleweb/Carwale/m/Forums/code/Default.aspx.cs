/*******************************************************************************************************
IN THIS CLASS THE NEW MEMBEERS WHO HAVE REQUESTED FOR REGISTRATION ARE SHOWN
*******************************************************************************************************/
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

namespace MobileWeb.Forums
{
	public class Default : Page
	{
		protected Repeater rptParent, rptHotDiscussions, rptNewDiscussions;
		protected DataSet dsCategories;
	
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
				LoadCategoryDetails();
				BindForumCategories();	
				BindHotDiscussions();
				BindNewDiscussions();
			}	
		}
		
		private void LoadCategoryDetails()
		{
			Forum obj = new Forum();
			obj.GetDataSet = true;
			obj.GetCategoriesDetails();
			dsCategories = obj.ds;
		}
		
		private void BindForumCategories()
		{
			Forum obj = new Forum();
			obj.GetRepeater = true;
			obj.Rpt = rptParent;
			obj.GetActiveForumCategories();
		}
		
		private void BindHotDiscussions()
		{
			Forum obj = new Forum();
			obj.GetRepeater = true;
			obj.Rpt = rptHotDiscussions;
			obj.GetHotDiscussions();
		}
		
		private void BindNewDiscussions()
		{
			Forum obj = new Forum();
			obj.GetRepeater = true;
			obj.Rpt = rptNewDiscussions;
			obj.GetNewDiscussions();
		}
		
        //Modified by supriya on 28/8/2013 to add column Url to fetch url of thread  
		public DataSet GetSubCategories(string id)
		{
			DataSet ds = new DataSet();
			DataTable dt = ds.Tables.Add();
			DataRow dr;
			
			dt.Columns.Add("SubCatId", typeof(string));
			dt.Columns.Add("SubCatName", typeof(string));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("LastThreadId", typeof(string));
			dt.Columns.Add("LastThread", typeof(string));
			dt.Columns.Add("LastPostedBy", typeof(string));
			dt.Columns.Add("LastPostDate", typeof(string));
			dt.Columns.Add("LastPostedById", typeof(string));
			dt.Columns.Add("Threads", typeof(string));
			dt.Columns.Add("Posts", typeof(string));
			dt.Columns.Add("Handle", typeof(string));
            dt.Columns.Add("Url", typeof(string));
						
			DataRow [] rows = dsCategories.Tables[0].Select(" ForumCategoryId = " + id);
			
			foreach(DataRow row in rows)
			{
				dr = dt.NewRow();
				
				dr["SubCatId"] 		= row["ID"];			
				dr["SubCatName"] 	= row["Name"];			
				dr["Description"] 	= row["Description"];			
				dr["LastThreadId"] 	= row["TopicId"];			
				dr["LastThread"] 	= row["Topic"];			
				dr["LastPostedBy"]	= row["LastPostBy"];
				dr["LastPostDate"]	= row["LastPostDate"];
				dr["LastPostedById"]= row["LastPostById"];
				dr["Threads"] 		= row["Threads"];
				dr["Posts"] 		= row["Posts"];
				dr["Handle"] 		= row["handlename"];
                dr["Url"]           = row["Url"];
				dt.Rows.Add(dr);
			}
			
			return ds;
		}
	}
}		