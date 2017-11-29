using Bikewale.Common;
using Bikewale.RabbitMQ;
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

namespace Bikewale.Used
{
    public class UploadBasic : Page
    {
        //Web controls
        protected Button btnUpload;
        protected Repeater rptImageList;

        //html controls
        protected HtmlInputFile fileInput2;
        protected HtmlGenericControl divAlertMsg, div_Photos, div_NotAuthorised;

        public string inquiryId = "-1";
        bool isDealer = false;

        public ClassifiedInquiryPhotos objPhotos;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            this.btnUpload.Click += new EventHandler(btnUpload_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            div_NotAuthorised.Visible = false;

            inquiryId = CookiesCustomers.SellInquiryId;
            Trace.Warn("inquiryId cookies : ", inquiryId);

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                if (CurrentUser.Id == "-1")
                {
                    Response.Redirect("/users/login.aspx?ReturnUrl=/used/sell/default.aspx?id=" + Request.QueryString["id"] + "#uploadphoto");
                }
                else
                {
                    inquiryId = Request.QueryString["id"];
                    CookiesCustomers.SellInquiryId = inquiryId;
                }
            }

            Trace.Warn("inquiry id : ", inquiryId);

            if (!IsPostBack)
            {
                if ((CurrentUser.Id == "-1" && !CommonOpn.CheckId(CookiesCustomers.SellInquiryId)))
                {
                    Response.Redirect("default.aspx");
                }
            }

            BindPhotos();
        }

