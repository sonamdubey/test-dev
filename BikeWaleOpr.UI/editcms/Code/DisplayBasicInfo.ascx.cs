
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

namespace BikeWaleOpr.EditCms
{
	public class DisplayBasicInfo : UserControl
	{
		protected string catId, basicId = "-1", catName, title, author, description, isNew;
		protected DateTime displayDate;
		public int pageId, minBikeSelection = 0;
		protected string allowBikeSelection = "0";
		protected string bikesCount = "0", imageCount = "0", otherCount = "0", pagesCount = "0";
		public string viewAdd = "";
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();			
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
		}
		
		public string BasicId
		{
			get
			{
				return basicId;
			}
			set
			{
				basicId = value;
			}
		}
		
		public int PageId
		{
			get
			{
				return pageId;
			}
			set
			{
				pageId = value;
			}
		}
		
		public string IsNew
		{
			get
			{
				return isNew;
			}
			set
			{
				isNew = value;
				if (isNew == "1")
					viewAdd = "Add";
				else
					viewAdd = "View";	
			}
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			if ( !IsPostBack )
			{
				if (BasicId != "-1")
				{
					GetData();
					GetLinkData();
				}	
			}
		}
		
		public void GetLinkData()
		{
            throw new Exception("Method not used/commented");
            //string sql = " SELECT "
            //    + " (SELECT COUNT (ID) FROM Con_EditCms_Bikes WHERE BasicId = @basicId AND IsActive = 1) AS BikesCount,"
            //    + " (SELECT COUNT (ID) FROM Con_EditCms_Images WHERE BasicId = @basicId) AS ImageCount,"
            //    + " (SELECT COUNT (ID) FROM Con_EditCms_OtherInfo WHERE BasicId = @basicId) AS OtherCount,"
            //    + " (SELECT COUNT (ID) FROM Con_EditCms_Pages WHERE BasicId = @basicId) AS PagesCount";
			
			
            //SqlDataReader dr = null;
            //Database db = new Database();
            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", BasicId)
            //};
            //try
            //{
				
            //    dr = db.SelectQry(sql, param);
            //    if (dr.Read())
            //    {
            //        bikesCount = dr["BikesCount"].ToString();
            //        imageCount = dr["ImageCount"].ToString();
            //        otherCount = dr["OtherCount"].ToString();
            //        pagesCount = dr["PagesCount"].ToString();
            //    }
            //}
            //catch(Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    dr.Close();
            //    db.CloseConnection();
            //}
		}
		
		public void GetData()
		{
            throw new Exception("Method not used/commented");

            //Database db = new Database();
            //SqlDataReader dr = null ;
            //string sql;
			
            //sql = " SELECT CC.Id AS CatId, CC.Name AS CatName, CC.AllowBikeSelection, CC.MinBikeSelection, CB.Title, CB.AuthorName, CB.DisplayDate, CB.[Description]"
            //    + " FROM Con_EditCms_Basic CB, Con_EditCms_Category CC"
            //    + " WHERE CC.Id = CB.CategoryId"
            //    + " AND CB.Id = @basicId";

            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@basicId", BasicId)
            //};
            //try
            //{
            //    dr = db.SelectQry( sql, param );
            //    if(dr.Read())
            //    {
            //        if(dr["Description"].ToString().Length > 100)
            //        {
            //            description = dr["Description"].ToString().Substring(0,99) + "...";		
            //        }
            //        else description = dr["Description"].ToString();
            //        catId = dr["CatId"].ToString();
            //        catName = dr["CatName"].ToString();
            //        title = dr["Title"].ToString();
            //        author = dr["AuthorName"].ToString();
            //        minBikeSelection = Convert.ToInt32(dr["MinBikeSelection"]);
            //        if (Convert.ToBoolean(dr["AllowBikeSelection"].ToString()))
            //        {
            //            allowBikeSelection = "1";
            //        }
            //        else
            //        {
            //            allowBikeSelection = "0";
            //        }	
            //        //description = dr["Description"].ToString();					
            //        displayDate = DateTime.Parse(dr["DisplayDate"].ToString());										
            //    }
            //    Trace.Warn("catName :" + catName + ", title: " + title + ", author: " + author);
            //    Trace.Warn("description: " + description);
            //    Trace.Warn("displayDate: " + displayDate.ToString("dd-MMM-yyyy"));
            //}
            //catch( SqlException err )	
            //{
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    dr.Close();
            //    db.CloseConnection();
            //}
		}
	}
}