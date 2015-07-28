using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using Bikewale.Common;
using System.Text.RegularExpressions;
using Bikewale.Controls;
using Ajax;
using Bikewale.RabbitMQ;
using System.Collections.Specialized;
using RabbitMqPublishing;

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
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			this.btnUpload.Click += new EventHandler( btnUpload_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
            div_NotAuthorised.Visible = false;

			inquiryId = CookiesCustomers.SellInquiryId;
            Trace.Warn("inquiryId cookies : ",inquiryId);

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                if (CurrentUser.Id == "-1")
                {
                    Response.Redirect("/users/login.aspx?ReturnUrl=/used/sell/uploadbasic.aspx?id=" + Request.QueryString["id"]);
                }
                else
                {
                    inquiryId = Request.QueryString["id"];
                    CookiesCustomers.SellInquiryId = inquiryId;
                }
            }
        
            Trace.Warn("inquiry id : ", inquiryId);            

            if ( !IsPostBack )
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

            Database db = null;
            DataSet ds = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT SI.ID FROM ClassifiedIndividualSellInquiries AS SI With(NoLock) "
                                    + " INNER JOIN Customers C WITH(NOLOCK) ON C.Id = SI.CustomerId "
                                    + " WHERE SI.CustomerId = @CustomerId AND SI.ID = @InquiryId AND C.IsFake = 0 ";

                    Trace.Warn("CookiesCustomers.CustomerId : ", CookiesCustomers.CustomerId);
                    Trace.Warn("CurrentUser.Id : ", CurrentUser.Id);

                    cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = CurrentUser.Id == "-1" ? CookiesCustomers.CustomerId : CurrentUser.Id;
                    cmd.Parameters.Add("@InquiryId", SqlDbType.BigInt).Value = inquiryId;

                    ds = db.SelectAdaptQry(cmd);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        canEdit = true;
                    }
                }
                Trace.Warn("canEdit : ", canEdit.ToString());
            }
            catch (SqlException err)
            {                
                Trace.Warn("err : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn("err : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();    
            }
            return canEdit;
        }
		
		void btnUpload_Click( object Sender, EventArgs e  )
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
		
		bool UploadPhotoFile(string inquiryId)
		{
			bool isCompleted;
			string photoId = "-1";

            OrginalFileName = fileInput2.Value;

            divAlertMsg.InnerText = "";

            // if file is not selected by the user or user trying to upload wrong file extension
            // abort further operation. Alert user           
            if ( OrginalFileName == "" )
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
                FolderPath = ImagingFunctions.GetPathToSaveImages("\\bikewaleimg\\used\\S" + inquiryId + @"\\");
                Trace.Warn("FolderPath", FolderPath);

                Trace.Warn("host : ", Request.ServerVariables["HTTP_HOST"]);
                if (Request.ServerVariables["HTTP_HOST"].IndexOf("localhost") < 0)
                {
                    Trace.Warn("server ...");
                    if (FolderPath.IndexOf("bikewale") >= 0)
                        FolderPath = FolderPath.Replace("\\bikewale\\", "\\carwale\\");
                }   
             
                //path for original image has been created like img.carwale.com/bikewale/*
					
				if(! Directory.Exists(FolderPath))
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

                SellBikeCommon objSell = new SellBikeCommon();
                photoId = objSell.SaveBikePhotos(inquiryId, LargeImgName, MediumImgName, ThumbImgName, "", false, false);

                string dirPath = "/bikewaleimg/used/S" + inquiryId + "/";
                string imageUrl = BikeCommonRQ.UploadImageToCommonDatabase(photoId, FileNameTime + FileExtension, ImageCategories.BIKEWALESELLER, dirPath);

                Trace.Warn("before rabbitmq");

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
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(false));
                rabbitmqPublish.PublishToQueue("BikeImage", nvc);

                //RabbitMq publish code 
				
				// Create image of 640x428				
                //Bikewale.Common.ImagingFunctions.ResizeImage(FileSavedLocation, LargeImgPath, 640, 428);
				
                //// Create 300x225 Size
                //Bikewale.Common.ImagingFunctions.ResizeImage(FileSavedLocation, MediumImgPath, 300, 225);
				
                //// Create Thumb Size
                //Bikewale.Common.ImagingFunctions.ResizeImage(FileSavedLocation, ThumbImgPath, 80, 60);
				
                //File.Delete(FileSavedLocation); 
				
                //Trace.Warn("ThumbImgName : " + ThumbImgName + " MediumImgName " + MediumImgName + " LargeImgName " + LargeImgName);

                //SellBikeCommon objSell = new SellBikeCommon();				
                //photoId = objSell.SaveBikePhotos(inquiryId, LargeImgName, MediumImgName, ThumbImgName, "", false, false);
				
				BindPhotos();
				
				isCompleted = true;
			}
			catch(Exception err)
			{	
				Trace.Warn("err : " + err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
				isCompleted = false;
			}
			
			return isCompleted;
		}
		
        protected string GetIsMainImageChecked(string status)
        {
            string checkStatus = string.Empty;

            if(!String.IsNullOrEmpty(status))
            {
                if (Convert.ToBoolean(status))
                {
                    checkStatus = "checked=true";
                }
            }

            return checkStatus; 
        }


		// File path properties	
		string ThumbImgPath
		{
			get{ return FolderPath + ThumbImgName; }
		}
				
		string MediumImgPath
		{
			get{ return FolderPath + MediumImgName; }	
		}
				
		string LargeImgPath
		{
			get{ return FolderPath + LargeImgName; }			
		}
		
		// File name properties
		string ThumbImgName
		{
			get{ return FileNameTime + "_80x60" + FileExtension; }
		}
				
		string MediumImgName
		{
			get{ return FileNameTime + "_300x225" + FileExtension; }	
		}
				
		string LargeImgName
		{
			get{ return FileNameTime + "_640x428" + FileExtension; }			
		}
		
		string _fileExtension = "";
		string FileExtension
		{
			get{ return _fileExtension; }
			set{ _fileExtension = value; }
		}
		
		string _orginalFileName = "";
		string OrginalFileName
		{
			get{ return _orginalFileName; }
			set{ _orginalFileName = value;}
		}
		
		string _folderPath = "";
		string FolderPath
		{
			get{ return _folderPath; }
			set{ _folderPath = value;}
		}
		
		string _filePathComplete = "";
		string FilePathComplete
		{
			get{ return _filePathComplete; }
			set{ _filePathComplete = value;}
		}
				
		string _fileNameTime = "";		
		string FileNameTime
		{
			get{ return _fileNameTime; }
			set{ _fileNameTime = value; }		
		}
		
		string FileSavedLocation
		{
			get{ return FolderPath + FileNameTime + FileExtension; }
		}				

	}   // End of class
}   // End of namespace
