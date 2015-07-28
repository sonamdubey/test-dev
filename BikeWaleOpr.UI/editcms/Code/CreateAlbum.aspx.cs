using System;
using System.Configuration;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using BikeWaleOpr.RabbitMQ;
using RabbitMqPublishing;
using System.Collections.Specialized;

namespace BikeWaleOpr.EditCms
{
    public class CreateAlbum : Page
    {
        #region Variables and Controls
        protected Button btnSave, btnUpdate, btnContinue, btnGallery;
        protected DropDownList ddlCategory, ddlDimensions, ddlMake, ddlModel;
        protected Label lblMessage, lblEditImageId, lblMainImgSet;
        protected HtmlInputFile inpPhoto, fl_HomePgPhoto;
        protected TextBox txtCaption, txtPosition;
        protected CheckBox chkIsMainImg, chkGenWaterMark;
        protected DataList dlstPhoto;
        protected bool isEditImage;
        protected DisplayBasicInfo BasicInfo;
        protected EditCmsCommon EditCmsCommon;
        protected string basicId = string.Empty, HostUrl = string.Empty;
        protected Button btnPosition;
        protected HtmlInputHidden hdnImageId, hdn_selModel;
        protected HtmlGenericControl lblGallery, divMainImage;
        protected HtmlInputText txtImageName;
        public string Gallery = string.Empty, EditImagePath = string.Empty, mainImgId = string.Empty, imageName = string.Empty, imagePathThumbnail = string.Empty, hostUrl = string.Empty, statusId = string.Empty;
        string timeStamp = CommonOpn.GetTimeStamp();

        #endregion

         #region Properties
        public string SelectedModel
        {
            get
            {
                if (Request.Form["ddlModel"] != null && Request.Form["ddlModel"].ToString() != "")
                    return Request.Form["ddlModel"].ToString();
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
        #endregion

        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }
        #endregion

        #region InitializeComponent
        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnPosition.Click += new EventHandler(btnPosition_Click);
            dlstPhoto.DeleteCommand += new DataListCommandEventHandler(dlstPhoto_Delete);
            btnGallery.Click += new EventHandler(btnGallery_Click);
        }
        #endregion

