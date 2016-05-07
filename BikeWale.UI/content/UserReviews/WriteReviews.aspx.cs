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
using Bikewale.Notifications.CoreDAL;
using System.Data.Common;

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

        public string BikeVersion
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
                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }

            customerId = CurrentUser.Id;

            if (!IsPostBack)
            {
                if (customerId == "-1")
                {
                    Response.Redirect("/users/login.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.ServerVariables["HTTP_X_ORIGINAL_URL"]));
                }
                LoadDefaultComments();
                BikeName = GetBike();

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

            string msgPage = "/content/userreviews/reviewmessage.aspx";

            if (customerId == "-1")
            {
                SaveCustomer();

                Response.Redirect(msgPage);
            }
            else
            {
                Response.Redirect(msgPage);
            }
        }

        private void SaveCustomer()
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("registercustomer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, txtName.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, txtEmail.Text.Trim()));

                    MySqlDatabase.InsertQuery(cmd);
                } 
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public string GetBike()
        {
            string Bike = "";
            string sql = "";

            if (modelId != "")
                sql = @" select ma.name as make, mo.name as model, '' as version, ma.id as makeid,
                            mo.id as modelid from bikemodels as mo, bikemakes as ma  
                            where ma.id = mo.bikemakeid and mo.id = @modelid";
            else
                sql = @" select ma.name as make, mo.name as model, cv.name as version, ma.id as makeid,
                            mo.id as modelid from bikemodels as mo, bikemakes as ma, bikeversions as cv  
                            where ma.id = mo.bikemakeid and mo.id = cv.bikemodelid and cv.id = @versionid";

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("@modelid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], (modelId != "" ? modelId : "-1")));
                    cmd.Parameters.Add(DbFactory.GetDbParam("@versionid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], (versionId != "" ? versionId : "-1")));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null && dr.Read())
                        {
                            BikeMake = dr["Make"].ToString();
                            BikeModel = dr["Model"].ToString();
                            BikeVersion = dr["Version"].ToString();
                            Bike = string.Format("{0} {1} {2}", BikeMake, BikeModel, BikeVersion).Trim();
                            ModelIdVer = dr["ModelId"].ToString();
                            MakeId = dr["MakeId"].ToString();
                        }
                    }
                }
            }
            catch (SqlException err)
            {
                Trace.Warn(err.Message);
                ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                objErr.SendMail();
            } // catch Exception

            return Bike;
        }

        public bool CheckVersionReview()
        {
            //this function checks whether this user has already added a review for this version
            bool found = false;
            string sql = "";
            string id = versionId == "" ? drpVersions.SelectedItem.Value : versionId;

            string email = txtEmail.Text.Trim().Replace("'", "''");

            if (id != "-1")
            {
                if (customerId != "-1")
                {
                    sql = @" select count(id) from customerreviews   
                         where customerid = @customerid and versionid = @versionid";
                }
                else if (txtEmail.Text.Trim() != "")
                {
                    sql = @" select count(id) from customerreviews  where  
                        (id in (select recordid from unregisteredcustomers   where  
                        email = @email and registrationtype = 3)  
                        or customerid in (select id from customers  where 
                        email = @email)) and isactive=1 and versionid = @versionid";
                }

                try
                {
                    if (sql != "")
                    {
                        using (DbCommand cmd = DbFactory.GetDBCommand(sql))
                        {
                            cmd.Parameters.Add(DbFactory.GetDbParam("@customerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], (customerId != "" ? customerId : "-1")));
                            cmd.Parameters.Add(DbFactory.GetDbParam("@versionid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], (id != "" ? id : "-1")));
                            cmd.Parameters.Add(DbFactory.GetDbParam("@email", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, email));

                            using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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
                catch (SqlException err)
                {
                    Trace.Warn(err.Message);
                    ErrorClass objErr = new ErrorClass(err, Request.ServerVariables["URL"]);
                    objErr.SendMail();
                } // catch Exception
            }
            return found;
        }


        void FillVersions()
        {
            string sql = "";
            CommonOpn op = new CommonOpn();
            try
            {
                sql = " select Name, Id from bikeversions  where isdeleted = 0 and bikemodelid = @bikemodelid order by name ";

                DbParameter[] param = new[] { DbFactory.GetDbParam("@bikemodelid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ModelIdVer) };

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

            if (versionId == "")
                versionId = drpVersions.SelectedItem.Value;

            try
            {
                float _mileage = default(float);
                if (!string.IsNullOrEmpty(txtMileage.Text.Trim()) && float.TryParse(txtMileage.Text.Trim(), out _mileage)) ;


                using (DbCommand cmd = DbFactory.GetDBCommand("entrycustomerreviews"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], MakeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ModelIdVer));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_styler", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], hdnRateST.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comfortr", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], hdnRateCM.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_performancer", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], hdnRatePE.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_valuer", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], hdnRateVC.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_fueleconomyr", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], hdnRateFE.Value));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_overallr", DbParamTypeMapper.GetInstance[SqlDbType.Float], (Convert.ToInt32(hdnRateST.Value) + Convert.ToInt32(hdnRateCM.Value) + Convert.ToInt32(hdnRatePE.Value) + Convert.ToInt32(hdnRateVC.Value) + Convert.ToInt32(hdnRateFE.Value)) / 5));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pros", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, txtPros.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cons", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, txtCons.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 8000, SanitizeHTML.ToSafeHtml(ftbDescription.Text.Trim())));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_title", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, txtTitle.Text.Trim()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_entrydatetime", DbParamTypeMapper.GetInstance[SqlDbType.DateTime], DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isowned", DbParamTypeMapper.GetInstance[SqlDbType.Bit], !(radNot.Checked)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isnewlypurchased", DbParamTypeMapper.GetInstance[SqlDbType.Bit], (radNew.Checked) ? true : false));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_familiarity", DbParamTypeMapper.GetInstance[SqlDbType.SmallInt], ddlFamiliar.SelectedValue));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mileage", DbParamTypeMapper.GetInstance[SqlDbType.Float], _mileage));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 40, CommonOpn.GetClientIP()));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    recordId = cmd.Parameters["par_id"].Value.ToString();

                    //update the source id of the review
                    SourceIdCommon.UpdateSourceId(EnumTableType.CustomerReviews, recordId);
                }
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
            return recordId;
        }
    }
}