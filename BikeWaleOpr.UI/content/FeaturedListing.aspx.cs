using BikeWaleOpr.Common;
using BikeWaleOpr.RabbitMQ;
using MySql.CoreDAL;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class FeaturedListing : Page
    {
        protected Button btnSave, btnUpdate;//, btnUpdateFeaturedBike;
        protected Label lblMessage, lblResult;
        protected DropDownList drpMake, drpModel, drpVersion;
        protected CheckBox chkIsModel, chkIsVisible, chkIsActive, chkIsResearch, chkIsPrice;
        protected RadioButton rdFD, rdRT;
        protected TextBox txtDescription, txtLink;
        protected HtmlInputFile flphoto;
        protected DataGrid dtgrdFeaturedListing;
        protected HtmlInputHidden hdn_SelectedModel, hdn_SelectedVersion;
        protected HtmlImage imgFLPhoto;

        public int serialNo = 0, count = 0;
        public string visibleCount = "";
        string updateData = "";
        protected string originalImgPath = string.Empty, hostURL = string.Empty, timeStamp = CommonOpn.GetTimeStamp(), priorityList = string.Empty;

        public string SelectedModel
        {
            get
            {
                if (Request.Form["drpModel"] != null && Request.Form["drpModel"].ToString() != "")
                    return Request.Form["drpModel"].ToString();
                else
                    return "-1";
            }
        }

        public string ModelContents
        {
            get
            {
                if (Request.Form["hdn_drpModel"] != null && Request.Form["hdn_drpModel"].ToString() != "")
                    return Request.Form["hdn_drpModel"].ToString();
                else
                    return "";
            }
        }

        public string SelectedVersion
        {
            get
            {
                if (Request.Form["drpVersion"] != null && Request.Form["drpVersion"].ToString() != "")
                    return Request.Form["drpVersion"].ToString();
                else
                    return "-1";
            }
        }

        public string VersionContents
        {
            get
            {
                if (Request.Form["hdn_drpVersion"] != null && Request.Form["hdn_drpVersion"].ToString() != "")
                    return Request.Form["hdn_drpVersion"].ToString();
                else
                    return "";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new System.EventHandler(btnSave_OnClick);
            this.btnUpdate.Click += new System.EventHandler(btnUpdate_OnClick);
            dtgrdFeaturedListing.PageIndexChanged += new DataGridPageChangedEventHandler(Page_Change);
            //this.btnUpdateFeaturedBike.Click += new System.EventHandler(btnUpdateFeaturedBike_OnClick);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            CommonOpn op = new CommonOpn();
            if (Request.QueryString["UpdateId"] != null && Request.QueryString["UpdateId"].ToString() != "")
            {
                updateData = Request.QueryString["UpdateId"].ToString();

                if (!CommonOpn.CheckId(updateData))
                {
                    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/FeaturedListing.aspx");
                }
            }

            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
            //btnUpdateFeaturedBike.Attributes.Add("onclick","javascript:if (ConfirmUpdateFeaturedBike() == false ) return false;");
            if (!IsPostBack)
            {
                FillMakes();
                BindGrid();
                if (updateData != "")
                {
                    FillData();
                    btnUpdate.Enabled = true;
                    btnSave.Enabled = false;
                }
            }

            visibleCount = GetVisibleListingCount();
            GetAllPriority();
        }

        void btnSave_OnClick(object sender, EventArgs e)
        {
            string saveId = "";


            if (String.IsNullOrEmpty(flphoto.PostedFile.FileName))
            {
                lblMessage.Text = "Please Select Image to upload";
            }
            else
            {
                saveId = SaveData("-1");
                Trace.Warn("Save Id : " + saveId);
                if (saveId != "" && saveId != "0")
                {
                    if (UploadImage(saveId))
                    {
                        lblMessage.Text = "Data Saved Successfully";
                    }
                }
            }
            ClearText();
            BindGrid();
        }

        void btnUpdate_OnClick(object sender, EventArgs e)
        {
            string updateId = "";

            if (Convert.ToInt32(visibleCount) < 1 && (chkIsActive.Checked == false || chkIsVisible.Checked == false))
            {
                lblMessage.Text = "UPDATION FAILED!! Atleast one featured listing should be visible and active.";
            }
            else
            {
                if (updateData != "")
                {
                    updateId = SaveData(updateData);
                    Trace.Warn("Update Id : " + updateId);
                    if (updateId != "" && updateId != "0")
                    {
                        if (flphoto.PostedFile.FileName != "")
                        {
                            if (UploadImage(updateId))
                            {
                                lblMessage.Text = "Data updated Successfully";
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Data updated Successfully";
                        }
                    }
                }
            }

            BindGrid();

            ClearText();
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
        }

        void Page_Change(object sender, DataGridPageChangedEventArgs e)
        {
            // Set CurrentPageIndex to the page the user clicked.
            dtgrdFeaturedListing.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }


        void FillMakes()
        {
            CommonOpn op = new CommonOpn();
            string sql;
            sql = "select ID, Name from bikemakes where isdeleted <> 1 order by name";
            try
            {
                op.FillDropDown(sql, drpMake, "Name", "ID");
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            } // catch Exception

            ListItem item = new ListItem("--Select--", "0");
            drpMake.Items.Insert(0, item);
        }

        string SaveData(string updateId)
        {
            string lastSavedId = "";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("con_addfeaturedlisting"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int64, updateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeid", DbType.Int64, chkIsModel.Checked ? SelectedModel : SelectedVersion));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_description", DbType.String, 1000, txtDescription.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismodel", DbType.Boolean, chkIsModel.Checked ? true : false));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isvisible", DbType.Boolean, chkIsVisible.Checked ? true : false));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, chkIsActive.Checked ? true : false));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_entrydatetime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lastsavedid", DbType.Int64, ParameterDirection.Output));


                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    if (cmd.Parameters["par_lastsavedid"].Value.ToString() != "")
                        lastSavedId = cmd.Parameters["par_lastsavedid"].Value.ToString();
                }

            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            return lastSavedId;
        }

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 17th Jan 2014
        /// Summary : To replicate images using RabbitMQ
        /// </summary>
        /// <param name="imgName">Image Id</param>
        /// <returns></returns>
        bool UploadImage(string imgName)
        {
            bool isUploaded = false;
            string fullTempImagePath = "";
            string imgPath = "";

            imgPath = ImagingOperations.GetPathToSaveImages("\\bw\\featured\\");

            //Check the image path is exist or not if not exist create it
            Trace.Warn("imgPath=" + imgPath);
            Trace.Warn("img Name : " + imgName);
            if (!Directory.Exists(imgPath))
            {
                Directory.CreateDirectory(imgPath);
            }

            string tempImageName = GetSelectedBikeName().Replace('/', '-').ToLower() + "-" + imgName + ".jpg";
            fullTempImagePath = imgPath + tempImageName;
            string hostUrl = ConfigurationManager.AppSettings["RabbitImgHostURL"];
            string imageUrl = "http://" + hostUrl + "/bw/featured/";

            flphoto.PostedFile.SaveAs(fullTempImagePath);
            //rabbitmq publishing
            RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
            NameValueCollection nvc = new NameValueCollection();
            //add items to nvc
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ID).ToLower(), imgName);
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "FEATUREDLISTING");
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ONLYREPLICATE).ToLower(), Convert.ToString(true));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl + tempImageName);
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), "/bw/featured/" + tempImageName + "?" + timeStamp);
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMASTER).ToLower(), "1");
            rabbitmqPublish.PublishToQueue(ConfigurationManager.AppSettings["ImageQueueName"], nvc);

            UpdateBikePhotoContent(imgName, hostUrl, "/bw/featured/", tempImageName + "?" + timeStamp);

            //DeleteTempImgs(fullTempImagePath);

            isUploaded = true;

            return isUploaded;
        }

        /// Function to delete the provided file
        void DeleteTempImgs(string imgPath)
        {
            FileInfo tempFile = new FileInfo(imgPath);
            tempFile.Delete();// delete the provided file
        }

        /// <summary>
        ///     Function to get the selected bike name for saving the photo name
        /// </summary>
        /// <returns></returns>
        protected string GetSelectedBikeName()
        {
            string makeName = String.Empty;

            if (drpMake.SelectedValue != "0" && hdn_SelectedModel.Value != "" && hdn_SelectedVersion.Value != "")
            {
                makeName += drpMake.SelectedItem.Text.Replace(" ", "") + "-";
                makeName += hdn_SelectedModel.Value.Split('|')[1].Replace(" ", "") + "-";
                makeName += hdn_SelectedVersion.Value.Split('|')[1].Replace(" ", "");
            }
            else if (drpMake.SelectedValue != "0" && hdn_SelectedModel.Value != "")
            {
                makeName += drpMake.SelectedItem.Text.Replace(" ", "") + "-";
                makeName += hdn_SelectedModel.Value.Split('|')[1].Replace(" ", "");
            }
            Trace.Warn("makeName : ", makeName);
            return makeName;
        }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 31/8/2012
        ///     Function to update the contents of the bike photos
        ///     Modified By : Sadhana Upadhyay on 14th Jan 2014
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hostUrl"></param>
        /// <param name="imagePath"></param>
        /// <param name="largeImage"></param>
        /// <param name="smallImage"></param>
        protected void UpdateBikePhotoContent(string id, string hostUrl, string imagePath, string originalImagePath)
        {
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "con_updatefeaturedlistingphoto";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int64, id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 100, hostUrl));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbType.String, 200, (!String.IsNullOrEmpty(flphoto.PostedFile.FileName)) ? (imagePath + originalImagePath) : Convert.DBNull));

                    MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }   // End of UpdateBikePhotoContent method

        //string GetBikeActualImage()
        //{
        //    string sql = "";
        //    string imgPath = "";

        //    SqlDataReader dr = null;
        //    Database db = new Database();

        //    sql = "SELECT SmallPic FROM BikeModels WHERE ID = " + SelectedModel + "";

        //    try
        //    {
        //        dr = db.SelectQry(sql);	

        //        if(dr.Read())
        //        {
        //            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "bikewale.com" ) >= 0 ) 
        //            {
        //                imgPath = CommonOpn.ImagePathForSavingImages("/models/") + dr["SmallPic"].ToString();
        //            }
        //            else
        //            {
        //                imgPath = CommonOpn.ResolvePhysicalPath("/bikewaleimg/models/") + dr["SmallPic"].ToString();
        //            }

        //            Trace.Warn("imgPath=" + imgPath);
        //        }
        //    }
        //    catch(Exception err)
        //    {
        //        Trace.Warn(err.Message + err.Source);
        //        ErrorClass.LogError(err,Request.ServerVariables["URL"]);
        //        
        //    }
        //    finally
        //    {
        //        if (dr != null)
        //        {
        //            dr.Close();
        //        }
        //        db.CloseConnection();
        //    }

        //    return imgPath;
        //}   // End of GetBikeActualImage function

        string GetVisibleListingCount()
        {
            string sql = "";
            string count = "";

            sql = "select count(id) as TCount from con_featuredlistings as fl where fl.isvisible = 1 and fl.isactive = 1";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr.Read())
                        {
                            count = dr["TCount"].ToString();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }

            return count;
        }   // End of GetVisibleListingCount method

        //Modified By Sadhana Upadhyay on 22 July to get Priorities
        void BindGrid()
        {
            string sql = "";

            int pageSizeM = dtgrdFeaturedListing.PageSize;

            sql = @" SELECT fl.Id, concat(cma.name , ' ' , cmo.name) AS BikeName, IsActive, IsVisible, 
                IsModel, Description, EntryDateTime, ifnull(fl.HostURL,'') as HostURL,ifnull(fl.OriginalImagePath,'') as  OriginalImagePath, fl.DisplayPriority, if(fl.IsReplicated,1,0) as IsReplicated 
				from con_featuredlistings as fl, bikemakes as cma, bikemodels as cmo 
				where fl.bikeid = cmo.id and cmo.bikemakeid = cma.id and fl.ismodel = 1 
				
				UNION ALL 
				
				SELECT fl.Id, concat(cma.name , ' ' , cmo.name , ' ' , cv.name) AS BikeName, IsActive, IsVisible, 
                IsModel, Description, EntryDateTime, ifnull(fl.HostURL,'') as HostURL,ifnull(fl.OriginalImagePath,'') as  OriginalImagePath, fl.DisplayPriority, if(fl.IsReplicated,1,0) as IsReplicated 
				from con_featuredlistings as fl, bikemakes as cma, bikemodels as cmo, bikeversions as cv 
				where fl.bikeid = cv.id and cv.bikemodelid = cmo.id and cmo.bikemakeid = cma.id and fl.ismodel = 0 
                order by isactive desc, displaypriority";

            CommonOpn objCom = new CommonOpn();

            try
            {
                objCom.BindGridSet(sql, dtgrdFeaturedListing, pageSizeM);

                if (dtgrdFeaturedListing.Items.Count > 0)
                {
                    if (dtgrdFeaturedListing.CurrentPageIndex == 0)
                        serialNo = 0;
                    else
                        serialNo = pageSizeM * dtgrdFeaturedListing.CurrentPageIndex;
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }   // End of BindGrid method

        public string GetString(string str)
        {
            if (Convert.ToInt16(str) > 0)
                return "<img src=https://opr.carwale.com/Images/tick.jpg /> ";
            else
                return "<img src=https://opr.carwale.com/images/delete.gif /> ";
        }

        //public string  GetImage( string str )
        //{
        //    string imgPath = "";
        //    string fullPath = "";

        //    if (httpcontext.current.request.servervariables["http_host"].indexof("bikewale.com") >= 0)
        //    {
        //        imgpath = commonopn.imagepath + "featuredbike/";
        //    }
        //    else
        //    {
        //        imgpath = "https://server/images/featured/";
        //    }

        //    fullPath = imgPath + str + ".jpg";
        //    Trace.Warn("imgPath" + fullPath);

        //    return fullPath;
        //}

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 14th Jan 2014
        /// Summary : To get HostUrl, ImagePath, SmallImageName, LargeImageName.
        /// </summary>
        void FillData()
        {
            string sql = "";

            AjaxFunctions aj = new AjaxFunctions();

            if (updateData != "")
            {
                sql = @" select cma.id as MakeId, cmo.id as ModelId, BikeId, if(IsActive,1,0) as IsActive,if(IsVisible,1,0) as  IsVisible,
                    if(IsModel,1,0) as  IsModel, Description, fl.hosturl as hostUrl, fl.OriginalImagePath, fl.largeimagename as largeImgPath 
					from con_featuredlistings as fl, bikemakes as cma, bikemodels as cmo
					where fl.bikeid = cmo.id and cmo.bikemakeid = cma.id and fl.ismodel = 1
					and fl.id = " + updateData +

                    @" union all
				
					select cma.id as MakeId, cmo.id as ModelId, BikeId, if(IsActive,1,0) as IsActive,if(IsVisible,1,0) as  IsVisible,
                    if(IsModel,1,0) as  IsModel, Description, fl.hosturl as hostUrl, fl.OriginalImagePath, fl.largeimagename as largeImgPath 
					from con_featuredlistings as fl, bikemakes as cma, bikemodels as cmo, bikeversions as cv
					where fl.bikeid = cv.id and cv.bikemodelid = cmo.id and cmo.bikemakeid = cma.id and fl.ismodel = 0
					and fl.id = " + updateData;

                CommonOpn objCom = new CommonOpn();

                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                    {
                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {
                                txtDescription.Text = dr["Description"].ToString();
                                drpMake.SelectedValue = dr["MakeId"].ToString();

                                if (Convert.ToBoolean(dr["IsActive"]) == false)
                                {
                                    chkIsActive.Checked = false;
                                }
                                if (Convert.ToBoolean(dr["IsModel"]) == false)
                                {
                                    chkIsModel.Checked = false;

                                    drpModel.DataSource = aj.GetModels(dr["MakeId"].ToString());
                                    drpModel.DataTextField = "Text";
                                    drpModel.DataValueField = "Value";
                                    drpModel.DataBind();
                                    drpModel.Items.Insert(0, new ListItem("Any", "0"));
                                    drpModel.SelectedIndex = drpModel.Items.IndexOf(drpModel.Items.FindByValue(dr["ModelId"].ToString()));

                                    drpVersion.DataSource = aj.GetVersions(dr["ModelId"].ToString());
                                    drpVersion.DataTextField = "Text";
                                    drpVersion.DataValueField = "Value";
                                    drpVersion.DataBind();
                                    drpVersion.Items.Insert(0, new ListItem("Any", "0"));
                                    drpVersion.SelectedIndex = drpVersion.Items.IndexOf(drpVersion.Items.FindByValue(dr["BikeId"].ToString()));

                                    drpModel.Enabled = true;
                                    drpVersion.Enabled = true;
                                }
                                else
                                {
                                    drpModel.DataSource = aj.GetModels(dr["MakeId"].ToString());
                                    drpModel.DataTextField = "Text";
                                    drpModel.DataValueField = "Value";
                                    drpModel.DataBind();
                                    drpModel.Items.Insert(0, new ListItem("Any", "0"));
                                    drpModel.SelectedIndex = drpModel.Items.IndexOf(drpModel.Items.FindByValue(dr["ModelId"].ToString()));

                                    drpVersion.DataSource = aj.GetVersions(dr["ModelId"].ToString());
                                    drpVersion.DataTextField = "Text";
                                    drpVersion.DataValueField = "Value";
                                    drpVersion.DataBind();
                                    drpVersion.Items.Insert(0, new ListItem("Any", "0"));

                                    drpModel.Enabled = true;
                                    drpVersion.Enabled = true;
                                }

                                if (Convert.ToBoolean(dr["IsVisible"]) == false)
                                {
                                    chkIsVisible.Checked = false;
                                }
                                originalImgPath = dr["OriginalImagePath"].ToString();
                                hostURL = dr["hostUrl"].ToString();
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    Trace.Warn(err.Message + err.Source);
                    ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                    
                }
            }
        }

        void ClearText()
        {
            txtDescription.Text = "";
            drpMake.SelectedIndex = 0;
            drpModel.SelectedIndex = 0;
            drpVersion.SelectedIndex = 0;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 24 July 2014
        /// Summary : To get all priority list of featured bikes
        /// </summary>
        protected void GetAllPriority()
        {
            string sql = string.Empty;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    sql = "select displaypriority from con_featuredlistings  where displaypriority is not null and displaypriority <> 0 order by displaypriority";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                if (String.IsNullOrEmpty(priorityList))
                                {
                                    priorityList += dr["DisplayPriority"].ToString();
                                    count++;
                                }
                                else
                                {
                                    priorityList += "," + dr["DisplayPriority"].ToString();
                                    count++;
                                }
                            }
                        }
                    }
                }
                Trace.Warn("priority list : ", priorityList);
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
        }


    }//class
}// namespace