        #region Page Load
        void Page_Load(object Sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
            btnSave.Attributes.Add("onclick", "javascript:if(Validate()==false) return false;");
            btnUpdate.Attributes.Add("onclick", "javascript:if(Validate()==false) return false; else { GetModelVal(); return true; }");

            if (Request["bid"] != null && Request.QueryString["bid"] != "")
            {
                basicId = Request.QueryString["bid"].ToString();

                if (CommonOpn.CheckId(basicId) == false)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            if (!String.IsNullOrEmpty(Request.QueryString["save"]))
            {
                if (Request.QueryString["save"] == "1")
                    lblMessage.Text = "Data Saved Successfully.";
            }

            if (!Page.IsPostBack)
            {
                //if (HttpContext.Current.User.Identity.IsAuthenticated != true)
                //    Response.Redirect("../users/Login.aspx?ReturnUrl=../editcms/basicinfo.aspx");

                //if (Request.Cookies["Customer"] == null)
                //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../editcms/basicinfo.aspx");

                //int pageId = 53;
                //CommonOpn op = new CommonOpn();
                //if (!op.verifyPrivilege(pageId))
                //    Response.Redirect("../NotAuthorized.aspx");

                HostUrl = GetHostUrl(basicId);
                Trace.Warn("HostUrl: " + HostUrl);
                GetMakes();
                LoadCategory();
                FillList();
                //LoadEditImageDetails();
                isEditImage = false;
                if (!String.IsNullOrEmpty(Request.QueryString["EditImageId"]))
                {
                    isEditImage = true;
                    lblEditImageId.Text = Request.QueryString["EditImageId"].ToString();
                    LoadEditImageDetails();
                }
                GetMainImage();
                ShowGallery();
            }
            else
            {
                isEditImage = false;
            }
            EditCmsCommon.BasicId = basicId;
            EditCmsCommon.PageId = 4;
            EditCmsCommon.PageName = "Manage Photos";
        }
        #endregion

        #region Save Button Click
        void btnSave_Click(object Sender, EventArgs e)
        {
            if (inpPhoto.PostedFile.ContentType == "image/gif" || inpPhoto.PostedFile.ContentType == "image/jpeg")
            {
                string saveId = "";
                saveId = SaveData("-1");

                if (saveId != "")
                {
                    GetMainImage();
                    FillList();
                    txtCaption.Text = "";
                    ddlCategory.SelectedIndex = 0;
                    chkIsMainImg.Checked = false;
                    lblMessage.Text = "Data Saved Successfully.";
                }
            }
            else
            {
                lblMessage.Text = "Select valid photo format";
            }
        }
        #endregion

        #region Update Button Click
        void btnUpdate_Click(object Sender, EventArgs e)
        {
            bool isValid = false;
            if (inpPhoto.Value != "" && inpPhoto.PostedFile.ContentType != "image/jpeg")
                isValid = false;
            else
                isValid = true;

            if (inpPhoto.Value != "" && inpPhoto.PostedFile.ContentType != "image/gif" && isValid == false)
                isValid = false;
            else
                isValid = true;

            if (isValid)
            {
                string saveId = "";
                saveId = SaveData(lblEditImageId.Text);

                if (saveId != "")
                {
                    FillList();
                    txtCaption.Text = "";
                    ddlCategory.SelectedIndex = 0;
                    chkIsMainImg.Checked = false;
                    lblMessage.Text = "Data Saved Successfully.";
                    lblEditImageId.Text = "-1";
                    Response.Redirect("/editcms/createalbum.aspx?bid=" + basicId + "&save=1");
                }
            }
            else
            {
                lblMessage.Text = "Select valid photo format";
                isEditImage = true;
            }
        }
        #endregion

        #region Position Button Click
        void btnPosition_Click(object sender, EventArgs e)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();

            string conStr = db.GetConString();

            if (txtPosition.Text != "")
            {
                con = new SqlConnection(conStr);
                try
                {
                    cmd = new SqlCommand("Con_EditCms_UpdateSequence", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    prm = cmd.Parameters.Add("@BasicId", SqlDbType.BigInt);
                    prm.Value = basicId;

                    prm = cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                    prm.Value = int.Parse(hdnImageId.Value.ToString());

                    prm = cmd.Parameters.Add("@ToSequence", SqlDbType.Int);
                    prm.Value = int.Parse(txtPosition.Text);

                    prm = cmd.Parameters.Add("@LastUpdatedBy", SqlDbType.BigInt);
                    prm.Value = CurrentUser.Id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception err)
                {
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    FillList();
                }
            }
        }
        #endregion

        #region Delete Photo From list
        void dlstPhoto_Delete(object sender, DataListCommandEventArgs e)
        {
            string sql = "Update Con_EditCms_Images Set IsActive = 0 WHERE ID=" + dlstPhoto.DataKeys[e.Item.ItemIndex];
            Trace.Warn(dlstPhoto.DataKeys[e.Item.ItemIndex].ToString());
            Database db = new Database();
            try
            {
                db.UpdateQry(sql);
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            FillList();
            GetMainImage();

        }
        #endregion

        #region Show Gallery Button Click
        void btnGallery_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            Database db = new Database();
            string conStr = db.GetConString();

            SqlConnection con;

            con = new SqlConnection(conStr);

            if (btnGallery.Text == "Show Gallery")
            {
                sql = "Update Con_EditCms_Basic Set ShowGallery = 1 Where Id = @BasicId";
            }
            else
            {
                sql = "Update Con_EditCms_Basic Set ShowGallery = 0 Where Id = @BasicId";
            }
            try
            {

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;

                con.Open();

                int check = cmd.ExecuteNonQuery();

                if (check > 0)
                {
                    ShowGallery();
                }
                else
                {
                    lblGallery.InnerText = "Show Gallery Status Not Changed";
                }
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
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        #endregion

        #region GetMakes
        /// <summary>
        /// Gets the list of makes that we need to tag the images with
        /// </summary>
        private void GetMakes()
        {
            BindControls.BindAllMakes(ddlMake);
            ddlModel.Items.Insert(0, new ListItem("--Select Model--", "-1"));
        }
        #endregion

        #region GetHostUrl (Obsolete)
        /// <summary>
        /// Get the host url for the main image. (Obsolete) - Main Images are now part of the Images Table
        /// </summary>
        /// <param name="basicId">Article Id</param>
        /// <returns></returns>
        private string GetHostUrl(string basicId)
        {
            string retVal = string.Empty, sql = string.Empty;
            Database db = new Database();
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand();

            sql = "SELECT HostUrl FROM Con_EditCms_Basic Where Id = @BasicId";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;

            try
            {
                dr = db.SelectQry(cmd);
                while (dr.Read())
                {
                    retVal = dr["HostURL"].ToString();
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("SqlEX: " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("EX: " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                    dr.Close();
                db.CloseConnection();
            }
            Trace.Warn("retVal: " + retVal);
            return retVal;
        }
        #endregion

        #region LoadEditImageDetails (Obsolete)
        /// <summary>
        /// Load the details of the image for Editing (Obsolete) - Images are now deleted and then uploaded again. No Editing.
        /// </summary>
        private void LoadEditImageDetails()
        {
            string sql = "SELECT * FROM Con_EditCms_Images WHERE ID = " + lblEditImageId.Text;
            SqlDataReader dr = null;
            Database db = new Database();
            string makeId = string.Empty, modelId = string.Empty, imageName = string.Empty, imagePath = string.Empty, hostUrl = string.Empty, StatusId=string.Empty;

            try
            {
                dr = db.SelectQry(sql);
                if (dr.Read())
                {
                    txtCaption.Text = dr["Caption"].ToString();
                    ddlCategory.SelectedValue = dr["ImageCategoryId"].ToString();
                    makeId = dr["MakeId"].ToString();
                    modelId = dr["ModelId"].ToString();
                    imageName = dr["ImageName"].ToString();
                    imagePath = dr["ImagePathThumbNail"].ToString();
                    hostUrl = dr["HostUrl"].ToString();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                    dr.Close();
                db.CloseConnection();
            }
            txtImageName.Value = imageName.Split('.')[0].Substring(0, imageName.LastIndexOf('-'));
            EditImagePath = ImagingOperations.GetPathToShowImages("/bikewaleimg/ec/", hostUrl) + imagePath;
            BindControls.BindAllMakes(ddlMake);
            ddlMake.SelectedValue = makeId;
            GetModels(makeId, modelId);
        }
        #endregion

        #region GetModels
        /// <summary>
        /// Get the models with respect to a certain make and select the desired model from that list.
        /// </summary>
        /// <param name="makeId">Make Id for which the models need to be fetched</param>
        /// <param name="modelId">Model Id which needs to be selected from the list of models retrieved</param>
        private void GetModels(string makeId, string modelId)
        {
            string sql = string.Empty;
            Database db = new Database();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();

            sql = "SELECT ID AS Value, Name AS Text FROM BikeModels WHERE IsDeleted = 0 AND BikeMakeId = @MakeId Order by Text";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MakeId", SqlDbType.Int).Value = makeId;

            try
            {
                ds = db.SelectAdaptQry(cmd);
                ddlModel.DataSource = ds.Tables[0];
                ddlModel.DataTextField = "Text";
                ddlModel.DataValueField = "Value";
                ddlModel.DataBind();
                ddlModel.Items.Insert(0, new ListItem("--Select Model--", "0"));
                ddlModel.SelectedValue = modelId;
            }
            catch (SqlException ex)
            {
                ErrorClass objEx = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objEx.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objEx = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objEx.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }
        #endregion



        #region FillList
        /// <summary>
        /// Gets the details of the images that have been saved for the particular article
        /// Modified By : Sadhana Upadhyay on 27th jan 2014
        /// Summary : To get Status Id of images
        /// </summary>
        void FillList()
        {
            CommonOpn op = new CommonOpn();
            Database db = new Database();

            string sql;
            sql = " SELECT CEI.ID, CEI.BasicID, CEI.Caption, CEI.Sequence, CEI.HostURL, CEI.ImageName, CEI.ImagePathThumbNail, CEI.MakeId, CEI.ModelId, CEI.StatusId "
                + " FROM Con_EditCms_Images CEI"
                + " WHERE CEI.BasicId = " + basicId + " AND IsActive = 1 AND IsMainImage = 0 ORDER BY Sequence Asc";

            Trace.Warn(sql);

            try
            {
                op.BindListReader(sql, dlstPhoto);

                if (dlstPhoto.Items.Count > 0) btnContinue.Style.Add("display", "block");
                else btnContinue.Style.Add("display", "none");
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

      

        #region SaveData
        /// <summary>
        /// Saves the data related to an image to the Database
        /// Modified By : Sadhana Upadhyay on 27th Jan 2014
        /// Summary : to add image record in IMG_AllBikePhotos
        /// </summary>
        /// <param name="saveid">-1 in the case of a new image and Image Id in the case of an edit operation</param>
        /// <returns>String(Image Id)</returns>
        string SaveData(string saveid)
        {
            string sql = string.Empty;
            Database db = new Database();
            string lastSavedId = "";
            SqlCommand cmdParam = new SqlCommand();
            bool hasCustomImage = false;
            string conStr = db.GetConString();

            SqlParameter prm;

            try
            {
                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "Con_EditCms_Images_Save";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        prm = cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                        prm.Value = saveid;
                        Trace.Warn("saveid : ", saveid);
                        prm = cmd.Parameters.Add("@BasicId", SqlDbType.BigInt);
                        prm.Value = basicId;

                        prm = cmd.Parameters.Add("@ImageCategoryId", SqlDbType.BigInt);
                        prm.Value = ddlCategory.SelectedItem.Value;

                        prm = cmd.Parameters.Add("@Caption", SqlDbType.VarChar, 250);
                        prm.Value = txtCaption.Text.Trim();

                        prm = cmd.Parameters.Add("@LastUpdatedBy", SqlDbType.BigInt);
                        prm.Value = CurrentUser.Id;

                        prm = cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 250);
                        prm.Value = ConfigurationManager.AppSettings["imgHostURL"];

                        prm = cmd.Parameters.Add("@IsReplicated", SqlDbType.Bit);
                        prm.Value = 0;

                        prm = cmd.Parameters.Add("@ImageId", SqlDbType.BigInt);
                        prm.Direction = ParameterDirection.Output;

                        prm = cmd.Parameters.Add("@MakeId", SqlDbType.Int);
                        prm.Value = ddlMake.SelectedValue;

                        prm = cmd.Parameters.Add("@ModelId", SqlDbType.Int);
                        prm.Value = hdn_selModel.Value;

                        prm = cmd.Parameters.Add("@ImageName", SqlDbType.VarChar, 150);
                        prm.Value = txtImageName.Value.Trim().Replace(' ', '-').ToLower();

                        prm = cmd.Parameters.Add("@IsMainImage", SqlDbType.Bit);
                        prm.Value = chkIsMainImg.Checked;

                        prm = cmd.Parameters.Add("@HasCustomImage", SqlDbType.Bit);
                        prm.Value = ddlDimensions.SelectedValue == "-1" ? false : true;

                        prm = cmd.Parameters.Add("@ImagePath", SqlDbType.VarChar, 50);
                        prm.Value = "/bikewaleimg/ec/" + (chkIsMainImg.Checked ? basicId + "/img/m/" : basicId + "/img/");

                        prm = cmd.Parameters.Add("@StatusId", SqlDbType.TinyInt);
                        prm.Value = 1;

                        prm = cmd.Parameters.Add("@TimeStamp", SqlDbType.VarChar, 25);
                        prm.Value = timeStamp;

                        con.Open();
                        cmd.ExecuteNonQuery();

                        lastSavedId = cmd.Parameters["@ImageId"].Value.ToString();
                    }
                }
                string dirPathCmn = "/bikewaleimg/ec/" + basicId + "/img/ol/";
                string imageNameCmn = txtImageName.Value.Trim().Replace(' ', '-') + "-" + lastSavedId + ".jpg";


                //update to common image datatable here
                //give dir path where original file is saved
                //modify dirpath for common database enteries

                // Insert record in IMG_AllBikePhotos
                BikeCommonRQ bikeRQ = new BikeCommonRQ();
                string rabbitMQUrl = bikeRQ.UploadImageToCommonDatabase(lastSavedId, imageNameCmn, ImageCategories.EDITCMS, dirPathCmn);

                Trace.Warn("Rabbit MQ Path : " + rabbitMQUrl);
                if (inpPhoto.Value != "")
                {
                    UploadPhotoFile(txtImageName.Value.Trim().Replace(' ', '-'), lastSavedId, rabbitMQUrl, out hasCustomImage);
                }
                
            }
            catch (SqlException err)
            {
                Trace.Warn("SqlEX: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("EX: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return lastSavedId;
        }
        #endregion
        
        #region GetMainImage
        /// <summary>
        /// Get the Main Image (if any) and display it
        /// Modified By : Sadhana Upadhyay on 11th Dec 2013
        /// Modified By : Sadhana Upadhyay on 27th Jan 2014 
        /// Summary :  To get StatusId of Main image 
        /// </summary>
        protected void GetMainImage()
        {
            string mainImageString = string.Empty;
            string sql = string.Empty;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr = null;
            Database db = new Database();

            sql = "Select Id, ImageName, ImagePathThumbnail, HostUrl, StatusId from Con_EditCms_Images Where BasicId = @BasicId And IsMainImage = 1";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@BasicId", SqlDbType.BigInt).Value = basicId;
            try
            {
                dr = db.SelectQry(cmd);
                while (dr.Read())
                {
                    mainImgId = dr["Id"].ToString();
                    imageName = dr["ImageName"].ToString();
                    hostUrl = dr["HostUrl"].ToString();
                    imagePathThumbnail = dr["ImagePathThumbnail"].ToString();
                    statusId = dr["StatusId"].ToString();
                }
            }
            catch (SqlException ex)
            {
                Trace.Warn("SqlEx: " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                Trace.Warn("Ex: " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
                db.CloseConnection();
            }

        }
        #endregion

        #region LoadCategory
        /// <summary>
        /// Load all the categories that are related to Images
        /// </summary>
        private void LoadCategory()
        {
            CommonOpn op = new CommonOpn();
            string sql = "SELECT Id, Name FROM Con_PhotoCategory";

            try
            {
                op.FillDropDown(sql, ddlCategory, "Name", "Id");
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            ListItem item = new ListItem("Select", "0");
            ddlCategory.Items.Insert(0, item);
        }
        #endregion

        /*
			Storage location of Images:
			
			Large images: http://img.carwale.com/bikewaleimg/ec/<basicId>/img/l/<image_name>  
			Main images: http://img.carwale.com/bikewaleimg/ec/<basicId>/img/m/<image_name>
			Thumbnail images: http://img.carwale.com/bikewaleimg/ec/<basicId>/img/t/<image_name>
            Custom images: http://img.carwale.com/bikewaleimg/ec/<basicId>/img/c/<image_name>
            Original images: http://img.carwale.com/bikewaleimg/ec/<basicId>/img/ol/<image_name>						
		*/

//        #region UploadPhotoFile
//        /// <summary>
//        /// Resizes the images chosen by the user and uploads the files to the server
//        /// </summary>
//        /// <param name="imageName">Name of the file after the upload</param>
//        /// <param name="photoId">Image Id</param>
//        /// <param name="hasCustomImage">Whether the image has a custom size other than the usual ones.</param>
//        /// <returns>True/False</returns>
//        bool UploadPhotoFile(string imageName, string photoId, out bool hasCustomImage)
//        {
//            CommonOpn co = new CommonOpn();
//            bool isCompleted;
//            hasCustomImage = false;
//            try
//            {
//                string selectedFileName = inpPhoto.Value;
//                string fileTempName = "Temp_" + photoId;
//                //string galleryPath = CommonOpn.ImagePathForSavingImagesEditCms("bikeWa/ec/" + basicId + "/img/");
//                //string galleryPath = ImagingFunctions.ImagePathForSavingImages("/bikewaleimg/ec/" + basicId + "/img/");
//                string galleryPath = ImagingOperations.GetPathToSaveImages("\\bikewaleimg\\ec\\" + basicId + "\\img\\");
//                Trace.Warn("galleryPath : ", galleryPath);

//                if (!Directory.Exists(galleryPath))
//                    Directory.CreateDirectory(galleryPath);

//                if (!Directory.Exists(galleryPath + "l//"))
//                    Directory.CreateDirectory(galleryPath + "l//");

//                if (!Directory.Exists(galleryPath + "t//"))
//                    Directory.CreateDirectory(galleryPath + "t//");

//                if (!Directory.Exists(galleryPath + "m//"))
//                    Directory.CreateDirectory(galleryPath + "m//");

//                if (!Directory.Exists(galleryPath + "ol//"))
//                    Directory.CreateDirectory(galleryPath + "ol//");

//                if (ddlDimensions.SelectedValue != "-1")
//                {
//                    if (!Directory.Exists(galleryPath + "c//"))
//                    {
//                        Directory.CreateDirectory(galleryPath + "c//");
//                    }
                    
//                    if (!Directory.Exists(galleryPath + "c200//"))
//                    {
//                        Directory.CreateDirectory(galleryPath + "c200//");
//                        Trace.Warn("c200 directory created");
//                    }

//                    if (!Directory.Exists(galleryPath + "c140//"))
//                    {
//                        Directory.CreateDirectory(galleryPath + "c140//");
//                        Trace.Warn("c140 directory created");
//                    }
//                }

//                string tempFilePath = galleryPath + fileTempName + "." + selectedFileName.Split('.')[1];
//                inpPhoto.PostedFile.SaveAs(tempFilePath);

//                string wdt = string.Empty, ht = string.Empty;
//                string[] size;

//                if (chkIsMainImg.Checked)
//                {
////                    ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "m\\" + imageName + "-" + photoId + ".jpg", 100, 75);
//                    ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "m\\" + imageName + "-" + photoId + ".jpg", 88, 59);
//                    ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "m\\" + imageName + "-" + photoId + "_m.jpg", 160, 120);
//                    if (chkGenWaterMark.Checked)
//                    {
//                        ImagingFunctions.AddWatermark(tempFilePath, galleryPath + "m\\" + imageName + "-" + photoId + "_l.jpg", Server.MapPath("/common/bw_watermark.png"), 620, 400, 4);
//                    }
//                    else
//                    {
//                        ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "m\\" + imageName + "-" + photoId + "_l.jpg", 620, 400);
//                    }
//                    if (ddlDimensions.SelectedValue != "-1")
//                    {
//                        hasCustomImage = true;

//                        if (ddlDimensions.SelectedValue == "500|270")
//                        {
//                            Trace.Warn("custom image path : " + galleryPath + "m\\" + imageName + "-" + photoId + "_c.jpg");
//                            fl_HomePgPhoto.PostedFile.SaveAs(galleryPath + "m\\" + imageName + "-" + photoId + "_c.jpg");
                            
//                            string tmpPath = galleryPath + "m\\" + imageName + "-" + photoId + "_c.jpg";
//                            Trace.Warn("img path custom : ", tmpPath);
//                            ImagingFunctions.GenerateThumbnail(tmpPath, galleryPath + "m\\" + imageName + "-" + photoId + "_c200.jpg", 200, 125);
//                            Trace.Warn("img path custom 200 : ", galleryPath + "m\\" + imageName + "-" + photoId + "_c200.jpg");
//                            ImagingFunctions.GenerateThumbnail(tmpPath, galleryPath + "m\\" + imageName + "-" + photoId + "_c140.jpg", 140, 90);
//                            Trace.Warn("img path custom 140 : ", galleryPath + "m\\" + imageName + "-" + photoId + "_c140.jpg");
//                        }
//                        else
//                        {
//                            size = ddlDimensions.SelectedValue.ToString().Split('|');
                            
//                            wdt = size[0];
//                            ht = size[1];
//                            ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "m\\" + imageName + "-" + photoId + "_c.jpg", int.Parse(wdt), int.Parse(ht));
//                        }
//                    }
//                    string propHeight = SaveOriginalImage(tempFilePath, galleryPath + "m\\" + imageName + "-" + photoId + "_ol.jpg", 1600);
//                }
//                else
//                {
//                    if (chkGenWaterMark.Checked)
//                    {
//                        ImagingFunctions.AddWatermark(tempFilePath, galleryPath + "l\\" + imageName + "-" + photoId + ".jpg", Server.MapPath("/common/bw_watermark.png"), 620, 400, 4);
//                    }
//                    else
//                    {
//                        ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "l\\" + imageName + "-" + photoId + ".jpg", 620, 400);
//                    }
//                    ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "t\\" + imageName + "-" + photoId + ".jpg", 160, 120);
//                    Trace.Warn("Thumbnail Done");
//                    if (ddlDimensions.SelectedValue != "-1")
//                    {
//                        hasCustomImage = true;
//                        Trace.Warn("ddlDim: " + ddlDimensions.SelectedValue);
//                        size = ddlDimensions.SelectedValue.ToString().Split('|');
//                        wdt = size[0];
//                        ht = size[1];
//                        ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "c\\" + imageName + "-" + photoId + ".jpg", int.Parse(wdt), int.Parse(ht));

//                        ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "c200\\" + imageName + "-" + photoId + ".jpg", 200, 125);
//                        Trace.Warn("img path custom 200 : ", galleryPath + "c200\\" + imageName + "-" + photoId + ".jpg");
//                        ImagingFunctions.GenerateThumbnail(tempFilePath, galleryPath + "c140\\" + imageName + "-" + photoId + ".jpg", 140, 90);
//                        Trace.Warn("img path custom 140 : ", galleryPath + "c140\\" + imageName + "-" + photoId + ".jpg");
//                    }
//                    string propWdtHt = SaveOriginalImage(tempFilePath, galleryPath + "ol\\" + imageName + "-" + photoId + ".jpg", 1600);
//                    Trace.Warn("Original Image Saved");
//                    string imageSizes = string.Empty;
//                    imageSizes = "620,400|160,120|" + wdt + "," + ht + "|" + propWdtHt;
//                    SaveImageSizes(imageSizes, photoId);
//                }
//                File.Delete(tempFilePath);
//                isCompleted = true;
//            }
//            catch (Exception err)
//            {
//                Trace.Warn("Error: " + err.Message);
//                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
//                objErr.SendMail();
//                isCompleted = false;
//            }
//            return isCompleted;
//        }
//        #endregion

        /// <summary>
        /// Created By : Sadhana Upadhyay on 27th Jan 2014
        /// Summary : To add image Replication Function
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="photoId"></param>
        /// <param name="hasCustomImage"></param>
        /// <returns></returns>
        bool UploadPhotoFile(string imageName, string photoId, string rabbitMqPath, out bool hasCustomImage)
        {
            CommonOpn co = new CommonOpn();
            bool isCompleted;
            hasCustomImage = false;
            try
            {
                string selectedFileName = inpPhoto.Value;
                string fileTempName = "Temp_" + photoId;
                //path till image folder
                string galleryPath = ImagingOperations.GetPathToSaveImages("/bikewaleimg/ec/" + basicId + "/img/");
                Trace.Warn("Gallery Path : "+ galleryPath);
                if (!Directory.Exists(galleryPath + "ol//"))
                    Directory.CreateDirectory(galleryPath + "ol//");

                string tempFilePath = galleryPath + "/ol/" + imageName + "-" + photoId + ".jpg";
                //save the file 
                if (chkIsMainImg.Checked && ddlDimensions.SelectedValue == "500|270")
                    fl_HomePgPhoto.PostedFile.SaveAs(tempFilePath);
                else
                    inpPhoto.PostedFile.SaveAs(tempFilePath);
                //path to be passed in rabbitq
                //publish to rabbitmq here
                NameValueCollection nvc = new NameValueCollection();
                RabbitMqPublish rabbitMqPublish = new RabbitMqPublish();
                //RabbitMqImagePublish rabbitmqPublish = new RabbitMqImagePublish();
                //nvc keys should be in small letters
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ID).ToLower(), photoId);
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "EditCms");
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), rabbitMqPath);
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), ddlDimensions.SelectedValue.ToString().Split('|')[0]);
                if (ddlDimensions.SelectedValue.ToString().Split('|')[0] != "-1")
                {
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), ddlDimensions.SelectedValue.ToString().Split('|')[1]);

                }
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(chkGenWaterMark.Checked));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(chkIsMainImg.Checked));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(true));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISSPECIALIMAGE).ToLower(), Convert.ToString(true));

