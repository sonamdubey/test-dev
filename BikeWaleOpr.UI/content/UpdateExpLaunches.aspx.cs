using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.IO;
using System.Xml;
using System.Configuration;
using BikeWaleOpr.Controls;
using BikeWaleOpr.Common;
using BikeWaleOpr.RabbitMQ;
using RabbitMqPublishing;
using System.Collections.Specialized;

namespace BikeWaleOpr.Content
{
	public class UpdateExpLaunches : Page
	{
        protected TextBox txtEstMinPri, txtEstMaxPri, txtExpLaunch;
        protected HtmlInputFile filSmall, filLarge;
        protected HtmlInputButton btnUpdate;
        protected DateControl calFrom;
        protected HtmlSelect ddlHour, ddlMinutes;
        protected HtmlGenericControl spnMessage, spnBikeName, spnRes;
        //protected HtmlImage imgSmallPicPath, imgLargePicPath;
        protected string Id = "-1", date = "", cName = "", expLaunch = "", estMinPri = "", estMaxPri = "", modelId = "", hostUrl = string.Empty, smallPicImgPath = string.Empty, largePicImgPath = string.Empty, isReplicated = string.Empty;
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

            Trace.Warn("Year : " + dtMon.Year.ToString());
            Trace.Warn("Month : " + dtMon.Month.ToString());
            Trace.Warn("Day : " + dtMon.Day.ToString());
            Trace.Warn("HH : " + ddlHour.Value.ToString());
            Trace.Warn("Min : " + ddlMinutes.Value.ToString());
            Trace.Warn("FilSmall Value : " + filSmall.Value);
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
                if (filSmall.Value != "")
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
            Trace.Warn("+++inside  fill details");
            Trace.Warn("++Hours .. " + calFrom.Value.Hour);
            Trace.Warn("++Mins .. " + calFrom.Value.Minute);
            Trace.Warn("++PM .. " + calFrom.Value);

             if (date != "") 
             {
                 Trace.Warn("date : " + Convert.ToDateTime(date));
                 calFrom.Value = Convert.ToDateTime(date);

                
                 string[] dtSplit = date.Split(' ');

                 if (dtSplit[1] != "") 
                 {
                     Trace.Warn("Hours .. " + Convert.ToDateTime(date).Hour);
                     Trace.Warn("Mins .. " + Convert.ToDateTime(date).Minute);
                     Trace.Warn("PM .. " + Convert.ToDateTime(date).TimeOfDay);
                     
                      if (dtSplit[2] != "PM") 
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
             //imgSmallPicPath.Src = BikeWaleOpr.ImagingOperations.GetPathToShowImages(smallPicImgPath, hostUrl);
             //imgLargePicPath.Src = BikeWaleOpr.ImagingOperations.GetPathToShowImages(largePicImgPath, hostUrl);
        }

        // Modified By : Sadhana Upadhyay on 20th Dec
        //Modified By : Sadhana Upadhyy on 28th Jan 2014 to update IsReplication
        bool UpdateLaunchDate(string Id, string minPrice, string maxPrice, string expLaunch, string newLaunch, string modelId)
        {
            Trace.Warn("newLaunch: " + newLaunch);
                       
            string[] tempDate = newLaunch.Split('-');
            
            DateTime newLaunchDate = new DateTime(int.Parse(tempDate[0]), int.Parse(tempDate[1]), int.Parse(tempDate[2]), int.Parse(tempDate[3]), int.Parse(tempDate[4]), 0);

            Trace.Warn("Inside sp... ");
            bool retVal = false;
            
            SqlParameter prm;
            Database db = new Database();
            CommonOpn op = new CommonOpn();

            Trace.Warn("ModelId ... " + modelId);

            if (modelId == "" || modelId == "0")
                modelId = "";

            try
            {
                using(SqlConnection con=new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "CON_UpdateExpectedBikeLaunches";
                        cmd.Connection = con;

                        prm = cmd.Parameters.Add("@Id", SqlDbType.BigInt);
                        prm.Value = Id;
                        Trace.Warn("Id ... " + Id);

                        prm = cmd.Parameters.Add("@ExpectedLaunch", SqlDbType.VarChar, 250);
                        prm.Value = expLaunch;
                        Trace.Warn("expLaunch ... " + expLaunch);

                        prm = cmd.Parameters.Add("@LaunchDate", SqlDbType.DateTime);
                        prm.Value = newLaunchDate;
                        Trace.Warn("newLaunchDate ... " + newLaunchDate);

                        prm = cmd.Parameters.Add("@EstimatedPriceMin", SqlDbType.BigInt);
                        prm.Value = minPrice;
                        Trace.Warn("minPrice ... " + minPrice);

                        prm = cmd.Parameters.Add("@EstimatedPriceMax", SqlDbType.BigInt);
                        prm.Value = maxPrice;
                        Trace.Warn("EstimatedPriceMax ... " + maxPrice);

                        if (!String.IsNullOrEmpty(modelId))
                        {
                            if (!String.IsNullOrEmpty(filSmall.Value))
                            {
                                smallPicImgPath = ("/bikewaleimg/upcoming/" + cName.Replace(" ", "") + "-" + modelId + "us.jpg?" + timeStamp).ToLower();
                            }
                            if (!String.IsNullOrEmpty(filLarge.Value))
                            {
                                largePicImgPath = ("/bikewaleimg/upcoming/" + cName.Replace(" ", "") + "-" + modelId + "ub.jpg?" + timeStamp).ToLower();
                            }
                        }
                        cmd.Parameters.Add("@SmallPicImagePath", SqlDbType.VarChar, 100).Value = smallPicImgPath;
                        cmd.Parameters.Add("@LargePicImagePath", SqlDbType.VarChar, 100).Value = largePicImgPath;

                        prm = cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 100);
                        if (!String.IsNullOrEmpty(filSmall.Value) || !String.IsNullOrEmpty(filLarge.Value))
                            prm.Value = ConfigurationManager.AppSettings["imgHostURL"];
                        else
                        {
                            prm.Value = hostUrl;
                            Trace.Warn("Host URL : "+ hostUrl);
                        }

                        if (!String.IsNullOrEmpty(filSmall.Value) || !String.IsNullOrEmpty(filLarge.Value))
                        {
                            cmd.Parameters.Add("@IsReplication", SqlDbType.Bit).Value = 0;
                            isReplicated = "0";
                        }
                        else
                            cmd.Parameters.Add("@IsReplication", SqlDbType.Bit).Value = 1;

                        Trace.Warn("Url  ... " + ConfigurationManager.AppSettings["imgHostURL"]);

                        con.Open();
                        //run the command
                        cmd.ExecuteNonQuery();
                        retVal = true;
                    }
                    con.Close();
                }
            }
            catch (SqlException err)
            {
                //catch the sql exception. if it is equal to 2627, then say that it is for duplicate entry 
                HttpContext.Current.Trace.Warn(err.Message.ToString());
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.UpdateLaunchDate");
                objErr.SendMail();
                Trace.Warn("SqlException");
                retVal = false;
                //retVal = "SqlException : " + err.Message;
            } // catch SqlException
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message.ToString());
                ErrorClass objErr = new ErrorClass(err, "AjaxFunctions.UpdateLaunchDate");
                objErr.SendMail();
                Trace.Warn("Exception");
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
            string imgPath = ImagingOperations.GetPathToSaveImages("\\bikewaleimg\\upcoming\\");
            string hostUrl = ConfigurationManager.AppSettings["RabbitImgHostURL"].ToString();
            string imageUrl = ("http://" + hostUrl + "/bikewaleimg/upcoming/" + cName.Replace(" ", "") + "-" + modelId).ToLower();
           
