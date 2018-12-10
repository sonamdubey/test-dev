using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.IO;
using RabbitMqPublishing;
using System.Collections.Specialized;
using Carwale.Notifications;
using Carwale.UI.Common;
using CarwaleAjax;
using Carwale.Utility;
using Carwale.Entity.Forums;
using Carwale.BL.Forums;
using Carwale.Entity.Enum;
using Carwale.BL.Customers;

namespace Carwale.UI.Users
{
    public class EditUserProfile : Page
    {
        protected TextBox txtAboutMe, txtSignature;// txtHandle;
        protected Label lblMessage;
        protected HtmlInputFile flAvtar, flReal;
        bool[] thumbAvailable = { false, false, false }; // {75,160,400}
        protected HtmlInputHidden hdnChk;
        protected Button btnSave;
        protected HtmlImage imAv, imRp;
        public UserProfile result;
        UserProfile param = null;
        public string currentUser = "";
        protected string userId = string.Empty;
        protected string userIdReal = string.Empty;
        protected string imgCategory = "6";
        protected string statusId = "1";
        string avtarImageName = "";
        string thumbImageName = "";
        string realImageName = "";
        string HostURL = "";
        string realOriginalImgPath = "";
        string avtOriginalImgPath = "";
        bool isAvtUpdated = false;
        bool isRealUpdated = false;
        string photoNameConstant = "";
        protected HiddenField setToken;
        public string ThumbNail
        {
            get
            {
                if (ViewState["ThumbNail"] != null && ViewState["ThumbNail"].ToString() != "")
                    return ViewState["ThumbNail"].ToString();
                else
                    return "";
            }
            set { ViewState["ThumbNail"] = value; }
        }

        public string AvtarPhoto
        {
            get
            {
                if (ViewState["AvtarPhoto"] != null && ViewState["AvtarPhoto"].ToString() != "")
                    return ViewState["AvtarPhoto"].ToString();
                else
                    return "";
            }

            set { ViewState["AvtarPhoto"] = value; }
        }

        public string RealPhoto
        {
            get
            {
                if (ViewState["RealPhoto"] != null && ViewState["RealPhoto"].ToString() != "")
                    return ViewState["RealPhoto"].ToString();
                else
                    return "";
            }

            set { ViewState["RealPhoto"] = value; }
        }

