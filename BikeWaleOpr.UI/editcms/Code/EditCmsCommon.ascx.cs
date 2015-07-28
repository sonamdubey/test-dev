
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
	public class EditCmsCommon : UserControl
	{
        protected string catId, basicId = "-1", catName, title, author, description, isPublished, pageName;
		protected DateTime displayDate;
		public int pageId, minBikeSelection = 0;
		protected string allowBikeSelection = "0";
		protected string heading;
		
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

        public string PageName
        {
            get
            {
                return pageName;
            }
            set
            {
                pageName = value;
            }
        }
		
		
		void Page_Load( object Sender, EventArgs e )
		{
				if (BasicId != "-1")
				{
					GetData();
					switch (PageId)
					{
						case 1:
							heading = "Basic Info";
							break;
						case 2:
							heading = "Tag Bike(s)";
							break;
						case 3:
							heading = "Extended Info";
							break;
						case 4:
							heading = "Manage Photos";
							break;
						case 5:
							heading = "Manage Article";
							break;
                        case 6:
                            heading = "Manage Videos";
							break;
						default:
							heading = "";
							break;
					}
					//GetLinkData();
				}	
		
		}
		
		public void GetData()
		{
			Database db = new Database();
			SqlDataReader dr = null ;
			string sql;
			
			sql = " SELECT CC.Id AS CatId, CC.Name AS CatName, CC.AllowBikeSelection, CC.MinBikeSelection, CB.Title, CB.AuthorName, CB.DisplayDate, CB.[Description], CB.IsPublished"
				+ " FROM Con_EditCms_Basic CB, Con_EditCms_Category CC"
				+ " WHERE CC.Id = CB.CategoryId"
				+ " AND CB.Id = @basicId";

            Trace.Warn("sql ", sql);

			SqlParameter [] param = 
			{
				new SqlParameter("@basicId", BasicId)
			};
			try
			{
				dr = db.SelectQry( sql, param );
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        if (dr["Description"].ToString().Length > 100)
                        {
                            description = dr["Description"].ToString().Substring(0, 99) + "...";
                        }
                        else description = dr["Description"].ToString();
                        catId = dr["CatId"].ToString();
                        catName = dr["CatName"].ToString();
                        title = dr["Title"].ToString();
                        author = dr["AuthorName"].ToString();
                        minBikeSelection = Convert.ToInt32(dr["MinBikeSelection"]);
                        if (Convert.ToBoolean(dr["AllowBikeSelection"].ToString()))
                        {
                            allowBikeSelection = "1";
                        }
                        else
                        {
                            allowBikeSelection = "0";
                        }

                        isPublished = dr["IsPublished"].ToString().ToUpper() == "FALSE" ? "Not Published" : "Published";
                        //description = dr["Description"].ToString();					
                        displayDate = DateTime.Parse(dr["DisplayDate"].ToString());
                    } 
                }
                Trace.Warn("IsPublished :" + dr["IsPublished"].ToString().ToUpper());
				Trace.Warn("catName :" + catName + ", title: " + title + ", author: " + author);
				Trace.Warn("description: " + description);
				Trace.Warn("displayDate: " + displayDate.ToString("dd-MMM-yyyy"));
			}
			catch( SqlException err )	
			{
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
                if(dr != null)
				    dr.Close();
				db.CloseConnection();
			}
		}
	}
}