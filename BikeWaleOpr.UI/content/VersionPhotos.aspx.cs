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
using BikeWaleOPR.DAL.CoreDAL;
using BikeWaleOPR.Utilities;
using System.Data.Common;


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
        string timeStamp = CommonOpn.GetTimeStamp();

        string qryStrModel = "";

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnUpdateModel.Click += new EventHandler(btnUpdateModel_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            if (Request.QueryString["model"] != null && Request.QueryString["model"].ToString() != "")
            {
                qryStrModel = Request.QueryString["model"].ToString();

                if (!CommonOpn.CheckId(qryStrModel))
                {
                    Response.Redirect("BikeModels.aspx");
                }
            }
            else
            {
                Response.Redirect("BikeModels.aspx");
            }

            if (!IsPostBack)
            {
                BindRepeater();
                pnlAdd.Visible = true;
            }

        } // Page_Load

        void btnFind_Click(object Sender, EventArgs e)
        {
            BindRepeater();
            pnlAdd.Visible = true;
        }

        void btnSave_Click(object Sender, EventArgs e)
        {
            Trace.Warn("Uploading Photos...");
            string originalImgPath = string.Empty;

            for (int i = 0; i < rptFeatures.Items.Count; i++)
            {
                Literal lt = (Literal)rptFeatures.Items[i].FindControl("ltId");
                CheckBox chk = (CheckBox)rptFeatures.Items[i].FindControl("chkUpload");

                if (chk.Checked)
                {
                    UpdateVersions(lt.Text, out originalImgPath);
                    SavePhoto(lt.Text, originalImgPath.Split('?')[0]);
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
        void btnUpdateModel_Click(object sender, EventArgs e)
        {
            if (Request.Form["optModel"] == null || !CommonOpn.CheckId(Request.Form["optModel"]))
                return;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "savemodelphotos";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Request.Form["optModel"]));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], qryStrModel));

                    MySqlDatabase.UpdateQuery(cmd);

                    BindRepeater();
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        } // btnUpdateModel_Click	

        void UpdateVersions(string versionId, out string originalImagePath)
        {

            originalImagePath = string.Empty;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "saveversionphotos";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, ConfigurationManager.AppSettings["imgHostURL"]));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], versionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_timestamp", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20, timeStamp));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 150, ParameterDirection.Output));

                        MySqlDatabase.ExecuteNonQuery(cmd);

                        originalImagePath = cmd.Parameters["par_originalimagepath"].Value.ToString();
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
        }

        void SavePhoto(string versionId, string originalImagePath)
        {
            verId = versionId;
            string hostUrl = ConfigurationManager.AppSettings["RabbitImgHostURL"].ToString();
            string imageUrl = "http://" + hostUrl + originalImagePath;

            string dirPath = ImagingOperations.GetPathToSaveImages((originalImagePath.Substring(0, originalImagePath.LastIndexOf('/') + 1)).Replace("/", "\\"));

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            ImagingOperations.SaveImageContent(filLarge, originalImagePath.Replace("/", "\\"));
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

        void BindRepeater()
        {
            string sql = "";
            int _modelid = default(int);
            if (int.TryParse(qryStrModel, out _modelid))
            {
                sql = @" select ve.Id, ve.Name, ve.Smallpic, ve.Largepic, ve.Hosturl, ve.Isreplicated,ve.Originalimagepath,ve.Isreplicated, 
				(select smallpic from bikemodels where id=ve.bikemodelid ) as modelsmall, 
				(select largepic from bikemodels where id=ve.bikemodelid ) as modellarge 
				from bikeversions ve where ve.isdeleted =0 and ve.bikemodelid=" + qryStrModel;

            }
            try
            {

                if (!string.IsNullOrEmpty(sql))
                {
                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(sql))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                        {
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
                    } 
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
        }

    } // class
} // namespace