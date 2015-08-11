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
using System.Configuration;
using RabbitMqPublishing;
using System.Collections.Specialized;
using BikeWaleOpr.RabbitMQ;
using System.IO;


namespace BikeWaleOpr.Content
{
	public class VersionPhotos : Page
	{
		protected HtmlGenericControl spnError;
		protected Button btnSave, btnUpdateModel;
		protected Repeater rptFeatures;
		protected Label lblBike;
		protected Panel pnlAdd;
		protected HtmlInputFile filLarge;
        protected string verId = string.Empty, isReplicated = string.Empty;
        string timeStamp=CommonOpn.GetTimeStamp();
		
		string qryStrModel = "";
										 
		protected override void OnInit( EventArgs e )
		{
			InitializeComponent();
		}
		
		void InitializeComponent()
		{
			base.Load += new EventHandler( Page_Load );
			btnSave.Click += new EventHandler( btnSave_Click );
			btnUpdateModel.Click += new EventHandler( btnUpdateModel_Click );
		}
		
		void Page_Load( object Sender, EventArgs e )
		{			
			if( Request.QueryString["model"] != null && Request.QueryString["model"].ToString() != "")
			{
				qryStrModel = Request.QueryString["model"].ToString();
				
				if( !CommonOpn.CheckId(qryStrModel))
				{
					Response.Redirect("BikeModels.aspx");
				}
			}
			else
			{
                Response.Redirect("BikeModels.aspx");
			}
			
			if(!IsPostBack)
			{
				BindRepeater();
				pnlAdd.Visible = true;
			}
			
		} // Page_Load
	
		void btnFind_Click( object Sender, EventArgs e )
		{
			BindRepeater();
			pnlAdd.Visible = true;
		}
		
		void btnSave_Click( object Sender, EventArgs e )
		{
			Trace.Warn( "Uploading Photos..." );
            string originalImgPath=string.Empty;
					
			for ( int i=0; i < rptFeatures.Items.Count; i++ )
			{
				Literal lt = (Literal) rptFeatures.Items[i].FindControl( "ltId" );
				CheckBox chk = (CheckBox) rptFeatures.Items[i].FindControl( "chkUpload" );
				
				if ( chk.Checked )
				{
                    UpdateVersions(lt.Text, out originalImgPath);
                    SavePhoto(lt.Text, originalImgPath);
				}
			}
			
			spnError.InnerHtml = "Data Saved Successfully.";
			BindRepeater();
		}	
		
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 29th Jan 2014
        /// Summary : To Set IsReplication = 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void btnUpdateModel_Click( object sender, EventArgs e )
		{
			Database db = new Database();
			
			if ( Request.Form["optModel"] == null || !CommonOpn.CheckId( Request.Form["optModel"] ) )
				return;
							
			try
			{
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SaveModelPhotos";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = Request.Form["optModel"];
                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = qryStrModel;

                    db.UpdateQry(cmd);

                    BindRepeater();
                }
			}
			catch(SqlException err)
			{
				Trace.Warn(err.Message + err.Source);
				ErrorClass objErr = new ErrorClass(err,Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			
		} // btnUpdateModel_Click	

        void UpdateVersions(string versionId, out string originalImagePath)
        {

            originalImagePath = string.Empty;
            Database db = new Database();

            //sql = "UPDATE BikeVersions SET IsReplicated = 0,"
            //    + " SmallPic='" + versionId + "s.jpg?" + timeStamp + "', "
            //    + " LargePic='" + versionId + "b.jpg?" + timeStamp + "', "
            //    + " HostURL = '" + ConfigurationManager.AppSettings["imgHostURL"] + "'"
            //    + " WHERE ID=" + versionId;

            try
            {
                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SaveVersionPhotos";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 100).Value = ConfigurationManager.AppSettings["imgHostURL"];
                        cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = versionId;
                        cmd.Parameters.Add("@OriginalImagePath", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        originalImagePath = cmd.Parameters["@OriginalImagePath"].Value.ToString();
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objerr = new ErrorClass(err, Request.ServerVariables["url"]);
                objerr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objerr = new ErrorClass(err, Request.ServerVariables["url"]);
                objerr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
        }

        void SavePhoto(string versionId,string originalImagePath)
        {
            verId = versionId;
            string hostUrl = ConfigurationManager.AppSettings["RabbitImgHostURL"].ToString();
            string imageUrl = "http://" + hostUrl + originalImagePath;

            string dirPath = ImagingOperations.GetPathToSaveImages((originalImagePath.Substring(0, originalImagePath.LastIndexOf('/') + 1)).Replace("/", "\\"));

            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            ImagingOperations.SaveImageContent(filLarge, originalImagePath.Replace("/","\\"));
            //rabbitmq publishing
            RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
            NameValueCollection nvc = new NameValueCollection();
            //add items to nvc
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ID).ToLower(), versionId);
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "BIKEVERSION");
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(true));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ONLYREPLICATE).ToLower(), Convert.ToString(true));
            nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl);
            nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), imageUrl);
            rabbitmqPublish.PublishToQueue(ConfigurationManager.AppSettings["ImageQueueName"], nvc);
        }
		
        //public string  GetDisplayImagePath()
        //{
        //    string imgPath = "";
			
        //    if ( HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf( "carwale.com" ) >= 0 ) 
        //    {
        //        imgPath = CommonOpn.ImagePath;
        //    }
        //    else
        //    {
        //        imgPath = "http://server/Images/";
        //    }
			
        //    return imgPath;
        //}
		
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 29th Jan 2014
        /// Summary : To get IsReplicated Column
        /// </summary>
		void BindRepeater()
		{
			string sql = "";

            sql = " SELECT VE.ID, VE.Name, VE.SmallPic, VE.LargePic, VE.HostURL, Ve.IsReplicated,VE.OriginalImagePath, "
				+ " (SELECT SmallPic FROM BikeModels WHERE Id=Ve.BikeModelId ) AS ModelSmall, "
				+ " (SELECT LargePic FROM BikeModels WHERE Id=Ve.BikeModelId ) AS ModelLarge "
				+ " FROM BikeVersions Ve WHERE VE.IsDeleted =0 AND Ve.BikeModelId=" + qryStrModel;
			
			Trace.Warn(sql);
			
			Database db = new Database();
			
			DataSet ds = new DataSet();
			SqlConnection cn = new SqlConnection( db.GetConString() );

            try
            {

                cn.Open();

                SqlDataAdapter adp = new SqlDataAdapter(sql, cn);
                adp.Fill(ds, "Categories");

                rptFeatures.DataSource = ds.Tables["Categories"];
                rptFeatures.DataBind();

                if (ds.Tables[0].Rows.Count < 1)
                {
                    btnUpdateModel.Enabled = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    btnUpdateModel.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
			finally
			{
				if ( cn.State == ConnectionState.Open ) cn.Close();
			}
		}

	} // class
} // namespace