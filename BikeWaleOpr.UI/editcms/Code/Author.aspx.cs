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
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace BikeWaleOpr.EditCms
{
    public class Author : Page
	{
        protected DropDownList ddlAuthor;
		protected Button btnSave, btnUpdate;
        protected TextBox txtDesignation, txtBriefProfile;
		protected RichTextEditor rteDescription;
        protected HtmlInputFile inpPhoto;
		protected string bid = string.Empty;
		protected HtmlInputHidden hdnSubCat, hdnBasicId;
        protected HtmlGenericControl subCatContainer, alertObj;
        string PicPath = string.Empty;
		protected DisplayBasicInfo BasicInfoControl;
		protected EditCmsCommon EditCmsCommon;
        protected Label lblResult;
									 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
            ddlAuthor.AutoPostBack = true;
            ddlAuthor.SelectedIndexChanged += new EventHandler(ddlAuthor_SelectedIndexChanged);
		}

		void Page_Load( object Sender, EventArgs e )
		{
			if (!Page.IsPostBack)
			{
                //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
                //    Response.Redirect("../users/Login.aspx?ReturnUrl=../editcms/Author.aspx");
				
                //if ( Request.Cookies["Customer"] == null )
                //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../editcms/Author.aspx");
					
                //int pageId = 53;
                //CommonOpn op = new CommonOpn();
                //if ( !op.verifyPrivilege( pageId ) )
                //    Response.Redirect("../NotAuthorized.aspx");
			
				btnSave.Attributes.Add("onclick", "javascript:if (Validate() == false) return false;");
				//LoadCategory();	
				LoadAuthors();
                if (ddlAuthor.SelectedValue != "0")
                    LoadData(ddlAuthor.SelectedValue);
				//LoadHoursMins();
                //if (Request.QueryString["bid"] != null)
                //{
                //    bid = Request.QueryString["bid"];
                //    //LoadData(bid);						
                //    EditCmsCommon.BasicId = bid;
                //    EditCmsCommon.PageId = 1;
                //}
			}
            alertObj.Visible = false;
		} // Page_Load

        private void LoadAuthors()
        {
            try
            {
                string sql = @"select U.UserName,U.Id from OprUsers U inner join
                          Con_EditCms_Author A on A.Authorid=U.Id 
                          where U.IsActive=1 order by U.UserName ";


                Trace.Warn(sql);
                CommonOpn op = new CommonOpn();
                try
                {
                    op.FillDropDown(sql, ddlAuthor, "UserName", "Id");
                }
                catch (SqlException err)
                {
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception err)
                {
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                } // catch Exception

                ListItem item = new ListItem("--Select Author--", "0");
                ddlAuthor.Items.Insert(0, item);
               // ddlAuthor.SelectedValue = CurrentUser.Id;
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
            }
        }

        void ddlAuthor_SelectedIndexChanged(object Sender, EventArgs e)
        {
            if (ddlAuthor.SelectedValue != "0")
                LoadData(ddlAuthor.SelectedValue);
        }


        private void LoadData(string AuthorId)
        {
            
            string sql = "Select * from Con_EditCms_Author where Authorid=@Authorid";

            SqlConnection con;	
            Database db = new Database();           
            SqlParameter[] param = 
			{
				new SqlParameter("@Authorid", AuthorId)
			};
            con = new SqlConnection(db.GetConString());		
            try
            {
                //SqlDataReader dr = db.SelectQry(sql, param);
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddRange(param);
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        Trace.Warn(dr["Designation"].ToString());
                        txtDesignation.Text = dr["Designation"].ToString();                       
                        rteDescription.Text = dr["FullProfile"].ToString();
                        txtBriefProfile.Text = dr["BriefProfile"].ToString();
                    }
					dr.Close();
                }                           
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

		void btnSave_Click( object Sender, EventArgs e )
		{
			SqlConnection con;			
			SqlParameter prm;
			Database db = new Database();			
			string conStr = db.GetConString();		
           			
			con = new SqlConnection( conStr );				
			
			try
			{
                if (UploadPhotoFile(ddlAuthor.SelectedValue, inpPhoto.PostedFile.FileName))
                {

                    using (SqlCommand cmd = new SqlCommand("Con_EditCms_UpdateAuthorProfile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        prm = cmd.Parameters.Add("@AuthorId", SqlDbType.BigInt);
                        prm.Value = ddlAuthor.SelectedValue;

                        prm = cmd.Parameters.Add("@Designation", SqlDbType.VarChar, 25);
                        prm.Value = txtDesignation.Text.Trim();

                        prm = cmd.Parameters.Add("@PicPath", SqlDbType.VarChar, 100);
                        prm.Value = PicPath;

                        prm = cmd.Parameters.Add("@BriefProfile", SqlDbType.VarChar, 100);
                        prm.Value = txtBriefProfile.Text.Trim();

                        prm = cmd.Parameters.Add("@FullProfile", SqlDbType.VarChar, 500);
                        prm.Value = rteDescription.Text.Trim();

                        con.Open();

                        cmd.ExecuteNonQuery();

                        lblResult.Text = "Record Saved";
                    }                   
                }
			}
			catch(SqlException err)
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}          
		}

        bool UploadPhotoFile(string photoId,string FilePath)
        {
            CommonOpn co = new CommonOpn();
            bool isCompleted;

            try
            {
                string selectedFileName = inpPhoto.Value;
                string fileTempName = "Temp_" + photoId;
                string galleryPath = ImagingOperations.GetPathToSaveImages("\\bikewaleimg\\ec\\autherimg\\" + photoId);
                Trace.Warn("gallery path : ", galleryPath);

                if (!Directory.Exists(galleryPath))
                    Directory.CreateDirectory(galleryPath);

                string tempFilePath = galleryPath + fileTempName + "." + selectedFileName.Split('.')[1];
                inpPhoto.PostedFile.SaveAs(tempFilePath);

                if (File.Exists(galleryPath + "\\" + photoId + "s.jpg"))
                    File.Delete(galleryPath + "\\" + photoId + "s.jpg");
                if (File.Exists(galleryPath + "\\" + photoId + "b.jpg"))
                    File.Delete(galleryPath + "\\" + photoId + "b.jpg");


                ImagingFunctions.GenerateSquareThumbnail(tempFilePath, galleryPath + "\\" + photoId + "s.jpg", 55);
                ImagingFunctions.ResizePicture(tempFilePath, galleryPath + "\\" + photoId + "b.jpg", 100, 135);

                PicPath = photoId;

                File.Delete(tempFilePath);
                isCompleted = true;
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                Trace.Warn("mesage" + err.Message);
                objErr.SendMail();
                isCompleted = false;
            }
            return isCompleted;
        }		
	} // class
} // namespace