using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Carwale.UI.Common;
using Carwale.UI.Controls;
using Carwale.Notifications;
using Carwale.Entity.Enum;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using Carwale.Cache.Forums;

namespace Carwale.UI.Editorial
{
    public class UserReviews : Page
    {
        /* ASP.NET CONTROLS */
        protected Label lblMessage;
        protected Button butSave;
        protected TextBox txtTitle, txtEmail, txtPros, txtCons, txtMileage, txtName;
        protected DropDownList drpVersions, ddlFamiliar;
        protected RichTextEditor ftbDescription;
        protected RadioButton radNot, radNew, radOld;

        /* VARIABLES */
        public string modelId = "", versionId = "", customerId = "-1";
        protected string displayVersion = "";
        protected bool isModerator;

        /* HTML CONTROLS */
        protected HtmlGenericControl divEmail, divEmailLabel, divName, divNameLabel;
        protected HtmlInputHidden hdnRateST, hdnRateCM, hdnRatePE, hdnRateVC, hdnRateFE, hdnRateOA;


        public string CarName
        {
            get
            {
                if (ViewState["CarName"] != null)
                    return ViewState["CarName"].ToString();
                else
                    return "";
            }
            set { ViewState["CarName"] = value; }
        }

        public string CarMake
        {
            get
            {
                if (ViewState["CarMake"] != null)
                    return ViewState["CarMake"].ToString();
                else
                    return "";
            }
            set { ViewState["CarMake"] = value; }
        }

        public string CarModel
        {
            get
            {
                if (ViewState["CarModel"] != null)
                    return ViewState["CarModel"].ToString();
                else
                    return "";
            }
            set { ViewState["CarModel"] = value; }
        }
        public string MaskingName
        {
            get
            {
                if (ViewState["MaskingName"] != null)
                    return ViewState["MaskingName"].ToString();
                else
                    return "";
            }
            set { ViewState["MaskingName"] = value; }
        }

        public string CarVersion
        {
            get
            {
                if (ViewState["CarVersion"] != null)
                    return ViewState["CarVersion"].ToString();
                else
                    return "";
            }
            set { ViewState["CarVersion"] = value; }
        }

        public string MakeId
        {
            get
            {
                if (ViewState["MakeId"] != null)
                    return ViewState["MakeId"].ToString();
                else
                    return "-1";
            }
            set { ViewState["MakeId"] = value; }
        }

        public string ModelIdVer
        {
            get
            {
                if (ViewState["ModelIdVer"] != null)
                    return ViewState["ModelIdVer"].ToString();
                else
                    return "-1";
            }
            set { ViewState["ModelIdVer"] = value; }
        }

