using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bikewale.Common;
using Bikewale.Controls;

namespace Bikewale.Content
{
    public class WriteReviews : System.Web.UI.Page
    {
        /* ASP.NET CONTROLS */
        protected Label lblMessage;
        protected Button butSave;
        protected TextBox txtTitle, txtEmail, txtPros, txtCons, txtMileage, txtName;
        protected DropDownList drpVersions, ddlFamiliar;
        protected RichTextEditor ftbDescription;
        protected RadioButton radNot, radNew, radOld;

        /* VARIABLES */
        public string modelId = "", versionId = "", customerId = "";
        protected string displayVersion = "";

        /* HTML CONTROLS */
       // protected HtmlGenericControl divEmail, divEmailLabel;
        protected HtmlTableRow trEmail, trName;
        protected HtmlInputHidden hdnRateST, hdnRateCM, hdnRatePE, hdnRateVC, hdnRateFE, hdnRateOA;

        public string BikeName
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

        public string BikeMake
        {
            get
            {
                if (ViewState["BikeMake"] != null)
                    return ViewState["BikeMake"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeMake"] = value; }
        }

        public string BikeModel
        {
            get
            {
                if (ViewState["BikeModel"] != null)
                    return ViewState["BikeModel"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeModel"] = value; }
        }

        public string CarVersion
        {
            get
            {
                if (ViewState["BikeVersion"] != null)
                    return ViewState["BikeVersion"].ToString();
                else
                    return "";
            }
            set { ViewState["BikeVersion"] = value; }
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
            base.Load += new EventHandler(Page_Load);
            butSave.Click += new EventHandler(butSave_Click);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            //also get the forumId
            if (Request["bikem"] != null && Request.QueryString["bikem"] != "")
            {
                modelId = Request.QueryString["bikem"];

                //verify the id as passed in the url
                if (CommonOpn.CheckId(modelId) == false)
                {
                    //redirect to the error page
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
            else if (Request["bikev"] != null && Request.QueryString["bikev"] != "")
            {
                Trace.Warn("Carv");
                versionId = Request.QueryString["bikev"];
                Trace.Warn("Carv1");
                //verify the id as passed in the url
                if (CommonOpn.CheckId(versionId) == false)
                {
                    //redirect to the error page
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
                Trace.Warn("Carv1");
            }
            else
            {
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx",false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

            customerId = CurrentUser.Id;

            if (!IsPostBack)
            {                
                if(customerId=="-1")
                {                   
                    Response.Redirect("/users/login.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_REWRITE_URL"]));
                }
                LoadDefaultComments();
                BikeName = GetCar();

                if (BikeName == "")
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }

                if (modelId != "")
                {
                    MakeModelVersion mmv = new MakeModelVersion();
                    mmv.GetModelDetails(modelId);                 
                    BackUrl = "/" + UrlRewrite.FormatURL(mmv.Make) + "-bikes/" + UrlRewrite.FormatURL(mmv.Model) + "/";
                    displayVersion = "display;";
                    FillVersions();
                }
                else
                {
                   // BackUrl = "/content/overview.aspx?car=" + versionId + "&model=" + ModelIdVer;
                    displayVersion = "none;";
                }

                //if the customer is already registered then dont ask information for the 
                //customer name and location, else ask the name, email and the location
                if (customerId == "-1")
                {
                    trEmail.Visible = true;
                    trName.Visible = true;
                }
                else
                {
                    trEmail.Visible = false;
                    trName.Visible = false;
                }


                if (CheckVersionReview() == true)
                {
                    //a review has already been added for this version hence return false
                    string Bike = versionId == "" ? (BikeName + " " + drpVersions.SelectedItem.Text) : BikeName;

                    lblMessage.Text = "You have already added a review for " + Bike + ".<br>You are not allowed to add more than one review for the same version of a bike.<br><br>";
                    butSave.Enabled = false;
                }
            }
        }


        private void LoadDefaultComments()
        {
            string defaultComments = "";
            defaultComments = " <p><strong>Style</strong></p>"
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
                string Bike = versionId == "" ? (BikeName + " " + drpVersions.SelectedItem.Text) : BikeName;

                lblMessage.Text = "You have already added a review for " + Bike + ".<br>You are not allowed to add more than one review for the same version of a Bike.<br><br>";
                return;
            }

            string recordId = SaveDetails();
         //   string msgPage = "/content/userreviews/reviewmessage.aspx?url=" + BackUrl;

            string msgPage = "/content/userreviews/reviewmessage.aspx";

            if (customerId == "-1")
            {
                //add this record in Customer Table to add this customer

                //make the minimal registration
                //MinimalRegistration mr = new MinimalRegistration(txtEmail.Text.Trim(),
                //                                                 "",
                //                                                 "Review written for id #" + recordId,
                //                                                 recordId,
                //                                                 EnumRegistrationType.ReviewType);
                //mr.SaveData();

                SaveCustomer();
                //redirect to the login page
               // Response.Redirect("/Research/NotAuthorized.aspx?redirect=" + msgPage);
                Response.Redirect(msgPage);
            }
            else
            {
                //redirect it to message page
                Response.Redirect(msgPage);
            }
        }

        private void SaveCustomer()
        {
            using (SqlCommand cmd = new SqlCommand("RegisterCustomer"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = txtEmail.Text.Trim();
                Database db = new Database();
                try
                {
                    db.InsertQry(cmd);
                }
                catch (Exception ex)
                {
                    Trace.Warn(ex.Message);
                    ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
        }

        public string GetCar()
        {
            string Bike = "";
            Database db = new Database();
            SqlDataReader dr = null;

            string sql = "";

            if (modelId != "")
                sql = " SELECT MA.Name AS Make, MO.Name AS Model, '' AS Version, MA.ID AS MakeId, MO.ID AS ModelId FROM BikeModels AS MO, BikeMakes AS MA With(NoLock) "
                    + " WHERE MA.ID = MO.BikeMakeId AND MO.ID = @modelId";
            else
                sql = " SELECT MA.Name AS Make, MO.Name AS Model, CV.Name AS Version, MA.ID AS MakeId, MO.ID AS ModelId FROM BikeModels AS MO, BikeMakes AS MA, BikeVersions AS CV With(NoLock) "
                    + " WHERE MA.ID = MO.BikeMakeId AND MO.ID = CV.BikeModelId AND CV.ID = @versionId";

            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.Add("@modelId", SqlDbType.BigInt).Value = (modelId != "" ? modelId : "-1");
            cmd.Parameters.Add("@versionId", SqlDbType.BigInt).Value = (versionId != "" ? versionId : "-1");

            try
            {
                dr = db.SelectQry(cmd);

                if (dr.Read())
                {
                    BikeMake = dr["Make"].ToString();
                    BikeModel = dr["Model"].ToString();
                    CarVersion = dr["Version"].ToString();
                    Bike = (BikeMake + " " + BikeModel + " " + CarVersion).Trim();
                    ModelIdVer = dr["ModelId"].ToString();
                    MakeId = dr["MakeId"].ToString();
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
            finally
            {
                if (dr != null)
                    dr.Close();  

                db.CloseConnection();
            }
            return Bike;
        }

        public bool CheckVersionReview()
        {
            //this function checks whether this user has already added a review for this version
            bool found = false;
            Database db = new Database();
            SqlDataReader dr = null;

            string sql = "";
            string id = versionId == "" ? drpVersions.SelectedItem.Value : versionId;
            Trace.Warn("CheckVersionReview : " + id);

            string email = txtEmail.Text.Trim().Replace("'", "''");

            if (id != "-1")
            {
                if (customerId != "-1")
                {
                    sql = " SELECT Count(Id) FROM CustomerReviews With(NoLock) "
                        + " WHERE CustomerId = @CustomerId AND VersionId = @VersionId";
                }
                else if (txtEmail.Text.Trim() != "")
                {
                    sql = " SELECT Count(Id) FROM CustomerReviews With(NoLock) WHERE "
                        + " (ID IN (Select RecordId From UnregisteredCustomers With(NoLock) Where "
                        + " Email = @Email AND RegistrationType = 3) "
                        + " OR CustomerId IN (Select Id From Customers With(NoLock) Where "
                        + " Email = @Email)) AND IsActive=1 AND VersionId = @VersionId";
                }

                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt).Value = (customerId != "" ? customerId : "-1");
                cmd.Parameters.Add("@VersionId", SqlDbType.BigInt).Value = (id != "" ? id : "-1");
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = email;

                try
                {
                    if (sql != "")
                    {
                        dr = db.SelectQry(cmd);

                        if (dr.Read())
                        {
                            if (Convert.ToInt32(dr[0]) > 0)
                                found = true;
                            else
                                found = false;
                        }                     
                    }
                }
                catch (SqlException err)
                {
                    Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                } // catch Exception
                finally
                {
                    if (dr != null)
                        dr.Close();  

                    db.CloseConnection();
                }
            }
            return found;
        }


        void FillVersions()
        {
            string sql = "";
            CommonOpn op = new CommonOpn();
            try
            {
                sql = " SELECT Name, ID FROM BikeVersions With(NoLock) WHERE IsDeleted = 0 AND BikeModelId = @BikeModelId ORDER BY Name ";

                SqlParameter[] param = 
				{
					new SqlParameter("@BikeModelId", modelId)
				};

                op.FillDropDown(sql, drpVersions, "Name", "ID", param);

                drpVersions.Items.Insert(0, new ListItem("For all versions", "-1"));
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception
        }

        /// <summary>
        /// Modified By : Sadhana Upadhyay on 2nd April 2014
        /// Summary : To capture Client IP
        /// </summary>
        /// <returns></returns>
        string SaveDetails()
        {
            string recordId = "";

            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();
            CommonOpn op = new CommonOpn();

            string conStr = db.GetConString();

            con = new SqlConnection(conStr);

            if (versionId == "")
                versionId = drpVersions.SelectedItem.Value;

            Trace.Warn("versionId : " + versionId);
            try
            {
                cmd = new SqlCommand("EntryCustomerReviews", con);
                cmd.CommandType = CommandType.StoredProcedure;

                prm = cmd.Parameters.Add("@CustomerId", SqlDbType.BigInt);
                prm.Value = customerId;
                Trace.Warn("customerId : " + customerId);

                prm = cmd.Parameters.Add("@MakeId", SqlDbType.BigInt);
                prm.Value = MakeId;
                Trace.Warn("MakeId : " + MakeId);

                prm = cmd.Parameters.Add("@ModelId", SqlDbType.BigInt);
                prm.Value = ModelIdVer;
                Trace.Warn("ModelIdVer : " + ModelIdVer);

                prm = cmd.Parameters.Add("@VersionId", SqlDbType.BigInt);
                prm.Value = versionId;

                prm = cmd.Parameters.Add("@StyleR", SqlDbType.SmallInt);
                prm.Value = hdnRateST.Value;
                Trace.Warn("hdnRateST.Value : " + hdnRateST.Value);

                prm = cmd.Parameters.Add("@ComfortR", SqlDbType.SmallInt);
                prm.Value = hdnRateCM.Value;
                Trace.Warn("hdnRateCM.Value : " + hdnRateCM.Value);

                prm = cmd.Parameters.Add("@PerformanceR", SqlDbType.SmallInt);
                prm.Value = hdnRatePE.Value;
                Trace.Warn("hdnRatePE.Value : " + hdnRatePE.Value);

                prm = cmd.Parameters.Add("@ValueR", SqlDbType.SmallInt);
                prm.Value = hdnRateVC.Value;
                Trace.Warn("hdnRateVC.Value : " + hdnRateVC.Value);

                prm = cmd.Parameters.Add("@FuelEconomyR", SqlDbType.SmallInt);
                prm.Value = hdnRateFE.Value;
                Trace.Warn("hdnRateFE.Value : " + hdnRateFE.Value);

                prm = cmd.Parameters.Add("@OverallR", SqlDbType.Float);
                prm.Value = (Convert.ToInt32(hdnRateST.Value) + Convert.ToInt32(hdnRateCM.Value) + Convert.ToInt32(hdnRatePE.Value) + Convert.ToInt32(hdnRateVC.Value) + Convert.ToInt32(hdnRateFE.Value)) / 5;
                Trace.Warn("hdnRateOA.Value : " + hdnRateOA.Value);

                prm = cmd.Parameters.Add("@Pros", SqlDbType.VarChar, 100);
                prm.Value = txtPros.Text.Trim();
                Trace.Warn("txtPros.Text.Trim() : " + txtPros.Text.Trim());

                prm = cmd.Parameters.Add("@Cons", SqlDbType.VarChar, 100);
                prm.Value = txtCons.Text.Trim();
                Trace.Warn("txtCons.Text.Trim() : " + txtCons.Text.Trim());

                prm = cmd.Parameters.Add("@Comments", SqlDbType.VarChar, 8000);
                prm.Value = SanitizeHTML.ToSafeHtml(ftbDescription.Text.Trim());
                Trace.Warn("ftbDescription.Text.Trim() : " + ftbDescription.Text.Trim());

                prm = cmd.Parameters.Add("@Title", SqlDbType.VarChar, 100);
                prm.Value = txtTitle.Text.Trim();
                Trace.Warn("txtTitle.Text.Trim() : " + txtTitle.Text.Trim());

                prm = cmd.Parameters.Add("@EntryDateTime", SqlDbType.DateTime);
                prm.Value = DateTime.Now;

                prm = cmd.Parameters.Add("@IsOwned", SqlDbType.Bit);
                prm.Value = !(radNot.Checked);

                prm = cmd.Parameters.Add("@IsNewlyPurchased", SqlDbType.Bit);
                if (radNew.Checked == true)
                    prm.Value = true;
                else
                    prm.Value = false;

                prm = cmd.Parameters.Add("@Familiarity", SqlDbType.SmallInt);
                prm.Value = ddlFamiliar.SelectedValue;

                prm = cmd.Parameters.Add("@Mileage", SqlDbType.Float);
                if (txtMileage.Text.Trim() != "")
                    prm.Value = txtMileage.Text.Trim().Replace("'", "''");
                else
                    prm.Value = 0;

                prm = cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                prm.Direction = ParameterDirection.Output;

                prm = cmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 40);
                prm.Value = CommonOpn.GetClientIP();

                con.Open();
                //run the command
                cmd.ExecuteNonQuery();

                recordId = cmd.Parameters["@ID"].Value.ToString();

                //update the source id of the review
                SourceIdCommon.UpdateSourceId(EnumTableType.CustomerReviews, recordId);

                Trace.Warn("done!!");
            }
            catch (SqlException err)
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
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return recordId;
        }		
    }
}