using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using System.IO;
using System.Data.Common;
using BikeWaleOPR.DAL.CoreDAL;
using BikeWaleOPR.Utilities;

namespace BikeWaleOpr.Content
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 6 Aug 2013
    /// Summary : Class to manage the videos on bikewale
    /// </summary>
    public class ManageVideos : Page
    {
        protected DropDownList ddlMake, ddlModel, ddlFilterMake, ddlFilterModel;
        protected CheckBox chkStatus, chkIsUpdate, chkShowActiveVideos;
        protected Button btnsub,btnUpdate,btnShow;
        protected TextBox txtId, txtVideoSrc, txtVideoTitle;
        protected Repeater rptModelVideos;
        protected HiddenField hdnFilterSelModelId, hdnSelModelId, hdnVideoId;
        protected int serialNo = 0;
        protected Label lblMessage, spnErrNoData;        

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnShow.Click += new EventHandler(BtnShow_Click);
            btnsub.Click += new EventHandler(BtnSub_Click);
            btnUpdate.Click += new EventHandler(BtnUpdate_Click);
            //btnShowAll.Click += new EventHandler(BtnShowAll_Click);
        }
 
        void Page_Load(object Sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                BindData(true);

                try
                {
                    MakeModelVersion mmv = new MakeModelVersion();

                    DataTable dt = mmv.GetMakes("New");

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        // Bind filter makes dropdownlist
                        ddlFilterMake.DataSource = dt;
                        ddlFilterMake.DataTextField = "Text";
                        ddlFilterMake.DataValueField = "Value";
                        ddlFilterMake.DataBind();
                        ddlFilterMake.Items.Insert(0, new ListItem("--Select Make--", "0"));

                        // Bind makes dropdownlist
                        ddlMake.DataSource = dt;
                        ddlMake.DataTextField = "Text";
                        ddlMake.DataValueField = "Value";
                        ddlMake.DataBind();
                        ddlMake.Items.Insert(0, new ListItem("--Select Make--", "0"));
                    }
                }
                catch (SqlException ex)
                {
                    Trace.Warn("sql ex : ", ex.Message);
                    ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    err.SendMail();
                }
                catch (Exception ex)
                {
                    Trace.Warn("ex : ", ex.Message);
                    ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    err.SendMail();
                }
            }

            spnErrNoData.Text = "";
            //FillModelDropDown();
        }

        protected void BtnShow_Click(object Sender, EventArgs e)
        {
            try
            {
                string makeId = ddlFilterMake.SelectedValue == "0" ? "" : ddlFilterMake.SelectedValue;
                string modelId = hdnFilterSelModelId.Value == "0" ? "" : hdnFilterSelModelId.Value;
                bool isActive = chkShowActiveVideos.Checked;

                DataSet ds = null;

                ds = GetVideosData(makeId, modelId, isActive);

                if (ds != null)
                {
                    rptModelVideos.DataSource = ds;
                    rptModelVideos.DataBind();
                }

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    //divModelVideos.Visible = false;
                    spnErrNoData.Text = "No Data Exists.";                    
                }

                //prefill models dropdown list
                if (!String.IsNullOrEmpty(modelId) && modelId != "0")
                {                    
                    MakeModelVersion mmv = new MakeModelVersion();
                    DataTable dt = mmv.GetModels(makeId, "New");

                    // Bind makes dropdownlist
                    ddlFilterModel.DataSource = dt;
                    ddlFilterModel.DataTextField = "Text";
                    ddlFilterModel.DataValueField = "Value";
                    ddlFilterModel.DataBind();
                    ddlFilterModel.Items.Insert(0, new ListItem("--Select Model--", "0"));
                    ddlFilterModel.SelectedValue = modelId;
                }
            }
            catch (Exception ex)
            {
                Trace.Warn("ex : ", ex.Message);
                ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                err.SendMail();
            }
        }   // End of BtnShow_Click method

        //protected void BtnShowAll_Click(object Sender, EventArgs e)
        //{            
        //    DataSet ds = null;
            
        //    bool isActive = chkShowActiveVideos.Checked;
        //    ds = GetVideosData("", "", isActive);

        //    if (ds != null)
        //    {
        //        rptModelVideos.DataSource = ds;
        //        rptModelVideos.DataBind();
        //    }
        //}

        /// <summary>
        /// Function to save new video into database.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void BtnSub_Click(object Sender, EventArgs e)
        {            
            string videoId = string.Empty, makeId = string.Empty, modelId = string.Empty, videoSrc = string.Empty, videoTitle = string.Empty;
            bool isActive = true;

            ReadData(ref videoId, ref makeId, ref modelId, ref videoSrc, ref videoTitle, ref isActive);

            bool exist = IsVideoExist(makeId, modelId, videoSrc);

            if (exist)
            {
                lblMessage.Text = "Video already exists.";
            }
            else
            {
                ManageVideosData("-1", makeId, modelId, videoSrc, videoTitle, isActive);
                lblMessage.Text = "Video added successfully.";
                ClearText();
                BindData(isActive);
            }
            
        }

        /// <summary>
        /// Summary : Function to update the old video data into database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string videoId = string.Empty, makeId = string.Empty, modelId = string.Empty, videoSrc = string.Empty, videoTitle = string.Empty;            
            bool isActive = true;

            ReadData(ref videoId, ref makeId, ref modelId, ref videoSrc, ref videoTitle, ref isActive);

            ManageVideosData(videoId, makeId, modelId, videoSrc, videoTitle, isActive);
            lblMessage.Text = "Video updated successfully.";
            ClearText();
            BindData(isActive);
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 6 Aug 2013
        /// Summary : Function to get the videos data as per requirement from database.
        /// </summary>
        /// <param name="makeId">Make Id of the bike. Its Optional.</param>
        /// <param name="modelId">Model Id of the bike. Its Optional.</param>
        /// <param name="isActive">Get active or inactive videos. Manadatory</param>
        /// <returns>Function returns data table containing videos data. If data is not available return null.</returns>
        private DataSet GetVideosData( string makeId, string modelId, bool isActive )
        {
            DataSet ds = null;
                        
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getvideos"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "getvideos";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (!string.IsNullOrEmpty(makeId)) ? makeId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (!string.IsNullOrEmpty(modelId)) ? modelId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_IsActive", DbParamTypeMapper.GetInstance[SqlDbType.Bit], isActive));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd);    
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("sql ex : ", ex.Message);
                ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                err.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("ex : ", ex.Message);
                ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                err.SendMail();
            }
            
            return ds;

        }   // End of GetVideosData

        /// <summary>
        /// Summary : Function to insert or update videos.
        /// </summary>
        /// <param name="videoId">In case of saving new video pass -1. Else pass video id of the video to be updated.</param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="videoSrc"></param>
        /// <param name="videoTitle"></param>
        /// <param name="IsActive"></param>
        private void ManageVideosData(string videoId, string makeId, string modelId, string videoSrc, string videoTitle, bool IsActive)
        {   
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("managevideos"))
                {                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "managevideos";

                    //cmd.Parameters.Add("@VideoId", SqlDbType.Int).Value = String.IsNullOrEmpty(videoId) ? "-1" : videoId;
                    //cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    //cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    //cmd.Parameters.Add("@VideoSrc", SqlDbType.VarChar, 500).Value = videoSrc.Trim();
                    //cmd.Parameters.Add("@VideoTitle", SqlDbType.VarChar, 300).Value = videoTitle.Trim();
                    //cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_videoid", DbParamTypeMapper.GetInstance[SqlDbType.Int], String.IsNullOrEmpty(videoId) ? "-1" : videoId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_videosrc", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 500, videoSrc.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_videotitle", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 300, videoTitle.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_IsActive", DbParamTypeMapper.GetInstance[SqlDbType.Bit], IsActive));

                    
                    MySqlDatabase.ExecuteNonQuery(cmd);                                        
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("sql ex : ", ex.Message);
                ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                err.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("ex : ", ex.Message);
                ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                err.SendMail();
            }
        }   // End of ManageVideosData method

        /// <summary>
        /// Summary : Function to check whether video exists for the given make and model with same src.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="videoSrc"></param>
        /// <returns></returns>
        private bool IsVideoExist(string makeId, string modelId, string videoSrc)
        {
            bool IsExists = false;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("isvideoexist"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;
                    //cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    //cmd.Parameters.Add("@VideoSrc", SqlDbType.VarChar).Value = videoSrc.Trim();
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.Int], makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_videosrc", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 500, videoSrc.Trim()));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null && dr.Read())
                        {
                            string count = dr["VideoCount"].ToString();

                            // If count is greater than video exists.
                            if (Convert.ToInt32(count) > 0)
                                IsExists = true;
                        }
                    }

                   
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("sql ex : ", ex.Message);
                ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                err.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("ex : ", ex.Message);
                ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                err.SendMail();
            }
            return IsExists;
        }   // End of IsVideoExist method

        private void ReadData(ref string videoId, ref string makeId, ref string modelId, ref string videoSrc, ref string videoTitle, ref bool isActive)
        {
            makeId = ddlMake.SelectedValue;
            modelId = hdnSelModelId.Value;
            videoSrc = txtVideoSrc.Text;
            videoTitle = txtVideoTitle.Text;
            isActive = chkStatus.Checked;
            videoId = hdnVideoId.Value;
        }        

        /// <summary>
        /// Function to show videos on the page
        /// </summary>
        /// <param name="isActive">Pass true or false to get the active or inactive videos.</param>
        private void BindData(bool isActive)
        {
            DataSet ds = null;

            ds = GetVideosData("", "", isActive);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                rptModelVideos.DataSource = ds;
                rptModelVideos.DataBind();
            }
        }
        
        /// <summary>        
        /// Function is used to clear specified fields.
        /// </summary>
        void ClearText()
		{
			ddlMake.SelectedValue = "0";
			ddlModel.SelectedValue = "0";
            txtVideoSrc.Text="";
            txtVideoTitle.Text = "";
            chkStatus.Checked = false;			
		}

    }//class
}//namespace