            if (!Directory.Exists(imgPath))
            {
                Directory.CreateDirectory(imgPath);
            }

            HttpContext.Current.Trace.Warn("Saving File...");

            Trace.Warn("Small Pic ... " + filSmall);
            Trace.Warn("Large Pic ... " + filLarge);
            Trace.Warn("Model Id .... " + modelId);
            Trace.Warn("inside rabbitmq");
            Trace.Warn("image url : " + imageUrl);
            Trace.Warn("host url : " + hostUrl);
            Trace.Warn("imgPath=" + imgPath);
            Trace.Warn("id : "+ Id);
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
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(false));
            nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ONLYREPLICATE).ToLower(), Convert.ToString(true));

            if (modelId != "" && modelId != "0")
            {
                if (!String.IsNullOrEmpty(filSmall.Value))
                {
                    ImagingOperations.SaveImageContent(filSmall, ("/bikewaleimg/upcoming/" + cName.Replace(" ", "") + "-" + modelId + "us.jpg").ToLower());
                    nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl + "us.jpg");
                    nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), "/bikewaleimg/upcoming/" + cName.Replace(" ", "").ToLower() + "-" + modelId + "us.jpg?" + timeStamp);
                    rabbitmqPublish.PublishToQueue("BikeImage", nvc);
                }
                if (!String.IsNullOrEmpty(filLarge.Value))
                {
                    ImagingOperations.SaveImageContent(filLarge, ("/bikewaleimg/upcoming/" + cName.Replace(" ", "") + "-" + modelId + "ub.jpg").ToLower());
                    nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl + "ub.jpg");
                    nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), "/bikewaleimg/upcoming/" + cName.Replace(" ", "").ToLower() + "-" + modelId + "ub.jpg?" + timeStamp);
                    rabbitmqPublish.PublishToQueue("BikeImage", nvc);
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
            Database db=new Database();

            try
            {
            using(SqlConnection con=new SqlConnection(db.GetConString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (Request.QueryString["Id"] != null && Request.QueryString["Id"].ToString() != "")
                    {
                        Id = Request.QueryString["Id"].ToString();
                    }

                    cmd.CommandText = "CON_GetExpectedBikeLaunches";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;

                    cmd.Parameters.Add("@LaunchDate", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@MakeName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ModelName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@EstimatedPriceMin", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@EstimatedPriceMax", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@HostURL", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@SmallPicImagePath", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@LargePicImagePath", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Date", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@IsReplicated", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    expLaunch = cmd.Parameters["@LaunchDate"].Value.ToString();
                    cName = cmd.Parameters["@MakeName"].Value.ToString() + "-" + cmd.Parameters["@ModelName"].Value.ToString();
                    estMaxPri = cmd.Parameters["@EstimatedPriceMax"].Value.ToString();
                    estMinPri = cmd.Parameters["@EstimatedPriceMin"].Value.ToString();
                    hostUrl = cmd.Parameters["@HostURL"].Value.ToString();
                    smallPicImgPath = cmd.Parameters["@SmallPicImagePath"].Value.ToString();
                    largePicImgPath = cmd.Parameters["@LargePicImagePath"].Value.ToString();
                    modelId = cmd.Parameters["@ModelId"].Value.ToString();
                    date = cmd.Parameters["@Date"].Value.ToString();
                    isReplicated = cmd.Parameters["@IsReplicated"].Value.ToString();
                    Trace.Warn("model id....: " + modelId);
                }
                con.Close();
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