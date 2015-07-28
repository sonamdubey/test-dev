using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BikeWaleOpr.Common;
using System.Configuration;
using RabbitMqPublishing;
using BikeWaleOpr.RabbitMQ;
using System.Collections.Specialized;
using System.IO;

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
        protected string cId = string.Empty, hostUrl = string.Empty, imagePath = string.Empty, imgName = string.Empty, timeStamp = CommonOpn.GetTimeStamp();
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
            else {
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
                Database db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "Con_GetBikeComparisonListing";
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataSet ds = new DataSet();
                    ds = db.SelectAdaptQry(cmd);
                    Trace.Warn("Dataset : "+ ds);
                    MyRepeater.DataSource = ds.Tables[0];
                    MyRepeater.DataBind();
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
            SqlDataReader dr = null;
            AjaxFunctions aj = new AjaxFunctions();
            Trace.Warn("Load Bike comparison");
            if (URLData != null)
            {

                btnSave.Text = "Update";
                btnCancel.Visible = true;

                //sql = " SELECT BL.ID, Ma1.ID as Make1 , Mo1.ID as Model1 , Bv1.ID AS Version1, "
                //    + " Ma2.ID as Make2 , Mo2.ID as Model2 , Bv2.ID AS Version2, BL.HostURL, BL.ImagePath, BL.ImageName, BL.DisplayPriority, BL.IsActive "
                //    + " FROM Con_BikeComparisonList BL, BikeMakes AS Ma1, BikeModels AS Mo1, BikeVersions AS Bv1, "
                //    + " BikeMakes AS Ma2, BikeModels AS Mo2, BikeVersions AS Bv2 "
                //    + " WHERE Bv1.ID = BL.VersionId1 AND Mo1.ID = Bv1.BikeModelId AND Ma1.ID = Mo1.BikeMakeId "
                //    + " AND Bv2.ID = BL.VersionId2 AND Mo2.ID = Bv2.BikeModelId AND "
                //    + " Ma2.ID = Mo2.BikeMakeId AND BL.ID = " + URLData;							

                CommonOpn objCom = new CommonOpn();

                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                      
                        Database db = new Database();
                        cmd.CommandText = "GetBikeComparisonDetailById";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = URLData;

                        dr = db.SelectQry(cmd);

                        if (dr.Read())
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
                            imagePath = dr["ImagePath"].ToString();
                            imgName = dr["ImageName"].ToString();
                            Trace.Warn(hostUrl + imagePath + imgName);
                            chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"].ToString());
                        }
                    }
                }
                catch (Exception err)
                {
                    Trace.Warn(err.Message + err.Source);
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.ConsumeError();
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
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
            SqlParameter prm;
            Database db = new Database();
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
                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        Trace.Warn("Connection : "+con);
                        cmd.CommandText = "BikeComparisionList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        prm = cmd.Parameters.Add("@Id", SqlDbType.SmallInt);
                        prm.Value = URLData;

                        prm = cmd.Parameters.Add("@VersionId1", SqlDbType.SmallInt);
                        prm.Value = drpVersion1.SelectedItem.Value;

                        prm = cmd.Parameters.Add("@VersionId2", SqlDbType.SmallInt);
                        prm.Value = drpVersion2.SelectedItem.Value;

                        prm = cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime);
                        prm.Value = DateTime.Now;

                        prm = cmd.Parameters.Add("@IsActive", SqlDbType.Bit);
                        prm.Value = isActive;

                        cmd.Parameters.Add("@CompId", SqlDbType.Int).Direction = ParameterDirection.Output;

                        prm = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        prm.Direction = ParameterDirection.Output;

                        

                        con.Open();
                        //run the command
                        cmd.ExecuteNonQuery();

                        int Status = Int16.Parse(cmd.Parameters["@Status"].Value.ToString());
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
                        cId = cmd.Parameters["@CompId"].Value.ToString();
                        if(!String.IsNullOrEmpty(filPhoto.Value))
                            UploadImage(cId);
                        ShowBikeComparision();
                        Response.Redirect("/content/bikecomparisonlist.aspx");
                    }
                    con.Close();
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
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        void UploadImage(string cId)
        {
            CommonOpn co = new CommonOpn();
            string imgPath = ImagingOperations.GetPathToSaveImages("\\bikewaleimg\\bikecomparison\\");
            

            try
            {
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }

                // Trace.Warn("Saving Path : " + galleryPath + drpVersion.SelectedValue + "_" + drpVersion1.SelectedValue + ".jpg");
                string imageName = (drpMake1.SelectedItem.Text + "_" + drpModel1.SelectedItem.Text + "_vs_" + drpMake2.SelectedItem.Text + "_" + drpModel2.SelectedItem.Text + ".jpg").Replace(" ","").ToLower();
                // upload file for temporary purpose
                Trace.Warn("Inside RabbitMQ");
                //rabbitmq code here 
                string hostUrl = ConfigurationManager.AppSettings["RabbitImgHostURL"].ToString();
                string imageUrl = "http://" + hostUrl + "/bikewaleimg/bikecomparison/" + imageName;
                RabbitMqPublish rabbitmqPublish = new RabbitMqPublish();
                NameValueCollection nvc = new NameValueCollection();
                //add items to nvc
                ImagingOperations.SaveImageContent(filPhoto, "/bikewaleimg/bikecomparison/" + imageName);
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ID).ToLower(), cId);
                Trace.Warn("Compare Id : " + cId);
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CATEGORY).ToLower(), "BikeComparisionList");
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEWIDTH).ToLower(), "-1");
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.CUSTOMSIZEHEIGHT).ToLower(), "-1");
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISWATERMARK).ToLower(), Convert.ToString(false));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISCROP).ToLower(), Convert.ToString(false));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ISMAIN).ToLower(), Convert.ToString(false));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.SAVEORIGINAL).ToLower(), Convert.ToString(false));
                nvc.Add(BikeCommonRQ.GetDescription(ImageKeys.ONLYREPLICATE).ToLower(), Convert.ToString(true));
                nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.LOCATION).ToLower(), imageUrl);
                nvc.Set(BikeCommonRQ.GetDescription(ImageKeys.IMAGETARGETPATH).ToLower(), "/bikewaleimg/bikecomparison/" + imageName + "?" + timeStamp);
                rabbitmqPublish.PublishToQueue("BikeImage", nvc);

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
        /// </summary>
        void SaveBikeComparePhoto(string photoId)
        {
            SqlParameter prm;
            try
            {
            Database db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SaveBikeComparisonPhoto";
                    cmd.CommandType = CommandType.StoredProcedure;

                    prm = cmd.Parameters.Add("@Id", SqlDbType.SmallInt);
                    prm.Value = photoId;

                    prm = cmd.Parameters.Add("@HostUrl", SqlDbType.VarChar, 100);
                    prm.Value = ConfigurationManager.AppSettings["imgHostURL"];

                    Trace.Warn("Host URL : " + hostURL);
                    prm = cmd.Parameters.Add("@ImageName", SqlDbType.VarChar, 150);
                    prm.Value = (drpMake1.SelectedItem.Text + "_" + drpModel1.SelectedItem.Text + "_vs_" + drpMake2.SelectedItem.Text + "_" + drpModel2.SelectedItem.Text + ".jpg?").Replace(" ", "").ToLower() + timeStamp;

                    prm = cmd.Parameters.Add("@ImagePath", SqlDbType.VarChar, 50);
                    prm.Value = "/bikewaleimg/bikecomparison/";

                    prm = cmd.Parameters.Add("@IsReplicated", SqlDbType.Bit);
                    prm.Value = 0;

                    db.UpdateQry(cmd);
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