        // Modified By : Sadhana Upadhyay to pass isApproved flag
        void BindPhotos()
        {
            if (CanEditListing())
            {
                objPhotos = new ClassifiedInquiryPhotos();
                Trace.Warn("BindPhotos inquiry id : ", inquiryId);
                bool isAprooved = false;
                objPhotos.BindWithRepeater(inquiryId, isDealer, rptImageList, isAprooved);

                Trace.Warn("rptImageList count : " + rptImageList.Items.Count.ToString());
            }
            else
            {
                div_NotAuthorised.InnerHtml = "<h3 class='grey-bg border-light padding5'>You are not authorized to edit this listing.</h3>";
                div_NotAuthorised.Visible = true;

                div_Photos.Visible = false;
            }
        }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 20/9/2012
        ///     Function will check whether customer can edit the current listing or not.
        ///     Modified By : Sadhana Upadhyay on 9 Oct 2014
        ///     Summary : added filter for fake customer in query
        /// </summary>
        /// <returns></returns>
        protected bool CanEditListing()
        {
            bool canEdit = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = @"select si.id from classifiedindividualsellinquiries as si  
                                    inner join customers c on c.id = si.customerid
                                    where si.customerid = @customerid and si.id = @inquiryid and c.isfake = 0 ";

                    cmd.Parameters.Add(DbFactory.GetDbParam("@customerid", DbType.Int32, CurrentUser.Id == "-1" ? CookiesCustomers.CustomerId : CurrentUser.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@inquiryid", DbType.Int32, inquiryId));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            canEdit = true;
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn("err : " + err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                Trace.Warn("err : " + err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
            }

            return canEdit;
        }

        void btnUpload_Click(object Sender, EventArgs e)
        {
            if (UploadPhotoFile(inquiryId))
            {
                Trace.Warn("file upload successfull");
            }
            else
            {
                Trace.Warn("Wrong file extension. Operation aborted");
            }
        }

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 11 Aug 2015
        /// Summary : To save Bike Image 
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        bool UploadPhotoFile(string inquiryId)
        {
            bool isCompleted;
            string photoId = "-1";

            OrginalFileName = fileInput2.Value;

            divAlertMsg.InnerText = "";

            // if file is not selected by the user or user trying to upload wrong file extension
            // abort further operation. Alert user           
            if (OrginalFileName == "")
            {
                divAlertMsg.Visible = true;
                divAlertMsg.InnerText = "Please select a file to upload.";

                return false;
            }
            else if (!Bikewale.Common.ImagingFunctions.IsValidFileExtension(OrginalFileName))
            {
                divAlertMsg.Visible = true;
                divAlertMsg.InnerText = "You are trying to upload invalid file. We accept only jpg, gif and png file formats.";
                return false;
            }

            try
            {
                //FolderPath = Server.MapPath("~/bikewaleimg/used/").ToLower() + "S" + inquiryId + @"\\";
                FolderPath = ImagingFunctions.GetPathToSaveImages("\\bw\\used\\S" + inquiryId + @"\\");
                Trace.Warn("FolderPath", FolderPath);

                Trace.Warn("host : ", Request.ServerVariables["HTTP_HOST"]);
                if (Request.ServerVariables["HTTP_HOST"].IndexOf("localhost") < 0)
                {
                    Trace.Warn("server ...");
                    if (FolderPath.IndexOf("bikewale") >= 0)
                        FolderPath = FolderPath.Replace("\\bikewale\\", "\\carwale\\");
                }

                //path for original image has been created like img.aeplcdn.com/bikewale/*

                if (!Directory.Exists(FolderPath))
                    Directory.CreateDirectory(FolderPath);

                // Get the index last index position of .(Dot)
                // Extract file extension from orginal file
                // Get the index last index position of .(Dot)
                // Extract file extension from orginal file
                int lastDotIndex = OrginalFileName.LastIndexOf('.');
                FileExtension = OrginalFileName.Substring(lastDotIndex, OrginalFileName.Length - lastDotIndex);

                FileNameTime = inquiryId + "_" + DateTime.Now.ToString("yyyyMMddhhmmssfff");

                Trace.Warn("FolderPath : ", FolderPath);
                Trace.Warn("FileSavedLocation : ", FileSavedLocation);

                //FileSavedLocation is FolderPath + FileNameTime + FileExtension

                fileInput2.PostedFile.SaveAs(FileSavedLocation);

                string dirPath = "/bw/used/S" + inquiryId + "/";

                SellBikeCommon objSell = new SellBikeCommon();
                photoId = objSell.SaveBikePhotos(inquiryId, dirPath + OriginalImageName, "", false, false);

                string imageUrl = BikeCommonRQ.UploadImageToCommonDatabase(photoId, FileNameTime + FileExtension, ImageCategories.BIKEWALESELLER, dirPath);

                if (Uri.IsWellFormedUriString(imageUrl, UriKind.RelativeOrAbsolute))
                {
                    //RabbitMq Publish code here
                    RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ID).ToLower(), photoId);
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "BikeWaleSeller");
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl);
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(true));
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMASTER).ToLower(), "1");
                    rabbitmqPublish.PublishToQueue(ConfigurationManager.AppSettings["ImageQueueName"], nvc);
                }

                BindPhotos();

                isCompleted = true;
            }
            catch (Exception err)
            {
                Trace.Warn("err : " + err.Message);
                ErrorClass.LogError(err, Request.ServerVariables["URL"]);
                
                isCompleted = false;
            }

            return isCompleted;
        }

        protected string GetIsMainImageChecked(string status)
        {
            string checkStatus = string.Empty;

            if (!String.IsNullOrEmpty(status))
            {
                if (Convert.ToBoolean(status))
                {
                    checkStatus = "checked=true";
                }
            }

            return checkStatus;
        }

        // File name properties
        //Added By : Sadhana Upadhyay on 30 July 2015 for Original image path
        string OriginalImageName
        {
            get { return FileNameTime + FileExtension; }
        }

        string _fileExtension = "";
        string FileExtension
        {
            get { return _fileExtension; }
            set { _fileExtension = value; }
        }

        string _orginalFileName = "";
        string OrginalFileName
        {
            get { return _orginalFileName; }
            set { _orginalFileName = value; }
        }

        string _folderPath = "";
        string FolderPath
        {
            get { return _folderPath; }
            set { _folderPath = value; }
        }

        string _filePathComplete = "";
        string FilePathComplete
        {
            get { return _filePathComplete; }
            set { _filePathComplete = value; }
        }

        string _fileNameTime = "";
        string FileNameTime
        {
            get { return _fileNameTime; }
            set { _fileNameTime = value; }
        }

        string FileSavedLocation
        {
            get { return FolderPath + FileNameTime + FileExtension; }
        }

    }   // End of class
}   // End of namespace