        public string TokenString
        {
            get {
                if (ViewState["tokenstring"] == null) ViewState["tokenstring"] = CustomerSecurity.getRandomString(10);
                    return ViewState["tokenstring"].ToString();   
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            lblMessage.Text = "";
            imgCategory = Convert.ToInt16(ImageCategories.FORUMSREALIMAGE).ToString();
            userId = CurrentUser.Id;
            userIdReal = CurrentUser.Id;

            //Ajax 	
            Ajax.Utility.RegisterTypeForAjax(typeof(AjaxForum));

            if (!IsPostBack)
            {
                if (Request.HttpMethod == "POST") Response.Redirect("/mycarwale/MyContactDetails.aspx", true);
                setToken.Value = TokenString;
                if (CurrentUser.Id == "-1")
                    Response.Redirect("Login.aspx?ReturnUrl=/users/EditUserProfile.aspx");
                else
                {
                    GetExistingProfileDetails();
                }

                currentUser = CurrentUser.Name;
            }
            else {
                if (setToken.Value != TokenString) Response.Redirect("/users/EditUserProfile.aspx", true);
            }
            photoNameConstant = DateTime.Now.ToString("ssmm") + CarwaleSecurity.EncryptUserId(long.Parse(CurrentUser.Id));
        }

        void btnSave_Click(object Sender, EventArgs e)
        {
            //upload image only if user is loged in otherwise force user to login
            if (CurrentUser.Id != "-1")
            {
                string commonPath = ConfigurationManager.AppSettings["CarwaleImgAbsolutePath"].ToString() + "\\cw\\userProfile\\";
                string avtarAbsolutePath = commonPath + "avtar\\";//str2
                string realAbsolutePath = commonPath + "real\\";//str3            
                if (isAvtUpdated == true)
                {
                    DeleteExistingImages(CurrentUser.Id, avtarAbsolutePath, "", AvtarPhoto, "");
                }
                if (isRealUpdated == true)
                {
                    DeleteExistingImages(CurrentUser.Id, "", realAbsolutePath, "", RealPhoto);
                }

                if (flAvtar.PostedFile.FileName != "")
                {
                    avtarImageName = "avt" + this.photoNameConstant + ".jpg";
                    AvtarPhoto = this.avtarImageName;
                }
                if (flReal.PostedFile.FileName != "")
                {
                    thumbImageName = photoNameConstant;
                    realImageName = photoNameConstant;
                    ThumbNail = thumbImageName;
                    RealPhoto = realImageName;
                }
                InsertImages(avtarImageName, thumbImageName, realImageName);
                // upload avtar image
                UploadAvtarImages(avtarAbsolutePath);
                // Upload thumbnail and real image
                UploadRealImages(realAbsolutePath);
                //Delete Existing Images

                // Insert details to the datatable
                Response.Redirect("/mycarwale/MyContactDetails.aspx");
            }
            else
            {
                Response.Redirect("/Login.aspx?ReturnUrl=/users/EditUserProfile.aspx");
            }
        }

        void UploadAvtarImages(string avtarImgPath)
        {            
            if (Directory.Exists(avtarImgPath) == false)
            {
                Directory.CreateDirectory(avtarImgPath);
            }

            if (flAvtar.PostedFile.FileName != "")
            {
                isAvtUpdated = true;
                string tempImageName = "tmp" + photoNameConstant + ".jpg";
                string fullTempImagePath = avtarImgPath + tempImageName;
                string finalAvtarImgPath = avtarImgPath + avtarImageName;
                flAvtar.PostedFile.SaveAs(finalAvtarImgPath);
           
                #region with  rabbitMQ
                if (ConfigurationManager.AppSettings["RabbitMQImage"].ToLower() == "true")
                {                   
                    string imageUrl = "http://"+Utility.Network.GetMachineIP()+":"+System.Configuration.ConfigurationManager.AppSettings["localimgsiteport"] + "/CW/userProfile/avtar/" + avtarImageName;
                    RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                    NameValueCollection nvc = new NameValueCollection();
           
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ID).ToLower(), CurrentUser.Id);
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "AvtarImage");
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl);
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(false));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ONLYREPLICATE).ToLower(), Convert.ToString(true));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), "/c/up/a/" + avtarImageName);
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ISMASTER).ToLower(), Convert.ToString(1));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ASPECTRATIO).ToLower(), "1.777");
                    rabbitmqPublish.PublishToQueue(ConfigurationManager.AppSettings["IPCQueueName"].ToString(), nvc);
                }
                #endregion
            }
        }

        void UploadRealImages(string realImgPath)
        {           
            if (Directory.Exists(realImgPath) == false)
            {
                Directory.CreateDirectory(realImgPath);
            }
            if (flReal.PostedFile.FileName != "")
            {
                isRealUpdated = true;
                string tempImageName = "tmp" + photoNameConstant + ".jpg";
                string fullTempImagePath = realImgPath + tempImageName;
                string finalRealImgPathT = realImgPath + photoNameConstant + "_75.jpg";
                string finalRealImgPathM = realImgPath + photoNameConstant + "_160.jpg";
                string finalRealImgPathB = realImgPath + photoNameConstant + "_400.jpg";
                string finalRealImgPath = realImgPath + photoNameConstant + ".jpg";
                string finalRealImgName = photoNameConstant + ".jpg";

                // Upload image as a temp image to resize later
                flReal.PostedFile.SaveAs(finalRealImgPath);
                if (ConfigurationManager.AppSettings["RabbitMQImage"].ToLower() == "true")
                {
                    string imageUrl = CommonRQ.UploadImageToCommonDatabase(CurrentUser.Id, finalRealImgName, ImageCategories.FORUMSREALIMAGE, "/CW/userProfile/real/");
                    RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ID).ToLower(), CurrentUser.Id);
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "ForumsRealImage");
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl);
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(false));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ISMASTER).ToLower(), Convert.ToString(1));
                    nvc.Add(CommonRQ.GetDescription(ImageKeys.ASPECTRATIO).ToLower(), "1.777");
                    rabbitmqPublish.PublishToQueue(ConfigurationManager.AppSettings["IPCQueueName"].ToString(), nvc);
                }

                else
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(fullTempImagePath);
                    SizeF sz = img.PhysicalDimension;
                    int tempImgWidth = Convert.ToInt32(sz.Width);
                    int tempImgHeight = Convert.ToInt32(sz.Height);                 
                    int actualWidthT = 75;
                    // function to generate thumbnail
                    ImageFunctions.GenerateSquareThumbnail(fullTempImagePath, finalRealImgPathT, actualWidthT);                 
                    int actualWidthM = 160;
                    int actualHeightM = 120;
                    ImageFunctions.GenerateThumbnail(fullTempImagePath, finalRealImgPathM, actualWidthM, actualHeightM);
                    //Big Image size
                    int actualWidthB = 400;
                    int actualHeightB = 300;
                    // function to generate main image
                    ImageFunctions.GenerateThumbnail(fullTempImagePath, finalRealImgPathB, actualWidthB, actualHeightB);
                    img.Dispose(); 
                    DeleteTempImgs(fullTempImagePath);
                }
            }
        }

        /// Function to delete the provided file
        void DeleteTempImgs(string imgPath)
        {
            FileInfo tempFile = new FileInfo(imgPath);
            tempFile.Delete();// delete the provided file
        }

        void DeleteExistingImages(string userId, string avtarImagePath, string realImagePath, string avtarImgName, string realImgName)
        {
            string avtarImgActualPath = "";
            string thumbImgActualPath = "";
            string realImgActualPath = "";
            string realImgActualPathB = "";

            avtarImgActualPath = avtarImagePath + avtarImgName;
            thumbImgActualPath = realImagePath + realImgName + "_75.jpg";
            realImgActualPath = realImagePath + realImgName + "_160.jpg";
            realImgActualPathB = realImagePath + realImgName + "_400.jpg";

            try
            {
                Trace.Warn("Deleteing Images" + avtarImgActualPath);
                if (avtarImgName != "")
                {
                    DeleteTempImgs(avtarImgActualPath);
                }
                if (realImgName != "")
                {
                    DeleteTempImgs(thumbImgActualPath);
                    DeleteTempImgs(realImgActualPath);
                    DeleteTempImgs(realImgActualPathB);
                }
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        void InsertImages(string avtar, string thumbnail, string real)
        {
            bool result = false;
            string userId = CurrentUser.Id;
            try
            {
                param = new UserProfile();
                param.AboutMe = txtAboutMe.Text.Trim();
                param.Signature = txtAboutMe.Text.Trim();
                param.AvtarPhoto = avtar == "" ? AvtarPhoto : avtar;
                param.RealPhoto = real == "" ? RealPhoto : real;
                param.ThumbNailUrl = thumbnail == "" ? ThumbNail : thumbnail;
                param.HostURL = ConfigurationManager.AppSettings["CDNHostURL"].ToString();
                UserBusinessLogic userDetails = new UserBusinessLogic();
                result = userDetails.InsertImages(userId, param);
            }           
            catch (Exception err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } 
            if (result)
                ReturnUrl();
        }

        public void GetExistingProfileDetails()
        {
            result = new UserProfile();
            try
            {
                UserBusinessLogic userDetails = new UserBusinessLogic();
                result = userDetails.GetProfileDetails(Convert.ToInt32(CurrentUser.Id));
                if (result != null)
                {
                    txtAboutMe.Text = result.AboutMe.ToString();
                    txtSignature.Text = result.Signature.ToString();
                    AvtarPhoto = result.AvtarPhoto.ToString();
                    RealPhoto = result.RealPhoto.ToString();
                    HostURL = result.HostURL.ToString();
                    statusId = result.StatusId.ToString();
                    avtOriginalImgPath = result.AvtOriginalImgPath.ToString();
                    realOriginalImgPath = result.RealOriginalImgPath.ToString();
                    if (avtOriginalImgPath != null && avtOriginalImgPath != "")
                        imAv.Src = ImageSizes.CreateImageUrl(HostURL, ImageSizes._160X89, avtOriginalImgPath);
                    else
                        imAv.Src = "https://" + HostURL + "/c//up//no.jpg";
                    if (realOriginalImgPath != null && realOriginalImgPath != "")
                        imRp.Src = ImageSizes.CreateImageUrl(HostURL, ImageSizes._160X89, realOriginalImgPath);
                    else
                        imRp.Src = "https://" + HostURL + "/c//up//no.jpg";
                }
            }
           catch (Exception err)
            {
                var objErr = new ExceptionHandler(err, "");
                objErr.LogException();
            }
        }     
        void ReturnUrl()
        {
            if ((Request["ReturnUrl"] != null) && (Request.QueryString["ReturnUrl"] != ""))
            {
                string returnUrl = Request.QueryString["ReturnUrl"];

                //validating return url
                if (ScreenInput.IsValidRedirectUrl(returnUrl) == true)
                {
                    Response.Redirect(returnUrl);
                }
                else
                {
                    Response.Redirect("/users/EditUserProfile.aspx");
                }
            }
        }
    }
}