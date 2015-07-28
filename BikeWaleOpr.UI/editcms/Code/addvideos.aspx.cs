using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;

namespace BikeWaleOpr.EditCms
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Jan 2014 
    /// </summary>
   
    public class Videos : Page
    {        
        protected TextBox txtVideoUrl;
        protected Button btnSave,btnUpdate,btnDelete;
        protected EditCmsCommon EditCmsCommon;
        protected HiddenField hdn_duration, hdn_likes, hdn_views;

        protected string basicId = string.Empty, videoUrl = string.Empty, views = string.Empty, likes = string.Empty,videoId = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            int pageId = 53;
            if (Request.QueryString["bid"] != null && Request.QueryString["bid"].ToString() != "")
            {
                basicId = Request.QueryString["bid"].ToString();
                EditCmsCommon.BasicId = basicId;
                EditCmsCommon.pageId = pageId;
                EditCmsCommon.PageName = "Manage Videos";
            }

            if (!IsPostBack)
            {
                btnDelete.Visible = false;
                GetVideos();
            }            
        }

        protected void btnSave_Click(object Sender, EventArgs e)
        {            
            string videoUrl = txtVideoUrl.Text.Trim();
            string videoId = ManageVideos.GetYouTubeVideoSrc(videoUrl);

            ManageVideos objMV = new ManageVideos();
            objMV.UpdateYoutubeVideoDetails(basicId, Convert.ToInt32(hdn_views.Value), Convert.ToInt32(hdn_likes.Value), videoUrl , videoId, Convert.ToDouble(hdn_duration.Value));

            GetVideos();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            
            ManageVideos objMV = new ManageVideos();
            objMV.DeleteYouTubeVideo(basicId);

            GetVideos();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 17 Jan 2014
        /// Summary    : Function show video and its details views count ,like count 
        /// </summary>
        protected void GetVideos()
        {
            try
            {
                DataSet ds = null;
                
                ManageVideos objMV = new ManageVideos();
                ds = objMV.GetYouTubeVideos(basicId);

                if (ds != null)
                {
                    DataTable dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        videoUrl = dt.Rows[0]["VideoId"].ToString();
                        views = dt.Rows[0]["Views"].ToString();
                        likes = dt.Rows[0]["Likes"].ToString();
                        //Trace.Warn("views" + views);
                        btnDelete.Visible = true;
                        btnSave.Text = "Update";
                    }
                    else
                    {
                        btnDelete.Visible = false;
                        btnSave.Text = "Save";                      
                    }
                }
                else
                {
                    btnDelete.Visible = false;
                    btnSave.Text = "Save";    
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("sql ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetVideos");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("ex : ", ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "GetVideos");
                objErr.SendMail();
            }
        }  
    }//class
}//namespace