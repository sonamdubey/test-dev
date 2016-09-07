using BikewaleOpr.common;
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
    public partial class BikeComparisonList : Page
    {
        protected DropDownList drpMake1, drpModel1, drpVersion1, drpMake2, drpModel2, drpVersion2;
        protected Button btnSave, btnCancel;
        protected Label lblMessage;
        protected Repeater MyRepeater;
        protected HtmlInputHidden hdn_drpModel1, hdn_drpVersion1, hdn_drpModel2, hdn_drpVersion2;
        protected HtmlInputFile filPhoto;
        protected HtmlInputCheckBox chkIsActive;
        protected string cId = string.Empty, hostUrl = string.Empty, originalImgPath = string.Empty, timeStamp = CommonOpn.GetTimeStamp();
        public string hostURL = string.Empty;

        public string SelectedVersion1
        {
            get
            {
                if (Request.Form["drpVersion1"] != null && Request.Form["drpVersion1"].ToString() != "")
                    return Request.Form["drpVersion1"].ToString();
                else
                    return "0";
            }
        }

        public string SelectedModel1
        {
            get
            {
                if (Request.Form["drpModel1"] != null && Request.Form["drpModel1"].ToString() != "")
                    return Request.Form["drpModel1"].ToString();
                else
                    return "0";
            }
        }

        public string SelectedModel2
        {
            get
            {
                if (Request.Form["drpModel1"] != null && Request.Form["drpModel1"].ToString() != "")
                    return Request.Form["drpModel1"].ToString();
                else
                    return "0";
            }
        }
        public string SelectedVersion2
        {
            get
            {
                if (Request.Form["drpVersion2"] != null && Request.Form["drpVersion2"].ToString() != "")
                    return Request.Form["drpVersion2"].ToString();
                else
                    return "0";
            }
        }

        public string ModelContents1
        {
            get
            {
                if (Request.Form["hdn_drpModel1"] != null && Request.Form["hdn_drpModel1"].ToString() != "")
                    return Request.Form["hdn_drpModel1"].ToString();
                else
                    return "";
            }
        }

        public string ModelContents2
        {
            get
            {
                if (Request.Form["hdn_drpModel2"] != null && Request.Form["hdn_drpModel2"].ToString() != "")
                    return Request.Form["hdn_drpModel2"].ToString();
                else
                    return "";
            }
        }

        public string VersionContents1
        {
            get
            {
                if (Request.Form["hdn_drpVersion1"] != null && Request.Form["hdn_drpVersion1"].ToString() != "")
                    return Request.Form["hdn_drpVersion1"].ToString();
                else
                    return "";
            }
        }

        public string VersionContents2
        {
            get
            {
                if (Request.Form["hdn_drpVersion2"] != null && Request.Form["hdn_drpVersion2"].ToString() != "")
                    return Request.Form["hdn_drpVersion2"].ToString();
                else
                    return "";
            }
        }


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //CommonOpn op = new CommonOpn();
            if (!IsPostBack)
            {
                Ajax.Utility.RegisterTypeForAjax(typeof(AjaxFunctions));
                FillMakes();
                ShowBikeComparision();
                LoadBikeComparision(Request.QueryString["id"]);

            }
            else
            {
                AjaxFunctions aj = new AjaxFunctions();
                aj.UpdateContents(drpModel1, hdn_drpModel1.Value, Request.Form["drpModel1"]);
                aj.UpdateContents(drpVersion1, hdn_drpVersion1.Value, Request.Form["drpVersion1"]);
                aj.UpdateContents(drpModel2, hdn_drpModel2.Value, Request.Form["drpModel2"]);
                aj.UpdateContents(drpVersion2, hdn_drpVersion2.Value, Request.Form["drpVersion2"]);
            }
        }

        //Fill Bike Makes

        void FillMakes()
        {
            MakeModelVersion mmv = new MakeModelVersion();
            DataTable dt = new DataTable();
            try
            {
                dt = mmv.GetMakes("New");
                drpMake1.DataSource = dt;
                drpMake1.DataValueField = "value";
                drpMake1.DataTextField = "text";
                drpMake1.DataBind();

                drpMake2.DataSource = dt;
                drpMake2.DataValueField = "value";
                drpMake2.DataTextField = "text";
                drpMake2.DataBind();
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

            ListItem item = new ListItem("--Select--", "-1");
            drpMake1.Items.Insert(0, item);
            drpMake2.Items.Insert(0, item);
        }

        // Shows all the data from the Con_BikeComparisonList table
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 10th Feb 2014
        /// Summary : to get Image HostUrl, ImagePath, Image Name
        /// Modified By : Sadhana Upadhyay on 13th Feb 2014
        /// Summary : Replaced Inline Query with procedure
        /// </summary>
        void ShowBikeComparision()
        {

            //string sql = " SELECT BL.ID as ID, BMA1.Name + ' ' + BMO1.Name + ' ' + BV1.Name as Bike1, "
            //                + " BMA2.Name + ' ' + BMO2.Name + ' ' +  BV2.Name as Bike2, BL.EntryDate, BL.HostURL, BL.ImagePath, BL.ImageName, BL.DisplayPriority, BL.IsActive "
            //                + " FROM Con_BikeComparisonList BL, BikeVersions AS BV1,  BikeModels AS BMO1, "
            //                + " BikeMakes AS BMA1 , BikeVersions AS BV2,  BikeModels AS BMO2, BikeMakes AS BMA2 "
            //                + " WHERE Bv1.ID = BL.VersionId1 AND BMO1.ID = Bv1.BikeModelId AND BMa1.ID = BMo1.BikeMakeId "
            //                + " AND Bv2.ID = BL.VersionId2 AND BMo2.ID = Bv2.BikeModelId AND BMa2.ID = BMo2.BikeMakeId AND BL.IsArchived = 0 "
            //                + " ORDER BY ID ASC";

            CommonOpn op = new CommonOpn();

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "con_getbikecomparisonlisting";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                        {
                            MyRepeater.DataSource = ds.Tables[0];
                            MyRepeater.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        // Loads data from the id passed in the URL into the drop down
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 10th Feb 2014
        /// Summary : to get Image HostUrl, ImagePath, Image Name
        /// Modified By : Sadhana Upadhyay on 13th feb 2014
        /// Summary : Rplaced Inline query with procedure
        /// </summary>

        void LoadBikeComparision(string URLData)
        {
            AjaxFunctions aj = new AjaxFunctions();

            if (URLData != null)
            {

                btnSave.Text = "Update";
                btnCancel.Visible = true;

                CommonOpn objCom = new CommonOpn();

                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "getbikecomparisondetailbyid";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int32, URLData));


                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                        {
                            if (dr != null && dr.Read())
                            {

                                drpMake1.SelectedValue = dr["Make1"].ToString();

                                drpModel1.DataSource = aj.GetModels(dr["Make1"].ToString());
                                drpModel1.DataTextField = "Text";
                                drpModel1.DataValueField = "Value";
                                drpModel1.DataBind();
                                drpModel1.Items.Insert(0, new ListItem("--Select--", "0"));
                                drpModel1.SelectedIndex = drpModel1.Items.IndexOf(drpModel1.Items.FindByValue(dr["Model1"].ToString()));
                                drpModel1.Enabled = true;

                                drpVersion1.DataSource = aj.GetVersions(dr["Model1"].ToString());
                                drpVersion1.DataTextField = "Text";
                                drpVersion1.DataValueField = "Value";
                                drpVersion1.DataBind();
                                drpVersion1.Items.Insert(0, new ListItem("--Select--", "0"));
                                drpVersion1.SelectedIndex = drpVersion1.Items.IndexOf(drpVersion1.Items.FindByValue(dr["Version1"].ToString()));
                                drpVersion1.Enabled = true;

                                drpMake2.SelectedValue = dr["Make2"].ToString();

                                drpModel2.DataSource = aj.GetModels(dr["Make2"].ToString());
                                drpModel2.DataTextField = "Text";
                                drpModel2.DataValueField = "Value";
                                drpModel2.DataBind();
                                drpModel2.Items.Insert(0, new ListItem("--Select--", "0"));
                                drpModel2.SelectedIndex = drpModel2.Items.IndexOf(drpModel2.Items.FindByValue(dr["Model2"].ToString()));
                                drpModel2.Enabled = true;

                                drpVersion2.DataSource = aj.GetVersions(dr["Model2"].ToString());
                                drpVersion2.DataTextField = "Text";
                                drpVersion2.DataValueField = "Value";
                                drpVersion2.DataBind();
                                drpVersion2.Items.Insert(0, new ListItem("--Select--", "0"));
                                drpVersion2.SelectedIndex = drpVersion2.Items.IndexOf(drpVersion2.Items.FindByValue(dr["Version2"].ToString()));
                                drpVersion2.Enabled = true;

                                hostUrl = dr["HostURL"].ToString();
                                hostURL = dr["HostURL"].ToString();
                                originalImgPath = dr["OriginalImagePath"].ToString();
                                chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"].ToString());
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    Trace.Warn(err.Message + err.Source);
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.ConsumeError();
                }
            }
        }

        // Saves/Updates data in the database
        /// <summary>
        /// Modified By : Sadhana Upadhyay
        /// Summary : To insert/update hostUrl, ImageName, IsActive, IsReplicated, CompId
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object Sender, EventArgs e)
        {
            int isActive = (chkIsActive.Checked == true ? 1 : 0);
            int URLData;

            if (Request.QueryString["id"] == null)
            {
                URLData = -1;
            }
            else
            {
                URLData = Int16.Parse(Request.QueryString["id"]);
            }

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "bikecomparisionlist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int16, URLData));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid1", DbType.Int16, drpVersion1.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid2", DbType.Int16, drpVersion2.SelectedItem.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_entrydate", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isactive", DbType.Boolean, isActive));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_compid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_status", DbType.Int16, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    int Status = Int16.Parse(cmd.Parameters["par_status"].Value.ToString());
                    if (Status == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Duplicate data not allowed');", true);
                    }
                    if (Status == 1)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Data successfully inserted');", true);
                    }
                    if (Status == 2)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Data successfully updated');", true);
                    }
                    cId = cmd.Parameters["par_compid"].Value.ToString();

                    // Removed memcached key which shows data on home page and new page
                    MemCachedUtil.Remove("BW_CompareBikes_Cnt_4");
                    MemCachedUtil.Remove("BW_CompareBikes_Cnt_1");
                    if (!String.IsNullOrEmpty(filPhoto.Value))
                        UploadImage(cId);
                    ShowBikeComparision();
                    Response.Redirect("/content/bikecomparisonlist.aspx");
                }
            }

            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.ConsumeError();
            } // catch SqlException
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.ConsumeError();
            } // catch Exception
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 11th Feb 2014
        /// Summary : To save Compare Bike Image using RabbitMQ
        /// Modified By : Sadhana Upadhyay on 11 Aug 2015
        /// Summary : To change image saving logic for IPC
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        void UploadImage(string cId)
        {
            CommonOpn co = new CommonOpn();
            string imgPath = ImagingOperations.GetPathToSaveImages("\\bw\\bikecomparison\\");


            try
            {
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }

                string imageName = (drpMake1.SelectedItem.Text + "_" + drpModel1.SelectedItem.Text + "_vs_" + drpMake2.SelectedItem.Text + "_" + drpModel2.SelectedItem.Text + ".jpg").Replace(" ", "").ToLower();
                // upload file for temporary purpose
                Trace.Warn("Inside RabbitMQ");
                //rabbitmq code here 
                string hostUrl = ConfigurationManager.AppSettings["RabbitImgHostURL"].ToString();
                string imageUrl = "http://" + hostUrl + "/bw/bikecomparison/" + imageName;
                //Save Original image
                ImagingOperations.SaveImageContent(filPhoto, "/bw/bikecomparison/" + imageName);

                RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                NameValueCollection nvc = new NameValueCollection();
                //add items to nvc
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ID).ToLower(), cId);
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "BikeComparisionList");
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(true));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ONLYREPLICATE).ToLower(), Convert.ToString(true));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl);
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), "/bw/bikecomparison/" + imageName + "?" + timeStamp);
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMASTER).ToLower(), "1");

                rabbitmqPublish.PublishToQueue(ConfigurationManager.AppSettings["ImageQueueName"], nvc);

                SaveBikeComparePhoto(cId);
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }   //End of UploadImage

        /// <summary>
        /// Created By : Sadhana Upadhyay on 19th Feb 2014
        /// Summary : To save Bike Comparison photo
        /// MOdified BY : Sadhana Upadhyay on 11 Aug 2015
        /// Summary : To save Bike photo to IPC
        /// </summary>
        void SaveBikeComparePhoto(string photoId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "savebikecomparisonphoto";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int16, photoId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hosturl", DbType.String, 100, ConfigurationManager.AppSettings["imgHostURL"]));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imagename", DbType.String, 150, (drpMake1.SelectedItem.Text + "_" + drpModel1.SelectedItem.Text + "_vs_" + drpMake2.SelectedItem.Text + "_" + drpModel2.SelectedItem.Text + ".jpg?").Replace(" ", "").ToLower() + timeStamp));

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_imagepath", DbType.String, 50, "/bw/bikecomparison/"));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isreplicated", DbType.Boolean, 0));

                    MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.ConsumeError();
            } // catch SqlException
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.ConsumeError();
            } // catch Exception
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 20th feb 2014
        /// Summary : To cancel Update Operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/content/bikecomparisonlist.aspx");
        }
    }
}
