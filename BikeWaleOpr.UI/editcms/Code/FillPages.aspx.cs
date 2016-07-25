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
using System.Drawing.Imaging;
using Ajax;
using System.IO;

namespace BikeWaleOpr.EditCms
{
	public class FillPages : Page
	{
		protected DropDownList drpPages;
		protected RichTextEditor rteDetails;
		protected DataList dlstPhoto;
		protected Label lblMessage;
		protected Button btnSave;
		protected DropDownList drpVersion;
		protected HtmlInputButton btnAddSpec;
		protected EditCmsCommon EditCmsCommon;
		
		protected string basicId = string.Empty, addSpec = string.Empty, versionId = string.Empty, makeName = string.Empty, modelName = string.Empty, modelId = string.Empty;
		protected int pid;
		protected string ArticleUrl = string.Empty;
		protected string ArticleTitle = string.Empty;
		protected string CategoryId = string.Empty;
		protected bool RowsReturned = false;
		
		private string ContentId
		{
			get
			{
				if(ViewState["ContentId"] != null && ViewState["ContentId"].ToString() != "")
					return ViewState["ContentId"].ToString();
				else 
					return "-1";
			}
			set
			{
				ViewState["ContentId"] = value;
			}
		}
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();			
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			//CommonOpn op = new CommonOpn();
			Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../editcms/default.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../editcms/default.aspx");
				
            //int pageId = 53;
            //if ( !op.verifyPrivilege( pageId ) )
            //    Response.Redirect("../NotAuthorized.aspx");
				
			if( Request.QueryString["pid"] != null && Request.QueryString["pid"].ToString() != "")
			{
				pid = int.Parse(Request.QueryString["pid"].ToString());
			}
				
			if( Request.QueryString["bid"] != null && Request.QueryString["bid"].ToString() != "")
			{
				basicId = Request.QueryString["bid"].ToString();
				if ( CommonOpn.CheckId(basicId) == false )
				{
					return;
				}
			}
			else
			{
				return;
			}
			
			if ( !IsPostBack )
			{
				FetchPageData();			
			}
			
			EditCmsCommon.BasicId = basicId; 
		}
		
		void FetchPageData()
		{
            //Database db = new Database();
            //SqlDataReader dr = null ;
            //modelId = "-1";
            //string sql;
			
            //sql = " Select Top 1 CP.Id, Cp.PageName, Cc.AddSpecification, Cpc.Data, IsNull(Cec.VersionId, '-1') As VersionId, CB.CategoryId, CB.Title, CB.Url, "
            //    + " IsNull(Cec.ModelId, '-1') As ModelId, Mo.Name As ModelName, Ma.Name As MakeName, Cv.Name AS VersionName, IsNull(Cpc.ID, -1) AS ContentId "
            //    + " From Con_EditCms_Category Cc, Con_EditCms_Basic Cb Inner Join Con_EditCms_Pages Cp ON Cp.BasicId = Cb.Id "
            //    + " Left Join Con_EditCms_PageContent Cpc On Cpc.PageId = Cp.Id Left Join Con_EditCms_Bikes Cec On Cec.BasicId = Cb.Id And Cec.IsActive = 1"
            //    + " Left Join BikeVersions AS Cv ON Cv.ID = Cec.VersionId Left Join BikeModels AS Mo ON Mo.ID = Cec.ModelId "
            //    + " LEFT Join BikeMakes AS Ma ON Ma.ID = Cec.MakeId Where Cp.BasicId = @BasicId And Cp.Id = @PageId AND Cc.Id = Cb.CategoryId ";
				
            //SqlParameter [] param = 
            //{
            //    new SqlParameter("@PageId", pid),
            //    new SqlParameter("@basicId", basicId)
            //};
            //try
            //{
            //    Trace.Warn(sql + "-----" + pid + "----" + basicId);
            //    dr = db.SelectQry( sql, param );
 
            //    if( dr.HasRows )
            //    {
            //        RowsReturned = true;
            //        while( dr.Read() )
            //        {
            //            EditCmsCommon.PageName = dr["PageName"].ToString();						
            //            addSpec = dr["AddSpecification"].ToString();
            //            makeName = dr["MakeName"].ToString();
            //            modelName = dr["ModelName"].ToString();
            //            versionId = dr["VersionId"].ToString();
            //            modelId = dr["ModelId"].ToString();						
            //            rteDetails.Text = dr["Data"].ToString();						
            //            ContentId = dr["ContentId"].ToString();
            //            ArticleUrl = dr["Url"].ToString();
            //            ArticleTitle = dr["Title"].ToString();
            //            CategoryId = dr["CategoryId"].ToString();
            //        }
            //    }
            //    else
            //    {
            //        RowsReturned = false;
            //    }                
            //    //Trace.Warn(rteDetails.Text);
            //    if( modelId != "-1" )
            //    {
            //        FillVersions(modelId);
            //    }
            //}
            //catch(SqlException err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch SqlException
            //catch(Exception err)
            //{
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();
            //}// catch Exception
            //finally
            //{
            //    if( dr != null )
            //        dr.Close();
            //    db.CloseConnection();
            //}
			
		}
		
		void FillVersions(string modelId)
		{	
			AjaxFunctions aj = new AjaxFunctions();
			
			drpVersion.Enabled = true;
	
			drpVersion.DataSource = aj.GetVersions(modelId);
			drpVersion.DataTextField = "Text";
			drpVersion.DataValueField = "Value";
			drpVersion.DataBind();
			drpVersion.Items.Insert(0, new ListItem("--Select--","0"));
			drpVersion.SelectedIndex = drpVersion.Items.IndexOf(drpVersion.Items.FindByValue(versionId));
		}
		
		void drpPages_Change(object Sender, EventArgs e)
		{
			Trace.Warn(drpPages.Items[drpPages.SelectedIndex].Value.ToString());
			//FillRichTextBox(int.Parse(drpPages.Items[drpPages.SelectedIndex].Value.ToString()));
		}
		
		void btnSave_Click(object Sender, EventArgs e)
		{
			if(SaveDescription(pid.ToString()))
			{
				Response.Redirect("addpages.aspx?bid=" + basicId);
			}
			else
			{
				lblMessage.Text = "Update failed";
			}
		}
		
		bool SaveDescription(string pageId)
		{

            throw new Exception("Method not used/commented");
            //bool isSaved = false;			
            //string sql = string.Empty;
            //Database db = new Database();
			
            //try
            //{
            //    if(ContentId == "-1")
            //    {
            //        sql = " Insert Into Con_EditCms_PageContent ( Data, PageId ) Values ( @Data, @PageId )";
					
            //        SqlParameter [] param = 
            //        {
            //            new SqlParameter( "@Data", rteDetails.Text.ToString() ),
            //            new SqlParameter( "@PageId", pageId )
            //        };
					
            //        isSaved = db.InsertQry( sql, param );	
            //    }
            //    else
            //    {
            //        sql = " Update Con_EditCms_PageContent Set Data = @Data Where Id = @ContentId ";
					
            //        SqlParameter [] param = 
            //        {
            //            new SqlParameter( "@Data", rteDetails.Text.ToString() ),
            //            new SqlParameter( "@ContentId", ContentId )
            //        };
					
            //        isSaved = db.UpdateQry( sql, param );	
            //    }
				
            //}
            //catch(SqlException err)
            //{
            //    isSaved = false;
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();				
            //}// catch Exception
            //catch(Exception err)
            //{
            //    isSaved = false;
            //    Trace.Warn(err.Message + err.Source);
            //    ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
            //    objErr.ConsumeError();				
            //}// catch Exception
			
            //return isSaved;
		}
		
	}
}