                isCompleted = rabbitMqPublish.PublishToQueue("BikeImage", nvc);
                Trace.Warn("RabbitMq Status : " + isCompleted.ToString());

                isCompleted = true;
            }
            catch (Exception err)
            {
                Trace.Warn("Error: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
                isCompleted = false;
            }
            return isCompleted;
        }

        #region SaveOriginalImage
        /// <summary>
        /// Save the Original Image Uploaded by the user to a pre defined location
        /// </summary>
        /// <param name="savedLocation">Location to save the image</param>
        /// <param name="targetLocation">Temp location where the image was uploaded</param>
        /// <param name="desiredWidth">Width to which the image size must be restricted.</param>
        /// <returns>String(Width,Height)</returns>
        private string SaveOriginalImage(string savedLocation, string targetLocation, int desiredWidth)
        {
            System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(savedLocation);
            SizeF sz = imgOriginal.PhysicalDimension;
            decimal ratio;
            int width = 0;
            int height = 0;

            if (desiredWidth < int.Parse(sz.Width.ToString()))
            {
                ratio = (decimal)desiredWidth / (decimal)sz.Height;
                width = desiredWidth;
                decimal temp = (decimal)sz.Width * ratio;
                height = (int)temp;
            }
            else
            {
                width = int.Parse(sz.Width.ToString());
                height = int.Parse(sz.Height.ToString());
            }

            System.Drawing.Image thumbnail = new Bitmap(width, height);
            Graphics graphic = Graphics.FromImage(thumbnail);
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            graphic.DrawImage(imgOriginal, rectangle);
            thumbnail.Save(targetLocation, ImageFormat.Jpeg);
            imgOriginal.Dispose();
            graphic.Dispose();
            return width.ToString() + "," + height.ToString();
        }
        #endregion

