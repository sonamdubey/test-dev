using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using BikeWaleOpr.Common;
using FreeTextBoxControls;
using Ajax;
using System.Configuration;
using BikeWaleOpr.RabbitMQ;
using RabbitMqPublishing;
using System.Collections.Specialized;

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
        protected string smallImgPath = string.Empty, largeImgPath = string.Empty, imgPath = string.Empty, hostURL = string.Empty, timeStamp = CommonOpn.GetTimeStamp(), priorityList = string.Empty;
		
		public string SelectedModel
		{
			get
			{
				if(Request.Form["drpModel"] != null && Request.Form["drpModel"].ToString() != "")
					return Request.Form["drpModel"].ToString();
				else
					return "-1";
			}
		}
		
		public string ModelContents
		{
			get
			{
				if(Request.Form["hdn_drpModel"] != null && Request.Form["hdn_drpModel"].ToString() != "")
					return Request.Form["hdn_drpModel"].ToString();
				else
					return "";
			}
		}
		
		public string SelectedVersion
		{
			get
			{
				if(Request.Form["drpVersion"] != null && Request.Form["drpVersion"].ToString() != "")
					return Request.Form["drpVersion"].ToString();
				else
					return "-1";
			}
		}
		
		public string VersionContents
		{
			get
			{
				if(Request.Form["hdn_drpVersion"] != null && Request.Form["hdn_drpVersion"].ToString() != "")
					return Request.Form["hdn_drpVersion"].ToString();
				else
					return "";
			}
		}
		
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			this.btnSave.Click += new System.EventHandler(btnSave_OnClick);
			this.btnUpdate.Click += new System.EventHandler(btnUpdate_OnClick);
            dtgrdFeaturedListing.PageIndexChanged += new DataGridPageChangedEventHandler(Page_Change);
			//this.btnUpdateFeaturedBike.Click += new System.EventHandler(btnUpdateFeaturedBike_OnClick);
		}
		
		void Page_Load( object Sender, EventArgs e )
		{
			CommonOpn op = new CommonOpn();
			
            //if( HttpContext.Current.User.Identity.IsAuthenticated != true) 
            //        Response.Redirect("../users/Login.aspx?ReturnUrl=../Contents/FeaturedListing.aspx");
				
            //if ( Request.Cookies["Customer"] == null )
            //    Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/FeaturedListing.aspx");
				
			if( Request.QueryString["UpdateId"] != null && Request.QueryString["UpdateId"].ToString() != "")
			{
				updateData = Request.QueryString["UpdateId"].ToString();
				
				if( !CommonOpn.CheckId(updateData) )
				{
					Response.Redirect("../Users/Login.aspx?ReturnUrl=../Contents/FeaturedListing.aspx");
				}
			}
			
			Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
			//btnUpdateFeaturedBike.Attributes.Add("onclick","javascript:if (ConfirmUpdateFeaturedBike() == false ) return false;");
			if (!IsPostBack )
			{
				FillMakes();
				BindGrid();
				if ( updateData != "" )
				{
					FillData();
					btnUpdate.Enabled = true;
					btnSave.Enabled = false;
				}
			}
			
			visibleCount = GetVisibleListingCount();
            GetAllPriority();
		}
		
		void btnSave_OnClick( object sender, EventArgs e )
		{
			string saveId = "";


            if (String.IsNullOrEmpty(flphoto.PostedFile.FileName))
            {
                lblMessage.Text = "Please Select Image to upload";
            }
            else
            { 
                saveId = SaveData("-1");
                Trace.Warn("Save Id : "+ saveId);
			    if( saveId != "" &&  saveId != "0")
			    {
				    if(UploadImage(saveId))
				    {
					    lblMessage.Text = "Data Saved Successfully";
				    }
			    }
            }			
			ClearText();
			BindGrid();
		}
		
		void btnUpdate_OnClick( object sender, EventArgs e )
		{
			string updateId = "";
			
			if( Convert.ToInt32( visibleCount ) < 1 && ( chkIsActive.Checked == false || chkIsVisible.Checked == false ) ) 
			{
				lblMessage.Text = "UPDATION FAILED!! Atleast one featured listing should be visible and active.";
			}
			else
			{
				if ( updateData != "" )
				{
					updateId = SaveData(updateData);
                    Trace.Warn("Update Id : "+updateId);
					if( updateId != "" &&  updateId != "0")
					{
						if(flphoto.PostedFile.FileName != "")
						{
							if(UploadImage(updateId))
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
			btnUpdate.Enabled = false ;
			btnSave.Enabled = true ;
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
			sql = "SELECT ID, Name FROM BikeMakes WHERE IsDeleted <> 1 ORDER BY NAME";
			try
			{
				op.FillDropDown( sql, drpMake, "Name", "ID" );
			}
			catch( SqlException err )
			{
				Trace.Warn(err.Message);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception
			
			ListItem item = new ListItem( "--Select--", "0" );
			drpMake.Items.Insert( 0, item );
		}
		
		string SaveData( string updateId )
		{
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			string conStr = db.GetConString();
			string lastSavedId = "";
			
			con = new SqlConnection( conStr );

			try
			{
				Trace.Warn( "Submitting Data" );
				
				cmd = new SqlCommand("CON_AddFeaturedListing", con);
				cmd.CommandType = CommandType.StoredProcedure;
				
				prm = cmd.Parameters.Add("@Id", SqlDbType.BigInt);
				prm.Value = updateId;
				
				prm = cmd.Parameters.Add("@BikeId", SqlDbType.BigInt);
				prm.Value = chkIsModel.Checked ? SelectedModel : SelectedVersion;
			
				prm = cmd.Parameters.Add("@Description", SqlDbType.VarChar,1000);
				prm.Value = txtDescription.Text.Trim();
				
				prm = cmd.Parameters.Add("@IsModel", SqlDbType.Bit);
				prm.Value = chkIsModel.Checked ? 1 : 0;
				
				prm = cmd.Parameters.Add("@IsVisible", SqlDbType.Bit);
				prm.Value = chkIsVisible.Checked ? 1 : 0;
				
				prm = cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
				prm.Value = chkIsActive.Checked ? 1 : 0;
				
				prm = cmd.Parameters.Add("@EntryDateTime", SqlDbType.DateTime);
				prm.Value = DateTime.Now;
				
				prm = cmd.Parameters.Add("@LastSavedId", SqlDbType.BigInt);
				prm.Direction = ParameterDirection.Output;
				
				con.Open();
    			cmd.ExecuteNonQuery();			
					
				Trace.Warn(cmd.Parameters["@LastSavedId"].Value.ToString());
				if ( cmd.Parameters["@LastSavedId"].Value.ToString() != "" ) 
					lastSavedId = cmd.Parameters["@LastSavedId"].Value.ToString();
								
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
				//close the connection	
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
			return lastSavedId;
		}
		
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 17th Jan 2014
        /// Summary : To replicate images using RabbitMQ
        /// </summary>
        /// <param name="imgName">Image Id</param>
        /// <returns></returns>
		bool UploadImage( string imgName )
		{
			bool isUploaded = false;
			string fullTempImagePath = "";
			string realImagePath = "";
			string imgPath = "";
            string smallImage = "";
            string largeImage = "";

            imgPath = ImagingOperations.GetPathToSaveImages("\\bikewaleimg\\featured\\");
			
			//Check the image path is exist or not if not exist create it
			Trace.Warn("imgPath=" + imgPath);
            Trace.Warn("img Name : "+imgName);
			if(!Directory.Exists(imgPath) )
			{
				Directory.CreateDirectory(imgPath);
			}

            string tempImageName = GetSelectedBikeName() + imgName + "_Temp.jpg";
            fullTempImagePath = imgPath + tempImageName;
            string hostUrl = ConfigurationManager.AppSettings["RabbitImgHostURL"];
            string imageUrl = "http://" + hostUrl + "/bikewaleimg/featured/";
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

            // Upload image as a temp image to resize later
            flphoto.PostedFile.SaveAs(fullTempImagePath);
            
            smallImage = (GetSelectedBikeName() + "-" + imgName + "s.jpg").ToLower();
            Trace.Warn("smallImage : ", smallImage);
            realImagePath = imgPath + smallImage;
            Trace.Warn("Real Image Path : ",realImagePath);

            //To replicate small image
            ImagingFunctions.GenerateThumbnail(fullTempImagePath, realImagePath, 140, 80); // Image size : 140 x 80px
            nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl + (GetSelectedBikeName() + "-" + imgName).ToLower() + "s.jpg");
            nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), "/bikewaleimg/featured/" + (GetSelectedBikeName() + "-" + imgName).ToLower() + "s.jpg?" + timeStamp);
            rabbitmqPublish.PublishToQueue("BikeImage", nvc);

            largeImage = (GetSelectedBikeName() + "-" + imgName + "b.jpg").ToLower();
            Trace.Warn("largeImage : ", largeImage);
            realImagePath = imgPath + largeImage;

            //To replicate large Image
            ImagingFunctions.GenerateThumbnail(fullTempImagePath, realImagePath, 200, 125); // Image size : 200 x 125px
            nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl + (GetSelectedBikeName() + "-" + imgName).ToLower() + "b.jpg");
            nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), "/bikewaleimg/featured/" + (GetSelectedBikeName() + "-" + imgName).ToLower() + "b.jpg?" + timeStamp);
            rabbitmqPublish.PublishToQueue("BikeImage", nvc);

            UpdateBikePhotoContent(imgName, hostUrl, "/bikewaleimg/featured/", largeImage + "?" + timeStamp, smallImage + "?" + timeStamp);

            DeleteTempImgs(fullTempImagePath);

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
        protected void UpdateBikePhotoContent(string id, string hostUrl, string imagePath, string largeImage, string smallImage)
        {
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Con_UpdateFeaturedListingPhoto";

                    cmd.Parameters.Add("@Id", SqlDbType.Decimal);
                    cmd.Parameters["@Id"].Precision = 18;
                    cmd.Parameters["@Id"].Scale = 0;
                    cmd.Parameters["@Id"].Value = id;
                    cmd.Parameters.Add("@HostURL", SqlDbType.VarChar, 100).Value = hostUrl;
                    cmd.Parameters.Add("@ImagePath", SqlDbType.VarChar, 100).Value = imagePath;

                    if (!String.IsNullOrEmpty(flphoto.PostedFile.FileName))
                    {
                        cmd.Parameters.Add("@LargeImageName", SqlDbType.VarChar, 100).Value = largeImage;
                        cmd.Parameters.Add("@SmallImageName", SqlDbType.VarChar, 100).Value = smallImage;
                    }

                    db.InsertQry(cmd);
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.ConsumeError();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.ConsumeError();
            }
            finally
            {
                db.CloseConnection();
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
        //        ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
        //        objErr.ConsumeError();
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
			SqlDataReader dr = null;
			Database db = new Database();
			
			sql = "SELECT COUNT(ID) AS TCount FROM Con_FeaturedListings AS FL WHERE FL.IsVisible = 1 AND FL.IsActive = 1";
			
			try
			{
				dr = db.SelectQry(sql);	
				
				if(dr.Read())
				{
					count = dr["TCount"].ToString();
				}
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.ConsumeError();
			}
			finally
			{
                if (dr != null)
                {
                    dr.Close();
                }
				db.CloseConnection();
			}
			
			return count;
		}   // End of GetVisibleListingCount method

        //Modified By Sadhana Upadhyay on 22 July to get Priorities
		void BindGrid()
		{
			string sql = "";
			
			int pageSizeM = dtgrdFeaturedListing.PageSize;
												
			sql = " SELECT FL.Id, (CMA.Name + ' ' + CMO.Name) AS BikeName, IsActive, IsVisible, "
                + " IsModel, Description, EntryDateTime, FL.HostURL, ImagePath, SmallImageName,FL.DisplayPriority "
				+ " FROM Con_FeaturedListings AS FL, BikeMakes AS CMA, BikeModels AS CMO "
				+ " WHERE FL.BikeId = CMO.Id AND CMO.BikeMakeId = CMA.Id AND FL.IsModel = 1 "
				
				+ " UNION ALL "
				
				+ " SELECT FL.Id, (CMA.Name + ' ' + CMO.Name + ' ' + CV.Name) AS BikeName, IsActive, IsVisible, "
                + " IsModel, Description, EntryDateTime, FL.HostURL, ImagePath, SmallImageName,FL.DisplayPriority "
				+ " FROM Con_FeaturedListings AS FL, BikeMakes AS CMA, BikeModels AS CMO, BikeVersions AS CV "
				+ " WHERE FL.BikeId = CV.Id AND CV.BikeModelId = CMO.Id AND CMO.BikeMakeId = CMA.Id AND FL.IsModel = 0 "
                + " ORDER BY IsActive DESC, DisplayPriority";
			
																				
			Trace.Warn(sql);
			CommonOpn objCom = new CommonOpn();	
					
			try
			{
				objCom.BindGridSet( sql, dtgrdFeaturedListing, pageSizeM );

                if (dtgrdFeaturedListing.Items.Count > 0)
                {
                    if (dtgrdFeaturedListing.CurrentPageIndex == 0)
                        serialNo = 0;
                    else
                        serialNo = pageSizeM * dtgrdFeaturedListing.CurrentPageIndex;
                }
			}
			catch(Exception err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}   // End of BindGrid method
		
		public string  GetString( string str )
		{
			if (str =="True")
				return "<img src=http://opr.carwale.com/Images/tick.jpg /> ";
			else
                return "<img src=http://opr.carwale.com/images/delete.gif /> ";
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
        //        imgpath = "http://server/images/featured/";
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
			string sql = "" ;
			
			AjaxFunctions aj = new AjaxFunctions();
			SqlDataReader dr = null;
			Database db = new Database();
			
			if ( updateData != "" )
			{
				sql = " SELECT CMA.Id AS MakeId, CMO.Id AS ModelId, BikeId, IsActive, IsVisible,"
                    + " IsModel, Description, FL.HostUrl as hostUrl, FL.ImagePath as imgPath, FL.SmallImageName as smallImgPath, FL.LargeImageName as largeImgPath "
					+ " FROM Con_FeaturedListings AS FL, BikeMakes AS CMA, BikeModels AS CMO"
					+ " WHERE FL.BikeId = CMO.Id AND CMO.BikeMakeId = CMA.Id AND FL.IsModel = 1"
					+ " AND FL.Id = " + updateData + ""
				
					+ " UNION ALL"
				
					+ " SELECT CMA.Id AS MakeId, CMO.Id AS ModelId, BikeId, IsActive, IsVisible,"
                    + " IsModel, Description, FL.HostUrl as hostUrl, FL.ImagePath as imgPath, FL.SmallImageName as smallImgPath, FL.LargeImageName as largeImgPath "
					+ " FROM Con_FeaturedListings AS FL, BikeMakes AS CMA, BikeModels AS CMO, BikeVersions AS CV"
					+ " WHERE FL.BikeId = CV.Id AND CV.BikeModelId = CMO.Id AND CMO.BikeMakeId = CMA.Id AND FL.IsModel = 0"
					+ " AND FL.Id = " + updateData + "";
				
				CommonOpn objCom = new CommonOpn(); 	
				
				Trace.Warn(sql);
				
				try
				{
					dr = db.SelectQry(sql);	
					
					if(dr.Read())
					{
						txtDescription.Text 	= dr["Description"].ToString();
						drpMake.SelectedValue 	= dr["MakeId"].ToString();
						
						if(Convert.ToBoolean(dr["IsActive"]) == false)
						{
							chkIsActive.Checked = false;
						}
						if(Convert.ToBoolean(dr["IsModel"]) == false)
						{
							chkIsModel.Checked = false;
							
							drpModel.DataSource = aj.GetModels(dr["MakeId"].ToString());
							drpModel.DataTextField = "Text";
							drpModel.DataValueField = "Value";
							drpModel.DataBind();
							drpModel.Items.Insert(0, new ListItem("Any","0"));
							drpModel.SelectedIndex = drpModel.Items.IndexOf(drpModel.Items.FindByValue(dr["ModelId"].ToString()));
							
							drpVersion.DataSource = aj.GetVersions(dr["ModelId"].ToString());
							drpVersion.DataTextField = "Text";
							drpVersion.DataValueField = "Value";
							drpVersion.DataBind();
							drpVersion.Items.Insert(0, new ListItem("Any","0"));
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
							drpModel.Items.Insert(0, new ListItem("Any","0"));
							drpModel.SelectedIndex = drpModel.Items.IndexOf(drpModel.Items.FindByValue(dr["ModelId"].ToString()));
							
							drpVersion.DataSource = aj.GetVersions(dr["ModelId"].ToString());
							drpVersion.DataTextField = "Text";
							drpVersion.DataValueField = "Value";
							drpVersion.DataBind();
							drpVersion.Items.Insert(0, new ListItem("Any","0"));
							
							drpModel.Enabled = true;
							drpVersion.Enabled = true;
						}
												
						if(Convert.ToBoolean(dr["IsVisible"]) == false)
						{
							chkIsVisible.Checked = false;
						}
                        smallImgPath =  dr["smallImgPath"].ToString();
                        largeImgPath =  dr["largeImgPath"].ToString();
                        imgPath = dr["imgPath"].ToString();
                        hostURL = dr["hostUrl"].ToString();

                       // imgFLPhoto.Src = BikeWaleOpr.ImagingOperations.GetPathToShowImages(imgPath + smallImgPath, hostUrl);
						
                        //if(dr["ShowResearch"].ToString() != "")
                        //    chkIsResearch.Checked = Convert.ToBoolean(dr["ShowResearch"]);
                        //else
                        //    chkIsResearch.Checked = false;
							
                        //if(dr["ShowPrice"].ToString() != "")
                        //    chkIsPrice.Checked = Convert.ToBoolean(dr["ShowPrice"]);
                        //else
                        //    chkIsPrice.Checked = false;
							
                        //if(dr["Link"].ToString() != "")
                        //{
                        //    txtLink.Text = dr["Link"].ToString();
							
                        //    if(dr["ShowRoadTest"].ToString() != "")
                        //    {
                        //        if(Convert.ToBoolean(dr["ShowRoadTest"]) == true)
                        //            rdRT.Checked = true;
                        //        else
                        //            rdFD.Checked = true;
                        //    }
                        //}
					}					
				}
				catch(Exception err)
				{
					Trace.Warn(err.Message + err.Source);
					ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
					objErr.ConsumeError();
				}
				finally
				{
					db.CloseConnection();
                    if (dr != null)
                    {
                        dr.Close();
                    }
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
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    sql = "select DisplayPriority from Con_FeaturedListings  WHERE DisplayPriority IS NOT NULL AND DisplayPriority <> 0 order by DisplayPriority";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd)) 
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
                Trace.Warn("priority list : " , priorityList);
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.ConsumeError();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.ConsumeError();
            }
            finally
            {
                db.CloseConnection();
            }
        }
		
        //void btnUpdateFeaturedBike_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        UpdateLiveContentCommon obj = new UpdateLiveContentCommon();
        //        obj.UpdateFeaturedBike();
        //        lblResult.Text = "Featured Bikes have been successfully updated";
        //    }
        //    catch(Exception ex)
        //    {
        //        lblResult.Text = "Some error occured";
        //        ErrorClass objErr = new ErrorClass(ex,HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.ConsumeError();
        //    }
        //}


		
	}//class
}// namespace