        public string BackUrl
        {
            get
            {
                if (ViewState["BackUrl"] != null)
                    return ViewState["BackUrl"].ToString();
                else
                    return "";
            }
            set { ViewState["BackUrl"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
            butSave.Click += new EventHandler(butSave_Click);
        }

        void Page_Load(object Sender, EventArgs e)
        {
            //also get the forumId
            if (Request["carm"] != null && Request.QueryString["carm"] != "")
            {
                modelId = Request.QueryString["carm"];
                if (CommonOpn.CheckId(modelId) == false)
                {
                    //redirect to the error page
                    UrlRewrite.Return404();
                    return;
                }
            }
            else if (Request["carv"] != null && Request.QueryString["carv"] != "")
            {           
                versionId = Request.QueryString["carv"];                
                if (CommonOpn.CheckId(versionId) == false)
                {
                    //redirect to the error page
                    UrlRewrite.Return404();
                    return;
                }              
            }
            else
                UrlRewrite.Return404();

            customerId = CurrentUser.Id;
            ForumsCache threadInfo = new ForumsCache();
            isModerator = threadInfo.IsModerator(CurrentUser.Id);
            if (!IsPostBack)
            {
                LoadDefaultComments();
                CarName = GetCar();
                if (CarName == "")
                    UrlRewrite.Return404();
                if (modelId != "")
                {
                    BackUrl = "/Research/BrowseCarsByVersion.aspx?model=" + MaskingName;
                    displayVersion = "display;";
                    FillVersions();
                }
                else
                {
                    BackUrl = "/Research/overview.aspx?car=" + versionId + "&model=" + ModelIdVer;
                    displayVersion = "none;";
                }

                //if the customer is already registered then dont ask information for the 
                //customer name and location, else ask the name, email and the location
                if (customerId == "-1")
                {
                    divEmail.Visible = true;
                    divEmailLabel.Visible = true;
                    divName.Visible = true;
                    divNameLabel.Visible = true;
                }
                else
                {
                    divEmail.Visible = false;
                    divEmailLabel.Visible = false;
                    divName.Visible = false;
                    divNameLabel.Visible = false;
                }


                if (CheckVersionReview() == true)
                {
                    //a review has already been added for this version hence return false
                    string car = versionId == "" ? (CarName + " " + drpVersions.SelectedItem.Text) : CarName;
                    lblMessage.Text = "You have already added a review for " + car + ".<br>You are not allowed to add more than one review for the same version of a car.<br><br>";
                    butSave.Enabled = false;
                }
            }
        } // Page_Load

        private void LoadDefaultComments()
        {
            string defaultComments = "";
            defaultComments = " <p><strong>Exterior</strong></p>"
                            + " <p>&nbsp;</p>"
                            + " <p><strong>Interior (Features, Space &amp; Comfort)</strong></p>"
                            + " <p>&nbsp;</p>"
                            + " <p><strong>Engine Performance, Fuel Economy and Gearbox</strong></p>"
                            + " <p>&nbsp;</p>"
                            + " <p><strong>Ride Quality &amp; Handling</strong></p>"
                            + " <p>&nbsp;</p>"
                            + " <p><strong>Final Words</strong></p>"
                            + " <p>&nbsp;</p>"
                            + " <p><strong>Areas of improvement</strong>&nbsp;&nbsp;</p>"
                            + " <p>&nbsp;</p>"
                            + " <p>&nbsp;</p>";
            ftbDescription.Text = defaultComments;
        }


        void butSave_Click(object Sender, EventArgs e)
        {

            if (CheckVersionReview() == true)
            {
                //a review has already been added for this version hence return false
                string car = versionId == "" ? (CarName + " " + drpVersions.SelectedItem.Text) : CarName;

                lblMessage.Text = "You have already added a review for " + car + ".<br>You are not allowed to add more than one review for the same version of a car.<br><br>";
                return;
            }
            if (customerId == "-1")
            {
                RegisterCustomer();
            }
            string recordId = SaveDetails();
            string msgPage = "/Research/ReviewMessage.aspx?url=" + BackUrl;

            Response.Redirect(msgPage);

        }


        public string GetCar()
        {
            string car = "";     
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetBasicCarDetailsById_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, !string.IsNullOrEmpty(modelId) ? modelId : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, !string.IsNullOrEmpty(versionId) ? versionId :Convert.DBNull));
                    using(IDataReader dr  = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            CarMake = dr["Make"].ToString();
                            CarModel = dr["Model"].ToString();
                            CarVersion = dr["Version"].ToString();
                            car = (CarMake + " " + CarModel + " " + CarVersion).Trim();
                            ModelIdVer = dr["ModelId"].ToString();
                            MakeId = dr["MakeId"].ToString();
                            MaskingName = dr["MaskingName"].ToString();
                        }
                    }
                }             
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)// catch Exception
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return car;
        }

        //to do
        public bool CheckVersionReview()
        {
            bool found = false;
            string id = versionId == "" ? drpVersions.SelectedItem.Value : versionId;
            string email = txtEmail.Text.Trim().Replace("'", "''");
            try
            {
                if (id != "-1")
                {
                    if (customerId != "-1" || !string.IsNullOrEmpty(email))
                    {
                        using (DbCommand cmd = DbFactory.GetDBCommand("GetUserReviewCount_v16_11_7"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId != "-1" ? customerId : Convert.DBNull));
                            cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, !string.IsNullOrEmpty(id) ? id : Convert.DBNull));
                            cmd.Parameters.Add(DbFactory.GetDbParam("v_Email", DbType.String, 100, !string.IsNullOrEmpty(txtEmail.Text.Trim()) ? email : Convert.DBNull));
                            using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                            {
                                if (dr.Read())
                                {
                                    if (Convert.ToInt32(dr[0]) > 0)
                                        found = true;
                                    else
                                        found = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exceptio
            return found;
        }


        void FillVersions()
        {               
            try
            {          
                DataSet ds = new DataSet();
                CommonOpn op = new CommonOpn();
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCarVersions_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionCond", DbType.String, 10, "All"));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int32, modelId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);                    
                }          
                if (ds != null && ds.Tables.Count > 0)
                    op.FillDropDownMySql(ds.Tables[0], drpVersions, "Name", "ID", "For all versions");
                drpVersions.Items.RemoveAt(0);
                drpVersions.Items.Insert(0, new ListItem("For all versions", "-1"));
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } 
        }

        string SaveDetails()
        {
            string recordId = "";
            CommonOpn op = new CommonOpn();
            if (versionId == "")
                versionId = drpVersions.SelectedItem.Value;  
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("EntryCustomerReviews_v17_6_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId",DbType.Int64,customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId",DbType.Int32,MakeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId",DbType.Int32,ModelIdVer));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId",DbType.Int32,versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StyleR",DbType.Int16,hdnRateST.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ComfortR",DbType.Int16,hdnRateCM.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PerformanceR",DbType.Int16,hdnRatePE.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ValueR",DbType.Int16,hdnRateVC.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_FuelEconomyR",DbType.Int16,hdnRateFE.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_OverallR",DbType.Decimal,(Convert.ToInt32(hdnRateST.Value) + Convert.ToInt32(hdnRateCM.Value) + Convert.ToInt32(hdnRatePE.Value) + Convert.ToInt32(hdnRateVC.Value) + Convert.ToInt32(hdnRateFE.Value)) / 5));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Pros",DbType.String,100,txtPros.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Cons",DbType.String,100,txtCons.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Comments",DbType.String,8000,SanitizeHTML.ToSafeHtml(ftbDescription.Text.Trim())));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Title",DbType.String,100,txtTitle.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_EntryDateTime",DbType.DateTime,DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ID",DbType.Int32,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsOwned",DbType.Boolean,!(radNot.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsNewlyPurchased",DbType.Boolean,radNew.Checked));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Familiarity",DbType.Int32,ddlFamiliar.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PlatformId", DbType.Int32,Platform.CarwaleDesktop));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Mileage", DbType.Decimal, txtMileage.Text.Trim() != "" ? txtMileage.Text.Trim().Replace("'", "''") : "0"));
                    MySqlDatabase.InsertQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
                    recordId = cmd.Parameters["v_ID"].Value.ToString();
                }
              
                SourceIdCommon.UpdateSourceId(EnumTableType.CustomerReviews, recordId);
            }
            catch (MySqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch SqlException
            catch (Exception err)
            {
                Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            return recordId;
        }

        public void RegisterCustomer()
        {
            AutomateRegistration ar = new AutomateRegistration();
            AutomateRegistrationResult arr = ar.ProcessRequest(txtName.Text.Trim(), txtEmail.Text.Trim(), "", "", "", "", "");
            customerId = arr.CustomerId;
        }
    } // class
} // namespace