        #region SaveImageSizes
        /// <summary>
        /// Saves the different sizes that image has been resized to, to the database.
        /// </summary>
        /// <param name="imageSizes">Different sizes(width,height) separated by | symbol</param>
        /// <param name="photoId">Image Id</param>
        private void SaveImageSizes(string imageSizes, string photoId)
        {
            string sql = string.Empty;
            Database db = new Database();
            bool isSizeSaved = false;
            SqlCommand cmd = new SqlCommand();

            sql = "Insert Into Con_EditCms_ImageSizes (ImageId, ImageWidth, ImageHeight) Values (@ImageId, @ImageWidth, @ImageHeight)";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            string[] sizes = imageSizes.Split('|');

            try
            {
                for (int i = 0; i < sizes.Length; ++i)
                {
                    string imgWidth = sizes[i].Split(',')[0].ToString();
                    string imgHeight = sizes[i].Split(',')[1].ToString();
                    cmd.Parameters.Add("@ImageId", SqlDbType.BigInt).Value = photoId;
                    cmd.Parameters.Add("@ImageWidth", SqlDbType.Int).Value = imgWidth;
                    cmd.Parameters.Add("@ImageHeight", SqlDbType.Int).Value = imgHeight;

                    isSizeSaved = db.InsertQry(cmd);
                }
            }
            catch (SqlException err)
            {
                Trace.Warn("Error: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("Error: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }
        #endregion

        #region ShowGallery
        /// <summary>
        /// Method to check if the Photo Gallery page of the article needs to be shown or not
        /// </summary>
        void ShowGallery()
        {
            string sql = string.Empty;
            Database db = new Database();
            bool showGallery = false;

            try
            {
                sql = "Select ShowGallery From Con_EditCms_Basic Where Id = @BasicId";
                SqlParameter[] param = 
				{
					new SqlParameter("@BasicId", basicId)
				};

                SqlDataReader dr = db.SelectQry(sql, param);

                if (dr.Read())
                {
                    showGallery = (bool)dr["ShowGallery"];

                    if (showGallery)
                    {
                        Gallery = "Yes";
                        btnGallery.Text = "Don't Show Gallery";
                        GetMainImage();
                    }
                    else
                    {
                        Gallery = "No";
                        btnGallery.Text = "Show Gallery";
                    }
                }
                dr.Close();
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
            }
            finally
            {
                db.CloseConnection();
            }
        }
        #endregion

    } // class
} // namespace