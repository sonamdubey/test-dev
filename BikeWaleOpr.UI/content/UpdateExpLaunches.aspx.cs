using BikeWaleOpr.Common;
using BikeWaleOpr.Controls;
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
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BikeWaleOpr.Content
{
    public class UpdateExpLaunches : Page
    {
        protected TextBox txtEstMinPri, txtEstMaxPri, txtExpLaunch;
        protected HtmlInputFile filLarge;
        protected HtmlInputButton btnUpdate;
        protected DateControl calFrom;
        protected HtmlSelect ddlHour, ddlMinutes;
        protected HtmlGenericControl spnMessage, spnBikeName, spnRes;
        //protected HtmlImage imgSmallPicPath, imgLargePicPath;
        protected string Id = "-1", date = "", cName = "", expLaunch = "", estMinPri = "", estMaxPri = "", modelId = "", hostUrl = string.Empty, smallPicImgPath = string.Empty, originalImgPath = string.Empty, isReplicated = string.Empty;
        protected static int imgVersion = 0;
        string timeStamp = CommonOpn.GetTimeStamp();

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            btnUpdate.ServerClick += new EventHandler(this.btnUpdate_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            GetExpectedBikeLaunches();

            if (!IsPostBack)
            {
                FillDetails();
            }
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            bool status = false;
            //string status = "";

            Trace.Warn("calFrom.Value : " + calFrom.Value.ToString());
            DateTime dtMon = calFrom.Value;

            Trace.Warn("FilLarge Value : " + filLarge.Value);

            string newLaunch = dtMon.Year + "-" + dtMon.Month + "-" + dtMon.Day + "-" + ddlHour.Value + "-" + ddlMinutes.Value + "-00";

            Trace.Warn("newLaunch:  " + newLaunch);
            status = UpdateLaunchDate(Id, txtEstMinPri.Text, txtEstMaxPri.Text, txtExpLaunch.Text, newLaunch, modelId);

            spnRes.InnerHtml = "Error Msg ... " + status;
            if (status)
            {
                if (filLarge.Value != "")
                {
                    SavePhoto(modelId);
                }

                imgVersion = imgVersion++;

                spnRes.InnerHtml = "Record Updated Successfully.";
            }
            else
            {
                spnRes.InnerHtml = "Unable to update";
            }
        }

        void FillDetails()
        {
            if (date != "")
            {
                Trace.Warn("date : " + Convert.ToDateTime(date));
                calFrom.Value = Convert.ToDateTime(date);


                string[] dtSplit = date.Split(' ');

                if (dtSplit[1] != "")
                {
                    if (dtSplit.Length >= 3 && dtSplit[2] != "PM")
                    {
                        if (Convert.ToInt32(Convert.ToDateTime(date).Hour) == 12)
                        {
                            ddlHour.SelectedIndex = ddlHour.Items.IndexOf(ddlHour.Items.FindByValue("0"));
                        }
                        else
                        {
                            ddlHour.SelectedIndex = ddlHour.Items.IndexOf(ddlHour.Items.FindByValue(Convert.ToDateTime(date).Hour.ToString()));
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(Convert.ToDateTime(date).Hour) == 12)
                        {
                            ddlHour.SelectedIndex = ddlHour.Items.IndexOf(ddlHour.Items.FindByValue(Convert.ToDateTime(date).Hour.ToString()));
                        }
                        else
                        {
                            ddlHour.SelectedIndex = ddlHour.Items.IndexOf(ddlHour.Items.FindByValue((Convert.ToInt32(Convert.ToDateTime(date).Hour) + 12).ToString()));
                            Trace.Warn("Hours 2nd.. " + (Convert.ToInt32(Convert.ToDateTime(date).Hour) + 12));
                        }
                    }

                    ddlMinutes.SelectedIndex = ddlMinutes.Items.IndexOf(ddlMinutes.Items.FindByValue(Convert.ToDateTime(date).Minute.ToString()));
                    Trace.Warn("Mins 2nd.. " + Convert.ToDateTime(date).Minute.ToString());
                }
            }
            else
            {
                calFrom.Value = DateTime.Now;
                ddlHour.SelectedIndex = ddlHour.Items.IndexOf(ddlHour.Items.FindByValue("0"));
                ddlMinutes.SelectedIndex = ddlMinutes.Items.IndexOf(ddlMinutes.Items.FindByValue("00"));
            }

            txtEstMinPri.Text = estMinPri;
            txtEstMaxPri.Text = estMaxPri;
            txtExpLaunch.Text = expLaunch;
            spnBikeName.InnerHtml = cName;
        }

        // Modified By : Sadhana Upadhyay on 20th Dec
        //Modified By : Sadhana Upadhyy on 28th Jan 2014 to update IsReplication
        bool UpdateLaunchDate(string Id, string minPrice, string maxPrice, string expLaunch, string newLaunch, string modelId)
        {
            Trace.Warn("newLaunch: " + newLaunch);

            string[] tempDate = newLaunch.Split('-');

            DateTime newLaunchDate = new DateTime(int.Parse(tempDate[0]), int.Parse(tempDate[1]), int.Parse(tempDate[2]), int.Parse(tempDate[3]), int.Parse(tempDate[4]), 0);

            bool retVal = false;

            Trace.Warn("ModelId ... " + modelId);

            if (modelId == "" || modelId == "0")
                modelId = "";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "con_updateexpectedbikelaunches";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int64, Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_expectedlaunch", DbType.String, 250, expLaunch));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_launchdate", DbType.DateTime, newLaunchDate));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_estimatedpricemin", DbType.Int64, minPrice));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_estimatedpricemax", DbType.Int64, maxPrice));
                    if (!String.IsNullOrEmpty(modelId))
                    {
                        if (!String.IsNullOrEmpty(filLarge.Value))
                        {
                            originalImgPath = ("/bw/upcoming/" + cName.Replace(" ", "") + "-" + modelId + ".jpg?" + timeStamp).ToLower();
                        }
                    }
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbType.String, 100, originalImgPath));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 100, ConfigurationManager.AppSettings["imghosturl"]));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isreplication", DbType.Boolean, (!String.IsNullOrEmpty(filLarge.Value)) ? false : true));
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    retVal = true;
                }
            }
            catch (SqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(err.Message.ToString());
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.UpdateLaunchDate");
                objErr.SendMail();
                retVal = false;
                //retVal = "SqlException : " + err.Message;
            } // catch SqlException
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message.ToString());
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.UpdateLaunchDate");
                objErr.SendMail();
                retVal = false;
            } // catch Exception
            return retVal;
        }   // End of UpdateLaunchDate

        //public void SavePhoto(string filSmallVal, string filLargeVal, string modelId)

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 15th Jan 2014
        /// Summary : To add image replication function
        /// </summary>
        /// <param name="modelId"></param>
        public void SavePhoto(string modelId)
        {
            string imgPath = ImagingOperations.GetPathToSaveImages("\\bw\\upcoming\\");
            string hostUrl = ConfigurationManager.AppSettings["imgHostURL"].ToString();
            string imageUrl = ("https://" + hostUrl + "/bw/upcoming/" + cName.Replace(" ", "") + "-" + modelId).ToLower() + ".jpg";
            string imageTargetPath = ("/bw/upcoming/" + cName.Replace(" ", "") + "-" + modelId + ".jpg").ToLower();
            if (!Directory.Exists(imgPath))
            {
                Directory.CreateDirectory(imgPath);
            }

            if (modelId != "" && modelId != "0")
            {
                if (!String.IsNullOrEmpty(filLarge.Value))
                {
                    ImagingOperations.SaveImageContent(filLarge, imageTargetPath);

                    //rabbitmq publishing
                    RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                    NameValueCollection nvc = new NameValueCollection();
                    //add items to nvc
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ID).ToLower(), Id);
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "EXPECTEDLAUNCH");
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(true));
                    nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ONLYREPLICATE).ToLower(), Convert.ToString(true));
                    nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl);
                    nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), imageTargetPath + "?" + timeStamp);
                    nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.ISMASTER).ToLower(), "1");
                    rabbitmqPublish.PublishToQueue(ConfigurationManager.AppSettings["ImageQueueName"], nvc);
                }
            }
        }   // End of SavePhoto        

        /// <summary>
        /// Created By : Sadhana Upadhyay on 20th Dec
        /// Summary : To get Expected Bike launch details
        /// Modified By : Sadhana Upadhyay on 29th Jan 2014 
        /// Summary : To get IsReplicated Flag
        /// </summary>
        public void GetExpectedBikeLaunches()
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    if (Request.QueryString["Id"] != null && Request.QueryString["Id"].ToString() != "")
                    {
                        Id = Request.QueryString["Id"].ToString();
                    }

                    cmd.CommandText = "con_getexpectedbikelaunches";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int32, Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_launchdate", DbType.String, 250, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelname", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_estimatedpricemin", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_estimatedpricemax", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_originalimagepath", DbType.String, 100, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_date", DbType.DateTime, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isreplicated", DbType.Boolean, ParameterDirection.Output));


                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    expLaunch = cmd.Parameters["par_launchdate"].Value.ToString();
                    cName = cmd.Parameters["par_makename"].Value.ToString() + "-" + cmd.Parameters["par_modelname"].Value.ToString();
                    estMaxPri = cmd.Parameters["par_EstimatedPriceMax"].Value.ToString();
                    estMinPri = cmd.Parameters["par_estimatedpricemin"].Value.ToString();
                    hostUrl = cmd.Parameters["par_HostURL"].Value.ToString();
                    originalImgPath = cmd.Parameters["par_originalimagepath"].Value.ToString();
                    modelId = cmd.Parameters["par_modelid"].Value.ToString();
                    date = cmd.Parameters["par_date"].Value.ToString();
                    isReplicated = cmd.Parameters["par_isreplicated"].Value.ToString();

                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message.ToString());
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.GetExpectedBikeLaunches");
                objErr.SendMail();
            } // catch SqlException
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message.ToString());
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.GetExpectedBikeLaunches");
                objErr.SendMail();
            } // catch Exception
        }   // End of GetExpectedBikeLaunches

    }//Class
}// Namespace